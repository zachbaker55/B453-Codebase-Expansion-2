using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public Rigidbody2D rb;
    private GroundDetection ground;
    public Vector2 velocity;

    public float jumpHeight;
    //public float timeToApex;
    public float timeToJumpApex;
    public float upwardMovementMultiplier = 1f;
    public float downwardMovementMultiplier = 6.17f;
    public int maxAirJumps = 0;

    //public bool variableJumpHeight;
    public float jumpCutOff;
    public float speedLimit;
    public float coyoteTime = 0.15f;
    public float jumpBuffer = 0.15f;

    public float jumpSpeed;
    private float defaultGravityScale;
    public float gravMultiplier;

    public bool canJumpAgain = false;
    private bool desiredJump;
    private float jumpBufferCounter = 0;
    private float coyoteTimeCounter = 0;
    private bool pressingJump;
    public bool onGround;
    private bool isJumping;

    private PlayerEffects playerEffects;
    private PlayerDash playerDash;
    private PlayerState playerState;
    private PlayerController playerCon;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundDetection>();
        playerEffects = GetComponent<PlayerEffects>();
        playerDash = GetComponent<PlayerDash>();
        playerState = GetComponent<PlayerState>();
        playerCon = GetComponent<PlayerController>();
        defaultGravityScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //Handles jump input
        if (Input.GetKeyDown(KeyCode.Space) && playerState.state != PlayerState.State.Talking)
        {
            playerState.state = PlayerState.State.Jumping;
            desiredJump = true;
            pressingJump = true;
        }
        else if (Input.GetKey(KeyCode.Space) && playerState.state != PlayerState.State.Talking)
        {
            pressingJump = true;
        }
        else
        {
            pressingJump = false;
        }

        if (!playerDash.isDashing)
        {
            setPhysics();
        }

        onGround = ground.getOnGround();

        if (jumpBuffer > 0)
        {
            if (desiredJump)
            {
                jumpBufferCounter += Time.deltaTime;

                if(jumpBufferCounter > jumpBuffer)
                {
                    desiredJump = false;
                    jumpBufferCounter = 0;
                }
            }
        }

        if(!isJumping && !onGround)
        {
            coyoteTimeCounter += Time.deltaTime;
        } else coyoteTimeCounter = 0;
        

    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;
        //Debug.Log("Velocity: " + velocity);

        if (desiredJump)
        {
            DoAJump();
            rb.velocity = velocity;
            return;
        }

        if (!playerDash.isDashing)
        {
            calculateGravity();
        }
        else
        {
            zeroGravity();
        }
    }

    private void setPhysics()
    {
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
        //Debug.Log("New Gravity: " + newGravity.y);
        //Debug.Log("newGravity.y: " + newGravity.y);
        //Debug.Log("Physics2D.gravity.y: " + Physics2D.gravity.y);
        //Debug.Log("gravMultiplier: " + gravMultiplier);
        rb.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
        //Debug.Log("Set Physics RB Gravity Scale: " + rb.gravityScale);

    }

    private void calculateGravity()
    {
        //When player is going up
        if(rb.velocity.y > 0.1f)
        {
            if (onGround)
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                if(pressingJump && isJumping)
                {
                    //Debug.Log("JUMPING");
                    gravMultiplier = upwardMovementMultiplier;
                }
                else
                {
                    //Debug.Log("Jump Cutoff");
                    gravMultiplier = jumpCutOff;
                }
            }
        }
        //When player is falling down
        else if(rb.velocity.y < -0.1f)
        {
            if (onGround)
            {
                gravMultiplier = defaultGravityScale;
                //rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
            else
            {
                gravMultiplier = downwardMovementMultiplier;
            }
        }
        else
        {
            if (onGround)
            {
                isJumping = false;
            }
            gravMultiplier = defaultGravityScale;
        }

        rb.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
    }

    private void zeroGravity()
    {
        //Debug.Log("Zero Gravity");
        rb.gravityScale = 0;
    }

    private void DoAJump()
    {
        if(onGround || coyoteTime > coyoteTimeCounter && !isJumping || playerCon.canSlideJump)
        {
            playerCon.canSlideJump = false;
            desiredJump = false;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            //Debug.Log("Physics Gravity: " + Physics2D.gravity.y);
            //Debug.Log("RB Gravity Scale: " + rb.gravityScale);
            //Debug.Log("Jump Height: " + jumpHeight);
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * rb.gravityScale * jumpHeight);

            //Debug.Log("Velocity Y: " + velocity.y);
            if(velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
                //Debug.Log("Jump Speed: " + jumpSpeed);
            }
            else if(velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(rb.velocity.y);
                //Debug.Log("Jump Speed: " + jumpSpeed);
            }

            velocity.y += jumpSpeed;
            isJumping = true;

            if(playerEffects != null)
            {
                playerEffects.jumpEffects();
            }
        }


    }
}
