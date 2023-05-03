using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryOfRandom.Behaviours.Randomizers.ReactionDiffusion
{
    [CreateAssetMenu(menuName = "Randomness/ReactionDiffusion/FKConfig", fileName = "FConfig")]
    public class ReactionDiffusionConfiguration : ScriptableObject
    {
        public float dA;
        public float dB;
        public float f;
        public float k;
    }
}
