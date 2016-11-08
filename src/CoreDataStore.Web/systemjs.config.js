(function(global) {
  System.config({
    paths: {
      // paths serve as alias
      'libs:': 'node_modules/'
    },
    // map tells the System loader where to look for things
    map: {
      // our app is within the app folder
      app: '.tmp',

      // angular bundles
      '@angular/core': 'libs:@angular/core/bundles/core.umd.js',
      '@angular/common': 'libs:@angular/common/bundles/common.umd.js',
      '@angular/compiler': 'libs:@angular/compiler/bundles/compiler.umd.js',
      '@angular/platform-browser': 'libs:@angular/platform-browser/bundles/platform-browser.umd.js',
      '@angular/platform-browser-dynamic': 'libs:@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
      '@angular/http': 'libs:@angular/http/bundles/http.umd.js',
      '@angular/router': 'libs:@angular/router/bundles/router.umd.js',
      '@angular/forms': 'libs:@angular/forms/bundles/forms.umd.js',
      '@angular/upgrade': 'libs:@angular/upgrade/bundles/upgrade.umd.js',
      "@angular/core/src/facade/lang": "libs:@angular/core/src/facade/lang.js",
      // other libraries
      'rxjs': 'libs:rxjs',
      'angular2-in-memory-web-api': 'libs:angular2-in-memory-web-api',

      'moment': 'libs:moment/moment.js',
      'ng2-bootstrap': 'libs:ng2-bootstrap',
      'ng2-select': 'libs:ng2-select'

    },
    // packages tells the System loader how to load when no filename and/or no extension
    packages: {
      app: { main: './main.js', defaultExtension: 'js' },
      rxjs: { defaultExtension: 'js' },
      'ng2-select': { defaultExtension: 'js' },
      'ng2-bootstrap': { defaultExtension: 'js' },
      'angular2-in-memory-web-api': { main: './index.js', defaultExtension: 'js' }
    }
  });
})(this);