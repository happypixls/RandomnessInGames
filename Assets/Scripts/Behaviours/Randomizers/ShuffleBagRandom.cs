using System.Collections.Generic;
using NaughtyAttributes;
using StoryOfRandom.Core.Collections;
using UnityEngine;

namespace StoryOfRandom.Behaviours.Randomizers
{
    public class ShuffleBagRandom : MonoBehaviour
    {
        [field: SerializeField]
        private Vector3Int ratios { get; set; }

        [field: SerializeField] 
        private GameObject prefab1;

        [field: SerializeField] 
        private GameObject prefab2;

        [field: SerializeField] 
        private GameObject prefab3;

        private List<GameObject> trackedObjects;
        private ShuffleBag<GameObject> shuffle;

        private void Start()
        {
            shuffle = new ShuffleBag<GameObject>();
            trackedObjects = new List<GameObject>();
        }

        [Button("Generate")]
        private void Generate()
        {
            Clear();
            shuffle = new ShuffleBag<GameObject>();
            shuffle.Add(prefab1, ratios.x);
            shuffle.Add(prefab2, ratios.y);
            shuffle.Add(prefab3, ratios.z);

            for (int i = 0; i < 10; i++)
                trackedObjects.Add(Instantiate(shuffle.Next(), new Vector3(i - 1, 0, 0), Quaternion.identity));
        }

        private void Clear()
        {
            foreach (var item in trackedObjects)
                Destroy(item);

            trackedObjects.Clear();
            shuffle.Clear();
        }
    }
}
