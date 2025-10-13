export interface CertificateDto {
  courseId: number;
  userId: number;
  teacherId: number;
}

export interface CertificateViewDto {
  certificateId: number;
  certificateName: string;
  issueDate: Date;
  certificateUrl: string;
  marks: number;
  outOf: number;
}
