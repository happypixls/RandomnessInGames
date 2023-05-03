using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StoryOfRandom.Behaviours.Randomizers
{
    public class TheBoringRandom : MonoBehaviour
    {
        [BoxGroup("Random values config")]
        [SerializeField] 
        private Vector2 randomRange;

        [BoxGroup("Random values config")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float bias;
        
        [BoxGroup("World config")]
        [SerializeField] 
        private Vector2 squareSize;

        [BoxGroup("Objects to generate")] 
        [SerializeField]
        private List<GameObject> prefabs;

        [BoxGroup("Generate v2 config")] 
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float shouldInstantiateBias;
        
        private List<GameObject> trackedObjects;

        private void Start() =>
            trackedObjects = new List<GameObject>();

        [Button("Generate with biased coin flip")]
        private void Generate()
        {
            Clear();
            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    if (Random.Range(randomRange.x, randomRange.y) > bias)
                        trackedObjects.Add(Instantiate(prefabs[0], new Vector3(i - (squareSize.x/2), 0, j - (squareSize.y/2)), Quaternion.identity));
                    else
                        trackedObjects.Add(Instantiate(prefabs[1], new Vector3(i - (squareSize.x/2), 0, j - (squareSize.y/2)), Quaternion.identity));
                }
            }
        }
        
        [Button("Generate with with coin and instantiation bias")]
        private void GenerateSporadic()
        {
            Clear();
            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    if (Random.Range(0.0f, 1.0f) > shouldInstantiateBias)
                    {
                        if (Random.Range(randomRange.x, randomRange.y) > bias)
                            trackedObjects.Add(Instantiate(prefabs[0], new Vector3(i - (squareSize.x/2), 0, j - (squareSize.y/2)), Quaternion.identity));
                        else
                            trackedObjects.Add(Instantiate(prefabs[1], new Vector3(i - (squareSize.x/2), 0, j - (squareSize.y/2)), Quaternion.identity));
                    }
                }
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
