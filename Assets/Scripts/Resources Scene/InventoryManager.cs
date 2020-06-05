using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Resources_Scene
{
    
    //script used to keep track of the resources the player has collected
    //when an item is collected, the linked image is loaded from Resource folder
    //the resources are linked to images through name convention
    public class InventoryManager : MonoBehaviour
    {
        private ResourcesCollection player;
        [SerializeField] private List<GameObject> inventorySlots = new List<GameObject>(6);
        [SerializeField] GameObject inventoryFullMessage;
        
        void Start()
        {
            player = FindObjectOfType<ResourcesCollection>();
            inventoryFullMessage.SetActive(false);
        }

        void Update()
        {
            if (player.IsItemCollected)
            {
                //6 slots have been assigned to the inventory
                if (GetEmptySlot() < 6)
                {
                    //a slot fills when a new item is collected
                    player.GetCollectedItem();
                    GetEmptySlot();
                    
                    //the empty slot is linked with the just found new image
                    //which has the same name as the item collected
                    inventorySlots[GetEmptySlot()].name = player.GetCollectedItem();
                    inventorySlots[GetEmptySlot()].GetComponent<UnityEngine.UI.Image>().sprite =
                        Resources.Load<Sprite>("Collectables UI/" + player.GetCollectedItem());
                    inventorySlots[GetEmptySlot()].GetComponentInChildren<TextMeshProUGUI>().text = player.GetCollectedItem();
                   
                    //when the player fills all 6 slots, a message is displayed
                    if (GetEmptySlot() >= 5)
                    {
                        inventoryFullMessage.SetActive(true);
                    }
                }
            }

        }
        
        //this checks method has 2 checks:
        
        //if the collected item name is recognized as new ->
        //in this case, it returns the slot on the screen which has
        //an empty image component (doesn't have an image assigned)
        
        //if the collected item has been collected before ->
        //in this case, it returns the same slot in the screen 
        //which has the corresponding image and re-uploads it
        private int GetEmptySlot()
        {
            for (int i = 0; i <= 5; i++)
            {
                if (inventorySlots[i].GetComponent<UnityEngine.UI.Image>().sprite == null || inventorySlots[i].name == player.GetCollectedItem())
                {
                    return i;
                }
            }

            return 6;
        }

    }
}
