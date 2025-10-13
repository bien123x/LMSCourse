export interface LoginDto {
  UserNameOrEmail: string;
  Password: string;
}

export interface RegisterDto {
  userName: string;
  email: string;
  passwordHash: string;
}

export interface RefreshRequestDto {
  refreshToken: string;
}

export interface DecodedToken {
  roles: string[];
  permissions: string[];
  exp: number;
}
