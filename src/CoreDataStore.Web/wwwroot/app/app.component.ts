import {ROUTER_DIRECTIVES, RouterConfig } from "@angular/router";
import {Component } from "@angular/core";


import { PropertiesComponent } from './components/properties.component';


/* Providers */
import { HTTP_PROVIDERS } from '@angular/http';
import { FORM_PROVIDERS, LocationStrategy, HashLocationStrategy } from '@angular/common';

import { Sorter } from './utils/sorter';
import { DataService } from './services/data.service';
import { TrackByService } from './services/trackby.service';


const APP_PROVIDERS = [
    Sorter,
    DataService,
    TrackByService,
    FORM_PROVIDERS,
    HTTP_PROVIDERS    
];

@Component({
  	selector:'second',
  	template: `<h1>second</h1>`
})

@Component({
	selector: 'my-app',
	directives: [ ROUTER_DIRECTIVES ],
  	providers: [ APP_PROVIDERS ],
	template: 
	`
		<div class="navbar navbar-inverse navbar-fixed-top">
	        <div class="container">
	            <div class="navbar-header">
	                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
	                    <span class="sr-only">Toggle navigation</span>
	                    <span class="icon-bar"></span>
	                    <span class="icon-bar"></span>
	                    <span class="icon-bar"></span>
	                </button>
	                <a asp-controller="Home" asp-action="Index" class="navbar-brand">CoreDataStore.Web</a>
	            </div>
	            <div class="navbar-collapse collapse">
	                <ul class="nav navbar-nav">
	                    <li><a [routerLink]="['/']" asp-controller="Home" asp-action="Index">Home</a></li>
	                    <li><a href="/swagger/ui/index.html">Swagger</a></li>
	                </ul>
	            </div>
	        </div>
	    </div>

	 	<router-outlet></router-outlet>
	`
})

// @Routes([
// 	{path :'/', component: PropertiesComponent},
//    	{path :'/second', component : SecondComponent}

// ])

export class AppComponent { }

