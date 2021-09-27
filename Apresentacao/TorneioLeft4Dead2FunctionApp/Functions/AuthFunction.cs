using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Auth.Commands;
using TorneioLeft4Dead2.Auth.Services;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class AuthFunction
    {
        private readonly IAuthService _authService;

        public AuthFunction(IAuthService authService)
        {
            _authService = authService;
        }

        [Function(nameof(AuthFunction) + "_" + nameof(Auth))]
        public async Task<HttpResponseData> Auth([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth")] HttpRequestData httpRequest)
        {
            try
            {
                var command = await httpRequest.DeserializeBodyAsync<LoginCommand>();
                var entity = await _authService.AuthAsync(command);

                return await httpRequest.OkAsync(entity);
            }
            catch (Exception exception)
            {
                return await httpRequest.BadRequestAsync(exception);
            }
        }
    }
}