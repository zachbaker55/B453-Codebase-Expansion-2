using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private PlayerController playerCon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCon.hasBall = true;
            PlayerPrefs.SetInt("Name", (playerCon.hasBall ? 1 : 0));
            Destroy(gameObject);
        }
    }
}
