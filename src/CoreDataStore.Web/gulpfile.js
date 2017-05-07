/// <binding BeforeBuild='build' />
"use strict";

var fs = require('fs'),
  del = require('del'),
  path = require('path'),
  gulp = require('gulp'),
  iF = require('gulp-if'),
  less = require('gulp-less'),
  sass = require('gulp-sass'),
  concat = require('gulp-concat'),
  cssmin = require('gulp-cssmin'),
  tslint = require('gulp-tslint'),
  tsc = require('gulp-typescript'),
  replace = require('gulp-replace'),
  jsMinify = require('gulp-uglify'),
  ghPages = require('gulp-gh-pages'),
  tsconfig = require('gulp-ts-config'),
  coveralls = require('gulp-coveralls'),
  sourcemaps = require('gulp-sourcemaps'),
  cssPrefixer = require('gulp-autoprefixer'),
  merge = require('merge-stream');

var SystemBuilder = require('systemjs-builder');

var buildDir = "wwwroot";
var NG_ENVIRONMENT = process.env.NG_ENVIRONMENT || '';
var BUILD = process.env.BUILD || 'local';
var LANDMARK = process.env.LANDMARK || '/api/';
var ATTRACTION = process.env.ATTRACTION || '/api/attraction';
var MAPSAPI = process.env.MAPSAPI || '/api/maps';
var REPORTSAPI = process.env.REPORTSAPI || '/api/reports';

var build = false;
process.argv.forEach(function(val, index, array) {
  if ('build' === val) {
    build = true;
  }
});
/**
 * Remove build directory.
 */
gulp.task('clean', (cb) => {
  return del([buildDir, '.tmp'], cb);
});

gulp.task('ghpage', function() {
  return gulp.src('./' + buildDir + '/**/*')
    .pipe(ghPages());
});

/**
 * Compile all SASS files.
 */
// gulp.task('sass', function() {
//   return gulp.src('src/sass/app.scss')
//     .pipe(sass().on('error', sass.logError))
//     .pipe(cssmin())
//     .pipe(gulp.dest(path.join(buildDir, "css")));
// });

/**
 * Compile all Less files.
 */
gulp.task("less", function() {
  return gulp
    .src(["src/less/*.less", "src/less/themes/*.less"])
    .pipe(less())
    .pipe(cssmin())
    .pipe(gulp.dest(path.join(buildDir, "css")));
});

/**
 * Lint all custom TypeScript files.
 */
gulp.task('tslint', () => {
  return gulp.src("src/**/*.ts")
    .pipe(tslint({
      formatter: "verbose"
    }))
    .pipe(tslint.report());
});


/**
 * Compile TypeScript sources in build directory.
 */

gulp.task('shims', () => {
  return gulp.src([
      'node_modules/core-js/client/shim.min.js',
      'node_modules/zone.js/dist/zone.js',
      'node_modules/reflect-metadata/Reflect.js',
      'node_modules/systemjs/dist/system.src.js'
    ])
    .pipe(concat('shims.js'))
    .pipe(iF(build, jsMinify()))
    .pipe(gulp.dest(path.join(buildDir, 'js')));
});

gulp.task('tsc', ['tslint'], () => {
  var tsDest = (NG_ENVIRONMENT === 'Dev') ? (buildDir + '/app') : '.tmp';
  var tsProject = tsc.createProject('tsconfig.json'),
    tsResult = tsProject.src().pipe(tsProject());

  return tsResult.js.pipe(gulp.dest(tsDest));
});

gulp.task('compile', ['tsc'], () => {
  if (NG_ENVIRONMENT !== 'Dev') {
    var builder = new SystemBuilder();
    return builder.loadConfig('systemjs.build.js')
      .then(() => builder.buildStatic('app', path.join(buildDir, 'js', 'bundle.js')));
  }
  return;
});


/**
 * Copy all resources that are not TypeScript files into build directory.
 */
gulp.task("resources", () => {
  return gulp.src(["!src/index.html", "!src/less", "!src/less/**/*", "!**/*.ts", "src/**/*"])
    .pipe(gulp.dest(buildDir));
});


/**
 * Copy all required fonts into build directory.
 */
