using System.Collections.Generic;
using NaughtyAttributes;
using StoryOfRandom.Core;
using UnityEngine;

namespace StoryOfRandom.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Story of random/LFSR random", fileName = "LFSRRandom")]
    public class LFSRRandom : ScriptableObject
    {
        [field: SerializeField]
        public Vector2 RandomRange { get; set; }
        public List<GameObject> TrackedObjects { get; set; }
        
        [field: SerializeField]
        private Vector2 SquareSize { get; set; }
        
        [field: SerializeField, BoxGroup("With Cubes")]
        private GameObject Prefab1;
        
        [field: SerializeField, BoxGroup("With Cubes")]
        private GameObject Prefab2;

        [Button("Generate")]
        public void Generate()
        {
            var Start = 0b1000000000000000000000000000000000000000000000000000000000000001; //64bit unsigned long
            var BitStream = "";
            for (var i = 0; i < SquareSize.x; i++)
            {
                for (var j = 0; j < SquareSize.y; j++)
                {
                    var NewBits = (~(Start ^ ~((Start >> 62) ^ (Start >> 63)))) & 1; //from xapp052.pdf
                    Start = (Start >> 1) | (NewBits << 63);
                    
                    var ImportantBit = (Start & 1);
                    
                    if(i == 0)
                        continue;
                    
                    BitStream += ImportantBit; //For debugging purposes only

                    TrackedObjects.Add((ImportantBit) == 1
                        ? Instantiate(Prefab1, new Vector3(i - (SquareSize.x / 2), 0, j - (SquareSize.y / 2)),
                            Quaternion.identity)
                        : Instantiate(Prefab2, new Vector3(i - (SquareSize.x / 2), 0, j - (SquareSize.y / 2)),
                            Quaternion.identity));
                }
            }
            Debug.Log(BitStream + " \n");
        }

        public void Clear()
        {
            foreach (var item in TrackedObjects)
                Destroy(item);
            
            TrackedObjects.Clear();
        }
    }
}
