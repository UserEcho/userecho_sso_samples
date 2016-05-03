<?php
/*
    PHP UserEcho SingleSignOn code example v1.0
    Set $message values
    Using like:
    echo "http://feedback.userecho.com/?sso_token=".get_sso_token();
    or in widget:
    var _ues = {
    ... ,
    params:{sso_token=”generated sso token here”}
    };

*/

function get_sso_token()
{
$sso_key = "YOUR_KEY"; // Your project personal api key
$project_alias = "YOUR_ALIAS"; // Your project alias

$message = array(
    "guid" => "12345", // User ID in your system - using for identify user in next time (first time auto-registion)
    "expires_date" => gmdate("Y-m-d H:i:s", time()+(86400)), // sso_token expiration date in format "Y-m-d H:i:s". Recommend set date now() + 1 day
    "display_name" => "My Name Example", // User display name in your system
    "email" => "test@test.com", // User email - using for notification about changes on topic
    "locale" => "en", // Default user language
    "avatar_url" => "http://test.com/1234.png" // NOT USED NOW. user avatar URL
    );

// random bytes value, length = 16
// Recommend use random to genetare $iv
$iv  = 'testTEST1234QWER';

// key hash, length = 16
$key_hash = substr( hash('sha1', $sso_key.$project_alias, true), 0, 16);
// if you use mb_string functions, try it  
//$key_hash = mb_substr( hash('sha1', $sso_key.$project_alias, true), 0, 16, 'Windows-1251')

$message_json = json_encode($message);

// double XOR first block message_json
for ($i = 0; $i < 16; $i++)
 $message_json[$i] = $message_json[$i] ^ $iv[$i];

// fill tail of message_json by bytes equaled count empty bytes (to 16)
$pad = 16 - (strlen($message_json) % 16);
$message_json = $message_json . str_repeat(chr($pad), $pad);

// encode json
$cipher = mcrypt_module_open(MCRYPT_RIJNDAEL_128,'','cbc','');
mcrypt_generic_init($cipher, $key_hash, $iv);
$encrypted_bytes = mcrypt_generic($cipher,$message_json);
mcrypt_generic_deinit($cipher);

// encode bytes to url safe string
return urlencode(base64_encode($encrypted_bytes));
}
?>