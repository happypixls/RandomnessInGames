using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace StoryOfRandom.Behaviours.Randomizers
{
    public class LFSRRandom : MonoBehaviour
    {
        [SerializeField] 
        private Vector2 squareSize;
        
        private List<GameObject> trackedObjects;
        
        [SerializeField, BoxGroup("With Cubes")]
        private GameObject prefab1;
        
        [SerializeField, BoxGroup("With Cubes")]
        private GameObject prefab2;

        private void Start() => trackedObjects = new List<GameObject>();
        
        [Button("Generate on CPU")]
        private void GenerateOnCPU()
        {
            Clear();
            
            var start = 0b1000000000000000000000000000000000000000000000000000000000000001; //64bit unsigned long
            var bitStream = "";
            for (var i = 0; i < squareSize.x; i++)
            {
                for (var j = 0; j < squareSize.y; j++)
                {
                    var newBits = (~(start ^ ~((start >> 62) ^ (start >> 63)))) & 1; //from xapp052.pdf
                    start = (start >> 1) | (newBits << 63);
                    
                    var importantBit = (start & 1);
                    
                    if(i == 0)
                        continue;
                    
                    bitStream += importantBit; //For debugging purposes only

                    trackedObjects.Add((importantBit) == 1
                        ? Instantiate(prefab1, new Vector3(i - (squareSize.x / 2), 0, j - (squareSize.y / 2)),
                            Quaternion.identity)
                        : Instantiate(prefab2, new Vector3(i - (squareSize.x / 2), 0, j - (squareSize.y / 2)),
                            Quaternion.identity));
                }
            }
            
            //uncomment to see the 0s and 1s :)
            //Debug.Log(bitStream + " \n");
        }

        private void Clear()
        {
            foreach (var item in trackedObjects)
                Destroy(item);
            
            trackedObjects.Clear();
        }
    }
}
