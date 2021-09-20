using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Core.Serialization;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using TorneioLeft4Dead2FunctionApp.Exceptions;

namespace TorneioLeft4Dead2FunctionApp.Extensions
{
    public static class HttpRequestDataExtensions
    {
        private static readonly JsonObjectSerializer Serializer = new(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        public static HttpResponseData Ok(this HttpRequestData httpRequest)
        {
            return httpRequest.CreateResponse(HttpStatusCode.OK);
        }

        public static async Task<HttpResponseData> OkAsync<T>(this HttpRequestData httpRequest, T data)
        {
            var response = httpRequest.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(data, Serializer);

            return response;
        }

        public static async Task<HttpResponseData> BadRequestAsync(this HttpRequestData httpRequest, Exception exception)
        {
            var friendlyResult = FriendlyResult.New(exception);

            return await httpRequest.BadRequestAsync(friendlyResult);
        }

        private static async Task<HttpResponseData> BadRequestAsync<T>(this HttpRequestData httpRequest, T data)
        {
            var response = httpRequest.CreateResponse(HttpStatusCode.BadRequest);

            await response.WriteAsJsonAsync(data, Serializer);

            response.StatusCode = HttpStatusCode.BadRequest;

            return response;
        }

        public static async Task<T> DeserializeBodyAsync<T>(this HttpRequestData httpRequest)
        {
            using var streamReader = new StreamReader(httpRequest.Body);
            var json = await streamReader.ReadToEndAsync();
            var t = JsonConvert.DeserializeObject<T>(json);

            return t;
        }
    }
}