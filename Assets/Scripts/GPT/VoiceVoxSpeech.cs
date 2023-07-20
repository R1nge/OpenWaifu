using System.Threading.Tasks;
using UnityEngine;
using VoicevoxBridge;

namespace GPT
{
    public class VoiceVoxSpeech : ISpeechSynth
    {
        private readonly int _voiceId;
        private readonly VOICEVOX _voiceVox;

        public VoiceVoxSpeech(int voiceId, VOICEVOX voiceVox)
        {
            _voiceId = voiceId;
            _voiceVox = voiceVox;
        }

        public async Task Synth(string text, AudioSource source)
        {
            int speaker = _voiceId;
            Voice voice = await _voiceVox.CreateVoice(speaker, text);
            await _voiceVox.Play(voice);
        }

        public void SetLanguage(string language)
        {
            //VoiceVox only supports Japanese
        }
    }
}