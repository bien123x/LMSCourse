import { Component, inject, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ButtonModule } from 'primeng/button';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  imports: [ButtonModule],
})
export class VideoComponent implements OnInit {
  private dynamicDialogRef = inject(DynamicDialogRef);
  private dynamicDialogConfig = inject(DynamicDialogConfig);

  videoId = 'e33ToWFLYD4'; // ID của video YouTube
  safeUrl!: SafeResourceUrl;
  private sanitizer = inject(DomSanitizer);

  ngOnInit(): void {
    console.log(this.dynamicDialogConfig);
    if (this.dynamicDialogConfig.data) {
      this.videoId = this.dynamicDialogConfig.data.videoUrl;
      this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
        `https://www.youtube.com/embed/${this.videoId}`
      );
    }
  }

  close() {
    this.dynamicDialogRef.close();
  }
}
