using App.Core.Data;
using App.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;

namespace App.Core.Services
{
    public interface IUsersService
    {
        UserProfile GetUserProfile(string userName);
        UserProfile GetUserProfile(int userId);
        void Save(UserProfile userProfile);

        OAuthMembership GetOAuthMembership(string provider, string providerUserId);
        void SaveOAuthMembership(string provider, string providerUserId, int userId);

        void Save(App.Core.Data.Membership membership, bool add);

        App.Core.Data.Membership GetMembership(int userId);
        App.Core.Data.Membership GetMembershipByConfirmToken(string token, bool withUserProfile);

        void SendAccountActivationMail(string email);
    }

    public class UsersService : IUsersService
    {
        private readonly IDatabaseContext db;
        private readonly IConfigService configService;
        private readonly IEmailService emailService;

        public UsersService(IDatabaseContext db, IConfigService configService, IEmailService emailService)
        {
            this.db = db;
            this.configService = configService;
            this.emailService = emailService;
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

        App.Core.Data.Membership IUsersService.GetMembership(int userId)
        {
            return this.db.Memberships.FirstOrDefault(x => x.UserId == userId);
        }

        App.Core.Data.Membership IUsersService.GetMembershipByConfirmToken(string token, bool withUserProfile)
        {
            var membership = this.db.Memberships.FirstOrDefault(x => x.ConfirmationToken.Equals(token.ToLower()));
            if (membership != null && withUserProfile)
            {
                membership.UserProfile = this.db.UserProfiles.First(x => x.UserId == membership.UserId);
            }
            return membership;
        }

        void IUsersService.Save(App.Core.Data.Membership membership, bool add)
        {
            if (add)
            {
                this.db.Add(membership);
            }
            this.db.SaveChanges();
        }

        // to do: transfer it to service
        void IUsersService.SendAccountActivationMail(string email)
        {
            var userProfile = (this as IUsersService).GetUserProfile(email);
            if (userProfile == null)
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.ProviderError);
            }

            var membership = (this as IUsersService).GetMembership(userProfile.UserId);
            if (membership == null)
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.ProviderError);
            }

            var configValues = this.configService.GetValues(new ConfigName[] { ConfigName.WebsiteUrlName, ConfigName.WebsiteTitle, ConfigName.WebsiteUrl });
            var viewData = new ViewDataDictionary { Model = userProfile };
            viewData.Add("Membership", membership);
            this.emailService.SendEmail(
                new SendEmailModel
                {
                    EmailAddress = email,
                    Subject = configValues[ConfigName.WebsiteUrlName.ToString()] + ": Confirm your registration",
                    WebsiteUrlName =  configValues[ConfigName.WebsiteUrlName.ToString()],
                    WebsiteTitle = configValues[ConfigName.WebsiteTitle.ToString()],
                    WebsiteURL = configValues[ConfigName.WebsiteUrl.ToString()]
                },
                "ConfirmRegistration",
                viewData
                );
        }

    }
}
