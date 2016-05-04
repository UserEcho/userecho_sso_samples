How to Use:

	from userecho_sso import UeSsoCipher
	
	# replace key with your key from Project settings -> Integrations -> Single Sign-On
	sso_key = '==========YOUR_SSO_KEY=========='
	
	# prepare json data
	data_json = {
	    'guid' : '123',
	    'display_name' : 'John Doe',
	    'email' : 'john.doe@test.com',
	    'locale' : 'en',
	    'avatar_url' : 'http://test.com/users/123/avatar.png',
	    }
	
	# generate token    
	sso_token = UeSsoCipher(key=sso_key).encrypt(data_json)
	
	print sso_token