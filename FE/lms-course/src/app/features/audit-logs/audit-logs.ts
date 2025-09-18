import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { AuditLogsService } from '../../core/services/audit-logs.service';
import { TableModule } from 'primeng/table';
import { FilterField, QueryDto, SortField } from '../../core/models/query-model';
import { AuditlogsDto } from '../../core/models/audit-logo-model';
import { CommonModule, DatePipe } from '@angular/common';
import { Subscription } from 'rxjs';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-audit-logs',
  templateUrl: './audit-logs.html',
  imports: [TableModule, DatePipe, InputTextModule, CommonModule],
})
export class AuditLogsComponent implements OnInit, OnDestroy {
  private auditLogsService = inject(AuditLogsService);
  auditLogs = signal<AuditlogsDto[]>([]);
  pageSize = signal<number>(10);
  totalRecords = signal<number>(0);
  loading = false;
  currentAuditLog = signal<AuditlogsDto | undefined>(undefined);

  getAuditLogsApi = new Subscription();

  ngOnInit(): void {
    this.loadAuditLogsLazy({
      first: 0,
      rows: 10,
    });
  }

  loadAuditLogsLazy(event: any) {
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
          filters.push({ field: key, value: f.value });
        }
      }
    }
    const query: QueryDto = {
      pageNumber: pageNumber,
      pageSize: this.pageSize(),
      sorts: sorts,
      filters: filters,
    };
    console.log('Query:', query);
    this.getAuditLogsApi = this.auditLogsService.getAllAuditLogByQuery(query).subscribe({
      next: (res) => {
        console.log(res);
        this.auditLogs.set(res.items);
        this.totalRecords.set(res.totalCount);
        this.loading = false;
      },
      error: (err) => {
        console.log('Request thất bại, nhưng không hiển thị 401 trên console');
      },
    });
  }

  getClass(statusCode: number, method: string): string {
    if (statusCode >= 200 && statusCode < 300) {
      return method === 'POST'
        ? 'text-green-500 font-bold' // POST thành công → xanh lá
        : 'text-blue-500 font-bold'; // GET/PUT thành công → xanh dương
    }

    if (statusCode >= 400 && statusCode < 500) {
      return 'text-yellow-500 font-bold'; // lỗi client
    }

    if (statusCode >= 500) {
      return 'text-red-500 font-bold'; // lỗi server
    }

    return 'text-gray-500';
  }

  ngOnDestroy(): void {
    this.getAuditLogsApi.unsubscribe();
  }
}
