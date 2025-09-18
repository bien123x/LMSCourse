// lockout policy
export interface LockoutPolicy {
  allowedForNewUsers: boolean;
  lockoutDuration: number; // thời gian khóa (giây)
  maxFailedAccessAttempts: number;
}

// password policy
export interface PasswordPolicy {
  requiredLength: number;
  requiredUniqueChars: number;
  requireNonAlphanumeric: boolean;
  requireLowercase: boolean;
  requireUppercase: boolean;
  requireDigit: boolean;
  forceUsersToPeriodicallyChangePassword: boolean;
  passwordChangePeriodDays: number;
}

// sign in policy
export interface SignInPolicy {
  requireConfirmedEmail: boolean;
  requireEmailVerificationToRegister: boolean;
  enablePhoneNumberConfirmation: boolean;
  requireConfirmedPhoneNumber: boolean;
}

// user policy
export interface UserPolicy {
  isUserNameUpdateEnabled: boolean;
  isEmailUpdateEnabled: boolean;
}

// tổng hợp tất cả
export interface IdentitySettingDto {
  lockout: LockoutPolicy;
  password: PasswordPolicy;
  signIn: SignInPolicy;
  user: UserPolicy;
}
