using System;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //Your API & PROJECT keys, get it from http://yourproject.userecho.com/settings/features/sso/
            const string UE_API_KEY = "your api key";
            const string UE_PROJECT_KEY = "your project key";
    
            string Token = UserEcho.UE_SSO.GetToken(new {
                guid = "test.user@justfortest.com", //User unique ID in your system
                display_name = "Test User", //Will be displayed at the UserEcho
                email = "test.user@justfortest.com", //User email
                expires_date = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss"), //Expires date in UTC 1 Day from current date in the example
                locale = "en"   //Pass user locale here
                },UE_API_KEY,UE_PROJECT_KEY);

            System.Console.WriteLine(Token);
        }
    }
}