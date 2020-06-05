using System;
using Player_Scripts;
using UnityEngine;

namespace _2DScene
{
    //This namespace is destined to all 2D scene related scripts
    //that don't belong to the player or enemies.
   
    //Script triggering the end of the level
    public class WinWall : MonoBehaviour
    {
        private bool won;
        [SerializeField] private GameObject winPanel;
        void Start()
        {
            winPanel.SetActive(false);
            won = false;
        }

        //2D trigger system
        //activates the "winning" message and destroys the player
        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<PlayerController2D>();
            if (player)
            {
                Destroy(player.gameObject);
                winPanel.SetActive(true);
                won = true;
            }
            
        }

        public bool HasWon() => won;
    }
}
