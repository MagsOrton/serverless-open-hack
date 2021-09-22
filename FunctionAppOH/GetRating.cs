using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunctionAppOH
{
    public class Rating
    {
        public string id { get; set; }

        public string userId { get; set; }

        public string productId { get; set; }

        public string timestamp { get; set; }

        public string locationName { get; set; }

        public int rating { get; set; }

        public string userNotes { get; set; }
    }

    public static class FunctioGetRating
    {
        [FunctionName("GetRating")]
        public static async Task<IActionResult>
        CreateRating(
            [HttpTrigger(AuthorizationLevel.Function,"get", Route = "GetRating/{ratingId?}")]
            HttpRequest req,
            [
                CosmosDB(databaseName: "%databaseName%",
                    collectionName: "%collectionName%",
                    ConnectionStringSetting = "CosmosDbConnectionString",
                    SqlQuery = "select top 1 * from c where c.id = {ratingId}")
            ]
            IEnumerable<Rating> ratings,
            ILogger log
        )
        {
            // Return Not Found if there's no rating
            if (!ratings.Any())
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ratings.First());
        }
    }
}
