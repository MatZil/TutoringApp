import { Inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UrlService {

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
  ) { }

  public getApiEndpointUrl(controller: string, endpoint: string): string {
    const baseApiUrl = this.baseUrl + 'api/';

    return baseApiUrl + controller + `/${endpoint}`;
  }
}
