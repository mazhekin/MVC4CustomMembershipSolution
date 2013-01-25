using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public interface IUnityContainerAccessor
    {
        IUnityContainer Container { get; }
    }
}