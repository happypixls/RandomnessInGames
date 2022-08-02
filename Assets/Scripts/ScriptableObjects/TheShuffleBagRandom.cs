using System;
using System.Collections.Generic;
using NaughtyAttributes;
using StoryOfRandom.Core;
using UnityEngine;

namespace StoryOfRandom.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Story of random/Shuffle bag", fileName = "ShuffleBag")]
    public class TheShuffleBagRandom : ScriptableObject
    {
        public Vector2 RandomRange { get; set; }
        
        public List<GameObject> TrackedObjects { get; set; }
        
        [field: SerializeField]
        private Vector3Int Ratios { get; set; }
        
        [field: SerializeField]
        private GameObject Prefab1 { get; set; }
        
        [field: SerializeField]
        private GameObject Prefab2 { get; set; }
        
        [field: SerializeField]
        private GameObject Prefab3 { get; set; }
        private ShuffleBag<GameObject> Shuffle { get; set; } = new ShuffleBag<GameObject>();

        [Button("Generate")]
        public void Generate()
        {
            Clear();
            Shuffle = new ShuffleBag<GameObject>();
            Shuffle.Add(Prefab1, Ratios.x);
            Shuffle.Add(Prefab2, Ratios.y);
            Shuffle.Add(Prefab3, Ratios.z);

            for (int i = 0; i < 6; i++)
                TrackedObjects.Add(Instantiate(Shuffle.Next(), new Vector3(i, 0, 0), Quaternion.identity));
        }

        public void Clear()
        {
            foreach (var item in TrackedObjects)
                Destroy(item);

            TrackedObjects.Clear();
            Shuffle.Clear();
        }
    }
}
