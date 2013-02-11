using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace App.Core.Data
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

        void IDatabaseContext.Add(Membership membership)
        {
            this.Set<Membership>().Add(membership);
        }

        IDbSet<Membership> IDatabaseContext.Memberships 
        {
            get { return this.Memberships; } 
        }

        IDbSet<Config> IDatabaseContext.Configs
        {
            get { return this.Configs; }
        }

        void IDatabaseContext.SaveChanges()
        {
            try
            {
                this.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var err in ex.EntityValidationErrors)
                {
                }
                throw ex;
            }
        }

        IDbSet<Role> IDatabaseContext.Roles 
        {
            get { return this.Roles; } 
        }

    }
}
