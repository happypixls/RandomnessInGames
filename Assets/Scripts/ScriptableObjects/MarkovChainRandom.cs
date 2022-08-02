using System;
using System.Collections.Generic;
using NaughtyAttributes;
using StoryOfRandom.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StoryOfRandom.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Story of random/Markov chain", fileName = "MarkovChain")]
    public class MarkovChainRandom : ScriptableObject
    {
        public Vector2 RandomRange { get; set; }
        public List<GameObject> TrackedObjects { get; set; }

        [field: SerializeField]
        private int Order { get; set; }
        
        [SerializeField] 
        [TextArea(20, 100)] 
        private string SampleText;
        
        [SerializeField] 
        [TextArea(20, 100)]
        private string GeneratedText;
        
        private Dictionary<string, List<string>> Chain { get; set; }
        private Dictionary<string, int> ChainCount { get; set; }

        private void OnEnable()
        {
            Chain = new Dictionary<string, List<string>>();
            ChainCount = new Dictionary<string, int>();
        }
        
        public void Generate()
        {
            Clear();
            var Tokens = SampleText.Split(new []{' '}, StringSplitOptions.None);
            
            for (int i = 0; i < Tokens.Length - Order; i++) 
            {
                for (int j = 0; j <= Order; j++)
                {
                    if (ChainCount.ContainsKey(Tokens[i + j]))
                        ChainCount[Tokens[i + j]]++;
                    else
                        ChainCount.Add(Tokens[i + j], 1);
                }
            }
            
            foreach (var item in ChainCount)
            {
                Debug.Log($"{item.Key} : {item.Value}");
            }
        }

        [Button("Generate text")]
        private void GenerateText()
        {
            Clear();
            GeneratedText = "";
            
            var Tokens = SampleText.Split(new []{' '}, StringSplitOptions.None);
            
            for (int i = 0; i < Tokens.Length - Order; i++) 
            {
                var CurrentToken = Tokens[i];
                
                if (!Chain.ContainsKey(CurrentToken))
                    Chain[CurrentToken] = new List<string>();
            
                var CombinedStringBasedOnOrder = "";
                for (int j = 1; j <= Order; j++)
                {
                    if (i + j < Tokens.Length)
                        CombinedStringBasedOnOrder += Tokens[i + j] + " ";
                }
                
                Chain[CurrentToken].Add(CombinedStringBasedOnOrder);
            }
            
            var StartingPoint = Tokens[Random.Range(0, Tokens.Length)];

            for (int i = 0; i < 20; i++)
            {
                var RandomString = Chain[StartingPoint][Random.Range(0, Chain[StartingPoint].Count)];
                GeneratedText += RandomString + ' ';

                var SplitRandomString = RandomString.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
                StartingPoint = SplitRandomString[SplitRandomString.Length - 1];
            }
        }
        
        public void Clear()
        {
            foreach (var item in TrackedObjects)
                Destroy(item);
            
            TrackedObjects.Clear();
        }
    }
}
