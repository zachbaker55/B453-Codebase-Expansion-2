using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    //public Dialogue dialogue;

    private bool playerInRange;
    [SerializeField] private float checkRadius;
    [SerializeField] private int index;
    private int ballCount;

    //[SerializeField] private TMP_Text nameText;

    [SerializeField] private TextAsset[] normalInkJSONs;
    [SerializeField] private TextAsset[] ballInkJSONs;
    [SerializeField] private PlayerState playerState;
    [SerializeField] private PlayerController playerCon;
    [SerializeField] private PlayerSlide playerSlide;
    [SerializeField] private PlayerDash playerDash;
    [SerializeField] private PlayerJump playerJump;

    private TextAsset[] inkJSONs;

    private void Awake()
    {
        playerInRange = false;
        index = 0;
        ballCount = 0;
    }

    private void Update()
    {
        if (playerCon.hasBall)
        {
            if(ballCount == 0)
            {
                index = 0;
                ballCount++;
            }
            inkJSONs = ballInkJSONs;
        }
        else
        {
            inkJSONs = normalInkJSONs;
        }


        if (playerState.state != PlayerState.State.Talking)
        {
            Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, checkRadius);
            foreach (Collider2D coll in results)
            {
                if (coll.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.J))
                {
                    playerDash.canDash = false;
                    playerSlide.canSlide = false;
                    playerCon.directionX = 0;

                    playerState.state = PlayerState.State.Talking;
                    DialogueManager.getInstance().enterDialogueMode(inkJSONs[index]);

                    if (index < inkJSONs.Length - 1)
                    {
                        index++;
                    }
                    Debug.Log("Index: " + index);
                }
            }
        }
        else if (playerState.state == PlayerState.State.Talking && Input.GetKeyDown(KeyCode.J) && DialogueManager.getInstance().getChoicesCount() == 0)
        {
            playerDash.canDash = false;
            playerSlide.canSlide = false;
            playerCon.directionX = 0;
            //Debug.Log("Pressed to continue");
            DialogueManager.getInstance().continueStory();
        }
    }
}
