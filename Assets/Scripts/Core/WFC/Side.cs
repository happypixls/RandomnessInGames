using System;
using UnityEngine;

namespace StoryOfRandom.Core.WFC
{
    [Serializable]
    public class Side
    {
        [field: SerializeField]
        public TerrainEdgeType TerrainEdge { get; set; }
        
        [field: SerializeField]
        public Position SidePosition { get; set; }
        
        [field: SerializeField]
        public Transform SnapPosition { get; set; }
    }
}
