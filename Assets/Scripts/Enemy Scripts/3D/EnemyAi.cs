using System.Collections;
using Player_Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy_Scripts._3D
{
    //This script handles the penguins AI state machine
    //the state machine is handled by a switch()
    //All the states are also within this script due to their simplicity
    
    //penguin are of 2 types: enemies and NPCs
    //if the host is an enemy, he will chase and attack (insult) the player
    //else, he will just wander around
    public class EnemyAi : MonoBehaviour
    {
       private int chaseRange;
       private int exitChaseRange;

        private float _moveSpeed = 3f;
        private float _rotSpeed = 100f;

        private bool _isWalking;
        private bool _isWandering;
        private bool _isRotLeft;
        private bool _isRotRight;

        private PlayerController3D _target;
        
        private string[] insults = {"I hate you!", "U Ugly!", "Go away!", "Loooser"};
        [SerializeField] private GameObject insultPlate;
        [SerializeField] private TextMesh insultText;
       
        private State _activeState;
        private enum State
        {
                WANDER,
                CHASE,
                ATTACK    
        }
        
        private void Start()
        {
            //for hostile penguins, a random insult will be extracted from the insults array
            if (CompareTag("Enemy"))
            {
                insultPlate.SetActive(false);
                insultText.text = RandomInsultText();
            }

            //initial state
            _activeState = State.WANDER;
            
            //required parameters for the WANDER states
            //these all start as false and are gradually activated
            _isWalking = false;
            _isWandering = false;
            _isRotLeft = false;
            _isRotRight = false;
            _isWandering = false;
            
            _target = FindObjectOfType<PlayerController3D>();
        }
        
        //this method gets the random insult text
        //assigned at to the insult plate in Start()
        private string RandomInsultText()
        {
            int randomString = Random.Range(0, 4);
            return insults[randomString];
        }
        
        
        void Update()
        {
            //separated functionality between the Enemies and NPCs
            //Enemy states
            if (CompareTag("Enemy"))
            {
                chaseRange = 5;
                exitChaseRange = 10;
                switch (_activeState)
                {
                    case State.WANDER:
                    Wandering();
                    SearchForTarget();
                        break;

                    case State.CHASE:
                        ChaseTarget();
                        break;

                    case State.ATTACK:
                        AttackTarget();
                        break;
                }
            }
           
            //NPCs states
            if(CompareTag("NPC"))
            {
                chaseRange = 0;
                exitChaseRange = 0;
                Wandering();
            }

        }

        #region WANDER state
        //this is the functionality of WANDER state
        //wait some time
        //walk in random direction
        //wait some time
        //rotate to change direction left or right
        //wait some time
        //walk again
        private IEnumerator Wander()
        {
            int rotationTime = Random.Range(1, 3);
            int timeBetweenRotations = Random.Range(1, 3);
            int rotateLorR = Random.Range(0, 3);
            int timeBeforeWalking = Random.Range(1, 3);
            int walkTime = Random.Range(2, 5);

            _isWandering = true;
            yield return new WaitForSeconds(timeBeforeWalking);
            _isWalking = true;
            yield return new WaitForSeconds(walkTime);
            _isWalking = false;
            yield return new WaitForSeconds(timeBetweenRotations);
            switch (rotateLorR)
            {
                case 1:
                    _isRotRight = true;
                    yield return new WaitForSeconds(rotationTime);
                    _isRotRight = false;
                    break;
                
                case 2:
                    _isRotLeft = true;
                    yield return new WaitForSeconds(rotationTime);
                    _isRotLeft = false;
                    break;
            }

            _isWandering = false;
        }
        
        //this method is called in update, so the coroutine is 
        //called continuously
        //hence, the enemies never settle
        private void Wandering()
        {
            if (!_isWandering)
            {
                StartCoroutine(Wander());
            }

            Rotate();
            Walk();
        }

        //rotate left or right
        private void Rotate()
        {
            if (_isRotRight)
            {
                transform.Rotate(transform.up * (Time.deltaTime * _rotSpeed));
            }

            if (_isRotLeft)
            {
                transform.Rotate(transform.up * (Time.deltaTime * -_rotSpeed));
            }
        }

        //move
        private void Walk()
        {
            if (_isWalking)
            {
                transform.position += transform.forward * (Time.deltaTime * _moveSpeed);
            }
        }

        #endregion
        
        #region Chase
        
        //if the script is on an Enemy, he searches for a target
        //if the target is within range, start chasing him (change state)
        private void SearchForTarget()
        {
            if (Vector3.Distance(_target.transform.position, transform.position) < chaseRange)
            {
                _activeState = State.CHASE;
            }
        }

        //the enemy is chasing the player only while he is within the chase range
        //when the player is outside the range, the enemy gives up chasing
        //when the player is inside the attack range, the enemy insults him
        private void ChaseTarget()
        {
            insultPlate.SetActive(false);

            var direction = GetDirection();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);

            if (direction.magnitude > chaseRange && direction.magnitude < exitChaseRange)
            {
                transform.Translate(0,0,0.05f);
            }
            else if(direction.magnitude > chaseRange && direction.magnitude > exitChaseRange)
            {
                _activeState = State.WANDER;
            }
            else if (direction.magnitude < chaseRange && direction.magnitude < exitChaseRange)
            {
                _activeState = State.ATTACK;
            }
        }

        #endregion

        #region Attack/Insult
        //attacking means insulting
        //the enemy displays the plate with an extracted insult wt the beginning
        //and faces the player's direction
        //when the player exits the attacking range, the state changes to chase
        private void AttackTarget()
        {

            var direction = GetDirection();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);

            if (direction.magnitude > chaseRange && direction.magnitude < exitChaseRange)
            {
                _activeState = State.CHASE;
            }
            insultPlate.SetActive(true);
        }
       
        //this method help facing the player when attacking
        //by only rotating on X axis
        //the penguin would fall if he Y was changed as well
        private Vector3 GetDirection()
        {
            Vector3 direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction;
        }
        #endregion
    
        
        //gizmos were used to visualize the chase and attack areas
        //on the scene editor.
        //chase -> yellow
        //attack -> green
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, exitChaseRange);
        }
    }
}
