using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VoicevoxBridge;

//Pipeline:
//Speech recognition using Whisper
//TODO: translate every language into english
//Prompt GPT 3.5 Turbo (has very limited memory span)
//Stream the result
//TODO: translate it to target language
//Update the text
//TODO: translate it into japanese
//TODO: Send it to voiceVox
//TODO: play audio from voiceVox

//Basically I need to tranlsate it
//And link it to VoiceVox

namespace GPT
{
    public class GptView : MonoBehaviour
    {
        [SerializeField] private PersonalitySO personality;
        [SerializeField] private AudioSource voiceAudio;
        [SerializeField] private TMP_InputField request;
        [SerializeField] private TextMeshProUGUI response;
        [SerializeField] private Button prompt;
        [SerializeField] private Toggle voiceToggle;
        [SerializeField] private TMP_Dropdown voiceSynthesisers;
        [SerializeField] private TMP_Dropdown whisperLanguages;

        [SerializeField] private VOICEVOX voiceVox;
        
        private Gpt _gpt;
        private ISpeechSynth _speechSynth;
        private string _language;

        private MicrophoneRecorder _microphoneRecorder;
        private bool _voice;

        [Inject]
        private void Construct(MicrophoneRecorder microphoneRecorder)
        {
            _microphoneRecorder = microphoneRecorder;
        }

        private void Awake()
        {
            InitGpt();
            
            InitSpeechSynthesisers();

            InitWhisper();

            _microphoneRecorder.OnStoppedRecording += clip => _gpt.Transcribe(clip, _language);

            prompt.onClick.AddListener(() => { SendRequest(request.text); });
            voiceToggle.onValueChanged.AddListener(voice => { _voice = voice; });
        }

        private void InitGpt()
        {
            _gpt = new Gpt(personality);

            _gpt.OnDeltaGenerated += UpdateText;
            _gpt.OnFinishedTranslation += Voice;

            _gpt.Init();
        }

        private void InitSpeechSynthesisers()
        {
            _speechSynth = new GoogleSpeech();
            _speechSynth.SetLanguage("en");

            voiceSynthesisers.ClearOptions();

            voiceSynthesisers.AddOptions(new List<TMP_Dropdown.OptionData>
            {
                new("Google"),
                new("11Labs")
            });

            voiceSynthesisers.onValueChanged.AddListener(index =>
            {
                _speechSynth = index switch
                {
                    0 => new GoogleSpeech(),
                    1 => new ElevenLabsSpeech(),
                    _ => _speechSynth
                };
            });

            voiceSynthesisers.onValueChanged.AddListener(SetLanguage);
        }

        private void InitWhisper()
        {
            whisperLanguages.ClearOptions();

            whisperLanguages.AddOptions(new List<TMP_Dropdown.OptionData>
            {
                new("English"),
                new("Russian"),
                new("Japanese")
            });

            whisperLanguages.onValueChanged.AddListener(SetLanguage);
        }

        private void SetLanguage(int index)
        {
            _language = index switch
            {
                0 => "en",
                1 => "ru",
                2 => "jp",
                _ => _language
            };

            _speechSynth.SetLanguage(_language);
        }

        private void SendRequest(string text)
        {
            Stop();
            _gpt.SendRequest(text);
        }

        private void Stop()
        {
            //TODO: add cancellation
            voiceAudio.Stop();
        }

        private void UpdateText(string text)
        {
            response.text = text;
        }

        private async void Voice(string text)
        {
            // if (_voice)
            // {
            //     _speechSynth.Synth(text, voiceAudio);
            // }
            
            int speaker = 3; // ずんだもん あまあま
           
            Voice voice = await voiceVox.CreateVoice(speaker, text);
            await voiceVox.Play(voice);
        }

        private void OnDestroy()
        {
            _gpt.OnDeltaGenerated -= UpdateText;
            _gpt.OnFinishedTranslation -= Voice;
            whisperLanguages.onValueChanged.RemoveAllListeners();
            voiceSynthesisers.onValueChanged.RemoveAllListeners();
        }
    }
}