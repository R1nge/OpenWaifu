using System.Collections.Generic;
using UnityEngine;

namespace GPT
{
    public class EmotionController
    {
        private readonly SkinnedMeshRenderer _face;
        private readonly Dictionary<string, int[]> _emotions;
        private string _lastEmotion;

        public EmotionController(SkinnedMeshRenderer face)
        {
            _face = face;
            _emotions = new Dictionary<string, int[]>
            {
                { "joy", new[] { 3, 8, 17, 18, 19, 32 } },
                { "fun", new[] { 2, 7 } },
                { "anger", new[] { 1, 6, 12 } },
            };
        }

        public void SetEmotion(string emotion)
        {
            _lastEmotion = emotion;
            for (int i = 0; i < _emotions[emotion].Length; i++)
            {
                _face.SetBlendShapeWeight(_emotions[emotion][i], 25);
            }
        }

        public void ResetEmotion()
        {
            for (int i = 0; i < _emotions[_lastEmotion].Length; i++)
            {
                _face.SetBlendShapeWeight(_emotions[_lastEmotion][i], 0);
            }
        }

        //Joy, fun, anger, neutral, sorrow, surprise, natural
    }
}