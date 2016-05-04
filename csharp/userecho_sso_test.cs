using System;

class Program
{
    static void Main()
    {
        //Your SSO key, get it from http://yourproject.userecho.com/settings/features/sso/
        const string UE_SSO_KEY = "==========YOUR_SSO_KEY==========";

        string Token = UserEcho.UE_SSO.GetToken(new {
            guid = "12345", //User unique ID in your system
            display_name = "John Doe", //Will be displayed at the UserEcho
            email = "john.doe@test.com", //User email
            expires = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds+3600, //Expires in 1 hour (seconds from epoch time + 3600 seconds)
            locale = "en"   //Pass user locale here
            },UE_SSO_KEY);

        System.Console.WriteLine(Token);
    }
}
