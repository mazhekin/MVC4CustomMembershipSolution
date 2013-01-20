using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace App.Core
{
    public interface IDatabaseContext
    {
        IDbSet<UserProfile> UserProfiles { get; }

        void Save(UserProfile userProfile);
    }
}
