using System.Net.Http;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Net.Http.Json;
using OpenQA.Selenium.DevTools.V85.CSS;
using Newtonsoft.Json;
using System.Text;
namespace TranslateApp
{

    public class TranslationInputModel
    {
        [Required]
        public string TextToTranslate { get; set; } = string.Empty;

        [Required]
        public string TargetLanguage { get; set; } = string.Empty;


    }
    public class TranslationResult
    {
        public Data Data { get; set; } = new Data();
        public string TranslatedData { get; set; } = string.Empty;
    }

    public class Data
    {
        public Translation[] Translations { get; set; } = Array.Empty<Translation>();
    }

    public class Translation
    {
        public string TranslatedText { get; set; } = string.Empty;
    }

    public class GoogleTranslateResponse
    {
        public Data Data { get; set; } = new Data();


    }


    public class TranslateService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TranslateService()
        {
            _httpClient = new HttpClient();
            _apiKey = "dbde5caa7cmshbfdd51a11d47c40p1e13d0jsn82a9c565cb9f"; 
        }

        public async Task<string> TranslateTextAsync(TranslationInputModel inputModel)
        {
            // Construct the request payload
            var payload = new
            {
                q = inputModel.TextToTranslate,
                target = inputModel.TargetLanguage
            };

            // Serialize the payload to JSON
            var jsonPayload = JsonConvert.SerializeObject(payload);

            // Construct the HTTP content from the JSON payload
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Send a POST request to the Google Translate API
            var response = await _httpClient.PostAsync($"https://translation.googleapis.com/language/translate/v2?key={_apiKey}", httpContent);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Deserialize the response content to a TranslationResult
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TranslationResult>(jsonResponse);

            // Return the translated text
            return result.Data.Translations[0].TranslatedText;
        }
    }
}






