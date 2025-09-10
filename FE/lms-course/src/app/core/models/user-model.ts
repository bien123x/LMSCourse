export interface ViewUserDto {
  userId: number;
  userName: string;
  name: string;
  surname: string;
  email: string;
  phoneNumber: string;
  isActive: boolean;
  roles: string;
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
