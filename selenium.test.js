var username = process.env.SAUCE_USERNAME || '';
var accessKey = process.env.SAUCE_ACCESS_KEY || '';
var TRAVIS_JOB_NUMBER = process.env.TRAVIS_JOB_NUMBER;

const { Builder, By, until } = require('selenium-webdriver');
var assert = require('assert');
var urlSaucelabs, browser;

if (username && accessKey) {
  urlSaucelabs = ['http://', username, ':', accessKey, '@ondemand.saucelabs.com:80/wd/hub'].join('');
  browser = new Builder().
  withCapabilities({
    'browserName': 'safari',
    'platform': 'iOS 10.2',
    'version': '10.0.',
    'device': 'iPad Simulator',
    'username': username,
    'accessKey': accessKey,
    'tunnelIdentifier': TRAVIS_JOB_NUMBER
  }).
  usingServer(urlSaucelabs).
  build();

  browser.get('http://127.0.0.1:3000/');

  browser.wait(findLink('#/references'), 2000).then(clickLink);
  browser.wait(findLink('#/maps'), 2000).then(clickLink);

  browser.wait(findLink('#/diagnostics'), 2000).
  then(clickLink).
  then(logTitle).
  then(closeBrowser, handleFailure);

  // describe('testing javascript in the browser', function() {
  //   beforeEach(function() {
  //     return browser.get('http://127.0.0.1:3000/');
  //   });

  //   afterEach(function() {
  //     return closeBrowser();
  //   });

  //   it('test title', function(done) {
  //     browser.getTitle().then(function(title) {
  //       assert.equal(title, 'CoreDataStore');
  //       done();
  //     });
  //   });
  // });

}

function logTitle() {
  browser.getTitle().then(function(title) {
    console.log('Current Page Title: ' + title);
  });
}


function clickLink(link) {
  link.click();
}

function findLink(link) {
  link = ['[href="', link, '"]'].join('');
  return browser.findElements(By.css(link)).then((res) => {
    return res[0];
  });
}

function closeBrowser() {
  browser.quit();
}

function handleFailure(err) {
  console.error('Something went wrong\n', err.stack, '\n');
  closeBrowser();
}
