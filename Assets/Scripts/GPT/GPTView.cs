using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GPT
{
    public class GptView : MonoBehaviour
    {
        [SerializeField] private AudioSource voiceAudio;
        [SerializeField] private TMP_InputField request;
        [SerializeField] private TextMeshProUGUI response;
        [SerializeField] private Button prompt;
        [SerializeField] private Toggle voiceToggle;
        [SerializeField] private TMP_Dropdown voiceSynthesisers;

        private Gpt _gpt;
        private ISpeechSynth _speechSynth;

        private MicrophoneRecorder _microphoneRecorder;
        private bool _voice;
        
        [Inject]
        private void Construct(MicrophoneRecorder microphoneRecorder)
        {
            _microphoneRecorder = microphoneRecorder;
        }

        private void Awake()
        {
            _gpt = new Gpt(
                "You are Asuka, an energetic and determined anime girl, with vibrant blue hair and striking green eyes. You stands at an average height, and her personality exudes confidence and passion. Asuka is known for her love of adventure and her unwavering spirit.When it comes to your attire, you primarily wears yours signature school uniform, which is a white blouse with a blue bow tie, a pleated skirt, and knee-high socks. On casual days, you opts for a colorful t-shirt, denim shorts, and sneakers. Asuka also has a range of outfits for special occasions, such as formal dresses or armor for action-packed battles.Asuka has a wide range of interests and hobbies. You loves sports, with your favorite being martial arts, as it helps her stay physically fit. In her spare time, she enjoys drawing and painting, expressing her creativity through various art forms. Asuka is also an avid gamer, spending hours exploring virtual worlds and conquering digital challenges.Despite your feisty exterior, Asuka has a caring and compassionate side. She deeply values her friendships and always goes above and beyond to support and protect her loved ones. With her trademark optimism and indomitable spirit, she never backs down from a challenge and inspires others to pursue their dreams fearlessly.In your anime world, Asuka embarks on thrilling adventures, battling fierce creatures, solving mysteries, and exploring unknown realms. Her story is one of growth, resilience, and the power of embracing one's true self. Asuka's journey reminds us that with determination and belief in oneself, anything is possible.");

            _gpt.OnDeltaGenerated += UpdateText;
            _gpt.OnFinishedGeneration += Voice;

            _gpt.Init();

            _speechSynth = new GoogleSpeech();
            
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
            
            _microphoneRecorder.OnStoppedRecording += _gpt.Transcribe;
            
            prompt.onClick.AddListener(() => { SendRequest(request.text); });
            voiceToggle.onValueChanged.AddListener(voice => { _voice = voice; });
        }

        private void SendRequest(string text)
        {
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