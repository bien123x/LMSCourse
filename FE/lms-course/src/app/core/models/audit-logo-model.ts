export interface AuditlogsDto {
  userId: number;
  httpMethod: string;
  url: string;
  statusCode: number;
  userName: number;
  ipAddress: string;
  duration: number;
  browserInfo: string;
  exception: string;
  createAt: Date;
}
