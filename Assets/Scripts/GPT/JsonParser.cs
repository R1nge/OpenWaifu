using Newtonsoft.Json;

namespace GPT
{
    public class JsonParser
    {
        public Data Parse(string json)
        {
            return JsonConvert.DeserializeObject<Data>(json);
        }

        public string CreateJson(Data dictionary)
        {
            return JsonConvert.SerializeObject(dictionary);
        }

        public struct Data
        {
            public string text;
            public string[] emotions;

            public Data(string text, string[] emotions)
            {
                this.text = text;
                this.emotions = emotions;
            }
        }
    }
}