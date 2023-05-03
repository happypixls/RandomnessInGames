using UnityEngine;

namespace StoryOfRandom.Behaviours.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] 
        private float dragSpeed;

        [SerializeField] 
        private float scrollSpeed;

        private Camera currentCamera;
        
        private Vector3 dragOrigin;
        private Vector3 moveOrigin;
        
        private void Start() => currentCamera = this.GetComponent<Camera>();

        private void Update()
        {
            this.transform.position += new Vector3(0, Input.mouseScrollDelta.y * scrollSpeed, 0);
            
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
                moveOrigin = transform.position;
                return;
            }

            if (!Input.GetMouseButton(0)) return;

            var position = currentCamera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            var move = new Vector3(position.x * dragSpeed, 0, position.y * dragSpeed);

            transform.position = moveOrigin - move;
        }
    }
}
