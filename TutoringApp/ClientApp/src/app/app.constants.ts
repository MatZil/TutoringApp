const constants: AppConstants = {
  WebTokenKey: 'tutoring_app_web_token',
  LoginRoute: 'login',
  RegistrationRoute: 'register',
  EmailConfirmationRoute: 'confirm-email',
  ModuleViewRoute: 'modules/:id',
  StudentRole: 'Student',
  AdminRole: 'Admin',
  LecturerRole: 'Lecturer',
  RoleClaimType: 'Role',
  EmailClaimType: 'Email'
};

interface AppConstants {
  readonly WebTokenKey: string;
  readonly RoleClaimType: string;
  readonly EmailClaimType: string;

  readonly LoginRoute: string;
  readonly RegistrationRoute: string;
  readonly EmailConfirmationRoute: string;
  readonly ModuleViewRoute: string;

  readonly StudentRole: string;
  readonly AdminRole: string;
  readonly LecturerRole: string;
}

export const AppConstants: AppConstants = constants;
