﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Filters101;
using Filters101.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Linq;
using Xunit;

namespace IntegrationTests
{
    
    public class AuthorsControllerGet
    {
        private readonly HttpClient _client;

        public AuthorsControllerGet()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>();
            var server = new TestServer(builder);
            _client = server.CreateClient();

            // client always expects json results
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        }

        [Fact]
        public async Task ReturnsListOfAuthors()
        {
            // ensure data
            // TODO: Move this to one location
            await _client.GetAsync("api/authors/populate");

            var response = await _client.GetAsync("/api/authors");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<Author>>(stringResponse);

            Assert.Equal(2, result.Count());
        }
    }
}
