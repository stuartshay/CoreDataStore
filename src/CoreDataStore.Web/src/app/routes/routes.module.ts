import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

// import { DndModule } from 'ng2-dnd';
// import { InfiniteScrollModule } from 'angular2-infinite-scroll';
// import { AgmCoreModule } from '@agm/core';
import { AgmCoreModule } from 'angular2-google-maps/core';
import { SelectModule } from 'ng2-select/ng2-select';
import { Ng2TableModule } from 'ng2-table/ng2-table';

import { MenuService } from '../core/menu/menu.service';

// import { HomeComponent } from './home/home.component';

import { DiagnosticsComponent } from './diagnostics/components/diagnostics';
import { ReferencesComponent } from './references/components/references';
import { ReferencesListComponent } from './references/components/referencesList';
import { FilterSelectboxComponent } from './references/components/filterSelectbox';
import { DetailsComponent } from './lpcreport/components/details';
import { DetailFormComponent } from './lpcreport/components/detailForm';
import { MapsComponent } from './maps/components/maps';

import { SharedModule } from '../shared/shared.module';

import appMenu from './menu';
import appRoutes from './routes';

@NgModule({
  imports: [
    SharedModule,
    Ng2TableModule,
    SelectModule,
    // DndModule.forRoot(),
    // InfiniteScrollModule,
    RouterModule.forRoot(appRoutes, { useHash: true }),
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyA8okzgfpEduXDLlebJtrgw6cmexiGNoN0'
    })
  ],
  declarations: [
    // HomeComponent,
    DiagnosticsComponent,
    ReferencesComponent,
    FilterSelectboxComponent,
    ReferencesListComponent,
    DetailsComponent,
    DetailFormComponent,
    MapsComponent,
  ],
  providers: [],
  exports: [
    // HomeComponent,
    DiagnosticsComponent,
    FilterSelectboxComponent,
    ReferencesComponent,
    ReferencesListComponent,
    DetailsComponent,
    MapsComponent
  ]
})

export class RoutesModule {
  constructor(private menu: MenuService) {
    menu.addMenu(appMenu);
  }
}
