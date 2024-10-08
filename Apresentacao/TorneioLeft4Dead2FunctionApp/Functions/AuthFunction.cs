using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Auth.Commands;
using TorneioLeft4Dead2.Auth.Services;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions;

public class AuthFunction(IAuthService authService)
{
    [Function($"{nameof(AuthFunction)}_{nameof(AuthAsync)}")]
    public async Task<HttpResponseData> AuthAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth")] HttpRequestData httpRequest)
    {
        try
        {
            var command = await httpRequest.DeserializeBodyAsync<LoginCommand>();
            var entity = await authService.AuthAsync(command);

            return await httpRequest.OkAsync(entity);
        }
        catch (Exception exception)
        {
            return await httpRequest.BadRequestAsync(exception);
        }
    }
}