var proxyMiddleware = require('http-proxy-middleware');
var fallbackMiddleware = require('connect-history-api-fallback');
var port = process.env.PORT || 8000;
var ApiEndpoint = process.env.APIENDPOINT || 'localhost';
var PortEndpoint = process.env.PORTENDPOINT || 5000;
module.exports = {
    port: port,
    files: ["wwwroot/**/*.{html,htm,css,js}"],
    server: {
        baseDir: "wwwroot",
        middleware: {
            1: proxyMiddleware('/api', {
                target: ['http://',ApiEndpoint,':',PortEndpoint].join(''),
                changeOrigin: false   
            }),       
            2: proxyMiddleware('/swagger', {
                target: ['http://',ApiEndpoint,':',PortEndpoint].join(''),
                changeOrigin: false   
            })           
        }
    }
};