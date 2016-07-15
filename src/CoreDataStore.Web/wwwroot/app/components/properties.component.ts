import { Component, OnInit } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';
//import { Observable } from 'rxjs/Observable';
//import { PAGINATION_DIRECTIVES } from 'ng2-bootstrap/ng2-bootstrap';

import { DataService } from '../services/data.service';
import { FilterTextboxComponent } from './filterTextbox.component';
import { FilterSelectboxComponent } from './filterSelectbox.component';
// import { CustomersCardComponent } from './customersCard.component';
import { PropertiesListComponent } from './propertiesList.component'
import { IProperty } from '../interfaces';

@Component({
    moduleId: module.id,
    selector: 'properties',
    templateUrl: 'properties.component.html',
    directives: [
        //PAGINATION_DIRECTIVES, 
        ROUTER_DIRECTIVES, PropertiesListComponent, FilterTextboxComponent,
        FilterSelectboxComponent
        /*CustomersCardComponent, CustomersGridComponent*/
    ],
})
export class PropertiesComponent implements OnInit {
    title: string;
    borough: string = '';
    objectType: string = '';
    page: number = 1;
    boroughs: string[] = [];
    objectTypes: string[] = [];
    properties: IProperty[] = [];
    filteredProperties: IProperty[] = [];
    totalItems: number = 100;
    // displayMode: DisplayModeEnum;
    // displayModeEnum = DisplayModeEnum;

    constructor(private dataService: DataService) {}

    ngOnInit() {
        this.title = 'Properties';
        // this.displayMode = DisplayModeEnum.Card;

        this.dataService.getProperties(this.borough, this.objectType, this.page)
            .subscribe((properties: IProperty[]) => {
                this.properties = this.filteredProperties = properties;
                // console.log(properties);
            });

        this.dataService.getBoroughs()
            .subscribe((boroughs: string[]) => {
                this.boroughs = boroughs;
                // console.log(boroughs);
            });

        this.dataService.getObjectTypes()
            .subscribe((objectTypes: string[]) => {
                this.objectTypes = objectTypes;
                // console.log(objectTypes);
            });
    }

    /*changeDisplayMode(mode: DisplayModeEnum) {
        this.displayMode = mode;
    }*/

    boroughChanged(data: string) {
        // console.log(data);
        this.borough = data;
        this.dataService.getProperties(this.borough, this.objectType, this.page)
            .subscribe((properties: IProperty[]) => {
                this.properties = this.filteredProperties = properties;
                // console.log(properties);
            });
    }

    objectTypeChanged(data: string) {
        // console.log(data);
        this.objectType = data;
        this.dataService.getProperties(this.borough, this.objectType, this.page)
            .subscribe((properties: IProperty[]) => {
                this.properties = this.filteredProperties = properties;
                // console.log(properties);
            });
    }

    pageChanged(event: any) {
        // console.log(event);
        // console.log('Page changed to: ' + event.page);
        // console.log('Number items per page: ' + event.itemsPerPage);  }
        this.page = event.page;
        this.dataService.getProperties(this.borough, this.objectType, this.page)
            .subscribe((properties: IProperty[]) => {
                this.properties = this.filteredProperties = properties;
                // console.log(properties);
            });
    }
}

/*
enum DisplayModeEnum {
  Card = 0,
  Grid = 1,
  Map = 2
}
*/