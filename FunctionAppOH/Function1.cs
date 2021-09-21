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
    public static class FunctionGetProductId
    {

        [FunctionName("CreateRating")]
        public static async Task<IActionResult> CreateRating(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string payload  = req.Query["payload"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            payload = payload  ?? data?.payload ;

            string responseMessage = string.IsNullOrEmpty(payload )
                ? "This HTTP triggered function executed successfully. Pass a payload  in the query string or in the request body for a personalized response."
                : $"The product name for your product id {payload }";

            return new OkObjectResult(responseMessage);
        }
        [FunctionName("GetRating")]
        public static async Task<IActionResult> GetRating(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string ratingId = req.Query["ratingId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            ratingId = ratingId ?? data?.ratingId;

            string responseMessage = string.IsNullOrEmpty(ratingId)
                ? "This HTTP triggered function executed successfully. Pass a ratingId in the query string or in the request body for a personalized response."
                : $"The product name for your product id {ratingId}";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetRatings")]
        public static async Task<IActionResult> GetRatings(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string userId = req.Query["userId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            userId = userId ?? data?.userId;

            string responseMessage = string.IsNullOrEmpty(userId)
                ? "This HTTP triggered function executed successfully. Pass a userId in the query string or in the request body for a personalized response."
                : $"The product name for your product id {userId}";

            return new OkObjectResult(responseMessage);
        }
    }
}
