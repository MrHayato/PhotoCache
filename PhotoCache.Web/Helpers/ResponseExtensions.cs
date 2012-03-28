using Nancy;

namespace PhotoCache.Web.Helpers
{
    public static class ResponseExtensions
    {
        public static Response Created(this Response response)
        {
            response.StatusCode = HttpStatusCode.Created;
            return response;
        }

        public static Response Accepted(this Response response)
        {
            response.StatusCode = HttpStatusCode.Accepted;
            return response;
        }

        public static Response Ok(this Response response)
        {
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}