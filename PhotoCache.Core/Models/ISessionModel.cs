using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoCache.Core.Models
{
    interface ISessionModel
    {
        string SessionKey { get; set; }
        DateTime? Expiry { get; set; }
    }
}
