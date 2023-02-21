using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController playerCon;
    private PlayerState playerState;
    [SerializeField] private Animator playerAnim;

    [SerializeField] private BoxCollider2D normalColl;
    [SerializeField] public BoxCollider2D slideColl;

    public float slideSpeed;
    public float slideTime;
    [SerializeField] private float slideCooldown;
    public bool isSliding;
    public bool canSlide;
    Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCon = GetComponent<PlayerController>();
        playerState = GetComponent<PlayerState>();

        isSliding = false;
        canSlide = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && playerCon.onGround && playerCon.directionX != 0 && canSlide && playerState.state != PlayerState.State.Talking)
        {
            playerState.state = PlayerState.State.Sliding;
            performSlide();
        }
    }

    private void performSlide()
    {
        canSlide = false;
        isSliding = true;
        playerAnim.SetBool("Sliding", true);

        normalColl.enabled = false;
        slideColl.enabled = true;

        velocity = new Vector2(12 * transform.localScale.x, rb.velocity.y);
        rb.velocity = velocity;

        rb.AddForce(transform.localScale.x * Vector2.right * slideSpeed, ForceMode2D.Impulse);

        StartCoroutine("stopSlide");
    }

    IEnumerator stopSlide()
    {
        yield return new WaitForSeconds(slideTime);

        normalColl.enabled = true;
        slideColl.enabled = false;

        isSliding = false;
        playerAnim.SetBool("Sliding", false);

        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
    }

    // Draws the Light bulb icon at position of the object.
    private void OnDrawGizmos()
    {
        //Physics2D.alwaysShowColliders = true;
    }
}
