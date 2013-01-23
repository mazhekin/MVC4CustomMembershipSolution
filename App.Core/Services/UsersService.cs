using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace App.Core.Services
{
    public interface IUsersService
    {
        UserProfile GetUserProfile(string userName);
        UserProfile GetUserProfile(int userId);
        void Save(UserProfile userProfile);

        OAuthMembership GetOAuthMembership(string provider, string providerUserId);
        void SaveOAuthMembership(string provider, string providerUserId, int userId);

        void Save(Membership membership, bool add);

        Membership GetMembership(int userId);
        Membership GetMembershipByConfirmToken(string token);
    }

    public class UsersService : IUsersService
    {
        private readonly IDatabaseContext db;

        public UsersService(IDatabaseContext db)
        {
            this.db = db;
        }

        UserProfile IUsersService.GetUserProfile(string userName)
        {
            return this.db.UserProfiles.FirstOrDefault(x => x.UserName.Equals(userName));
        }

        UserProfile IUsersService.GetUserProfile(int userId)
        {
            return this.db.UserProfiles.FirstOrDefault(x => x.UserId.Equals(userId));
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

        void IUsersService.SaveOAuthMembership(string provider, string providerUserId, int userId)
        {
            var oAuthMembership = this.db.OAuthMemberships.FirstOrDefault(x => x.Provider.Equals(provider) && x.ProviderUserId.Equals(providerUserId));
            if (oAuthMembership == null)
            {
                oAuthMembership = new OAuthMembership { Provider = provider, ProviderUserId = providerUserId };
                this.db.Add(oAuthMembership);
            }
            oAuthMembership.UserId = userId;
            this.db.SaveChanges();
        }

        Membership IUsersService.GetMembership(int userId)
        {
            return this.db.Memberships.FirstOrDefault(x => x.UserId == userId);
        }

        Membership IUsersService.GetMembershipByConfirmToken(string token)
        {
            return this.db.Memberships.FirstOrDefault(x => x.ConfirmationToken.Equals(token.ToLower()));
        }

        void IUsersService.Save(Membership membership, bool add)
        {
            if (add)
            {
                this.db.Add(membership);
            }
            this.db.SaveChanges();
        }
    }
}
