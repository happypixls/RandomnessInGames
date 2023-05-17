using System;
using System.Collections;
using System.Collections.Generic;
using StoryOfRandom.Core.WFC;
using UnityEngine;

namespace StoryOfRandom.Behaviours.Randomizers.WFC.Debuggers
{
    public class CellSelector : MonoBehaviour
    {
        private Vector3 point;
        private Camera currentCamera;

        private MapGenerator mapGenerator;
        
        private void Start()
        {
            currentCamera = Camera.main;
            mapGenerator = this.GetComponent<MapGenerator>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = Input.mousePosition;
                mousePos.z = currentCamera.transform.position.y;
                point = currentCamera.ScreenToWorldPoint(mousePos);

                // Needs refinement
                var x = Mathf.RoundToInt(point.x / 1);
                var y = Mathf.RoundToInt(point.z / 1 / 0.87f);
                
                mapGenerator.ShowNeighbours(y, x); // it is inverted because the map itself is inverted
            }
        }
    }
}
