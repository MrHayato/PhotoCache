using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentValidation.Results;
using Nancy;
using Nancy.Responses;
using Nancy.Validation;
using Newtonsoft.Json;
using PhotoCache.Web.Models;

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

        public static Response Error(this IResponseFormatter formatter, string message, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            var error = new RestErrorModel { Messages = new List<string>(new []{ message }) };
            var jsonResponse = new JsonResponse<RestErrorModel>(error, new DefaultJsonSerializer()) { StatusCode = status };
            return jsonResponse;
        }

        public static Response Error(this IResponseFormatter formatter, IEnumerable<string> messages, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            var error = new RestErrorModel { Messages = new List<string>(messages) };
            var jsonResponse = new JsonResponse<RestErrorModel>(error, new DefaultJsonSerializer()) { StatusCode = status };
            return jsonResponse;
        }

        public static Response Error (this IResponseFormatter formatter, IEnumerable<ValidationFailure> messages, HttpStatusCode status = HttpStatusCode.BadRequest )
        {
            List<string> errors = messages.Select(validationFailure => validationFailure.ErrorMessage).ToList();
            return formatter.Error(errors, status);
        }

        public static T FromJson<T>(this Stream body)
        {
            var reader = new StreamReader(body, true);
            body.Position = 0;
            var value = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
        }

        public static T FromXml<T>(this Stream body)
        {
            return default(T);
        }
    }
}