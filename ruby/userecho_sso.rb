# Ruby UserEcho SingleSignOn code example v1.2
# tested with ruby 1.9.3
require 'cgi'
require 'ezcrypto'
require 'json'

sso_key = 'YOUR_KEY'
project_alias = 'YOUR_ALIAS'
url = 'url'

message = {
  :guid => '<id>',
  :expires_date => (Time.now + 3600*24).strftime("%Y-%m-%d %H:%M:%S"),
  :email => '<email>',
  :display_name => '<display name>',
  :locale => 'en',
  :avatar_url => 'http://test.com/1234.png'
}

message_json = JSON.dump(message)

key = EzCrypto::Key.with_password project_alias, sso_key
encrypted_bytes = key.encrypt(message_json)

puts url+"/?sso_token=" + CGI.escape([encrypted_bytes].pack('m0')).gsub('+','%2B')