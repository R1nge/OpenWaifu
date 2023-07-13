using System;
using System.Collections.Generic;
using System.Linq;
using ElevenLabs;
using OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GPT
{
    public class GPT : MonoBehaviour
    {
        [SerializeField] private AudioSource voiceAudio;
        [SerializeField] private TMP_InputField request;
        [SerializeField] private TextMeshProUGUI response;
        [SerializeField] private Button prompt;
        [SerializeField] private Toggle voiceToggle;
        private ElevenLabsClient _elevenLabsApi;
        private OpenAIClient _gptApi;
        private MicrophoneRecorder _microphoneRecorder;
        private bool _voice;

        private void Awake()
        {
            _microphoneRecorder = FindObjectOfType<MicrophoneRecorder>();
            _microphoneRecorder.OnStoppedRecording += Transcribe;

            _elevenLabsApi = new ElevenLabsClient(new ElevenLabsAuthentication("1c6415d19799be04a9054d35d6a5f92d"));

            _gptApi = new OpenAIClient(
                new OpenAIAuthentication(
                    "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",
                    "org-q208lJQ2f6076dBHNaHpmN4b")
            );

            prompt.onClick.AddListener(() => { SendRequest(request.text); });
            voiceToggle.onValueChanged.AddListener(voice => { _voice = voice; });
        }

        private async void SendRequest(string prompt)
        {
            var messages = new List<Message>
            {
                new(Role.System, "You are Asuka, an energetic and determined anime girl, with vibrant blue hair and striking green eyes. You stands at an average height, and her personality exudes confidence and passion. Asuka is known for her love of adventure and her unwavering spirit.When it comes to your attire, you primarily wears yours signature school uniform, which is a white blouse with a blue bow tie, a pleated skirt, and knee-high socks. On casual days, you opts for a colorful t-shirt, denim shorts, and sneakers. Asuka also has a range of outfits for special occasions, such as formal dresses or armor for action-packed battles.Asuka has a wide range of interests and hobbies. You loves sports, with your favorite being martial arts, as it helps her stay physically fit. In her spare time, she enjoys drawing and painting, expressing her creativity through various art forms. Asuka is also an avid gamer, spending hours exploring virtual worlds and conquering digital challenges.Despite your feisty exterior, Asuka has a caring and compassionate side. She deeply values her friendships and always goes above and beyond to support and protect her loved ones. With her trademark optimism and indomitable spirit, she never backs down from a challenge and inspires others to pursue their dreams fearlessly.In your anime world, Asuka embarks on thrilling adventures, battling fierce creatures, solving mysteries, and exploring unknown realms. Her story is one of growth, resilience, and the power of embracing one's true self. Asuka's journey reminds us that with determination and belief in oneself, anything is possible."),
                new(Role.User, $"{prompt}"),
            };
            Debug.LogWarning($"Prompt: {prompt}" );
            
            var chatRequest = new ChatRequest(messages, Model.GPT3_5_Turbo, number: 1);
            var res = new string("");
            await _gptApi.ChatEndpoint.StreamCompletionAsync(chatRequest, result =>
            {
                foreach (var choice in
                         result.Choices.Where(choice => !string.IsNullOrWhiteSpace(choice.Delta?.Content)))
                {
                    // Partial response content
                    res += choice.Delta.Content;
                    //Debug.Log(choice.Delta.Content);
                    response.text = $"{res}";
                }

                foreach (var choice in result.Choices.Where(choice =>
                             !string.IsNullOrWhiteSpace(choice.Message?.Content)))
                {
                    // Completed response content
                    //response.text = $"{choice.Message.Role}: {choice.Message.Content}";
                    //Debug.Log($"{choice.Message.Role}: {choice.Message.Content}");
                    if (_voice)
                    {
                        Voice(choice.Message.Content);
                    }
                }
            });
        }

        private async void Transcribe(AudioClip clip)
        {
            var request = new AudioTranscriptionRequest(clip, language: "en");
            var result = await _gptApi.AudioEndpoint.CreateTranscriptionAsync(request);
            SendRequest(result);
        }

        private async void Voice(string text)
        {
            var voice = (await _elevenLabsApi.VoicesEndpoint.GetAllVoicesAsync()).FirstOrDefault();
            var defaultVoiceSettings = await _elevenLabsApi.VoicesEndpoint.GetDefaultVoiceSettingsAsync();
            var (clipPath, audioClip) = await _elevenLabsApi.TextToSpeechEndpoint.StreamTextToSpeechAsync(
                text,
                voice,
                clip =>
                {
                    // Event raised as soon as the clip has loaded enough data to play.
                    // May not provide or play full clip until Unity bug is addressed.
                    this.voiceAudio.PlayOneShot(clip);
                },
                defaultVoiceSettings);
        }
    }
}