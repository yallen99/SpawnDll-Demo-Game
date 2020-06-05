using UnityEngine;

namespace Enemy_Scripts._3D
{
    //This script handles the replacing of bad Pingus with nice ones
    //It uses trigger detection to do so
    public class HugCommand : MonoBehaviour
    {
        [SerializeField] private GameObject hugMessage;
        [SerializeField] private GameObject goodPingu;
        Vector3 _pos;
        private bool _targetFound;
        private GameObject _objToReplace;

        private void Start()
        {
            hugMessage.SetActive(false);
            _targetFound = false;
        }
        
        
        private void Update()
        {
            if (_targetFound && Input.GetKeyDown(KeyCode.E))
           {
             Destroy(_objToReplace);
             Instantiate(goodPingu, _pos, Quaternion.identity);
             hugMessage.SetActive(false);
           }
        }

        
        //when the player encounters an Enemy tagged object
        //it is signaled to the Update that the object can be replaced
        //the game object and its position (mad Pingu) are taken so the new one can
        //be instantiated in the same place and the mad one can be destroyed
        private void OnTriggerEnter(Collider other)
        {
            var target = other.GetComponent<EnemyAi>();
            if (target && target.CompareTag("Enemy"))
            {
                hugMessage.SetActive(true);
                _targetFound = true;
                _objToReplace = target.gameObject;
                _pos = target.transform.position;

            }
        }

        //if the player loses the target (is too far away)
        //the position and the target object are set to 0 & null
        private void OnTriggerExit(Collider other)
        {
            var target = other.GetComponent<EnemyAi>();
            if (target && target.CompareTag("Enemy"))
            {
                hugMessage.SetActive(false);
                _targetFound = false;
                _objToReplace = null;
                _pos = Vector3.zero;
            }
        }
    }
}