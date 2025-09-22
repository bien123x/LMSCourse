export interface ViewUserDto {
  userId: number;
  userName: string;
  name: string;
  surname: string;
  email: string;
  phoneNumber: string;
  isActive: boolean;
  roles: string;
  createdBy: string;
  creationTime: Date;
  modifiedBy: string;
  modificationTime: Date;
  passwordUpdateTime: Date;
  lockoutEndTime: Date;
  failedAccessCount: number;
}

export interface UserDto {
  userName: string;
  name: string;
  passwordHash: string;
  surname: string;
  email: string;
  phoneNumber: string;
  isActive: boolean;
  roles: string[];
}

export interface EditUserDto {
  userName: string;
  name: string;
  surname: string;
  email: string;
  phoneNumber: string;
  isActive: boolean;
  roles: string[];
}

export interface UserPermissionsDto {
  userPermissions: string[];
  rolePermissions: string[];
}

export interface ResetPasswordDto {
  passwordHash: string;
}

export interface LockEndTimeDto {
  lockEndtime: Date | null;
}

export interface ChangePasswordDto {
  nowPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}
