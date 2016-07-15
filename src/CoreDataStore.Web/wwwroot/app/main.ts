import { bootstrap }    from '@angular/platform-browser-dynamic';
import { enableProdMode, provide } from '@angular/core';
import { AppComponent } from './app.component';
//import {ROUTER_PROVIDERS} from '@angular/router'

import {LocationStrategy, 
        HashLocationStrategy,
        APP_BASE_HREF, } from '@angular/common'

if (window['g_AngularProdMode']) {
	enableProdMode();
}

bootstrap(AppComponent,[
    // ROUTER_PROVIDERS,
     provide(APP_BASE_HREF, { useValue: '/' }),
     provide(LocationStrategy, {useClass : HashLocationStrategy})
     ]);

