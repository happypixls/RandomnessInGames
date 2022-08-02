using System.Collections.Generic;
using NaughtyAttributes;
using StoryOfRandom.Core;
using UnityEngine;

namespace StoryOfRandom.ScriptableObjects
{
    //Code from http://devmag.org.za/2012/07/29/how-to-choose-colours-procedurally-algorithms/
    [CreateAssetMenu(menuName = "Story of random/Random colors", fileName = "RandomColors")]
    public class RandomColors : ScriptableObject
    {
        private static readonly int MainColor = Shader.PropertyToID("_BaseColor");

        [field: SerializeField]
        public Vector2 RandomRange { get; set; }
        
        [field: SerializeField]
        private Vector2 SquareSize { get; set; }
        
        [field: SerializeField]
        private Color ColorToOffsetFrom { get; set; }
        
        [field: SerializeField]
        private Color Color1 { get; set; }
        
        [field: SerializeField]
        private Color Color2 { get; set; }
        
        [field: SerializeField]
        private Color Color3 { get; set; }
        
        [field: SerializeField]
        private Gradient ColorGradient { get; set; }
        
        [field: SerializeField]
        private float Offset { get; set; }

        [SerializeField] 
        [Range(0.0f, 1.0f)]
        private float GreyControl;
        
        [field: SerializeField]
        public GameObject Prefab1 { get; set; }
        
        [field: SerializeField]
        public GameObject Prefab2 { get; set; }
        
        [field: SerializeField]
        private GameObject Ground { get; set; }
        
        private List<GameObject> TrackedObjects { get; set; }
        private MaterialPropertyBlock ColorPropertyBlock { get; set; }
        
        public void Generate()
        {
        }

        [Button("Generate with random offset")]
        private void RandomOffset()
        {
            Clear();
            TrackedObjects.Add(Instantiate(Ground, new Vector3(-0.5f, -10, -0.5f), Quaternion.identity));
            for (int i = 0; i < SquareSize.x; i++)
            {
                for (int j = 0; j < SquareSize.y; j++)
                {
                    ColorPropertyBlock = new MaterialPropertyBlock();
                    
                    var InstantiatedObject = Instantiate(Prefab1, new Vector3(i - (SquareSize.x / 2), 0, j - (SquareSize.y / 2)), Quaternion.Euler(0, Random.Range(0, 180), 0));
                    InstantiatedObject.transform.localScale = Vector3.one * Random.Range(1.0f, 3.0f);
                    
                    var ObjectRenderer = InstantiatedObject.GetComponentInChildren<MeshRenderer>();
                    
                    ObjectRenderer.GetPropertyBlock(ColorPropertyBlock);
                    ColorPropertyBlock.GetColor(MainColor);
                    
                    var Value = (ColorToOffsetFrom.r + ColorToOffsetFrom.g + ColorToOffsetFrom.b)/3;
                    var NewValue = Value + (2 * Random.Range(RandomRange.x, RandomRange.y)) * Offset - Offset;
                    var ValueRatio = NewValue / Value;
                    var NewColor = new Color(ColorToOffsetFrom.r * ValueRatio, ColorToOffsetFrom.g * ValueRatio, ColorToOffsetFrom.b * ValueRatio);
                    
                    ColorPropertyBlock.SetColor(MainColor, NewColor);
                    ObjectRenderer.SetPropertyBlock(ColorPropertyBlock);
                    
                    TrackedObjects.Add(InstantiatedObject);
                }
            }
        }

        [Button("Golden ratio with gradient")]
        private void GoldenRatioWithGradient()
        {
            Clear();
            TrackedObjects.Add(Instantiate(Ground, new Vector3(-0.5f, -10, -0.5f), Quaternion.identity));

            for (int i = 0; i < SquareSize.x; i++)
            {
                for (int j = 0; j < SquareSize.y; j++)
                {
                    ColorPropertyBlock = new MaterialPropertyBlock();
                    
                    var InstantiatedObject = Instantiate(Prefab2, new Vector3(i - (SquareSize.x / 2), 0, j - (SquareSize.y / 2)), Quaternion.Euler(0, Random.Range(0, 180), 0));
                    InstantiatedObject.transform.localScale = Vector3.one * Random.Range(1.0f, 3.0f);
                    
                    var ObjectRenderer = InstantiatedObject.GetComponentInChildren<MeshRenderer>();
                    
                    ObjectRenderer.GetPropertyBlock(ColorPropertyBlock, 2);
                    ColorPropertyBlock.GetColor(MainColor);

                    var NewColor = ColorGradient.Evaluate(Random.Range(-0.5f, 0.5f) + (0.618033988749895f * j) % 1);  
                    
                    ColorPropertyBlock.SetColor(MainColor, NewColor);
                    ObjectRenderer.SetPropertyBlock(ColorPropertyBlock, 2);
                    
                    TrackedObjects.Add(InstantiatedObject);
                }
            }
        }
        
        
        [Button("Triad mixing")]
        private void TriadMixing()
        {
            Clear();
            TrackedObjects.Add(Instantiate(Ground, new Vector3(-0.5f, -10, -0.5f), Quaternion.identity));

            for (int i = 0; i < SquareSize.x; i++)
            {
                for (int j = 0; j < SquareSize.y; j++)
                {
                    ColorPropertyBlock = new MaterialPropertyBlock();
                    
                    var InstantiatedObject = Instantiate(Prefab1, new Vector3(i - (SquareSize.x / 2), 0, j - (SquareSize.y / 2)),Quaternion.Euler(0, Random.Range(0, 180), 0));
                    
                    InstantiatedObject.transform.localScale = Vector3.one * Random.Range(1.0f, 3.0f);
                    
                    var ObjectRenderer = InstantiatedObject.GetComponentInChildren<MeshRenderer>();
                    
                    ObjectRenderer.GetPropertyBlock(ColorPropertyBlock);
                    ColorPropertyBlock.GetColor(MainColor);

                    var Index = Random.Range(0, 3);
                    
                    float MixRatio1 = (Index == 0) ? Random.Range(0.0f, 1.1f) * GreyControl : Random.Range(0.0f, 1.1f);

                    float MixRatio2 = 
                        (Index == 1) ? Random.Range(0.0f, 1.1f) * GreyControl : Random.Range(0.0f, 1.1f);

                    float MixRatio3 = 
                        (Index == 2) ? Random.Range(0.0f, 1.1f) * GreyControl : Random.Range(0.0f, 1.1f);

                    float sum = MixRatio1 + MixRatio2 + MixRatio3;

                    MixRatio1 /= sum;
                    MixRatio2 /= sum;
                    MixRatio3 /= sum;

                    var NewColor = new Color(
                        (MixRatio1 * Color1.r + MixRatio2 * Color2.r + MixRatio3 * Color3.r),
                        (MixRatio1 * Color1.g + MixRatio2 * Color2.g + MixRatio3 * Color3.g),
                        (MixRatio1 * Color1.b + MixRatio2 * Color2.b + MixRatio3 * Color3.b),
                        1.0f);
                    
                    ColorPropertyBlock.SetColor(MainColor, NewColor);
                    ObjectRenderer.SetPropertyBlock(ColorPropertyBlock);
                    
                    TrackedObjects.Add(InstantiatedObject);
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
