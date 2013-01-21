using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace App.Core
{
    public partial class Entities : IDatabaseContext
    {
        IDbSet<UserProfile> IDatabaseContext.UserProfiles
        {
            get { return this.UserProfiles; }
        }

        IDbSet<OAuthMembership> IDatabaseContext.OAuthMemberships
        {
            get { return this.OAuthMemberships; }
        }
                    
        void IDatabaseContext.Add(UserProfile userProfile)
        {
            this.Set<UserProfile>().Add(userProfile);
        }

        void IDatabaseContext.Add(OAuthMembership oAuthMembership)
        {
            this.Set<OAuthMembership>().Add(oAuthMembership);
        }

        void IDatabaseContext.SaveChanges()
        {
            this.SaveChanges();
        }
    }
}
