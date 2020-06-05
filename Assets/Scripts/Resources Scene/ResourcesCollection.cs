using UnityEngine;

namespace Resources_Scene
{
    public class ResourcesCollection : MonoBehaviour
    {

        //this script is attached to the player
        // it uses ray cast to identify if any objects are in front of the player
        // on a limited range
        
        [SerializeField] private GameObject collectInputMessage;
        private string _collectedItemName;
        private bool _itemCollected;

        private void Start()
        {
            collectInputMessage.SetActive(false);
            _itemCollected = false;
        }

        void Update()
        {
            RayCastObject();  
        }

        //casting the ray from player on a specific distance
        //colors were used for debugging
        private void RayCastObject()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitObject;
            if (Physics.Raycast(ray, out hitObject, 1.5f))
            {
                Debug.DrawLine(ray.origin, hitObject.point, Color.green);
                
                //display a message when objects are in range
                collectInputMessage.SetActive(true);
                
                //collect the item on an input 
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Destroy(hitObject.collider.gameObject);
                    collectInputMessage.SetActive(false);
                   
                    //the name of the object is required in InventoryManager script
                    //to identify the corresponding image from Resource folder
                    _collectedItemName = hitObject.collider.gameObject.name;
                    _itemCollected = true;
                }
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 2, Color.red);
                _itemCollected = false;
            }
        }

        //methods required in InventoryManager
        //to identify the items and trigger the load of images 
        public string GetCollectedItem() =>_collectedItemName;
        public bool IsItemCollected => _itemCollected;
    }
}