gulp.task('fonts', () => {
  return gulp.src([
      'bootstrap/fonts/**',
      'font-awesome/fonts/**',
      'simple-line-icons/fonts/**'
    ], { cwd: 'node_modules' })
    .pipe(gulp.dest(path.join(buildDir, 'fonts')));
});

/**
 * Watch for changes in TypeScript, HTML and CSS files.
 */
gulp.task('watch', () => {
  gulp.watch(['src/**/**.ts'], ['compile']).on('change', function(e) {
    console.log('TypeScript file ' + e.path + ' has been changed. Compiling.');
  });
  gulp.watch(['src/**/**.html', 'src/**/*.css', 'src/img/*.*'], ['resources']).on('change', function(e) {
    console.log('Resource file ' + e.path + ' has been changed. Updating.');
  });
  gulp.watch(['src/**/**.less'], ['less']).on('change', function(e) {
    console.log('LESS file ' + e.path + ' has been changed. Updating.');
  }); 
  // gulp.watch(['src/**/**.scss'], ['less']).on('change', function(e) {
  //   console.log('SASS file ' + e.path + ' has been changed. Updating.');
  // });
});

gulp.task('appsettings', function(cb) {
  var build = 'build: ' + BUILD;
  var ng2ENV = '\nng2ENV: ' + NG_ENVIRONMENT;
  var landmark = '\nApiEndpoint: ' + LANDMARK;
  var maps = '\nApiMaps: ' + MAPSAPI;
  var reports = '\nApiReports: ' + REPORTSAPI;
  var attraction = '\nApiAttraction: ' + ATTRACTION;
  return fs.writeFile('appsettings.yml', build + ng2ENV + landmark + maps + reports + attraction, cb);
});

gulp.task('api', function() {
  return gulp.src('appsettings.yml')
    .pipe(tsconfig('AppSettings', JSON.parse('{"parser": "yml"}')))
    .pipe(gulp.dest('./src/app/routes'))
});


gulp.task('bundle', function() {
  var bundleTpl;
  if (NG_ENVIRONMENT === 'Dev') {
    bundleTpl = '<script src="systemjs.config.js"></script>' +
      '<script>System.import(\'app\').catch(function(err) {console.error(err);});</script>';
  } else {
    bundleTpl = '<script type="text/javascript" src="js/bundle.js"></script>';
  }

  return gulp.src('src/index.html')
    .pipe(replace('<--bundleTpl-->', bundleTpl))
    .pipe(gulp.dest(buildDir));
});

gulp.task('systemjs', function() {
  if (NG_ENVIRONMENT === 'Dev') {
    return gulp.src('systemjs.config.js')
      .pipe(replace('.tmp', 'app'))
      .pipe(gulp.dest(buildDir));
  }
  return;
});

/**
 * Copy all required libraries into build directory.
 */
gulp.task("node_modules", () => {
  if (NG_ENVIRONMENT === 'Dev') {
    return gulp.src([
        'rxjs/**',
        'core-js/**',
        'zone.js/dist/**',
        'systemjs/dist/**',
        'ng2-select/**',
        'ng2-dnd/**',
        'angular2-infinite-scroll/**',
        'angular2-toaster/**',
        'angular2-google-maps/**',
        'ng2-table/**',
        'ngx-bootstrap/**',
        'screenfull/dist/screenfull.js',
        'jquery/dist/jquery.js',
        'jquery.browser/dist/jquery.browser.js',
        'moment/moment.js',
        '@angular/**'
      ], { cwd: "node_modules/**" }) /* Glob required here. */
      .pipe(gulp.dest(path.join(buildDir, "node_modules")));
  }
  return;
});

/**
 * Build the project.
 */
gulp.task("build", [
  'appsettings',
  'api',
  'compile',
  'shims',
  'less',
  // 'sass',
  'fonts',
  'resources',
  'node_modules',
  'systemjs',
  'bundle'
], () => {
  console.log('Building the project ...');
  if (NG_ENVIRONMENT !== 'Dev') {
    return gulp.src(path.join(buildDir, 'js', 'bundle.js'))
      .pipe(iF(build, jsMinify()))
      .pipe(gulp.dest(path.join(buildDir, 'js')));
  }
  return;

});
