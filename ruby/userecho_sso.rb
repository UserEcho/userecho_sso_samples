=begin
    Ruby UserEcho Single Sign-On code example v2.0
    Date: 2016-05-03
    
    Using: 
    Add your sso_key. Fill out data_json. Generate sso_token.
    Then use in your URL: http://[your_alias].userecho.com/?sso_token=sso_token
    OR in the JS widget:
    var _ues = {
        ... ,
    params:{sso_token:sso_token}
    };
=end
require 'openssl'
require 'json'
require "base64"
require 'cgi'

# your sso_key
sso_key = '==========YOUR_SSO_KEY=========='
# prepare json data
data_json = {
    :expires => Time.now.to_i+3600,
    :guid => '123',
    :display_name => 'John Doe',
    :email => 'john.doe@test.com',
    :locale => 'en',
    :avatar_url => 'http://test.com/users/123/avatar.png',
    }
# dump json to string
raw = JSON.dump(data_json)
# create cipher for encryption
cipher = OpenSSL::Cipher.new('AES-256-CBC')
cipher.encrypt
cipher.key = sso_key
# generate random iv
iv = cipher.random_iv
# generate token
sso_token = cipher.update(raw) + cipher.final
# prepend iv and apply base64encode
sso_token = Base64.strict_encode64(iv + sso_token)
# escape characters for url
sso_token = CGI.escape(sso_token)
puts sso_token