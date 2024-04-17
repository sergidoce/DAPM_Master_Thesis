using DAPM.OperatorMS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DAPM.OperatorMS.Api.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OperatorService> _logger;

        public OperatorService(ILogger<OperatorService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<byte[]> ExecuteMiner(string operatorName, string resourceName)
        {
            try
            {
                HttpResponseMessage csvResponse = await _httpClient.GetAsync($"http://localhost:5043/Resource?name={resourceName}");
                csvResponse.EnsureSuccessStatusCode();

                using (var csvStream = await csvResponse.Content.ReadAsStreamAsync())
                { 
                    using (var csvContent = new StreamContent(csvStream))
                    {
                        var formData = new MultipartFormDataContent
                        {
                            { csvContent, "event_log", "data.csv" }
                        };

                        HttpResponseMessage postResponse = await _httpClient.PostAsync("http://localhost:5000/execute", formData);

                        System.Diagnostics.Debug.WriteLine(postResponse.StatusCode);
                        System.Diagnostics.Debug.WriteLine(postResponse.ToString());

                        return await postResponse.Content.ReadAsByteArrayAsync();
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error executing miner: {ex.Message}");
                throw;
            }
            finally
            {
                _httpClient.Dispose();
            }
        }
    }
}