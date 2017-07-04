import { Injectable } from '@angular/core';
import { Http, Response, Headers, URLSearchParams } from '@angular/http';
import 'rxjs/add/operator/map';

import { AppSettings } from '../../../appsettings';

@Injectable()
export class MapsService {

  constructor(private http: Http) {}

  public getMaps() {
    return this.http.get(`${AppSettings.ApiMaps}maps`)
      .map((res: Response) => res.json());
  };

  public getMapsId(id: string) {
    var headers = new Headers({ 'Accept': 'application/vnd.geo+json' });
    return this.http.get(`${AppSettings.ApiMaps}maps/${id}`, { headers: headers })
      .map((res: Response) => res.json());
  };

  public getMapsFeatures(id: string) {
    var headers = new Headers({ 'Accept': 'application/vnd.geo+json' });
    return this.http.get(`${AppSettings.ApiMaps}maps/${id}/features`, { headers: headers })
      .map((res: Response) => res.json());
  };
}
