using System;
using System.Collections.Generic;
using UnityEngine;

namespace StoryOfRandom.Core.WFC
{
    public class HexGrid<T>
    {
        private readonly float _hexVerticalOffset;
        public List<T> HexCells { get; set; }
        public Vector2Int GridSize { get; private set; }
        public float CellSize { get; private set; }
        public Vector3 OriginPosition { get; private set; }

        public event Action<T, Position> EdgeSideDetected;

        public HexGrid(Vector2Int gridSize, float cellSize, Vector3 originPosition, Func<Vector3, Vector2, T> instantiationProcedure, float hexVerticalOffset = 0.87f)
        {
            this.GridSize = gridSize;
            this.CellSize = cellSize;
            this.OriginPosition = originPosition;
            _hexVerticalOffset = hexVerticalOffset;

            HexCells = new List<T>();
            
            ConstructGrid(instantiationProcedure);
        }

        private Vector3 GetWorldPosition(int x, int z)
        {
            return new Vector3(x, 0, 0) * CellSize +
                        new Vector3(0, 0, z) * CellSize * _hexVerticalOffset +
                        ((z % 2) == 1 ? new Vector3(1, 0, 0) * CellSize * 0.5f
                        : Vector3.zero);
        }
        
        private bool IsIndexOutOfBounds(int index) => index < 0 ||  index >= HexCells.Count;
        private bool IsIJWithinBounds(int i, int j) => (i >= 0 && i < GridSize.x) && (j >= 0 && j < GridSize.y);

        public List<T> GetNeighborsOfTileAtIndex(int i, int j) => GetNeighborsOfTileAtIndex(i * GridSize.y + j);
        
        public List<T> GetNeighborsOfTileAtIndex(int index)
        {
            int i = index / GridSize.y; //i -> column
            int j = index % GridSize.y; //j -> row

            var neighbors = new List<T>();
            bool isEvenColumn = i % 2 == 0;
            
            if (IsIndexOutOfBounds(index))
                return neighbors;
            
            if (!isEvenColumn)
            {
                //first number is i and second is j
                //Top => 0, -1
                var topIndex = i * GridSize.y + (j - 1);
                if (IsIJWithinBounds(i, j - 1))
                    neighbors.Add(HexCells[topIndex]);
                else
                    EdgeSideDetected(HexCells[index], Position.Top);
                
                //TopLeft -1, 0
                var topLeft = (i - 1) * GridSize.y + j;
                if(IsIJWithinBounds(i - 1, j))
                    neighbors.Add(HexCells[topLeft]);
                else
                    EdgeSideDetected(HexCells[index], Position.TopLeft);
                
                //BottomLeft -1, +1
                var bottomLeft = (i - 1) * GridSize.y + (j + 1);
                if(IsIJWithinBounds(i - 1, j + 1))
                    neighbors.Add(HexCells[bottomLeft]);
                else
                    EdgeSideDetected(HexCells[index], Position.BottomLeft);
                
                //Bottom 0, +1
                var bottomIndex = i * GridSize.y + (j + 1);
                if(IsIJWithinBounds(i, j + 1))
                    neighbors.Add(HexCells[bottomIndex]);
                else
                    EdgeSideDetected(HexCells[index], Position.Bottom);
                
                //BottomRight +1, +1
                var bottomRightIndex = (i + 1) * GridSize.y + (j + 1);
                if(IsIJWithinBounds(i + 1, j + 1))
                    neighbors.Add(HexCells[bottomRightIndex]);
                else
                    EdgeSideDetected(HexCells[index], Position.BottomRight);
                
                //TopRight +1, 0
                var topRightIndex = (i + 1) * GridSize.y + j;
                if(IsIJWithinBounds(i + 1, j))
                    neighbors.Add(HexCells[topRightIndex]);
                else
                    EdgeSideDetected(HexCells[index], Position.TopRight);
            }
            else
            {
                //Top => 0, -1
                var topIndex = i * GridSize.y + (j - 1);
                if(IsIJWithinBounds(i, j - 1))
                    neighbors.Add(HexCells[topIndex]);
                else
                    EdgeSideDetected(HexCells[index], Position.Top);
                
                //TopLeft => -1, -1
                var topLeft = (i - 1) * GridSize.y + (j - 1);
                if(IsIJWithinBounds(i - 1, j - 1))
                    neighbors.Add(HexCells[topLeft]);
                else
                    EdgeSideDetected(HexCells[index], Position.TopLeft);
                
                //BottomLeft => -1, 0
                var bottomLeft = (i - 1) * GridSize.y + j;
                if(IsIJWithinBounds(i - 1, j))
                    neighbors.Add(HexCells[bottomLeft]);
                else
                    EdgeSideDetected(HexCells[index], Position.BottomLeft);
                
                //Bottom = 0, +1
                var bottomIndex = i * GridSize.y + (j + 1);
                if(IsIJWithinBounds(i , j + 1))
                    neighbors.Add(HexCells[bottomIndex]);
                else
                    EdgeSideDetected(HexCells[index], Position.Bottom);
                
                //BottomRight => +1, 0
                var bottomRightIndex = (i + 1) * GridSize.y + j;
                if(IsIJWithinBounds(i + 1, j))
                    neighbors.Add(HexCells[bottomRightIndex]);
                else
                    EdgeSideDetected(HexCells[index], Position.BottomRight);
                
                //TopRight => +1, -1
                var topRightIndex = (i + 1) * GridSize.y + (j - 1);
                if(IsIJWithinBounds(i + 1, j - 1))
                    neighbors.Add(HexCells[topRightIndex]);
                else
                    EdgeSideDetected(HexCells[index], Position.TopRight);
            }
            
            return neighbors;
        }
        
        private void ConstructGrid(Func<Vector3, Vector2, T> instantiationProcedure)
        {
            for (int i = 0; i < GridSize.x; i++)
            {
                for (int j = 0; j < GridSize.y; j++)
                {
                    HexCells.Add(instantiationProcedure(GetWorldPosition(j, i), new Vector2(i, j)));
                }
            }
        }
    }
}
