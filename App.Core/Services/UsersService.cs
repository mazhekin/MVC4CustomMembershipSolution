using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Core.Services
{
    public interface IUsersService
    {
        void Save(UserProfile userProfile);

        OAuthMembership GetOAuthMembership(string provider, string providerUserId);
    }

    public class UsersService : IUsersService
    {
        private readonly IDatabaseContext db;

        public UsersService(IDatabaseContext db)
        {
            this.db = db;
        }

        void IUsersService.Save(UserProfile userProfile)
        {
            if (userProfile.UserId == 0)
            {
                this.db.Add(userProfile);
            }
            this.db.SaveChanges();
        }

        OAuthMembership IUsersService.GetOAuthMembership(string provider, string providerUserId)
        {
            return this.db.OAuthMemberships.FirstOrDefault(x => x.Provider.Equals(provider) && x.ProviderUserId.Equals(providerUserId));
        }
    }
}
