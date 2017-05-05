import { LayoutComponent } from '../layout/layout.component';

// import { HomeComponent } from './home/home.component';
import { DiagnosticsComponent } from './diagnostics/components/diagnostics';
import { ReferencesComponent } from './references/components/references';
import { DetailsComponent } from './lpcreport/components/details';
import { MapsComponent } from './maps/components/maps';


const routes = [

  {
    path: '',
    component: LayoutComponent,
    children: [
      // { path: 'home', component: HomeComponent },
      { path: 'diagnostics', component: DiagnosticsComponent },
      { path: 'references', component: ReferencesComponent },
      { path: 'details/:id', component: DetailsComponent },
      { path: 'maps', component: MapsComponent },
      { path: '**', redirectTo: 'references' }
    ]

  },

  // Not found
  { path: '**', redirectTo: 'references' }

];

export default routes;
