using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StoryOfRandom.Behaviours.Randomizers
{
    public class RandomColors : MonoBehaviour
    {
        private static readonly int MainColor = Shader.PropertyToID("_BaseColor");

        [SerializeField] 
        public Vector2 randomRange;

        [SerializeField] 
        private Vector2 squareSize;

        [Space(10)]
        [SerializeField] 
        private Color colorToOffsetFrom;

        [Space(10)]
        [SerializeField] 
        private Color color1;

        [SerializeField] 
        private Color color2;

        [SerializeField] 
        private Color color3;

        [Space(10)]
        [SerializeField] 
        private Gradient colorGradient;

        [Space(10)]
        [SerializeField] 
        private float offset;
        
        [SerializeField] 
        [Range(0.0f, 1.0f)]
        private float greyControl;

        [Space(20)]
        [SerializeField] 
        public GameObject prefab1;

        [SerializeField] 
        public GameObject prefab2;
        
        private List<GameObject> trackedObjects;
        private MaterialPropertyBlock colorPropertyBlock;

        private void Start() => trackedObjects = new List<GameObject>();
        
        [Button("Using Unity's Random")]
        private void UnityRandomHSV()
        {
            Clear();
            
            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    colorPropertyBlock = new MaterialPropertyBlock();
                    
                    var instantiatedObject = Instantiate(prefab1, new Vector3(i - (squareSize.x / 2), 0, j - (squareSize.y / 2)), Quaternion.Euler(0, Random.Range(0, 180), 0));
                    instantiatedObject.transform.localScale = Vector3.one * Random.Range(1.0f, 3.0f);
                    
                    var objectRenderer = instantiatedObject.GetComponentInChildren<MeshRenderer>();
                    
                    objectRenderer.GetPropertyBlock(colorPropertyBlock);
                    colorPropertyBlock.GetColor(MainColor);
                    
                    colorPropertyBlock.SetColor(MainColor, Random.ColorHSV());
                    objectRenderer.SetPropertyBlock(colorPropertyBlock);
                    
                    trackedObjects.Add(instantiatedObject);
                }
            }
        }
        
        [Button("Get random color from gradient")]
        private void GetRandomColorFromGradient()
        {
            Clear();
            
            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    colorPropertyBlock = new MaterialPropertyBlock();
                    
                    var instantiatedObject = Instantiate(prefab1, new Vector3(i - (squareSize.x / 2), 0, j - (squareSize.y / 2)), Quaternion.Euler(0, Random.Range(0, 180), 0));
                    instantiatedObject.transform.localScale = Vector3.one * Random.Range(1.0f, 3.0f);
                    
                    var objectRenderer = instantiatedObject.GetComponentInChildren<MeshRenderer>();
                    
                    objectRenderer.GetPropertyBlock(colorPropertyBlock);
                    colorPropertyBlock.GetColor(MainColor);
                    
                    colorPropertyBlock.SetColor(MainColor, colorGradient.Evaluate(Random.value));
                    objectRenderer.SetPropertyBlock(colorPropertyBlock);
                    
                    trackedObjects.Add(instantiatedObject);
                }
            }
        }
        
        //https://stackoverflow.com/questions/43044/algorithm-to-randomly-generate-an-aesthetically-pleasing-color-palette
        //http://devmag.org.za/2012/07/29/how-to-choose-colours-procedurally-algorithms/
        [Button("Generate colors with random offset from specified color")]
        private void GenerateWithRandomOffset()
        {
            Clear();
            
            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    colorPropertyBlock = new MaterialPropertyBlock();
                    
                    var instantiatedObject = Instantiate(prefab1, new Vector3(i - (squareSize.x / 2), 0, j - (squareSize.y / 2)), Quaternion.Euler(0, Random.Range(0, 180), 0));
                    instantiatedObject.transform.localScale = Vector3.one * Random.Range(1.0f, 3.0f);
                    
                    var objectRenderer = instantiatedObject.GetComponentInChildren<MeshRenderer>();
                    
                    objectRenderer.GetPropertyBlock(colorPropertyBlock);
                    colorPropertyBlock.GetColor(MainColor);
                    
                    var value = (colorToOffsetFrom.r + colorToOffsetFrom.g + colorToOffsetFrom.b)/3;
                    var newValue = value + (2 * Random.Range(randomRange.x, randomRange.y)) * offset - offset;
                    var valueRatio = newValue / value;
                    var newColor = new Color(colorToOffsetFrom.r * valueRatio, colorToOffsetFrom.g * valueRatio, colorToOffsetFrom.b * valueRatio);

                    colorPropertyBlock.SetColor(MainColor, newColor);
                    objectRenderer.SetPropertyBlock(colorPropertyBlock);
                    
                    trackedObjects.Add(instantiatedObject);
                }
            }
        }
        
        //https://stackoverflow.com/questions/43044/algorithm-to-randomly-generate-an-aesthetically-pleasing-color-palette
        //http://devmag.org.za/2012/07/29/how-to-choose-colours-procedurally-algorithms/
        [Button("Generate colors with golden ratio using gradient")]
        private void GenerateColorsWithGoldenRatioWithGradient()
        {
            Clear();

            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    colorPropertyBlock = new MaterialPropertyBlock();
                    
                    var instantiatedObject = Instantiate(prefab2, new Vector3(i - (squareSize.x / 2), 0, j - (squareSize.y / 2)), Quaternion.Euler(0, Random.Range(0, 180), 0));
                    instantiatedObject.transform.localScale = Vector3.one * Random.Range(1.0f, 3.0f);
                    
                    var objectRenderer = instantiatedObject.GetComponentInChildren<MeshRenderer>();
                    
                    objectRenderer.GetPropertyBlock(colorPropertyBlock, 2);
                    colorPropertyBlock.GetColor(MainColor);

                    var newColor = colorGradient.Evaluate(Random.Range(-0.5f, 0.5f) + (0.618033988749895f * j) % 1);  
                    
                    colorPropertyBlock.SetColor(MainColor, newColor);
                    objectRenderer.SetPropertyBlock(colorPropertyBlock, 2);
                    
                    trackedObjects.Add(instantiatedObject);
                }
            }
        }
        
        //https://stackoverflow.com/questions/43044/algorithm-to-randomly-generate-an-aesthetically-pleasing-color-palette
        //http://devmag.org.za/2012/07/29/how-to-choose-colours-procedurally-algorithms/
        [Button("Generate colors with triad mixing")]
        private void GenerateColorsTriadMixing()
        {
            Clear();

            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    colorPropertyBlock = new MaterialPropertyBlock();
                    
                    var instantiatedObject = Instantiate(prefab1, new Vector3(i - (squareSize.x / 2), 0, j - (squareSize.y / 2)),Quaternion.Euler(0, Random.Range(0, 180), 0));
                    
                    instantiatedObject.transform.localScale = Vector3.one * Random.Range(1.0f, 3.0f);
                    
                    var objectRenderer = instantiatedObject.GetComponentInChildren<MeshRenderer>();
                    
                    objectRenderer.GetPropertyBlock(colorPropertyBlock);
                    colorPropertyBlock.GetColor(MainColor);

                    var index = Random.Range(0, 3);
                    
                    float mixRatio1 = (index == 0) ? Random.Range(0.0f, 1.1f) * greyControl : Random.Range(0.0f, 1.1f);

                    float mixRatio2 = 
                        (index == 1) ? Random.Range(0.0f, 1.1f) * greyControl : Random.Range(0.0f, 1.1f);

                    float mixRatio3 = 
                        (index == 2) ? Random.Range(0.0f, 1.1f) * greyControl : Random.Range(0.0f, 1.1f);

                    float sum = mixRatio1 + mixRatio2 + mixRatio3;

                    mixRatio1 /= sum;
                    mixRatio2 /= sum;
                    mixRatio3 /= sum;

                    var newColor = new Color(
                        (mixRatio1 * color1.r + mixRatio2 * color2.r + mixRatio3 * color3.r),
                        (mixRatio1 * color1.g + mixRatio2 * color2.g + mixRatio3 * color3.g),
                        (mixRatio1 * color1.b + mixRatio2 * color2.b + mixRatio3 * color3.b),
                        1.0f);
                    
                    colorPropertyBlock.SetColor(MainColor, newColor);
                    objectRenderer.SetPropertyBlock(colorPropertyBlock);
                    
                    trackedObjects.Add(instantiatedObject);
                }
            }
        }
        
        //http://gurneyjourney.blogspot.com/2008/02/shapes-of-color-schemes.html
        [Button("Generate colors with Gurney scheme")]
        private void GenerateColorsWithGurneyScheme()
        {
            Clear();

            for (int i = 0; i < squareSize.x; i++)
            {
                for (int j = 0; j < squareSize.y; j++)
                {
                    colorPropertyBlock = new MaterialPropertyBlock();
                    
                    var instantiatedObject = Instantiate(prefab1, new Vector3(i - (squareSize.x / 2), 0, j - (squareSize.y / 2)), Quaternion.Euler(0, Random.Range(0, 180), 0));
                    instantiatedObject.transform.localScale = Vector3.one * Random.Range(1.0f, 3.0f);
                    
                    var objectRenderer = instantiatedObject.GetComponentInChildren<MeshRenderer>();
                    
                    objectRenderer.GetPropertyBlock(colorPropertyBlock);
                    colorPropertyBlock.GetColor(MainColor);

                    var newColor = Color.Lerp(color1, color2, Random.value);
                    newColor = Color.Lerp(newColor, color3, Random.value);
                    
                    colorPropertyBlock.SetColor(MainColor, newColor);
                    objectRenderer.SetPropertyBlock(colorPropertyBlock);
                    
                    trackedObjects.Add(instantiatedObject);
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
