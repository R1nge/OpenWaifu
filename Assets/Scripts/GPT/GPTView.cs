using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

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
            _gpt = new Gpt(personality);

            _gpt.OnDeltaGenerated += UpdateText;
            _gpt.OnFinishedGeneration += Voice;

            _gpt.Init();

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

            whisperLanguages.ClearOptions();

            whisperLanguages.AddOptions(new List<TMP_Dropdown.OptionData>
            {
                new("English"),
                new("Russian")
            });

            whisperLanguages.onValueChanged.AddListener(index =>
            {
                _language = index switch
                {
                    0 => "en",
                    1 => "ru",
                    _ => _language
                };
                
                _speechSynth.SetLanguage(_language);
            });

            _microphoneRecorder.OnStoppedRecording += clip => _gpt.Transcribe(clip, _language);

            prompt.onClick.AddListener(() => { SendRequest(request.text); });
            voiceToggle.onValueChanged.AddListener(voice => { _voice = voice; });
        }

        private void SendRequest(string text)
        {
            voiceAudio.Stop();
            _gpt.SendRequest(text);
        }

        private void UpdateText(string text)
        {
            response.text = text;
        }

        private void Voice(string text)
        {
            if (_voice)
            {
                _speechSynth.Synth(text, voiceAudio);
            }
        }

        private void OnDestroy()
        {
            _gpt.OnDeltaGenerated -= UpdateText;
            _gpt.OnFinishedGeneration -= Voice;
        }
    }
}