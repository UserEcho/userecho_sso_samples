How to Use:

	require_once('userecho_sso.php');
	
	// replace key with your key from Project settings -> Integrations -> Single Sign-On
	$sso_key = '==========YOUR_SSO_KEY==========';
	
	// prepare json data
	$data_json = [
	    'guid' 			=> '123',
	    'display_name' 	=> 'John Doe',
	    'email'			=>'john.doe@test.com',
	    'locale'		=> 'en',
	    'avatar_url'	=> 'http://test.com/users/123/avatar.png',
	    ];

	// generate token    
	$sso_token = new UeSsoCipher()->encrypt($sso_key, $data_json);
	
	echo $sso_token