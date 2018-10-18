using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace coreheroes.Controllers
{
    public class ClickTrackController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ClickTrackController(IHttpClientFactory clientFactory)
        {
            httpClientFactory = clientFactory;
        }

        [Route("api/clicktrack")]
        [HttpPost]
        public async Task<IActionResult> TrackAction([FromBody] TrackedItem trackedItem)
        {
            var client = httpClientFactory.CreateClient(HttpclientNames.ElasticHttpClient);
            trackedItem.date = DateTime.Now;
            await client.PostAsJsonAsync<TrackedItem>($"{ElasticUrls.CloudBaseUrl}/herotracker/clicks", trackedItem);
            return Ok(trackedItem);
        }


    }
    public class TrackedItem
    {
        public int id {get;set;}
        public string name{get;set;}
        public string element{get;set;}
        public string metadata {get;set;}
        public DateTime? date {get;set;}
    }
}
