using Enemy_Scripts._2D;
using UnityEngine;

namespace Player_Scripts
{
    
    //2D player controller managing the movement of the player
    //this script also moves the camera with the player to create
    //the flying effect
    public class PlayerController2D : MonoBehaviour
    {
        private bool alive;
        private bool dead;
        [SerializeField] private GameObject gameOver;
        [SerializeField] private GameObject startButton;
        private Camera mainCam;
        
        void Start()
        {
            alive = false;
            dead = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            mainCam = FindObjectOfType<Camera>();
        }

        //Only start playing the game after the player has pressed start
        //this will trigger the flight of the player
        
        public void StartGame()
        {
            alive = true;
            startButton.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        void Update()
        {
            Move();
            if (alive)
            {
                Fly();
            }
            
        }

        //move the player on both X and Y axes using the basic input
        private void Move()
        {
            if (Input.GetAxis("Horizontal") > 0.3)
            {
                transform.Translate(0.05f, 0,0 );
            }
            else if (Input.GetAxis("Horizontal") < -0.3)
            {
                transform.Translate(-0.05f, 0,0 );
            }
            if (Input.GetAxis("Vertical") > 0.1)
            {
                transform.Translate(0,0.05f,0);
                mainCam.transform.Translate(0,0.05f,0);
            }
           
        }

        //move the camera along with the player
        //since there is no way of going "down", the camera only needs to go "up"
        //with the player
        public void Fly()
        {
            transform.Translate(0, 0.01f, 0);
            mainCam.transform.Translate(0,0.01f, 0);
        }
        
        //collision detection between the player and enemies
        private void OnCollisionEnter2D(Collision2D other)
        {
            var enemy = other.collider.GetComponent<Enemy>();
            if (enemy)
            {
                TriggerDeath();
            }
        }

        //destroy the player on touch of the enemies
        private void TriggerDeath()
        {
            dead = true;
            Destroy(gameObject);
            gameOver.SetActive(true);
        }

        public bool IsPlayerDead()
        {
            return dead;
        }
    }
}
