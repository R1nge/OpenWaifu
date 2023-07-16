using UnityEngine;

namespace GPT
{
    [CreateAssetMenu(fileName = "Personality", menuName = "AI/Personality")]
    public class PersonalitySO : ScriptableObject
    {
        [SerializeField] private string personality;

        public string Personality => personality;
    }
}