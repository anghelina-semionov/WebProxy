using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuAPI.Settings
{
   public interface ISyncServiceSettings
    {
        public string Host { get; set; }

        public string UpsertHttpMethod { get; set; }

        public string DeleteHttpMethod { get; set; }
    }
}
