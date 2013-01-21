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

        IDbSet<OAuthMembership> OAuthMemberships { get; }

        void Add(UserProfile userProfile);

        void Add(OAuthMembership oAuthMembership);

        void SaveChanges();
    }
}
