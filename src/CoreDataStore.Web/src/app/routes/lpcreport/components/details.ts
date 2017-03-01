import { Component, ViewContainerRef, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { DetailFormComponent } from './detailsList';
import { LPCReportService } from '../services/lpcreport';
import { PlutoService } from '../services/pluto';
import { ReferencesService } from '../../references/services/references';

@Component({
  selector: 'properties-details',
  templateUrl: 'app/routes/lpcreport/components/details.html',
  providers: [LPCReportService, ReferencesService, PlutoService],
  encapsulation: ViewEncapsulation.None
})

export class DetailsComponent implements OnInit, OnDestroy {
  public title: string;
  public details: any;
  public landmarkProperties: any = [];
  public mapMarkers: any = [];
  public sub: any = null;
  // ng2Table
  private ng2TableData: Array < any > = [];
  public rows: Array < any > = [];
  public columns: Array < any > = [
    { title: 'Id', name: 'id', sort: 'asc' },
    { title: 'BLL', name: 'bbl' },
    { title: 'BIN_NUMBER', name: 'binNumber' },
    { title: 'BoroughID', name: 'boroughId' },
    { title: 'BLOCK', name: 'block' },
    { title: 'LOT', name: 'lot' },
    { title: 'LP_NUMBER', name: 'lpNumber' },
    { title: 'LM_NAME', name: 'name' },
    { title: 'PLUTO_ADDR', name: 'designatedAddress' },
    { title: 'DESIG_ADDR', name: 'plutoAddress' }
  ];
  public page: number = 1;
  public itemsPerPage: number = 10;
  public maxSize: number = 5;
  public numPages: number = 1;
  public length: number = 0;


  public config: any = {
    paging: true,
    sorting: { columns: this.columns },
    // filtering: { filterString: '' },
    className: ['table-striped', 'table-bordered']
  };

  /*@Inject(ActivatedRoute) */
  /*overlay: Overlay, vcRef: ViewContainerRef, public modal: Modal*/
  constructor(private lpcReportService: LPCReportService, private plutoService: PlutoService, private route: ActivatedRoute) {
    // overlay.defaultViewContainer = vcRef;
    this.sub = this.route.params.subscribe(params => {
      let id = +params['id'];
      // console.log(id);
      this.lpcReportService.getLPCReport(id).subscribe(
        data => {
          this.details = data;
          this.title = data.name;
          this.lpcReportService.getLandmarkProperties(this.details.lpNumber).subscribe(
            data => {
              this.landmarkProperties = data || [];
              this.ng2TableData = this.landmarkProperties;
              this.length = this.landmarkProperties.length;
            },
            err => console.log(err)
          );
          this.plutoService.getMapMarkers(this.details.lpNumber).subscribe(
            data => {
              this.mapMarkers = data;
            }
          );
        },
        err => console.log(err)
      );
    });
  }

  ngOnInit() {
    this.onChangeTable(this.config);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  public changePage(page: any, data: Array < any > = this.ng2TableData): Array < any > {
    let start = (page.page - 1) * page.itemsPerPage;
    let end = page.itemsPerPage > -1 ? (start + page.itemsPerPage) : data.length;
    return (data && data.length) ? data.slice(start, end) : data;
  }

  public changeSort(data: any, config: any): any {
    if (!config.sorting) {
      return data;
    }

    let columns = this.config.sorting.columns || [];
    let columnName: string = void 0;
    let sort: string = void 0;

    for (let i = 0; i < columns.length; i++) {
      if (columns[i].sort !== '' && columns[i].sort !== false) {
        columnName = columns[i].name;
        sort = columns[i].sort;
      }
    }

    if (!columnName) {
      return data;
    }

    // simple sorting
    if (data) {
      return data.sort((previous: any, current: any) => {
        if (previous[columnName] > current[columnName]) {
          return sort === 'desc' ? -1 : 1;
        } else if (previous[columnName] < current[columnName]) {
          return sort === 'asc' ? -1 : 1;
        }
        return 0;
      });
    } else {
      return data;
    }
  }

  public changeFilter(data: any, config: any): any {
    let filteredData: Array < any > = data;
    this.columns.forEach((column: any) => {
      if (column.filtering) {
        filteredData = filteredData.filter((item: any) => {
          return item[column.name].match(column.filtering.filterString);
        });
      }
    });

    if (!config.filtering) {
      return filteredData;
    }

    if (config.filtering.columnName) {
      return filteredData.filter((item: any) =>
        item[config.filtering.columnName].match(this.config.filtering.filterString));
    }

    let tempArray: Array < any > = [];
    filteredData.forEach((item: any) => {
      let flag = false;
      this.columns.forEach((column: any) => {
        if (item[column.name].toString().match(this.config.filtering.filterString)) {
          flag = true;
        }
      });
      if (flag) {
        tempArray.push(item);
      }
    });
    filteredData = tempArray;

    return filteredData;
  }

  public onChangeTable(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
    if (config.filtering) {
      ( < any > Object).assign(this.config.filtering, config.filtering);
    }

    if (config.sorting) {
      ( < any > Object).assign(this.config.sorting, config.sorting);
    }

    let filteredData = this.changeFilter(this.ng2TableData, this.config);
    let sortedData = this.changeSort(filteredData, this.config);
    this.rows = page && config.paging ? this.changePage(page, sortedData) : sortedData;
    this.length = (sortedData && sortedData.length) ? sortedData.length : 0;
  }

  public onCellClick(data: any): any {
    console.log(data);
  }
}
