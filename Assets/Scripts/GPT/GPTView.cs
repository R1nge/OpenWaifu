using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VoicevoxBridge;

//TODO: create a 3d hand from tracking data
//TODO: translate every language into english or detect target language
//TODO: add blink
//TODO: detect emotion behind user voice https://github.com/x4nth055/emotion-recognition-using-speech, 
//I'll have to create a wrapper around this python lib

namespace GPT
{
    public class GptView : MonoBehaviour
    {
        [SerializeField] private PersonalitySO personality;
        [SerializeField] private int speakerId; //TODO: create a list with enums (Voice charatcer name, Voice tone)
        [SerializeField] private AudioSource voiceAudio;
        [SerializeField] private TMP_InputField request;
        [SerializeField] private TextMeshProUGUI response;
        [SerializeField] private Button prompt;
        [SerializeField] private Toggle voiceToggle;
        //TODO: rewrite voicevox to get rid from external dependency
        [SerializeField] private VOICEVOX voiceVox;

        [SerializeField] private MyTcpClient tcpClient;

        private Gpt _gpt;
        private ISpeechSynth _speechSynth;
        private string _inputLanguage, _outputLanguage;

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

            _speechSynth = new VoiceVoxSpeech(speakerId, voiceVox);

            _microphoneRecorder.OnStoppedRecording += clip => _gpt.Transcribe(clip, _inputLanguage);

            prompt.onClick.AddListener(() => { SendRequest(request.text); });
            voiceToggle.onValueChanged.AddListener(voice => { _voice = voice; });
        }

        private void InitGpt()
        {
            _gpt = new Gpt(personality);

            _gpt.OnDeltaGenerated += UpdateText;
            _gpt.OnFinishedGeneration += tcpClient.RequestMessage;
            tcpClient.OnFinishedTranslation += Voice;

            _gpt.Init();
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
            if (_voice)
            {
                await _speechSynth.Synth(text, voiceAudio);
            }
        }

        private void OnDestroy()
        {
            _gpt.OnDeltaGenerated -= UpdateText;
            tcpClient.OnFinishedTranslation -= Voice;
        }
    }
}