using Nancy;
using PhotoCache.Core.Models;
using PhotoCache.Core.Persistence;

namespace PhotoCache.Web.Modules.APIModules
{
    public class SecureAPIModule : BaseAPIModule
    {
        public const string SessionItemKey = "Session";
        private readonly IRavenRepository<SessionModel> _sessions;

        public SecureAPIModule(IRavenRepository<SessionModel> sessions)
        {
            _sessions = sessions;

            Before += Authenticate;
        }

        private Response Authenticate(NancyContext context)
        {
            if (context.Items.ContainsKey(SessionItemKey))
            {
                //Does the session exist?
                var session = (SessionModel)context.Items[SessionItemKey];
                var storedSession = _sessions.Load(session.Id);

                if (storedSession != null)
                {
                    return null; //All clear
                }

                context.Items.Remove(SessionItemKey);
            }

            return Response.AsRedirect("~/");
        }
    }
}