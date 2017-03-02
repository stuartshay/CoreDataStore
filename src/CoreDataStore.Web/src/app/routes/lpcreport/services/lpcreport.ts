import { Injectable } from '@angular/core';
import { Http, Response, Headers, URLSearchParams } from '@angular/http';
import 'rxjs/add/operator/map';

import { AppSettings } from '../../appsettings';

@Injectable()
export class LPCReportService {

  constructor(private http: Http) {}

  getLPCReport(id: number) {
    return this.http.get(`${AppSettings.ApiEndpoint}LPCReport/${id}`)
      .map((res: Response) => res.json());
  };

  putLPCReport(id: number, params: any) {
    return this.http.put(`${AppSettings.ApiEndpoint}LPCReport/${id}`, params)
      .map((res: Response) => res);
  };

  getLPCReports(page = 1, limit = 10, borough: string, objectType: string) {
    let params = new URLSearchParams();
    if (borough && borough !== '') {
      params.set('Borough', borough);
    }
    if (objectType && objectType !== '') {
      params.set('ObjectType', objectType);
    }
    return this.http.get(`${AppSettings.ApiEndpoint}LPCReport/${limit}/${page}`, { search: params })
      .map((res: Response) => {
        return {
          total: parseInt(res.headers.get('x-inlinecount'), 10),
          reports: res.json()
        };
      });
  };


  getLandmarkProperties(LPCNumber: string, page = 1, limit = 100, Sort ? : string, Order ? : string) {
    let params = new URLSearchParams();
    if (Sort && Sort !== '') {
      params.set('Sort', Sort);
    }
    if (Order && Order !== '') {
      params.set('Order', Order);
    }
    if (LPCNumber && LPCNumber !== '') {
      params.set('LPCNumber', LPCNumber);
    }
    return this.http.get(`${AppSettings.ApiEndpoint}LPCReport/landmark/${limit}/${page}`, { search: params })
      .map((res: Response) => res.json());
  };
}
