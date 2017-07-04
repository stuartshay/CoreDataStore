import { Injectable } from '@angular/core';
import { Http, Response, Headers, URLSearchParams } from '@angular/http';
import 'rxjs/add/operator/map';

import { AppSettings } from '../../../appsettings';

@Injectable()
export class ReferencesService {

  constructor(private http: Http) {}
  getReferences(page = 1, limit = 10, borough: string, objectType: string) {
    let params = new URLSearchParams();
    if (borough && borough !== '') {
      params.set('Borough', borough);
    }
    if (objectType && objectType !== '') {
      params.set('ObjectType', objectType);
    }
    return this.http.get(`${AppSettings.ApiEndpoint}LPCReport/${limit}/${page}`, { search: params })
      .map((res: Response) => res.json());
  };

  getBoroughs() {
    return this.http.get(`${AppSettings.ApiEndpoint}Reference/borough`)
      .map((res: Response) => res.json());
  };

  getObjectTypes() {
    return this.http.get(`${AppSettings.ApiEndpoint}Reference/objectType`)
      .map((res: Response) => res.json());
  };

  getParentStyle() {
    return this.http.get(`${AppSettings.ApiEndpoint}Reference/parentStyle`)
      .map((res: Response) => res.json());
  };
}
