using _2DScene;
using Player_Scripts;
using UnityEngine;

namespace Enemy_Scripts._2D
{
    //namespace destined to enemy related scripts
    //possible containing scripts: Shooting, AI, StateMachines

    
    //Simple script responsible with basic enemy movement
    public class Enemy : MonoBehaviour
    {
        private PlayerController2D player;
        private WinWall _winWall;
        
        void Start()
        {
            player = FindObjectOfType<PlayerController2D>();
            _winWall = FindObjectOfType<WinWall>();
        }

        void Update()
        {
            if (!player.IsPlayerDead() && !_winWall.HasWon())
            {
                CheckDistanceToPlayer();
            }
        }

        //create area around the enemy that triggers the chasing state
        //chase the player if he is within range
        private void CheckDistanceToPlayer()
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= 5)
            {
                ChasePlayer();
            }
        }
        
        private void ChasePlayer()
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime);
        }
    }
    
}
