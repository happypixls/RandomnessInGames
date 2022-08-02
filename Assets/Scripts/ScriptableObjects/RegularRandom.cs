using System.Collections.Generic;
using NaughtyAttributes;
using StoryOfRandom.Core;
using UnityEngine;

namespace StoryOfRandom.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Story of random/Regular random", fileName = "RegularRandom")]
    public class RegularRandom : ScriptableObject
    {
        [field: SerializeField]
        public Vector2 RandomRange { get; set; }
        
        [field: SerializeField]
        private Vector2 SquareSize { get; set; }
        
        [field: SerializeField]
        private GameObject Prefab1;
        
        [field: SerializeField]
        private GameObject Prefab2;

        public List<GameObject> TrackedObjects { get; set; }
        
        [Button("Generate the boring random")]
        public void Generate()
        {
            Clear();
            for (int i = 0; i < SquareSize.x; i++)
            {
                for (int j = 0; j < SquareSize.y; j++)
                {
                    if (Random.Range(RandomRange.x, RandomRange.y) > 0.5f)
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
