using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VoicevoxBridge;

//TODO: create a 3d hand from tracking data
//TODO: translate every language into english or detect target language
//TODO: add blink
//TODO: change voice depending on emotion

namespace GPT
{
    public class GptView : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer face;
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

        private JsonParser _jsonParser;
        private EmotionController _emotionController;

        [Inject]
        private void Construct(MicrophoneRecorder microphoneRecorder)
        {
            _microphoneRecorder = microphoneRecorder;
        }

        private void Awake()
        {
            _jsonParser = new JsonParser();
            _emotionController = new EmotionController(face);

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
            _gpt.OnFinishedGeneration += tcpClient.GetMessage;
            tcpClient.OnMessageReceived += Parse;

            _gpt.Init();
        }

        private void Parse(string json)
        {
            var data = _jsonParser.Parse(json);
            print(data.text);

            data.emotions = data.emotions.Except(new[] { "others" }).ToArray();
            
            _emotionController.SetEmotion(data.emotions[0]);
            Voice(data.text);
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
            tcpClient.OnMessageReceived -= Voice;
        }
    }
}