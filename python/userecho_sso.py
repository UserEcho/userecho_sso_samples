"""
    Python UserEcho SinsgeSignOn code example v1.0
    Set $message values
    Using like:
    http://feedback.userecho.com/?sso_token=generated_sso_token_here
    or in widget:
    var _ues = {
    ... ,
    params:{sso_token=”generated sso token here”}
    };

"""

def get_sso_token(request):
    from Crypto.Cipher import AES
    from Crypto.Util import randpool
    from datetime import datetime, timedelta
    import hashlib
    import array
    import operator
    import base64
    import urllib
    import json

    message = {
    "guid" : "1234",
    "expires_date" : (datetime.utcnow()+timedelta(days=1)).strftime('%Y-%m-%d %H:%M:%S'),
    "display_name" : 'Test User Name',
    "email" : 'test@gmail.com',
    "locale" : "en",
    "avatar_url" : "http://test.com/users/1234/image.png",
    }

    sso_key = "YOUR_KEY"
    project_alias = "YOUR_ALIAS"

    iv  = randpool.RandomPool(AES.block_size).get_bytes(AES.block_size)
    key_hash = hashlib.sha1(sso_key+project_alias).digest()[:16]

    data_json = json.dumps(message)
    json_bytes = array.array('b', data_json[0 : len(data_json)])
    iv_bytes = array.array('b', iv[0 : len(iv)])

    # xor the iv into the first 16 bytes.
    for i in range(0, AES.block_size):
    	json_bytes[i] = operator.xor(json_bytes[i], iv_bytes[i])

    pad = AES.block_size - len(json_bytes.tostring()) % AES.block_size
    data = json_bytes.tostring() + (chr(pad) * pad)
    encrypted_bytes = AES.new(key_hash, AES.MODE_CBC, iv).encrypt(data)

    param_for_sso = urllib.quote(base64.b64encode(encrypted_bytes))

    return param_for_sso