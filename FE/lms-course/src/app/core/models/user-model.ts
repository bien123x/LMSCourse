export interface ViewUserDto {
  userId: number;
  userName: string;
  name: string;
  surname: string;
  email: string;
  phoneNumber: string;
  isActive: boolean;
  roles: string;
  createBy: string;
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
