const constants: AppConstants = {
  WebTokenKey: 'web_token',
  LoginRoute: 'login',
  RegistrationRoute: 'register',
  EmailConfirmationRoute: 'confirm-email',
  ModuleViewRoute: 'modules/:id'
};

interface AppConstants {
  readonly WebTokenKey: string;

  readonly LoginRoute: string;
  readonly RegistrationRoute: string;
  readonly EmailConfirmationRoute: string;
  readonly ModuleViewRoute: string;
}

export const AppConstants: AppConstants = constants;
