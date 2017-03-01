import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import 'rxjs/add/operator/map';

import { AppSettings } from '../../appsettings';

@Injectable()
export class PlutoService {

    constructor(private http: Http) {}

    getMapMarkers(lpcNumber: string) {
        return this.http.get(`${AppSettings.ApiEndpoint}Pluto/${lpcNumber}`)
            .map((res: Response) => res.json());
    };
}

