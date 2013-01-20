using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Core.Services
{
    public interface IMembershipService
    {
        void Save(UserProfile userProfile);
    }

    public class MembershipService : IMembershipService
    {
        private readonly IDatabaseContext db;

        public MembershipService(IDatabaseContext db)
        {
            this.db = db;
        }

        void IMembershipService.Save(UserProfile userProfile)
        {
            this.db.Save(userProfile);
        }
    }
}
