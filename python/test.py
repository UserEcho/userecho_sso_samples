from userecho_sso import UeSsoCipher
sso_key = '==========YOUR_SSO_KEY=========='
data_json = {
    'guid' : '123',
    'display_name' : 'John Doe',
    'email' : 'john.doe@test.com',
    'locale' : 'en',
    'avatar_url' : 'http://test.com/users/123/avatar.png',
    }
    
sso_token = UeSsoCipher(key=sso_key).encrypt(data_json)
print sso_token