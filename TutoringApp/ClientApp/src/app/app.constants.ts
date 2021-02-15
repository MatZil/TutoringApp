const constants: AppConstants = {
  WebTokenKey: 'web_token',
  LoginRoute: 'login',
  RegistrationRoute: 'register'
};

interface AppConstants {
  readonly WebTokenKey: string;

  readonly LoginRoute: string;
  readonly RegistrationRoute: string;
}

export const AppConstants: AppConstants = constants;
