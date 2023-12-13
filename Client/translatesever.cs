using System.Net.Http;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Net.Http.Json;
namespace TranslateApp
{

    public class TranslationInputModel
    {
        [Required]
        public string TextToTranslate { get; set; }

        [Required]
        public string TargetLanguage { get; set; }
    }
    public class TranslationResult
    {
        public Data Data { get; set; } = new Data();
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


    public class TranslationService
    {
        private readonly HttpClient _httpClient;

        public TranslationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> TranslateTextAsync(TranslationInputModel input)
        {
            var requestBody = new
            {
                q = input.TextToTranslate,
                target = input.TargetLanguage,
                key = "https://google-translate1.p.rapidapi.com/language/translate/v2"
            };

            var response = await _httpClient.PostAsJsonAsync("https://google-translate1.p.rapidapi.com/language/translate/v2", requestBody);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var translationResult = JsonSerializer.Deserialize<GoogleTranslateResponse>(responseContent);

                return translationResult.Data.Translations[0].TranslatedText;
            }
            else
            {
                throw new Exception("Error translating text.");
            }
        }
    }










}