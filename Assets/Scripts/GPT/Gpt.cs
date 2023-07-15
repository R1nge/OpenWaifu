using System;
using System.Collections.Generic;
using System.Linq;
using OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Models;
using UnityEngine;

namespace GPT
{
    public class Gpt
    {
        private readonly string _personality;
        private OpenAIClient _gptApi;

        public event Action<string> OnDeltaGenerated;
        public event Action<string> OnFinishedGeneration;

        public Gpt(string personality)
        {
            _personality = personality;
        }

        public void Init()
        {
            _gptApi = new OpenAIClient
            (
                new OpenAIAuthentication
                (
                    "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",
                    "org-q208lJQ2f6076dBHNaHpmN4b"
                )
            );
        }

        public async void Transcribe(AudioClip clip)
        {
            using (AudioTranscriptionRequest req = new AudioTranscriptionRequest(clip, language: "en"))
            {
                var result = await _gptApi.AudioEndpoint.CreateTranscriptionAsync(req);
                SendRequest(result);
            }
        }

        public async void SendRequest(string prompt)
        {
            var messages = new List<Message>
            {
                new(Role.System, _personality),
                new(Role.User, $"{prompt}"),
            };

            var chatRequest = new ChatRequest(messages, Model.GPT3_5_Turbo, number: 1);
            var res = new string("");
            await _gptApi.ChatEndpoint.StreamCompletionAsync(chatRequest, result =>
            {
                foreach (var choice in
                         result.Choices.Where(choice => !string.IsNullOrWhiteSpace(choice.Delta?.Content)))
                {
                    res += choice.Delta.Content;

                    OnDeltaGenerated?.Invoke(res);
                }

                foreach (var choice in result.Choices.Where(choice =>
                             !string.IsNullOrWhiteSpace(choice.Message?.Content)))
                {
                    OnFinishedGeneration?.Invoke(choice.Message.Content);
                }
            });
        }
    }
}