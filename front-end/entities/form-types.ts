export interface LoginUserFormData {
  email: string;
  password: string;
}

export interface SignupUserFormData {
  firstName: string;
  lastName: string;
  birthdate?: string;
  email: string;
  password: string;
}
