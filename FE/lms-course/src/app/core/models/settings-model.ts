export interface PasswordPolicy {
  passwordPolicyId: number;
  minLength: number;
  requiredUniqueChars: number;
  requireDigit: boolean;
  requireLowercase: boolean;
  requireUppercase: boolean;
  requireNonAlphanumeric: boolean;
}
