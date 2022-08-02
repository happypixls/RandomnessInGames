using UnityEngine;

namespace StoryOfRandom.Behaviours.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [field: SerializeField]
        private float DragSpeed { get; set; }
        
        [field: SerializeField]
        private float ScrollSpeed { get; set; }
        
        private void Update()
        {
            this.transform.position += new Vector3(0, Input.mouseScrollDelta.y * ScrollSpeed, 0);
        }
    }
}
