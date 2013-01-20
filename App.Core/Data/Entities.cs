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

        void IDatabaseContext.Save(UserProfile userProfile)
        {
            try
            {
                if (userProfile.UserId == 0)
                {
                    this.Set<UserProfile>().Add(userProfile);
                }
                this.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var err in ex.EntityValidationErrors)
                {
                    var test = err;
                }
            }
        }
    }
}
