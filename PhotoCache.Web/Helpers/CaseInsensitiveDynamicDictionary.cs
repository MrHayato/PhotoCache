using System.Dynamic;
using System.Linq;
using Nancy;

namespace PhotoCache.Web.Helpers
{
    public class CaseInsensitiveDynamicDictionary : DynamicDictionary
    {
        public CaseInsensitiveDynamicDictionary() { }
        public CaseInsensitiveDynamicDictionary(DynamicDictionary dict)
        {
            foreach (var name in dict.GetDynamicMemberNames())
                this[name] = dict[name];
        }
        public override bool TryGetMember(GetMemberBinder binder, out dynamic result)
        {
            base.TryGetMember(binder, out result);
            if (!result.HasValue)
            {
                var name = GetDynamicMemberNames().FirstOrDefault(x => x.ToLower() == binder.Name.ToLower());
                if (name != null)
                    result = this[name];
            }
            return true;
        }
    }
}