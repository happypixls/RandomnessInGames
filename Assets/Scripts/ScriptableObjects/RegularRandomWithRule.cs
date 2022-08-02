using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StoryOfRandom.Core;
using UnityEngine;

namespace StoryOfRandom.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Story of random/Regular random with rule", fileName = "RegularRandomWithRule")]
    public class RegularRandomWithRule : ScriptableObject
    {
        [field: SerializeField]
        public Vector2 RandomRange { get; set; }

        [field: SerializeField]
        private Vector2 SquareSize { get; set; }
        
        [field: SerializeField]
        private GameObject Prefab1 { get; set; }

        [field: SerializeField]
        private GameObject Prefab2 { get; set; }

        public List<GameObject> TrackedObjects { get; set; }

        [Button("Generate with a rule")]
        public void Generate()
        {
            for (int i = 0; i < SquareSize.x; i++)
            {
                for (int j = 0; j < SquareSize.y; j++)
                {
                    if ((i & j) != 0)
                        TrackedObjects.Add(Instantiate(Prefab1, new Vector3(i - (SquareSize.x/2), 0, j - (SquareSize.y/2)), Quaternion.identity));
                    else
                        TrackedObjects.Add(Instantiate(Prefab2, new Vector3(i - (SquareSize.x/2), 0, j - (SquareSize.y/2)), Quaternion.identity));
                }
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
