using App.Web.Models.Membership;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterClient(new TwitterAuthenticationClient(), "Twitter", new Dictionary<string,object>());
        /*    OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: "***********",
                consumerSecret: "****************************");
         */

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "*****************",
                appSecret: "****************************");

            //OAuthWebSecurity.RegisterGoogleClient();
        }

    }
}