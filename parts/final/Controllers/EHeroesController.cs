using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace coreheroes.Controllers
{
    public class EHerosController : Controller
    {


        private readonly IHttpClientFactory httpClientFactory;
        public EHerosController(IHttpClientFactory clientFactory)
        {
            
            httpClientFactory = clientFactory;
        }

        [Route("api/eheroes")]
        [HttpGet]
        public async Task<IActionResult> GetHeroes()
        {
            var client = httpClientFactory.CreateClient(HttpclientNames.ElasticHttpClient);
            //&sort=tophero:desc
            var response = await client.GetStringAsync($"{ElasticUrls.CloudBaseUrl}/heroes/_search?size=100&sort=tophero:desc");
            var jsonResponse = JObject.Parse(response);
            var heroes = jsonResponse["hits"]["hits"]
                .Select(h => new {id = h["_id"].Value<int>(), name=h["_source"]["name"].Value<string>()});

            return Ok(heroes.ToArray());
        }


        [Route("api/eheroes/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetHero(int id)
        {
            var client = httpClientFactory.CreateClient(HttpclientNames.ElasticHttpClient);
            var response = await client.GetStringAsync($"{ElasticUrls.CloudBaseUrl}/heroes/doc/{id}");
            
            var jsonResponse = JObject.Parse(response);
            if (!jsonResponse["found"].Value<bool>())
                    return NotFound();

            var hero = new {id = jsonResponse["_id"].Value<int>(), name=jsonResponse["_source"]["name"].Value<string>()};
            return Ok(hero);
        }


        [Route("api/eheroes/search")]
        [HttpGet]
        public async Task<IActionResult> Search(string q)
        {
            var client = httpClientFactory.CreateClient(HttpclientNames.ElasticHttpClient);
            // var response = await client.GetStringAsync($"{baseUrl}/heroes/_search?q={q}&size=100");
            // var jsonResponse = JObject.Parse(response);
            // if (jsonResponse["hits"]["total"].Value<int>() == 0)
            //     return Ok();
            // var heroes = jsonResponse["hits"]["hits"]
            //     .Select(h => new {id = h["_id"].Value<int>(), name=h["_source"]["name"].Value<string>()});

            var heroes = await WildcardSearch(client, q);
            return Ok(heroes.ToArray());
        }

        private async Task<IEnumerable<Hero>> WildcardSearch(HttpClient client, string term)
        {
            var searchResult = await client.PostAsJsonAsync<object>($"{ElasticUrls.CloudBaseUrl}/heroes/_search?&size=100", 
            new {
                 query = new { query_string =  new { query = $"*{term}*"    }  }}
            );
            
            var response = searchResult.Content.ReadAsStringAsync().Result;
            var jsonResponse = JObject.Parse(response);
            if (jsonResponse["hits"]["total"].Value<int>() == 0)
                return Enumerable.Empty<Hero>();
            return jsonResponse["hits"]["hits"]
                .Select(h => new Hero {id = h["_id"].Value<int>(), name=h["_source"]["name"].Value<string>()});

        }




        private class Hero
        {
            public int id{get;set;}
            public string name{get;set;}
        }

    }
}
