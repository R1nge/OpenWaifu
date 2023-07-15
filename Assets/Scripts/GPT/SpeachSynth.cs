using System.Threading.Tasks;
using UnityEngine;

namespace GPT
{
    public interface ISpeechSynth
    {
        Task Synth(string text, AudioSource source);
    }
}