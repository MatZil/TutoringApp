import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { UrlService } from './url.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  constructor(
    private httpClient: HttpClient,
    private urlService: UrlService
  ) { }

  public get(controller: string, endpoint: string, queryParams?: HttpParams): Observable<any> {
    const url = this.urlService.getApiEndpointUrl(controller, endpoint);

    return this.httpClient.get<any>(url, { params: queryParams });
  }

  public post(controller: string, endpoint: string, body: any, queryParams?: HttpParams): Observable<any> {
    const url = this.urlService.getApiEndpointUrl(controller, endpoint);

    return this.httpClient.post<any>(url, body, { params: queryParams });
  }

  public put(controller: string, endpoint: string, body: any, queryParams?: HttpParams): Observable<any> {
    const url = this.urlService.getApiEndpointUrl(controller, endpoint);

    return this.httpClient.put<any>(url, body, { params: queryParams });
  }

  public patch(controller: string, endpoint: string, body: any): Observable<any> {
    const url = this.urlService.getApiEndpointUrl(controller, endpoint);

    return this.httpClient.patch<any>(url, body);
  }

  public delete(controller: string, endpoint: string, queryParams?: HttpParams): Observable<any> {
    const url = this.urlService.getApiEndpointUrl(controller, endpoint);

    return this.httpClient.delete<any>(url, { params: queryParams });
  }
}
