"""
    Python UserEcho Single Sign-On code example v2.0
    Date: 2016-05-03
    
    Using: 
    Check test.py to generate sso_token
    Then use in your URL: http://[your_alias].userecho.com/?sso_token=sso_token
    OR in the JS widget:
    var _ues = {
        ... ,
    params:{sso_token:sso_token}
    };
"""
import base64
import urllib
import time
import json
from Crypto.Cipher import AES

class UeSsoCipher():
    bs = AES.block_size
    mode = AES.MODE_CBC

    def __init__(self, key):
        self.key = key

    def _pad(self, s):
        pad = self.bs - len(s) % self.bs
        return s if pad == self.bs else s + (pad) * chr(pad)

    def encrypt(self, data_json):
        from Crypto.Util import randpool
        # add expiration date in UTC timestamp from Epoch
        if not 'expires' in data_json:
            # add expires in 1 hour
            data_json['expires'] = int(time.time()) + 3600
        # dump JSON data to string add padding if needed
        raw = self._pad(json.dumps(data_json))
        # generate IV
        iv = randpool.RandomPool(self.bs).get_bytes(self.bs)
        # encrypt data and prepend IV
        res = base64.b64encode(iv + AES.new(self.key, self.mode, iv).encrypt(raw)) 
        # quote data to use as an url parameter and return
        return urllib.quote(res)