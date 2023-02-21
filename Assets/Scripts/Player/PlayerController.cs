using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator playerAnim;
    [SerializeField] private PlayerState playerState;
    private Rigidbody2D rb;
    private PlayerDash playerDash;
    private PlayerSlide playerSlide;
    GroundDetection ground;

    public float maxSpeed;
    public float maxAcceleration;
    public float maxDecleration;
    public float maxTurnSpeed;
    public float maxAirAcceleration;
    public float maxAirDeceleration;
    public float maxAirTurnSpeed;
    public float friction;

    public float directionX;
    private Vector2 desiredVelocity;
    public Vector2 velocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;

    public bool pressingKey;
    public bool onGround;
    public bool hasBall;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundDetection>();
        playerDash = GetComponent<PlayerDash>();
        playerSlide = GetComponent<PlayerSlide>();
        playerState = GetComponent<PlayerState>();

        hasBall = (PlayerPrefs.GetInt("hasBall") != 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerState.state != PlayerState.State.Talking)
        {
            directionX = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            Debug.Log("Player Con FROZEN");
            velocity = Vector2.zero;
            rb.velocity = new Vector2(0f, rb.velocity.y);
            directionX = 0;
            playerAnim.SetBool("Running", false);
            Debug.Log(playerState.state);
            return;
        }

        if(directionX != 0 && !playerDash.isDashing && !playerSlide.isSliding)
        {
            transform.localScale = new Vector3(directionX > 0 ? 1 : -1, 1, 1);
            pressingKey = true;
            playerAnim.SetBool("Running", true);
            playerState.state = PlayerState.State.Running;
        }
        else
        {
            pressingKey = false;
            playerAnim.SetBool("Running", false);
        }

        
        if (Input.GetKey(KeyCode.S) && !playerDash.isDashing && playerState.state != PlayerState.State.Talking)
        {
            playerAnim.SetFloat("CrouchDirection", 1f);
            playerAnim.SetBool("Crouching", true);
            playerState.state = PlayerState.State.Crouching;
        }
        else if(Input.GetKeyUp(KeyCode.S) && !playerDash.isDashing && playerState.state != PlayerState.State.Talking)
        {
            playerAnim.SetTrigger("CrouchUp");
            
        }
        else
        {
            playerAnim.SetBool("Crouching", false);
            playerAnim.ResetTrigger("CrouchUp");
        }


        desiredVelocity = new Vector2(directionX, 0f) * Mathf.Max(maxSpeed - friction, 0f);
    }

    private void FixedUpdate()
    {
        if (playerDash.isDashing || playerSlide.isSliding || playerState.state == PlayerState.State.Talking)
        {
            return;
        }
     
        onGround = ground.getOnGround();

        velocity = rb.velocity;

        if (onGround)
        {
            runWithoutAcceleration();
        }
        else
        {
            runWithAcceleration();
        }
    }

    private void runWithoutAcceleration()
    {
        velocity.x = desiredVelocity.x;

        rb.velocity = velocity;
    }

    private void runWithAcceleration()
    {
        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        deceleration = onGround ? maxDecleration : maxAirDeceleration;
        turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;

        if (pressingKey)
        {
            //When character turning around
            if(Mathf.Sign(directionX) != Mathf.Sign(velocity.x))
            {
                maxSpeedChange = turnSpeed * Time.deltaTime;
            }
            //When horizontal input the same as character's direction
            else
            {
                maxSpeedChange = acceleration * Time.deltaTime;
            }
        }
        else
        {
            maxSpeedChange = deceleration * Time.deltaTime;
        }

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        rb.velocity = velocity;
    }
}
