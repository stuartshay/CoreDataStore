// export NG_ENVIRONMENT=Dev
// export LANDMARK=http://informationcart.eastus2.cloudapp.azure.com:80/api/
// export MAPSAPI=http://informationcart.eastus2.cloudapp.azure.com:82/api/

const LANDMARK = process.env.LANDMARK || 'http://informationcart.eastus2.cloudapp.azure.com:80/api/';
const MAPSAPI = process.env.MAPSAPI || 'http://informationcart.eastus2.cloudapp.azure.com:82/api/';
const NG_ENVIRONMENT = process.env.NG_ENVIRONMENT || '';

var path = require('path');
var express = require('express');
var port = process.env.PORT || 3000;
var app = express();
global.base = __dirname;

// view engine setup
app.set('views', path.join(__dirname, 'wwwroot'));
app.set('view engine', 'jade');

app.use(express.static(path.join(__dirname, 'wwwroot')));

/* GET home page. */
app.get('/', function(req, res, next) {
  res.render('index', {
    'LANDMARK': LANDMARK,
    'MAPSAPI': MAPSAPI,
    'ng2ENV': NG_ENVIRONMENT
  });
});

// catch 404 and forward to error handler
app.use(function(req, res, next) {
  var err = new Error('Not Found');
  err.status = 404;
  next(err);
});

// error handler
app.use(function(err, req, res, next) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  // render the error page
  res.status(err.status || 500);
  res.render('error');
});

app.listen(port, '0.0.0.0', function() {
  console.log('Listening on port %d', port);
});
