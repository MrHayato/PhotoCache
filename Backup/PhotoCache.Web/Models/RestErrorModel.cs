using System.Collections.Generic;

namespace PhotoCache.Web.Models
{
    public class RestErrorModel
    {
        public List<string> Messages { get; set; }
    }

    public class ValidationErrorModel
    {
        public Dictionary<string, string> Messages { get; set; } 
    }
}