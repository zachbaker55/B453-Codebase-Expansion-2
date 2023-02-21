using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public bool gameStart = true;

    public Transform start;
    public Transform checkpoint;
    public PlayerDeath playerDeath;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        gameStart = true;
    }


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (playerDeath.playerDead)
        {
            player.transform.position = start.position;
            playerDeath.playerDead = false;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        if (scene.name == "DogHouse")
        {
            gameStart = false;
            return;
        }

        if(scene.name == "SampleScene")
        {
            if (gameStart)
            {
                player.transform.position = checkpoint.position;
            }
            else
            {
                player.transform.position = start.position;
            }
        }
    }
}
