using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FunctionAppOH
{
    public static class FunctioGetRatings
    {

        [FunctionName("GetRatings")]
        public static async Task<IActionResult> GetRatings(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetRatings/{id}")] HttpRequest req,
            [CosmosDB(
                 databaseName: "%databaseName%",
                 collectionName: "%collectionName%",
                 ConnectionStringSetting = "CosmosDbConnectionString",
                 SqlQuery  = "select * from c where c.userId = {id}")]IEnumerable<Ratings> ratings,
            ILogger log)
        {

            log.LogInformation("GetRatings HTTP trigger function processed a request.");

            return new OkObjectResult(ratings);
        }
    }
}
