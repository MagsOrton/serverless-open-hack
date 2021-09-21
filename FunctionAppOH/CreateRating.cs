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
                int rating = data.rating;
                string userNotes = data.userNotes;

                if (true /*add check for valid data*/) {
                    try
                    {
                        await documentsOut.AddAsync(new
                        {
                            id = System.Guid.NewGuid().ToString(),
                            userId = userId,
                            productId = productId,
                            timestamp = DateTime.UtcNow,
                            locationName = locationName,
                            rating = rating,
                            userNotes = userNotes
                        });
                        return new OkObjectResult("New rating was inserted successfully");
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
    }
  }
