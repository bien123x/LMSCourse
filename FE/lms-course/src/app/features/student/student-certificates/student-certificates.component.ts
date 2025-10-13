import { ChangeDetectorRef, Component, inject, OnInit, signal } from '@angular/core';
import { CertificateService } from '../../../core/services/certificate.service';
import { AuthService } from '../../../core/services/auth.service';
import { Subject, switchMap, takeUntil } from 'rxjs';
import { CertificateViewDto } from '../../../core/models/certificate-model';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-student-certificates',
  templateUrl: './student-certificates.component.html',
  imports: [TableModule, ButtonModule, DialogModule, DatePipe],
})
export class StudentCertificatesComponent implements OnInit {
  private certificateService = inject(CertificateService);
  private authService = inject(AuthService);

  private destroy$ = new Subject<void>();

  private cd = inject(ChangeDetectorRef);

  certificates = signal<CertificateViewDto[]>([]);
  visibleViewCertificate = false;

  certificateUrl?: SafeResourceUrl;

  constructor(private sanitizer: DomSanitizer) {}

  openCertificate(path: string) {
    const fullUrl = 'https://localhost:7202' + path;
    this.certificateUrl = this.sanitizer.bypassSecurityTrustResourceUrl(fullUrl);
    this.visibleViewCertificate = true;
  }

  ngOnInit(): void {
    this.authService
      .me()
      .pipe(
        switchMap((user) => {
          return this.certificateService.getByUserId(user.userId);
        }),
        takeUntil(this.destroy$)
      )
      .subscribe((res) => {
        this.certificates.set(res);
        console.log(res);
        this.cd.detectChanges();
      });
  }
}
