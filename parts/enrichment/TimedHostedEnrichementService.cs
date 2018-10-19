using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace coreheroes.Enrichment
{
    internal class TimedHostedEnrichementService : IHostedService, IDisposable
    {
        private readonly IHttpClientFactory httpClientFactory;
        private Timer _timer;
        private readonly ILogger _logger;

        public TimedHostedEnrichementService(ILogger<TimedHostedEnrichementService> logger, IHttpClientFactory clientFactory)
        {
            httpClientFactory = clientFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }


        private const string baseUrl = "https://0d5d3591bdfa4050b21e4ca8934e7885.us-east-1.aws.found.io:9243";
        private void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");

            var client = httpClientFactory.CreateClient(HttpclientNames.ElasticHttpClient);

            var query = GetHeroScoresQuery();
            var heroScoresQueryContent  = new StringContent(query, Encoding.UTF8, "application/json");
            var searchResult = client.PostAsync($"{baseUrl}/herotracker/_search?&size=0", heroScoresQueryContent).Result;
            
            var response = searchResult.Content.ReadAsStringAsync().Result;
            var jsonResponse = JObject.Parse(response);
            if (jsonResponse["hits"]["total"].Value<int>() == 0)
            {
                _logger.LogInformation("No Items found in aggregation.. enrichment ignored");
            }   
            var heroScores = jsonResponse["aggregations"]["popularlements"]["buckets"]
                .Select(b => new {elementName = b["key"].Value<string>(), heroscores = b["clickcount"]["buckets"].Select(c => new { id = c["key"].Value<int>(), score=c["doc_count"].Value<int>()})});
            
            heroScores.ToList()
            .ForEach(hs => {
                foreach(var heroScore in hs.heroscores)
                {
                    var url = $"{baseUrl}/heroes/doc/{heroScore.id}/_update";
                    var content = $@"{{""doc"":{{""{hs.elementName}"":{heroScore.score}}}}}";
                    var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                    var enrichResult = client.PostAsync(url, httpContent).Result;
                    _logger.LogInformation($"enriching item {heroScore.id} with element {hs.elementName} and score of {heroScore.score}  {(enrichResult.IsSuccessStatusCode ? "success" : enrichResult.StatusCode.ToString())}");
                }        

            });

        }

        private string GetHeroScoresQuery() => 
        @"{
            ""aggs"" : {
                ""popularlements"" : {
                    ""terms"" : {
                        ""field"" : ""element.keyword"", ""size"": 20
                    },
                    ""aggs"" : {
                        ""clickcount"" : { ""terms"": { ""field"" : ""id"" , ""size"" : 20} } 
                    }
                }
            }
        }";


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}