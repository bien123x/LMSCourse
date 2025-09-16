export interface LoginDto {
  UserNameOrEmail: string;
  Password: string;
}

export interface RegisterDto {
  UserName: string;
  Email: string;
  PasswordHash: string;
}

export interface RefreshRequestDto {
  refreshToken: string;
}

export interface DecodedToken {
  roles: string[];
  permissions: string[];
  exp: number;
}
