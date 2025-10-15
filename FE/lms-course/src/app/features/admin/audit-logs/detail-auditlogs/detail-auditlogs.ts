import { Component, inject, OnInit, signal } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AuditlogsDto } from '../../../../core/models/audit-logo-model';
import { TableModule } from 'primeng/table';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-detail-auditlogs',
  templateUrl: './detail-auditlogs.html',
  imports: [TableModule, DatePipe],
})
export class DetailAuditlogsComponent implements OnInit {
  private ref = inject(DynamicDialogRef);
  private config = inject(DynamicDialogConfig);

  auditlog = signal<AuditlogsDto | null>(null);
  valueTable = signal<{ label: string; value: any }[]>([]);

  ngOnInit(): void {
    if (this.config.data) {
      const data = this.config.data;
      this.auditlog.set(data.auditlog);

      this.valueTable.set([
        { label: 'Mã trạng thái', value: this.auditlog()?.statusCode },
        { label: 'Phương thức', value: this.auditlog()?.httpMethod },
        { label: 'Đường dẫn', value: this.auditlog()?.url },
        { label: 'Người dùng', value: this.auditlog()?.userName },
        { label: 'Ngày', value: this.auditlog()?.createdAt },
        { label: 'Thời gian', value: this.auditlog()?.duration },
        { label: 'Thông tin trình duyệt', value: this.auditlog()?.browserInfo },
        { label: 'Lỗi', value: this.auditlog()?.exception },
      ]);
    }
  }
}
