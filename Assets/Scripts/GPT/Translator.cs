using System;
using System.Net.Http;
using System.Text;
using UnityEngine;

namespace GPT
{
    public class Translator
    {
        public event Action<string> OnFinishedTranslation;
        private readonly string _ip;

        public Translator(string ip)
        {
            _ip = ip;
        }

        private struct Translation
        {
            public Translation(string text, string source, string target, string format, string apiKey)
            {
                q = text;
                this.source = source;
                this.target = target;
                this.format = format;
                api_key = apiKey;
            }
            
            public string q;
            public string source;
            public string target;
            public string format;
            public string api_key;
        }
        
        public enum Languages
        {
            EN,
            RU,
            JP
        }
        
        //TODO: auto detect input language
        public async void TranslateToJapanese(string text)
        {
            HttpClient client = new HttpClient();
            var req = new Translation
            {
                q = text,
                source = "en",
                target = "ja",
                format = "text",
                api_key = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
            };

            var json = JsonUtility.ToJson(req);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(_ip, content);
            string responseString = await response.Content.ReadAsStringAsync();
            //TODO: redo
            var str = responseString.Replace("translatedText", "");
            Debug.Log("Received: " + str);
            
            OnFinishedTranslation?.Invoke(str);
        }
    }
}