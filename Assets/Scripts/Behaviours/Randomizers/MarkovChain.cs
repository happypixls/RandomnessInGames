using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StoryOfRandom.Behaviours.Randomizers
{
    //https://www.gutenberg.org/cache/epub/25267/pg25267-images.html
    public class MarkovChain : MonoBehaviour
    {
        [SerializeField] 
        private int order;
        
        [SerializeField] 
        [TextArea(20, 100)] 
        private string sampleText;
        
        [SerializeField] 
        [TextArea(20, 100)]
        private string generatedText;
        
        private List<GameObject> trackedObjects;
        
        private Dictionary<string, List<string>> chain;
        private Dictionary<string, int> chainCount;

        private void Start()
        {
            chain = new Dictionary<string, List<string>>();
            chainCount = new Dictionary<string, int>();
            trackedObjects = new List<GameObject>();
        }

        [Button("Generate text")]
        private void GenerateText()
        {
            Clear();
            generatedText = "";
            
            var tokens = sampleText.Split(new []{' '}, StringSplitOptions.None);
            
            for (int i = 0; i < tokens.Length - order; i++) 
            {
                var currentToken = tokens[i];
                
                if (!chain.ContainsKey(currentToken))
                    chain[currentToken] = new List<string>();
            
                var combinedStringBasedOnOrder = "";
                for (int j = 1; j <= order; j++)
                {
                    if (i + j < tokens.Length)
                        combinedStringBasedOnOrder += tokens[i + j] + " ";
                }
                
                chain[currentToken].Add(combinedStringBasedOnOrder);
            }
            
            var startingPoint = tokens[Random.Range(0, tokens.Length)];

            for (int i = 0; i < 20; i++)
            {
                var randomString = chain[startingPoint][Random.Range(0, chain[startingPoint].Count)];
                generatedText += randomString + ' ';

                var splitRandomString = randomString.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
                startingPoint = splitRandomString[splitRandomString.Length - 1];
            }
        }
        
        private void Clear()
        {
            foreach (var item in trackedObjects)
                Destroy(item);
            
            trackedObjects.Clear();
        }
    }
}
