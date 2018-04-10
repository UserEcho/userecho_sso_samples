<?php
/*
    PHP UserEcho Single Sign-On code example v3.0
    Date: 2018-04-10
    Comment: mcrypt methods replaced by openssl
    
    Using: 
    Check test.php to generate sso_token
    Then use in your URL: http://[your_alias].userecho.com/?sso_token=sso_token
    OR in the JS widget:
    var _ues = {
        ... ,
    params:{sso_token:sso_token}
    };
*/

class UeSsoCipher
{
    const CIPHER = "AES-256-CBC";
    /*
        Generates sso_token
        @param  $key - your sso_key
        @param  $data_json - prepared data in json format
        @returns string
     */
    public function encrypt($key, $data_json)
    {
        // add expires if does not exist
        if (!array_key_exists('expires',$data_json)){
            # add 1 hour
            $data_json['expires'] = time()+3600;
        }
        $iv = openssl_random_pseudo_bytes(openssl_cipher_iv_length($cipher=self::CIPHER));
        $ciphertext_raw = openssl_encrypt(json_encode($data_json), self::CIPHER, $key, $options=OPENSSL_RAW_DATA, $iv);
        return urlencode(base64_encode($iv . $ciphertext_raw));
    }
}
?>