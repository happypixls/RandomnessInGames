using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace StoryOfRandom.Behaviours.Randomizers
{
    public class NoiseRandom : MonoBehaviour
    {
        [SerializeField, Tooltip("This is mapped to the scale")]
        private Vector2 randomRange;

        [SerializeField, Tooltip("Works with sporadic methods only")]
        [Range(0.0f, 1.0f)]
        private float scarcityFactor;

        [SerializeField] 
        private Vector2 squareSize;

        [SerializeField] 
        private GameObject prefab1;

        [SerializeField] 
        private GameObject prefab2;

        [SerializeField] 
        private GameObject prefab3;

        [SerializeField] 
        private GameObject prefab4;

        [SerializeField] 
        private GameObject prefab5;
        
        private List<GameObject> trackedObjects { get; set; }

        private void Start() => trackedObjects = new List<GameObject>();
        
        [Button("Generate with perlin noise")]
        public void GeneratePerlin()
        {
            Clear();
            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    float xCoord = 0.5f + i / squareSize.x * randomRange.x;
                    float yCoord = 0.5f + j / squareSize.y * randomRange.y;
                    float sample = Mathf.PerlinNoise(xCoord, yCoord);
                    
                    if (sample > 0.5f)
                        trackedObjects.Add(Instantiate(prefab1, new Vector3(i - (squareSize.x/2), 0, j - (squareSize.y/2)), Quaternion.identity));
                    else
                        trackedObjects.Add(Instantiate(prefab2, new Vector3(i - (squareSize.x/2), 0, j - (squareSize.y/2)), Quaternion.identity));
                }
            }
        }
        
        [Button("Generate with perlin noise sporadic")]
        public void GeneratePerlinSporadic()
        {
            Clear();
            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    if (Random.Range(0.0f, 1.0f) > scarcityFactor)
                    {
                        float xCoord = 0.5f + i / squareSize.x * randomRange.x;
                        float yCoord = 0.5f + j / squareSize.y * randomRange.y;
                        float sample = Mathf.PerlinNoise(xCoord, yCoord);
                        
                        if (sample > 0.5f)
                            trackedObjects.Add(Instantiate(prefab1, new Vector3(i - (squareSize.x/2), 0, j - (squareSize.y/2)), Quaternion.identity));
                        else
                            trackedObjects.Add(Instantiate(prefab2, new Vector3(i - (squareSize.x/2), 0, j - (squareSize.y/2)), Quaternion.identity));
                    }
                }
            }
        }
        
        //Read more about it here:
        //https://forum.unity.com/threads/an-overview-of-noise-functions-in-the-unity-mathematics-package.1098193/
        //This method is purely me playing, experimenting  and having fun,
        //no logic whatsoever behind the design choice nor a reason why it ended up like that :)
        [Button("Generate with simplex noise")]
        public void GenerateSimplex()
        {
            Clear();
            
            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    float xCoord = 0.5f + i / squareSize.x * randomRange.x;
                    float yCoord = 0.5f + j / squareSize.y * randomRange.y;
                    
                    float3 sample = noise.psrdnoise(new float2(xCoord, yCoord), new float2(2, 2));
                    //float3 sample = noise.srdnoise(new float2(xCoord, yCoord), 0) * 1.5f;

                    if (sample.x > 0.3f)
                        trackedObjects.Add(Instantiate(prefab1, new Vector3(i - (squareSize.x/2), sample.x * 1.25f, j - (squareSize.y/2)), Quaternion.identity));
                    else if( sample.y > 0.5f)
                        trackedObjects.Add(Instantiate(prefab2, new Vector3(i - (squareSize.x/2), sample.x * 1.25f, j - (squareSize.y/2)), Quaternion.identity));
                    else if(sample.z > 0.6f)
                        trackedObjects.Add(Instantiate(prefab3, new Vector3(i - (squareSize.x/2), sample.x * 1.25f, j - (squareSize.y/2)), Quaternion.identity));
                    else
                        trackedObjects.Add(Instantiate(prefab4, new Vector3(i - (squareSize.x/2), sample.x * 1.25f, j - (squareSize.y/2)), Quaternion.identity));
                    
                }
            }
        }
        
        [Button("Generate with simplex sporadic")]
        public void GenerateSimplexSporadic()
        {
            Clear();
            
            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    float xCoord = 0.5f + i / squareSize.x * randomRange.x;
                    float yCoord = 0.5f + j / squareSize.y * randomRange.y;
                    
                    float3 sample = noise.psrdnoise(new float2(xCoord, yCoord), new float2(2, 2));
                    //float3 sample = noise.srdnoise(new float2(xCoord, yCoord), 0) * 1.5f;

                    if (sample.x > 0.3f)
                    {
                        if(UnityEngine.Random.Range(0.0f, 1.0f) > scarcityFactor)
                            trackedObjects.Add(Instantiate(prefab1, new Vector3(i - (squareSize.x/2), sample.x * 1.25f, j - (squareSize.y/2)), Quaternion.identity));
                        else
                            trackedObjects.Add(Instantiate(prefab5, new Vector3(i - (squareSize.x/2), sample.x * 1.25f, j - (squareSize.y/2)), Quaternion.identity));
                    }
                    else if (sample.y > 0.5f)
                    {
                        if(UnityEngine.Random.Range(0.0f, 1.0f) > scarcityFactor)
                            trackedObjects.Add(Instantiate(prefab2, new Vector3(i - (squareSize.x/2), sample.x * 1.25f, j - (squareSize.y/2)), Quaternion.identity));
                        else
                            trackedObjects.Add(Instantiate(prefab3, new Vector3(i - (squareSize.x/2), sample.x * 1.25f, j - (squareSize.y/2)), Quaternion.identity));
                    }
                    else if (sample.z > 0.6f)
                    {
                        trackedObjects.Add(Instantiate(prefab3, new Vector3(i - (squareSize.x/2), sample.x * 1.25f, j - (squareSize.y/2)), Quaternion.identity));
                    }
                    else
                    {
                        trackedObjects.Add(Instantiate(prefab4, new Vector3(i - (squareSize.x/2), sample.x * 1.25f, j - (squareSize.y/2)), Quaternion.identity));
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
