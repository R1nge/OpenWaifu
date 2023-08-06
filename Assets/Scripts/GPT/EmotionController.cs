using System.Collections.Generic;
using UnityEngine;

namespace GPT
{
    public class EmotionController
    {
        private readonly SkinnedMeshRenderer _face;
        private readonly Dictionary<string, int[]> _emotions;
        private string _lastEmotion;

        //neutral, natural
        //disgust: 0.036

        public EmotionController(SkinnedMeshRenderer face)
        {
            _face = face;
            _emotions = new Dictionary<string, int[]>
            {
                { "joy", new[] { 3, 8, 17, 18, 19, 32 } },
                { "fun", new[] { 2, 7 } },
                { "anger", new[] { 1, 6, 12 } },
                { "surprise", new[] { 5, 10, 21, 32 } },
                { "sadness", new[] { 4, 9, 20, 31 } }, //sadness == sorrow
                { "fear", new[] { 4, 9, 20, 31 } } // fear == sorrow
            };
        }

        public void SetEmotion(string emotion)
        {
            if (!_emotions.ContainsKey(emotion))
            {
                Debug.LogError("EmotionController: Emotion not found");
                return;
            }

            _lastEmotion = emotion;

            for (int i = 0; i < _emotions[emotion].Length; i++)
            {
                _face.SetBlendShapeWeight(_emotions[emotion][i], 100);
            }
        }

        public void ResetEmotion()
        {
            if (_lastEmotion == null) return;

            for (int i = 0; i < _emotions[_lastEmotion].Length; i++)
            {
                _face.SetBlendShapeWeight(_emotions[_lastEmotion][i], 0);
            }
        }
    }
}