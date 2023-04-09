using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions;

public class PingFunction
{
    private static readonly Guid InstanceId = Guid.NewGuid();

    [Function(nameof(PingFunction) + "_" + nameof(GetAsync))]
    public async Task<HttpResponseData> GetAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ping")] HttpRequestData httpRequest)
    {
        return await httpRequest.OkAsync(new
        {
            instanceId = InstanceId,
            now = DateTime.Now
        });
    }
}