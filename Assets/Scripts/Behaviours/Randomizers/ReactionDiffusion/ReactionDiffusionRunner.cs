using UnityEngine;

namespace StoryOfRandom.Behaviours.Randomizers.ReactionDiffusion
{
    public class ReactionDiffusionRunner : MonoBehaviour
    {
        [SerializeField]
        private int width = 512;
        [SerializeField]
        private int height = 512;

        [SerializeField]
        private ReactionDiffusionConfiguration config;

        [SerializeField]
        private Terrain terrain;

        [SerializeField]
        private ComputeShader textureUpdaterComputeShader;
        
        [SerializeField]
        private bool invertColors;
        
        [SerializeField]
        private bool enableRDToTerrain;
        
        private TerrainData terrainData;
        private Texture2D texture;
        private float[,] heights;
        private Color[] pixels;
        
        private int kernelId;

        private const int ThreadGroupSize = 8;
        private const int ThreadGroupZ = 1;

        private ComputeBuffer currentGridBuffer;
        private ComputeBuffer nextGridBuffer;
        
        private RDCellS[] gridData;
        private Color[] colors;
        
        private void Start()
        {
            InitialTexture();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            var renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = texture;
            terrainData = terrain.terrainData;
            terrainData.size = new Vector3(width, height, width);
            
            heights = new float[width, height];
            
            colors = new Color[width * height];
            gridData = new RDCellS[width * height];
            
            RDCellS[] initialGrid = new RDCellS[width * height];
            for (int i = 0; i < width * height; i++)
            {
                initialGrid[i] = new RDCellS(1.0f, 0.0f);
            }

            for (int i = 0; i < 5; i++)
            {
                var pointX = Random.Range(0, width - 1);
                var pointY = Random.Range(0, height - 1);

                initialGrid[pointX * width + pointY].b = 1.0f;
                initialGrid[(pointX + 1) * width + pointY].b = 1.0f;
                initialGrid[pointX * width + pointY + 1].b = 1.0f;
                initialGrid[(pointX + 1) * width + pointY + 1].b = 1.0f;
            }

            currentGridBuffer = new ComputeBuffer(width * height, 2 * sizeof(float), ComputeBufferType.Structured);
            currentGridBuffer.SetData(initialGrid);

            nextGridBuffer = new ComputeBuffer(width * height, 2 * sizeof(float), ComputeBufferType.Structured);

            kernelId = textureUpdaterComputeShader.FindKernel("TextureUpdater");
            textureUpdaterComputeShader.SetBuffer(kernelId, "currentGrid", currentGridBuffer);
            textureUpdaterComputeShader.SetBuffer(kernelId, "nextGrid", nextGridBuffer);
            textureUpdaterComputeShader.SetInt("width", width);
            textureUpdaterComputeShader.SetInt("height", height);
            textureUpdaterComputeShader.SetFloat("dA", config.dA);
            textureUpdaterComputeShader.SetFloat("dB", config.dB);
            textureUpdaterComputeShader.SetFloat("f", config.f);
            textureUpdaterComputeShader.SetFloat("k", config.k);
        }

        private void InitialTexture()
        {
            texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

            texture.filterMode = FilterMode.Bilinear;
            texture.Apply();
        }

        private void Update()
        {
            //This is being set here in order to modify values in realtime
            textureUpdaterComputeShader.SetFloat("dA", config.dA);
            textureUpdaterComputeShader.SetFloat("dB", config.dB);
            textureUpdaterComputeShader.SetFloat("f", config.f);
            textureUpdaterComputeShader.SetFloat("k", config.k);
            
            GenerateNextGrid();
        }

        private void GenerateNextGrid()
        {
            textureUpdaterComputeShader.Dispatch(kernelId, width / ThreadGroupSize, height / ThreadGroupSize, ThreadGroupZ);

            nextGridBuffer.GetData(gridData);
            
            for (int index = 0; index < width * height; index++)
            {
                int i = index % width;
                int j = index / width;
                int gridIndex = j * width + i;
                
                if(!invertColors)
                    colors[gridIndex] = new Color(gridData[gridIndex].b, gridData[gridIndex].b, gridData[gridIndex].b);
                else
                    colors[gridIndex] = new Color(1 - gridData[gridIndex].b, 1 - gridData[gridIndex].b, 1 - gridData[gridIndex].b);
                if(enableRDToTerrain)
                    heights[i, j] = colors[gridIndex].grayscale * 0.1f;
            }

            texture.SetPixels(colors);
            texture.Apply();
            
            if(enableRDToTerrain)
                terrainData.SetHeights(0, 0, heights);
            
            currentGridBuffer.SetData(gridData);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    heights[i, j] = 0.0f;
            
            terrainData.SetHeights(0, 0, heights);
            currentGridBuffer.Release();
            nextGridBuffer.Release();
        }
    }
}
