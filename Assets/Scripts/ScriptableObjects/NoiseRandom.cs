using System.Collections.Generic;
using NaughtyAttributes;
using StoryOfRandom.Core;
using UnityEngine;
using Unity.Mathematics;

namespace StoryOfRandom.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Story of random/Noise random", fileName = "NoiseRandom")]
    public class NoiseRandom : ScriptableObject
    {
        [field: SerializeField, Tooltip("This is mapped to the scale")]
        public Vector2 RandomRange { get; set; }
        
        [field: SerializeField]
        private Vector2 SquareSize { get; set; }
        
        [field: SerializeField]
        private GameObject Prefab1 { get;set; }
        
        [field: SerializeField]
        private GameObject Prefab2 { get;set; }

        [field: SerializeField]
        private GameObject Prefab3 { get;set; }
        
        [field: SerializeField]
        private GameObject Prefab4 { get;set; }

        [field: SerializeField]
        private GameObject Prefab5 { get;set; }
        
        private List<GameObject> TrackedObjects { get; set; }
        
        [Button("Generate")]
        public void Generate()
        {
            for (int i = 0; i < SquareSize.x; i++)
            {
                for (int j = 0; j < SquareSize.y; j++)
                {
                    float xCoord = 0.5f + i / SquareSize.x * RandomRange.x;
                    float yCoord = 0.5f + j / SquareSize.y * RandomRange.y;
                    float sample = Mathf.PerlinNoise(xCoord, yCoord);
                    
                    if (sample > 0.5f)
                        TrackedObjects.Add(Instantiate(Prefab1, new Vector3(i - (SquareSize.x/2), 0, j - (SquareSize.y/2)), Quaternion.identity));
                    else
                        TrackedObjects.Add(Instantiate(Prefab2, new Vector3(i - (SquareSize.x/2), 0, j - (SquareSize.y/2)), Quaternion.identity));
                }
            }
        }
        
        //Read more about it here:
        //https://forum.unity.com/threads/an-overview-of-noise-functions-in-the-unity-mathematics-package.1098193/
        //This method is purely me playing, experimenting  and having fun,
        //no logic whatsoever behind the design choice nor a reason why it ended up like that :)
        [Button("Generate 3D")]
        public void Generate3D()
        {
            Clear();
            
            for (int i = 0; i < SquareSize.x; i++)
            {
                for (int j = 0; j < SquareSize.y; j++)
                {
                    float xCoord = 0.5f + i / SquareSize.x * RandomRange.x;
                    float yCoord = 0.5f + j / SquareSize.y * RandomRange.y;
                    
                    // float3 sample = noise.psrdnoise(new float2(xCoord, yCoord), new float2(2, 2));
                    float3 sample = noise.srdnoise(new float2(xCoord, yCoord), 0) * 1.5f;

                    if (sample.x > 0.3f)
                        TrackedObjects.Add(Instantiate(Prefab1, new Vector3(i - (SquareSize.x/2), sample.x * 1.25f, j - (SquareSize.y/2)), Quaternion.identity));
                    else if( sample.y > 0.5f)
                        TrackedObjects.Add(Instantiate(Prefab2, new Vector3(i - (SquareSize.x/2), sample.x * 1.25f, j - (SquareSize.y/2)), Quaternion.identity));
                    else if(sample.z > 0.6f)
                        TrackedObjects.Add(Instantiate(Prefab3, new Vector3(i - (SquareSize.x/2), sample.x * 1.25f, j - (SquareSize.y/2)), Quaternion.identity));
                    else
                        TrackedObjects.Add(Instantiate(Prefab4, new Vector3(i - (SquareSize.x/2), sample.x * 1.25f, j - (SquareSize.y/2)), Quaternion.identity));
                    
                }
            }
        }
        
        [Button("Generate 3D less density")]
        public void GenerateLessDensity()
        {
            Clear();
            
            for (int i = 0; i < SquareSize.x; i++)
            {
                for (int j = 0; j < SquareSize.y; j++)
                {
                    float xCoord = 0.5f + i / SquareSize.x * RandomRange.x;
                    float yCoord = 0.5f + j / SquareSize.y * RandomRange.y;
                    
                    // float3 sample = noise.psrdnoise(new float2(xCoord, yCoord), new float2(2, 2));
                    float3 sample = noise.srdnoise(new float2(xCoord, yCoord), 0) * 1.5f;

                    if (sample.x > 0.3f)
                    {
                        if(UnityEngine.Random.Range(0.0f, 1.0f) > 0.7f)
                            TrackedObjects.Add(Instantiate(Prefab1, new Vector3(i - (SquareSize.x/2), sample.x * 1.25f, j - (SquareSize.y/2)), Quaternion.identity));
                        else
                            TrackedObjects.Add(Instantiate(Prefab5, new Vector3(i - (SquareSize.x/2), sample.x * 1.25f, j - (SquareSize.y/2)), Quaternion.identity));
                    }
                    else if (sample.y > 0.5f)
                    {
                        if(UnityEngine.Random.Range(0.0f, 1.0f) > 0.7f)
                            TrackedObjects.Add(Instantiate(Prefab2, new Vector3(i - (SquareSize.x/2), sample.x * 1.25f, j - (SquareSize.y/2)), Quaternion.identity));
                        else
                            TrackedObjects.Add(Instantiate(Prefab3, new Vector3(i - (SquareSize.x/2), sample.x * 1.25f, j - (SquareSize.y/2)), Quaternion.identity));
                    }
                    else if (sample.z > 0.6f)
                    {
                        TrackedObjects.Add(Instantiate(Prefab3, new Vector3(i - (SquareSize.x/2), sample.x * 1.25f, j - (SquareSize.y/2)), Quaternion.identity));
                    }
                    else
                    {
                        TrackedObjects.Add(Instantiate(Prefab4, new Vector3(i - (SquareSize.x/2), sample.x * 1.25f, j - (SquareSize.y/2)), Quaternion.identity));
                    }
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
