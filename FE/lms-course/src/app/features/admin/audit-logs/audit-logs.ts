import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { TableModule } from 'primeng/table';
import { CommonModule, DatePipe } from '@angular/common';
import { first, map, Subject, Subscription, switchMap, takeUntil } from 'rxjs';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { ButtonModule } from 'primeng/button';
import { Dialog, DialogModule } from 'primeng/dialog';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';
import { DatePicker, DatePickerModule } from 'primeng/datepicker';
import { AuditLogsService } from '../../../core/services/audit-logs.service';
import { FilterField, QueryDto, SortField } from '../../../core/models/query-model';
import {
  AuditlogsDto,
  HttpMethodValue,
  StatusCodeValue,
} from '../../../core/models/audit-logo-model';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { DetailAuditlogsComponent } from './detail-auditlogs/detail-auditlogs';

@Component({
  selector: 'app-audit-logs',
  templateUrl: './audit-logs.html',
  imports: [
    TableModule,
    DatePipe,
    InputTextModule,
    CommonModule,
    IconFieldModule,
    InputIconModule,
    ButtonModule,
    DialogModule,
    SelectModule,
    FormsModule,
    DatePicker,
    DatePickerModule,
  ],
  providers: [DynamicDialogRef],
})
export class AuditLogsComponent implements OnInit, OnDestroy {
  private auditLogsService = inject(AuditLogsService);
  private cd = inject(ChangeDetectorRef);

  private destroy$ = new Subject<void>();
  private loadAuditLogs$ = new Subject<QueryDto>();

  auditLogs = signal<AuditlogsDto[]>([]);
  pageSize = signal<number>(6);
  totalRecords = signal<number>(0);
  loading = false;
  currentAuditLog = signal<AuditlogsDto | undefined>(undefined);

  getAuditLogsApi = new Subscription();

  statusCodes: StatusCodeValue[] = [];
  selectedStatusCode: string | undefined;

  httpMethods: HttpMethodValue[] = [];
  selectedHttpMethod: string | undefined;
  filterGlobalValue: string = '';
  rangeDates: Date[] | undefined;

  private dynamicDialogRef = inject(DynamicDialogRef);
  private dialogService = inject(DialogService);

  ngOnInit(): void {
    this.auditLogsService
      .getDistinctStatusCode()
      .pipe(
        map((listStatusCode) => {
          const statusCodes: StatusCodeValue[] = listStatusCode.map((stc) => {
            return { value: stc.toString() };
          });
          return statusCodes;
        }),
        takeUntil(this.destroy$)
      )
      .subscribe((res) => {
        this.statusCodes = res;
        this.cd.detectChanges();
      });
    this.auditLogsService
      .getDistinctHttpMethod()
      .pipe(
        map((listHttpMethod) => {
          const httpMethods: HttpMethodValue[] = listHttpMethod.map((hm) => {
            return { value: hm };
          });
          return httpMethods;
        }),
        takeUntil(this.destroy$)
      )
      .subscribe((res) => {
        this.httpMethods = res;
        this.cd.detectChanges();
      });

    this.loadAuditLogs$
      .pipe(
        switchMap((query) => {
          this.loading = true;
          return this.auditLogsService.getAllAuditLogByQuery(query);
        }),
        takeUntil(this.destroy$)
      )
      .subscribe({
        next: (res) => {
          res.items.forEach((el) => (el.createdAt = new Date(el.createdAt + 'Z')));
          this.auditLogs.set(res.items);
          this.totalRecords.set(res.totalCount);
          this.loading = false;
          this.cd.markForCheck();
        },
        error: (err) => {
          console.log(err);
          this.loading = false;
          this.cd.markForCheck();
        },
      });

    this.loadAuditLogsLazy({ first: 0, rows: this.pageSize() });
  }

  log(value: any) {
    console.log(value);
  }

  loadAuditLogsLazy(event: any) {
    // console.log(event);
    const pageNumber = event?.first != null && event?.rows ? event.first / event.rows + 1 : 1;
    this.pageSize.set(event.rows);
    this.loading = true;
    let sorts: SortField[] = [];
    if (event.multiSortMeta) {
      sorts = event.multiSortMeta.map((s: any) => ({
        field: s.field,
        order: s.order === 1 ? 'asc' : 'desc',
      }));
    }
    // console.log(sorts);
    // // filters
    const filters: FilterField[] = [];
    if (event.filters && Object.keys(event.filters).length > 0) {
      for (const key of Object.keys(event.filters)) {
        const f = event.filters[key];
        if (f && f.value) {
          if (Array.isArray(f.value) && f.value.length === 2) {
            const startStr = f.value[0].toISOString().replace('Z', '');
            const endStr = f.value[1].toISOString().replace('Z', '');

            filters.push({
              field: key,
              value: `${startStr}*${endStr}`, // gửi ISO string
            });
          } else {
            filters.push({ field: key, value: f.value });
          }
        }
      }
    }
    const query: QueryDto = {
      pageNumber: pageNumber,
      pageSize: this.pageSize(),
      sorts: sorts,
      filters: filters,
    };
    // console.log('Query:', query);
    this.loadAuditLogs$.next(query);
  }

  clearTable(dt: any) {
    this.selectedStatusCode = undefined;
    this.selectedHttpMethod = undefined;
    this.filterGlobalValue = '';
    this.rangeDates = undefined;
    dt.clear();
  }

  selectDTPicker(event: any, dt: any) {
    // console.log(event);
    if (this.rangeDates != undefined && this.rangeDates.length === 2 && this.rangeDates[1]) {
      dt.filter(this.rangeDates, 'createdAt', 'between');
    } else {
    }
  }

  getMethodColor(method: string): string {
    switch (method) {
      case 'GET':
        return 'text-blue-500'; // xanh dương
      case 'POST':
        return 'text-green-500'; // xanh lá
      case 'PUT':
        return 'text-yellow-500'; // vàng
      case 'DELETE':
        return 'text-red-500'; // đỏ
      default:
        return 'text-gray-500'; // mặc định
    }
  }

  getStatusColor(status: number): string {
    if (status >= 200 && status < 300) {
      return 'bg-green-100 text-green-700'; // thành công
    } else if (status >= 400 && status < 500) {
      return 'bg-yellow-100 text-yellow-700'; // lỗi client
    } else if (status >= 500) {
      return 'bg-red-100 text-red-700'; // lỗi server
    }
    return 'bg-gray-100 text-gray-700'; // mặc định
  }

  viewDetailAuditLog(auditLog: AuditlogsDto) {
    console.log(auditLog);

    this.dynamicDialogRef = this.dialogService.open(DetailAuditlogsComponent, {
      header: 'Chi tiết nhật ký',
      width: '800px',
      maximizable: true,
      closable: true,
      modal: true,
      data: { auditlog: auditLog },
    });

    this.dynamicDialogRef.onClose.subscribe();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.dynamicDialogRef.close();
  }
}
