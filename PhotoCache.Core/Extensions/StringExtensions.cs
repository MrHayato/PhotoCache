using System.Text;

namespace PhotoCache.Core.Extensions
{
    public static class StringExtensions
    {
        public static string GetSha1Hash(this string value)
        {
            var encoding = new UTF8Encoding();
            var hash = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            var hashed = hash.ComputeHash(encoding.GetBytes(value));
            return encoding.GetString(hashed);
        }
    }
}