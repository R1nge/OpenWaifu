using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Utilities.Async;

namespace GPT
{
    public class GoogleSpeech : ISpeechSynth
    {
        public async Task Synth(string text, AudioSource source)
        {
            await GoogleTTS(text, source);
        }

        //32 Tokens max
        private async Task GoogleTTS(string text, AudioSource source)
        {
            using UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(
                $"https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q={text}&tl=en",
                AudioType.MPEG);
            await req.SendWebRequest();
            var clip = DownloadHandlerAudioClip.GetContent(req);
            source.PlayOneShot(clip);
        }
    }
}