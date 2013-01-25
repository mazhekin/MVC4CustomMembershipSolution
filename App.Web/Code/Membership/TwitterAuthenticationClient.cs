using DotNetOpenAuth.AspNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Code.Membership
{
    public class TwitterAuthenticationClient : IAuthenticationClient
    {
        string IAuthenticationClient.ProviderName
        {
            get { return "twitter"; }
        }

        void IAuthenticationClient.RequestAuthentication(HttpContextBase context, Uri returnUrl)
        {
            context.Response.Redirect("http://www.twitter.com");   
        }

        AuthenticationResult IAuthenticationClient.VerifyAuthentication(HttpContextBase context)
        {
            throw new NotImplementedException();
        }
    }
}