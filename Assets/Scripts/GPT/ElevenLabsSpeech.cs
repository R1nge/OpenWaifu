using System.Linq;
using System.Threading.Tasks;
using ElevenLabs;
using UnityEngine;

namespace GPT
{
    public class ElevenLabsSpeech : ISpeechSynth
    {
        private readonly ElevenLabsClient _elevenLabsApi = new(new ElevenLabsAuthentication("1c6415d19799be04a9054d35d6a5f92d"));

        public async Task Synth(string text, AudioSource source)
        {
            await Run(text, source);
        }

        public void SetLanguage(string language)
        {
        }

        private async Task Run(string text, AudioSource source)
        {
            var voice = (await _elevenLabsApi.VoicesEndpoint.GetAllVoicesAsync()).FirstOrDefault();
            var defaultVoiceSettings = await _elevenLabsApi.VoicesEndpoint.GetDefaultVoiceSettingsAsync();
            await _elevenLabsApi.TextToSpeechEndpoint.StreamTextToSpeechAsync(
                text,
                voice,
                source.PlayOneShot,
                defaultVoiceSettings);
        }
    }
}