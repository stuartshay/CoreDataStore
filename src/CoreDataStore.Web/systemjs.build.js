  System.config({
    defaultExtensions: 'js',
    module: "commonjs",
    paths: {
      // paths serve as alias
      'npm:': 'node_modules/'
    },
    // map tells the System loader where to look for things
    map: {
      // our app is within the app folder
      app: '.tmp',

      // angular bundles
      '@angular/core': 'npm:@angular/core/bundles/core.umd.js',
      '@angular/common': 'npm:@angular/common/bundles/common.umd.js',
      '@angular/compiler': 'npm:@angular/compiler/bundles/compiler.umd.js',
      '@angular/platform-browser': 'npm:@angular/platform-browser/bundles/platform-browser.umd.js',
      '@angular/platform-browser-dynamic': 'npm:@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
      '@angular/http': 'npm:@angular/http/bundles/http.umd.js',
      '@angular/router': 'npm:@angular/router/bundles/router.umd.js',
      '@angular/forms': 'npm:@angular/forms/bundles/forms.umd.js',
      '@angular/upgrade': 'npm:@angular/upgrade/bundles/upgrade.umd.js',
      "@angular/core/src/facade/lang": "npm:@angular/core/src/facade/lang.js",
      // other libraries
      'rxjs': 'npm:rxjs',
      'angular2-in-memory-web-api': 'npm:angular2-in-memory-web-api',

      'moment': 'npm:moment/moment.js',
      'jquery': 'npm:jquery/dist/jquery.js',
      'jquery.browser': 'npm:jquery.browser/dist/jquery.browser.js',
      'screenfull': 'npm:screenfull/dist/screenfull.js',
      'angular2-toaster': 'npm:angular2-toaster/bundles/angular2-toaster.umd.js',
      'angular2-google-maps/core': 'npm:angular2-google-maps/core/core.umd.js',
      // 'ngx-bootstrap': 'npm:ngx-bootstrap',
      'ngx-bootstrap': 'npm:ngx-bootstrap/bundles/ngx-bootstrap.umd.min.js',
      'ngx-bootstrap/*': 'npm:ngx-bootstrap/bundles/ngx-bootstrap.umd.min.js',
      'ngx-bootstrap/sortable.js': 'npm:ngx-bootstrap/sortable/index.js',
      'ng2-table': 'npm:ng2-table',
      'ng2-select': 'npm:ng2-select'

    },
    // packages tells the System loader how to load when no filename and/or no extension
    packages: {
      app: { main: 'main.js', defaultExtension: 'js' },
      rxjs: { defaultExtension: 'js' },
      'ng2-select': { defaultExtension: 'js' },
      'ng2-table': { defaultExtension: 'js' },
      'angular2-in-memory-web-api': { main: './index.js', defaultExtension: 'js' }
    }
  });
