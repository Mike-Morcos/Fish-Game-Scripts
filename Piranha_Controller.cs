//------------------------------------------------------------------------------------------------------------
// This script will control the enemy fish behavior. The enemy will move linearly on the X axis untill
// the player is within a certain radius. The enemy will then rotate towards the player, wait two seconds
// and move towards the player with a faster speed within a random range.
//------------------------------------------------------------------------------------------------------------
using UnityEngine;

public class Piranha_Controller : MonoBehaviour
{
    #region Variables
    private float attackRadius = 6f;
    private Transform player;
    private Vector2 movingVector;
    private float attackTimer = 2f;
    private Rigidbody2D rb;
    private float randomSpeed;

    private Animator anim;

    private bool playerIsSeen;

    private bool isMoving;
    private bool isAttacking;
    #endregion

    void Start()
    {
        // Grabbing components 
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        // Setting the enemies moving direction to the right.
        movingVector = Vector2.right;
        // The enemy will start off moving
        isMoving = true;
        // Setting a random speed for the enemy within a certain range
        randomSpeed = Random.Range(100f, 150f);
        // Setting the player transform variable to the player's transform
        player = GameObject.Find("Player").transform;
    }
    void FixedUpdate()
    {

        // If the enemy is near the player within a certain radius the player is seen.
        if (Vector2.Distance(player.position, transform.position) < attackRadius)
        {
            playerIsSeen = true;
        }
        // If the player is seen we will rotate the target towards the player and set isMoving to false
        if (playerIsSeen && !isAttacking)
        {
            // The enemy is no longer moving
            isMoving = false;

            // We will play the idle animation
            anim.Play("piranhaIdle");
           

            // Get the Vector in the players direction
            Vector3 vectorToTarget = player.position - transform.position;
            // Set the velocity to zero
            rb.velocity = Vector2.zero;

            // Rotate the enemies forward vector towards the player position
            Quaternion lookRotation = Quaternion.LookRotation((vectorToTarget).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 3f);

            // Decrementing the attack timer
            attackTimer -= Time.fixedDeltaTime;
            if (attackTimer < 0)
            {
                // changing the moving direction to the players position
                movingVector = player.position - transform.position;
                // making the enemy move three times faster
                randomSpeed *= 3f;
                attackTimer = 100;
                // The enemy is now moving and attacking
                isMoving = true;
                isAttacking = true;
            }
        }
        // If the enemy is moving
        if (isMoving)
        {
            // Set the velocity to the moving vector and play the swim animation
            rb.velocity = movingVector.normalized * randomSpeed * Time.fixedDeltaTime;
            anim.Play("piranhaSwim");
         
        }

    }
}
