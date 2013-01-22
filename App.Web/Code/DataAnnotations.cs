using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App.Web.Code
{
    public struct RegularExpressions
    {
        public static string EmailAddress = @"^[a-zA-Z0-9_\+-]+(\.[a-zA-Z0-9_\+-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.([a-zA-Z]{2,4})$";
    }

    public class ValidEmailAddressAttribute : RegularExpressionAttribute
    {
        public ValidEmailAddressAttribute() : base(RegularExpressions.EmailAddress) { }
    }

}