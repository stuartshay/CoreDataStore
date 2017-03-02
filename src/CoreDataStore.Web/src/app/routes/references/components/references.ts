import { Component, OnInit } from '@angular/core';

import { SessionService } from '../../../shared/services/session';

import { ReferencesService } from '../services/references';
import { LPCReportService } from '../../lpcreport/services/lpcreport';
// import { FilterTextboxComponent } from '../../components/filterTextbox';
// import { FilterSelectboxComponent } from '../../components/filterSelectbox';
// import { CustomersCardComponent } from './customersCard.component';
import { ReferencesListComponent } from './referencesList';
import { FilterSelectboxComponent } from './filterSelectbox';

@Component({
  selector: 'properties',
  templateUrl: 'app/routes/references/components/references.html',
  providers: [ReferencesService, LPCReportService, SessionService]
})

export class ReferencesComponent implements OnInit {
  title: string;
  borough: string;
  objectType: string;
  page: number = 1;
  limit: number = 20;
  perPage: any[] = [10, 20, 50, 100];
  totalItems: number = 100;
  fromItem: number = 1;
  toItem: number = 20;
  boroughs: string[] = [];
  objectTypes: string[] = [];
  properties: any[] = []; // LPCReports list;
  filteredReference: any[] = [];

  // displayMode: DisplayModeEnum;
  // displayModeEnum = DisplayModeEnum;

  constructor(private session: SessionService, private referenceService: ReferencesService, private lpcReportService: LPCReportService) {
    let page = this.session.get('page');
    this.page = (parseInt(page, 10) > 0) ? page : 1;
  }

  ngOnInit() {
    this.title = 'LPC Reports';
    this.getObjectTypes();
    this.getBoroughs();

    let objectType = this.session.get('objectType');
    this.objectType = (objectType) ? objectType : '';

    let borough = this.session.get('borough');
    this.borough = (borough) ? borough : '';

    this.getLPCReports(this.page, this.limit, this.borough, this.objectType);
  }

  getLPCReports(page, limit, borough, objectType) {
    this.lpcReportService.getLPCReports(page, limit, borough, objectType).subscribe(
        data => {
          this.properties = this.filteredReference = data.reports;
          this.totalItems = data.total;

          this.objectType = objectType;
          this.borough = borough;
          this.page = page;
          this.fromItem = ((page - 1) * limit) + 1;
          this.toItem = (this.totalItems  < (page * limit)) ? this.totalItems  : (page * limit);
        },
        () => console.log('done loading getLPCReports')
    );
  }

  getObjectTypes() {
    this.referenceService.getObjectTypes().subscribe(
      data => { this.objectTypes = data; },
      err => console.error(err)
    );
  }

  getBoroughs() {
    this.referenceService.getBoroughs().subscribe(
      data => { this.boroughs = data; },
      err => console.error(err)
    );
  }

  /*changeDisplayMode(mode: DisplayModeEnum) {
      this.displayMode = mode;
  }*/

  boroughChanged(data: string) {
    // console.log(data);
    this.borough = data;
    this.session.set('borough', data);
    this.page = 1;
    this.session.set('page', this.page);
    this.getLPCReports(this.page, this.limit, this.borough, this.objectType);
  }

  objectTypeChanged(data: string) {
    // console.log(data);
    this.objectType = data;
    this.session.set('objectType', data);
    this.page = 1;
    this.session.set('page', this.page);
    this.getLPCReports(this.page, this.limit, this.borough, this.objectType);
  }

  pageChanged(event: any) {
    // console.log(event);
    this.page = event.page;
    this.session.set('page', this.page);
    this.getLPCReports(event.page, this.limit, this.borough, this.objectType);
  }

  perPageChanged(limit: any) {
    this.page = 1;
    this.limit = limit;
    this.session.set('page', this.page);
    this.getLPCReports(1, limit, this.borough, this.objectType);
  }
}

/*
enum DisplayModeEnum {
  Card = 0,
  Grid = 1,
  Map = 2
}
*/
