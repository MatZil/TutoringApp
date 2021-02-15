import { AppConstants } from '../app.constants';

export function TokenGetter(): string {
    return localStorage.getItem(AppConstants.WebTokenKey);
}
