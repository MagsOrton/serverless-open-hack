using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppOH
{
    public static class FunctioGetRatings
    {

        [FunctionName("GetRatings")]
        public static async Task<IActionResult> GetRatings(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
            [CosmosDB(
                 databaseName: "my-database",
                collectionName: "my-container",
                ConnectionStringSetting = "CosmosDbConnectionString")]IAsyncCollector<dynamic> documentsOut,
            ILogger log)
        {


            log.LogInformation("C# HTTP trigger function processed a request.");

            string userId  = req.Query["userId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            userId = userId  ?? data?.userId ;

            string responseMessage = string.IsNullOrEmpty(userId )
                ? "This HTTP triggered function executed successfully. Pass a userId in the query string or in the request body for a personalized response."
                : $"The userId {userId }";

            return new OkObjectResult(responseMessage);
        }
    }
}
