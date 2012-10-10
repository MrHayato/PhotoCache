using System;
using System.Linq;
using System.Security.Cryptography;
using PhotoCache.Core.ReadModels;

namespace PhotoCache.Core.Models
{
    public class SessionModel : IModel, ISessionModel
    {
        private const int ExpiryDays = 1; //Number of days a user stays logged in without the "remember me" option

        public Guid Id { get; set; }
        public string SessionKey { get; set; }
        public DateTime? Expiry { get; set; }

        public SessionModel(bool expires)
        {
            if (expires)
                RenewExpiry();
            else
                Expiry = null;

            SessionKey = GenerateSessionKey();
        }

        public void RenewExpiry()
        {
            Expiry = DateTime.Now.AddDays(ExpiryDays);
        }

        public string GenerateSessionKey()
        {
            var rngProvider = new RNGCryptoServiceProvider();
            var key = new byte[48];
            rngProvider.GetBytes(key);
            
            string sessionKey = "";
            key.ToList().ForEach(b => sessionKey += b.ToString("x2"));

            return sessionKey;
        }
    }
}
