/*
    Node.js UserEcho Single Sign-On code example v2.0
    Date: 2016-05-03
    
    Using: 
    Add your sso_key. Fill out data_json. Generate sso_token.
    Then use sso_token in your URL: http://[your_alias].userecho.com/?sso_token=sso_token
    OR in the JS widget:
    var _ues = {
        ... ,
    params:{sso_token:sso_token}
    };
*/

var crypto = require('crypto');

function generateRandomString(size){
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    for( var i=0; i < size; i++ )
        text += possible.charAt(Math.floor(Math.random() * possible.length));
    return text;
}

// your sso_key
var sso_key = '==========YOUR_SSO_KEY==========';
// Block size in AES encryption
var bs = 16; 
// prepare json data
var data_json = {
    expires:Math.round(new Date().getTime()/1000.0),
    guid:'123',
    display_name:'John Doe',
    email:'john.doe@test.com',
    locale:'en',
    avatar_url:'http://test.com/users/123/avatar.png',
    };
// stringify json
var raw = JSON.stringify(data_json);
// generate random iv
var iv = generateRandomString(bs);
// generate token    
var aes = crypto.createCipheriv('aes-256-cbc', sso_key, iv);
sso_token = aes.update(raw, 'utf-8', 'binary') + aes.final('binary');
// prepend iv and apply base64encode
sso_token = new Buffer(iv + sso_token, 'binary').toString('base64');
// escape characters for url
sso_token = encodeURIComponent(sso_token);

console.log(sso_token);