using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    public bool playerGrounded;

    [SerializeField] private Animator playerAnim;
    private PlayerController playerCon;
    private PlayerJump playerJump;
    // Start is called before the first frame update
    void Start()
    {
        playerCon = GetComponent<PlayerController>();
        playerJump = GetComponent<PlayerJump>();
    }

    // Update is called once per frame
    void Update()
    {
        checkForLanding();   
    }

    private void checkForLanding()
    {
        if(!playerGrounded && playerJump.onGround)
        {
            playerGrounded = true;

            playerAnim.SetTrigger("Landed");
        }
        else if(playerGrounded && !playerJump.onGround)
        {
            playerGrounded = false;
        }
    }

    public void jumpEffects()
    {
        playerAnim.ResetTrigger("Landed");
        playerAnim.SetTrigger("Jump");
    }
}
