<?
require_once('userecho_sso.php');
$sso_key = '==========YOUR_SSO_KEY==========';
$data_json = [
    'guid' 			=> '123',
    'display_name' 	=> 'John Doe',
    'email'			=>'john.doe@test.com',
    'locale'		=> 'en',
    'avatar_url'	=> 'http://test.com/users/123/avatar.png',
    ];

$ue = new UeSsoCipher();
$sso_token = $ue->encrypt($sso_key, $data_json);
echo $sso_token;
?>