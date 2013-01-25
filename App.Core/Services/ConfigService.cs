using App.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Core.Services
{
    public sealed class ConfigName
    {

        private readonly String name;

        public static readonly ConfigName WebsiteUrlName = new ConfigName("WebsiteUrlName");
        public static readonly ConfigName WebsiteTitle = new ConfigName("WebsiteTitle");
        public static readonly ConfigName WebsiteUrl = new ConfigName("WebsiteUrl");

        private ConfigName(String name)
        {
            this.name = name;
        }

        public override String ToString()
        {
            return name;
        }

    }

    public interface IConfigService
    {
        string GetValue(ConfigName name);
        IDictionary<string, string> GetValues(ConfigName[] configNames);
    }

    public class ConfigService : IConfigService
    {
        private readonly IDatabaseContext db;

        public ConfigService(IDatabaseContext db)
        {
            this.db = db;
        }

        string IConfigService.GetValue(ConfigName name)
        {
            var config = this.db.Configs.FirstOrDefault(x => x.Key.Equals(name.ToString()));
            if (config != null)
            {
                return config.Value;
            }
            return null;
        }

        IDictionary<string, string> IConfigService.GetValues(ConfigName[] configNames)
        {
            var names = configNames.Select(x => x.ToString());
            var configs = this.db.Configs.Where(x => names.Contains(x.Key));

            return configs.ToDictionary(x => x.Key, x => x.Value);
        }

    }
}
