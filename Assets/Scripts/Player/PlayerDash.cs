using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    //Dash Variables
    public bool canDash;
    public bool isDashing;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    //[SerializeField] private float dashCooldown = 1f;
    private float dashCounter = 0;
    private Vector2 dashDirection;
    //bool dashingFromGround;

    private Rigidbody2D rb;
    PlayerController playerCon;
    PlayerState playerState;
    PlayerAfterImage afterImage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCon = GetComponent<PlayerController>();
        playerState = GetComponent<PlayerState>();
        afterImage = GetComponent<PlayerAfterImage>();
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dashInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //Debug.Log(dashInput);
        if (Input.GetKeyDown(KeyCode.J) && dashCounter < 1 && canDash && !isDashing && playerState.state != PlayerState.State.Talking)
        {
            Debug.Log("Dash Activated");
            
            if (dashInput.x == -1 && dashInput.y == 1 && playerCon.onGround == false)
            {
                dashDirection = new Vector2(-1, 1).normalized;
            }
            else if(dashInput.x == 1 && dashInput.y == 1 && playerCon.onGround == false)
            {
                dashDirection = new Vector2(1, 1).normalized;
            }
            else if(dashInput.x == -1 && dashInput.y == -1 && playerCon.onGround == false)
            {
                dashDirection = new Vector2(-1, -1).normalized;
            }
            else if(dashInput.x == 1 && dashInput.y == -1 && playerCon.onGround == false)
            {
                dashDirection = new Vector2(1, -1).normalized;
            }
            else if(dashInput.x == 0 && dashInput.y == 1)
            {
                dashDirection = Vector2.up * 1.1f;
                //dashingFromGround = true;
            }
            else if(dashInput.x ==0 && dashInput.y == -1 && playerCon.onGround == false)
            {
                dashDirection = Vector2.down;
            }
            else if(playerCon.onGround == false)
            {
                //dashDirection = new Vector2(playerCon.directionX, 0);
                dashDirection = Vector2.right * transform.localScale.x;
                Debug.Log("Horizontal Dash");
            }

            if(dashDirection == Vector2.zero)
            {
                return;
            }

            StartCoroutine(Dash());
        }
        if (playerCon.onGround && !isDashing)
        {
            //Debug.Log("On Ground");
            dashCounter = 0;
        }
    }

    private void FixedUpdate()
    {
        
    }

    private IEnumerator Dash()
    {
        Debug.Log("Dashing");
        canDash = false;
        isDashing = true;
        dashCounter++;
        Debug.Log("Dash Counter: " + dashCounter);
        playerState.state = PlayerState.State.Dashing;
        float originalGravity = rb.gravityScale;
        
        
        rb.gravityScale = 0f;
        Debug.Log("Gravity Scale: " + rb.gravityScale);

        rb.velocity = new Vector2(0f, 0f);
        rb.velocity += dashDirection * dashingPower;
        afterImage.Activate(true);
        yield return new WaitForSeconds(dashingTime);

        rb.gravityScale = originalGravity;

        afterImage.Activate(false);
        isDashing = false;
        canDash = true;
        dashDirection = Vector2.zero;
    }
}
