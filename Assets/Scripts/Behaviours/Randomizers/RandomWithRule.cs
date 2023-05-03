using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace StoryOfRandom.Behaviours.Randomizers
{
    public class RandomWithRule : MonoBehaviour
    {
        [BoxGroup("Random values config")] 
        [SerializeField]
        private Vector2 squareSize;
        
        [BoxGroup("Objects to generate")] 
        [SerializeField]
        private List<GameObject> prefabs;

        private List<GameObject> trackedObjects;
        
        private void Start() =>
            trackedObjects = new List<GameObject>();
        
        [Button("Generate with a rule")]
        public void Generate()
        {
            Clear();
            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    if ((i & j) != 0)
                        trackedObjects.Add(Instantiate(prefabs[0], new Vector3(i - (squareSize.x/2), 0, j - (squareSize.y/2)), Quaternion.identity));
                    else
                        trackedObjects.Add(Instantiate(prefabs[1], new Vector3(i - (squareSize.x/2), 0, j - (squareSize.y/2)), Quaternion.identity));
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
