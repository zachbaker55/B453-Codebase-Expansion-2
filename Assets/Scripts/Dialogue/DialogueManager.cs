using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    //[SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject[] choices;
    private TMP_Text[] choicesText;
    private static DialogueManager instance;
    private Story currentStory;

    public bool dialogueIsPlaying;

    public Animator dialogueAnim;
    [SerializeField] private PlayerState playerState;
    [SerializeField] private PlayerDash playerDash;
    [SerializeField] private PlayerSlide playerSlide;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one Dialogue manager was found");
        }

        instance = this;
    }

    public static DialogueManager getInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;

        choicesText = new TMP_Text[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TMP_Text>();
            index++;
        }
    }

    private void Update()
    {

    }

    public void enterDialogueMode(TextAsset inkScript)
    {
        currentStory = new Story(inkScript.text);
        dialogueIsPlaying = true;
        dialogueAnim.SetBool("isOpen", true);

        continueStory();
    }

    private void exitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialogueAnim.SetBool("isOpen", false);
        //dialogueText.text = "";
        playerState.state = PlayerState.State.Idle;
        playerDash.canDash = true;
        playerSlide.canSlide = true;
    }

    public void continueStory()
    {
        if (currentStory.canContinue)
        {
            Debug.Log("Story Continuing");
            StopAllCoroutines();
            StartCoroutine(typeSentence(currentStory.Continue()));

            displayChoices();   
        }
        else
        {
            exitDialogueMode();
        }
    }

    private void displayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        
        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can hold. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        foreach(Choice choice in currentChoices)
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for(int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(selectFirstChoice()); 
    }

    public void makeChoice(int choiceIndex)
    {
        Debug.Log("Choice has been made");
        currentStory.ChooseChoiceIndex(choiceIndex);
        //continueStory();
    }

    public int getChoicesCount()
    {
        return currentStory.currentChoices.Count;
    }

    IEnumerator typeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator selectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject); 
    }
}
