using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player_Movement : MonoBehaviour
{
    public enum FACEDIRECTION { FACELEFT = -1, FACERIGHT = 1 };// Enum to store the direction the player is facing
    public FACEDIRECTION Facing = FACEDIRECTION.FACERIGHT;// The direction we start off facing

    private float horz, vert, maxSpeed = 400f;// variables to store axial data and a max speed
    //public float acceleration = 2f;
    private bool isDashing;

    //public static Player_Movement PlayerInstance = null;
    public static float playerScale;
    public static float playerScore;
    public static bool isDead;
    public static bool hasWon;
    public static float fartBar;

    Rigidbody2D rb;
    public AudioSource bite, explode, fart;
    private Animator anim;
    private SpriteRenderer sr;
    private CapsuleCollider2D capCollider;

    public GameObject bloodFX, explodeFX, fartFX;

    public Transform fartSpawnPoint;
    private float fartTimer;


    void Awake()
    {
       // PlayerInstance = this;
        // grabbing components
        anim = GetComponent<Animator>();
        capCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        isDead = false;
        hasWon = false;

        playerScore = .2f;
        playerScale = transform.localScale.x;

        fartTimer = 1f;
        fartBar = 100f;

    }
    void Update()
    {
        horz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space))
        {
            isDashing = true;
        }
        else
            isDashing = false;

        fartTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q) && fartBar > 95f)
        {
            fartFX.transform.localScale = transform.localScale;
            Instantiate(fartFX, fartSpawnPoint.transform.position /*+ particleOffset*/, transform.rotation);
            fart.Play();          
            fartBar = 0f;            
        }
    }

    private void FixedUpdate()
    {
        if (isDead && playerScore <= 4f || hasWon)
        {
            anim.enabled = false;
            sr.enabled = false;
            horz = 0;
            vert = 0;
            capCollider.enabled = false;
            fartBar = 0f;
        }
        else if (!isDead && playerScore <= 4f)
        {
            Vector2 MoveDirection = new Vector3(horz, vert);
            if (isDashing)
            {
                rb.AddForce(MoveDirection.normalized * maxSpeed * 4 * Time.fixedDeltaTime);
            }
            else
                rb.AddForce(MoveDirection.normalized * maxSpeed * Time.fixedDeltaTime);
            //rb.velocity = new Vector2(horz, vert).normalized * maxSpeed * Time.fixedDeltaTime;
            //Debug.Log(rb.velocity);

            //    rb.velocity = new Vector3
            //(
            //    Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed),
            //    Mathf.Clamp(rb.velocity.y,-maxSpeed, maxSpeed)
            //);
            if (horz == 0 && vert == 0)
            {
                anim.Play("ClownFishIdle");
            }
            else
            {
                anim.Play("ClownFishSwim");
            }

            //Flip direction if required
            if ((horz < 0f && Facing != FACEDIRECTION.FACELEFT) ||
            (horz > 0f && Facing != FACEDIRECTION.FACERIGHT))
            {
                FlipDirection();

            }


            //---------------------------------------------------------------------------------------------------------

            float smooth = 20.0f;

            float tiltAngleY = 20.0f;
            float tiltAroundX = Input.GetAxis("Vertical") * tiltAngleY;
            // float flipOnX = 180f;
            // Rotate the cube by converting the angles into a quaternion.
            //Quaternion target = Quaternion.Euler(0,0,tiltAroundX).normalized;

            if (Facing == FACEDIRECTION.FACERIGHT)
            {
                Quaternion target = Quaternion.Euler(0f, 0f, tiltAroundX);//.normalized;
                                                                          //flipOnX = 180f;
                                                                          //tiltAngleY = 20f;
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
            }
            else if (Facing == FACEDIRECTION.FACELEFT)
            {
                Quaternion target = Quaternion.Euler(0f, 180f, tiltAroundX);//.normalized;
                                                                            //flipOnX = 180f;
                                                                            //tiltAngleY = -20f;
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
            }

        }

        if (playerScore > 3.9f && !isDead)
        {

            horz = 0;
            vert = 0;
            capCollider.enabled = false;
            //explode.Play();

            // Scaling the death effects to match the players scale
            explodeFX.transform.localScale = transform.localScale;
            bloodFX.transform.localScale = transform.localScale;
            // Instantiating death effects to the players position and rotation
            Instantiate(bloodFX, transform.position /*+ particleOffset*/, transform.rotation);
            Instantiate(explodeFX, transform.position /*+ particleOffset*/, transform.rotation);
            explode.Play();
            // Setting the static booleans to true so we can update the Ui_menu
            isDead = true;
            hasWon = true;

        }


    }
    void OnCollisionEnter2D(Collision2D other)
    {
        // If not player then exit
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Piranah") || other.gameObject.CompareTag("NeedleFish"))
        {
            if (transform.localScale.x > other.transform.localScale.x)
            {
                bite.Play();
                Destroy(other.gameObject);
                transform.localScale += new Vector3(.02f, .02f, 0f);
                playerScore += .02f;
                playerScale = transform.localScale.x;
                fartBar += 10f;

            }
            else
            {
                bite.Play();
                isDead = true;
            }

        }

    }
    private void FlipDirection()
    {
        Facing = (FACEDIRECTION)((int)Facing * -1f);
        transform.Rotate(0f, 180f, 0f);
    }
}
