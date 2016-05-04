<?php
/*
    PHP UserEcho Single Sign-On code example v2.0
    Date: 2016-05-03
    
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
    const BLOCK_SIZE = 16;

    /*
        Generate sso_token
        @param  $key - your sso_key
        @param  $data_json - prepared data in json format
        @return string
     */
    public function encrypt($key, $data_json)
    {
        // add expires if does not exist
        if (!array_key_exists('expires',$data_json))
        {
            # add 1 hour
            $data_json['expires'] = time()+3600;
        }

        $iv = $this->getRandomString(self::BLOCK_SIZE);
        $raw = $this->pad(json_encode($data_json));
        $cipher = mcrypt_module_open(MCRYPT_RIJNDAEL_128, '', 'cbc', '');
        mcrypt_generic_init($cipher, $key, $iv);
        $encryptedBytes = mcrypt_generic($cipher, $raw);
        mcrypt_generic_deinit($cipher);
        return urlencode(base64_encode($iv . $encryptedBytes));
    }

    /* Padding string */
    private function pad($raw)
    {
        $pad = self::BLOCK_SIZE - (strlen($raw) % self::BLOCK_SIZE);
        return ($pad == self::BLOCK_SIZE)? $raw : $raw . str_repeat(chr($pad), $pad);
    }

    private function getRandomString($length)
    {
        $str       = 'abcdefjhigklmnopqrstuvwzxyABCDEFGHJKLMNPQRSTUVWXYZ123456789';
        $strLength = strlen($str);
        $res       = '';
        for ($i = 0; $i < $length; $i++) {
            $res .= $str[rand(0, $strLength - 1)];
        }
        return $res;
    }
}
?>