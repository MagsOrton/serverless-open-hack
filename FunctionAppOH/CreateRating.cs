using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using RestSharp;

namespace FunctionAppOH
{
    public static class FunctioCreateRating
    {

        [FunctionName("CreateRating")]
        public static async Task<IActionResult> CreateRating(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName:  "%databaseName%", collectionName: "%collectionName%",
            ConnectionStringSetting = "CosmosDbConnectionString"
            )]IAsyncCollector<dynamic> documentsOut,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

           
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            
            if (data != null) {
                string userId = data.userId;
                string productId = data.productId;
                string locationName = data.locationName;
                int rating;
                if (!int.TryParse(data.rating.ToString(), out rating)) 
                    rating = -1;

                string userNotes = data.userNotes;

                if (IsUserIdValid(userId) && IsProductIdValid(productId) && IsRatingValid(rating) /*add check for valid data*/) {
                    try
                    {
                        var newRating = new Ratings()
                        {
                            id = Guid.NewGuid().ToString(),
                            userId = userId,
                            productId = productId,
                            timestamp = DateTime.UtcNow,
                            locationName = locationName,
                            rating = rating,
                            userNotes = userNotes
                        };
                        await documentsOut.AddAsync(newRating);
                        return new OkObjectResult(newRating);
                    }
                    catch(Exception ex)
                    {
                        log.LogError(ex.Message);
                        return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                    }
                   
                }
            }

            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        private static bool IsUserIdValid(string userId) 
        {
            Guid guidUserId;
            if (Guid.TryParse(userId, out guidUserId))
            {
                var client = new RestClient("https://serverlessohapi.azurewebsites.net/api");
                var request = new RestRequest("GetUser", Method.GET).AddParameter("userId", userId);
                var response = client.Get<object>(request);
                return response.IsSuccessful;
            }
            return false;
        }

        private static bool IsProductIdValid(string productId)
        {
            Guid guidProductId;
            if (Guid.TryParse(productId, out guidProductId))
            {
                var client = new RestClient("https://serverlessohapi.azurewebsites.net/api");
                var request = new RestRequest("GetProduct", Method.GET).AddParameter("productId", productId);
                var response = client.Get<object>(request);
                return response.IsSuccessful;
            }
            return false;
        }

        private static bool IsRatingValid(int rating) 
        {
            return rating >= 0 && rating <= 5;           
        }
    }
  }
