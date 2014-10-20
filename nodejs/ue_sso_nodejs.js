var crypto = require('crypto');

//Get params from your settings
//https://[Your project alias].userecho.com/settings/features/sso/
var subdomain = 'Fill your PROJECT_KEY alias';
var key = 'Fill your API_KEY';

var message = {
  guid:'12345', // User ID in your system - using for identify user in next time (first time auto-registion)
  expires_date:'2015-01-01 00:03:47', // sso_token expiration date in format "Y-m-d H:i:s". Recommend set date now() + 1 day
  display_name:'My Name Example', // User display name in your system
  email:'test@test.com', // User email - using for notification about changes on feedback
  locale:'en', // Default user language
  avatar_url:'http://test.com/1234.png' // NOT USED NOW. user avatar URL
}

var iv = Buffer("testTEST1234QWER", 'binary');

var json = JSON.stringify(message);

var salted = Buffer(
      crypto.createHash('sha1')
        .update(key + subdomain, 'utf8')
        .digest()
      , 'binary'
    );

var xored = Buffer(json, 'binary');

// xor the iv into the first 16 bytes.
for (var i = 0; i < 16; xored[i] = xored[i] ^ iv[i++]);

var aes = crypto.createCipheriv('aes128', salted.slice(0, 16), iv),
    token = '';

token += aes.update(xored.toString('binary'), 'binary', 'base64');
token += aes.final('base64');

token = encodeURIComponent(token);

console.log('UserEcho URL with SSO TOKEN: http://%s.userecho.com?sso_token=%s', subdomain, token);  