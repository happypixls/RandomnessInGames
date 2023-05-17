using System.Collections.Generic;
using System.Linq;
using StoryOfRandom.Core.WFC;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StoryOfRandom.Behaviours.Randomizers.WFC
{
    public class MapGenerator: MonoBehaviour
    {
        [SerializeField]
        private Vector2Int gridSize;

        [SerializeField]
        private GameObject defaultTile;
        
        [SerializeField]
        private List<GameObject> tiles;
        
        private Dictionary<TerrainEdgeType, List<TerrainEdgeType>> ruleSet;

        private HexGrid<GameObject> hexGrid;

        private void Start()
        {
            // hexGrid = new HexGrid<GameObject>(gridSize, 1, Vector3.zero,
            //     (tilePosition, index) =>
            //     {
            //         var go = Instantiate(defaultTile,
            //             tilePosition + new Vector3((-gridSize.x / 2), 0, (-gridSize.y / 2)), Quaternion.identity);
            //         go.name = $"[{index.x * gridSize.y + index.y}]({index.x}, {index.y})";
            //         return go;
            //     });
            
            hexGrid = new HexGrid<GameObject>(gridSize, 1, Vector3.zero,
                (tilePosition, index) =>
                {
                    var go = Instantiate(tiles.PickRandomElement(),
                        tilePosition, Quaternion.identity);
                    go.name = $"[{index.x * gridSize.y + index.y}]({index.x}, {index.y})";
                    return go;
                });
            
            hexGrid.EdgeSideDetected += (tile, position) =>
            {
                var tileSides = tile.GetComponent<Tile>().tileSides;
                tileSides.Remove(tileSides.Single(t => t.SidePosition == position));
            };

            // var lst = hexGrid.GetNeighborsOfTileAtIndex(560); //knowing the index in the array
            var lst = hexGrid.GetNeighborsOfTileAtIndex(18, 46); //knowing the column and row location
            
            for (int i = 0; i < lst.Count; i++)
                lst[i].transform.position += (Vector3.up);

            Random.InitState(12); //13 is interesting -- 12 has rivers on smaller 5x5 grid, good for debugging
        }

        //Debugging purpose, to be removed later
        private int previousX = -1;
        private int previousY = -1;
        private List<GameObject> lstNeighbors = new List<GameObject>();
        public void ShowNeighbours(int x, int y)
        {
            if (previousX == x || previousY == y)
                return;

            previousX = x;
            previousY = y;

            for (int i = 0; i < lstNeighbors.Count; i++)
                lstNeighbors[i].transform.position += Vector3.down;


            lstNeighbors = hexGrid.GetNeighborsOfTileAtIndex(x, y); //knowing the column and row location
            
            for (int i = 0; i < lstNeighbors.Count; i++)
                lstNeighbors[i].transform.position += Vector3.up;
        }
        
        private void InitializeRulesSet()
        {
            //It is obviously redundant, however I've left it as a place holder to modify the rules later
            ruleSet = new Dictionary<TerrainEdgeType, List<TerrainEdgeType>>
            {
                {
                    TerrainEdgeType.Water, new List<TerrainEdgeType>()
                    {
                        TerrainEdgeType.Dirt,
                        TerrainEdgeType.Grass,
                        TerrainEdgeType.Stone,
                        TerrainEdgeType.Sand,
                        TerrainEdgeType.Water
                    }
                },
                {
                    TerrainEdgeType.Sand, new List<TerrainEdgeType>()
                    {
                        TerrainEdgeType.Dirt,
                        TerrainEdgeType.Grass,
                        TerrainEdgeType.Stone,
                        TerrainEdgeType.Sand,
                        TerrainEdgeType.Water
                    }
                },
                {
                    TerrainEdgeType.Stone, new List<TerrainEdgeType>()
                    {
                        TerrainEdgeType.Dirt,
                        TerrainEdgeType.Grass,
                        TerrainEdgeType.Stone,
                        TerrainEdgeType.Sand,
                        TerrainEdgeType.Water
                    }
                },
                {
                    TerrainEdgeType.Grass, new List<TerrainEdgeType>()
                    {
                        TerrainEdgeType.Dirt,
                        TerrainEdgeType.Grass,
                        TerrainEdgeType.Stone,
                        TerrainEdgeType.Sand,
                        TerrainEdgeType.Water
                    }
                },
                {
                    TerrainEdgeType.Dirt, new List<TerrainEdgeType>()
                    {
                        TerrainEdgeType.Dirt,
                        TerrainEdgeType.Grass,
                        TerrainEdgeType.Stone,
                        TerrainEdgeType.Sand,
                        TerrainEdgeType.Water
                    }
                },
                {
                    TerrainEdgeType.River, new List<TerrainEdgeType>()
                    {
                        TerrainEdgeType.River,
                        TerrainEdgeType.Water
                    }
                }
            };
        }
        
    }
}
