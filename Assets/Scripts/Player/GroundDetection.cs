using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    private bool onGround;
    //[SerializeField] private PlayerController playerCon;

    [SerializeField] private float groundCheckLength = 0.95f;
    [SerializeField] private Vector3 colliderOffset;
    [SerializeField] private Vector3 animOffset;

    [SerializeField] private LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        //playerCon = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.Raycast(transform.position + (transform.localScale.x * animOffset) + colliderOffset, Vector2.down, groundCheckLength, groundLayer) || Physics2D.Raycast(transform.position + (transform.localScale.x * animOffset) - colliderOffset, Vector2.down, groundCheckLength, groundLayer);
    }

    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        if (onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + (transform.localScale.x * animOffset) + colliderOffset, transform.position + (transform.localScale.x * animOffset) + colliderOffset + Vector3.down * groundCheckLength);
        Gizmos.DrawLine(transform.position + (transform.localScale.x * animOffset) - colliderOffset, transform.position + (transform.localScale.x * animOffset) - colliderOffset + Vector3.down * groundCheckLength);
    }

    public bool getOnGround()
    {
        return onGround;
    }
}
