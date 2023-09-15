using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Typewriter : MonoBehaviour
{
    #region Fields And Variables
    private DialogueManager dialogueManager;
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [SerializeField] 
    private float timePerCharacter = 0.02f;
    private WaitForSecondsRealtime charDelay;

    private string textToType;
    private Action onCompleteCallback;
    private int characterCount;
    private bool isTyping;
    #endregion


    #region Methods
    public void Start()
    {
        charDelay = new WaitForSecondsRealtime(timePerCharacter);
    }
    
    public void OnTypeDialogue(string textToDisplay, Action onCompleteCallback)
    {
        StopAllCoroutines();

        textToType = textToDisplay;
        characterCount = dialogueText.textInfo.characterCount;
        this.onCompleteCallback = onCompleteCallback;
        StartCoroutine(TypewriterCoroutine());
    }

    public bool IsTyping()
    {
        return isTyping;
    }

    public void AbortAndShowCompleteDialogueText()
    {
        StopAllCoroutines();
        ShowCompleteDialogueText();
    }

    //This function displays the dialogue text one character at a time. RichText tags are not shown.
    private IEnumerator TypewriterCoroutine()
    {
        dialogueText.SetText(textToType);
        dialogueText.ForceMeshUpdate();
        dialogueText.maxVisibleCharacters = 0;
        
        isTyping = true;
       
        for (int i = 0; i < characterCount; i++)
        {
            dialogueText.maxVisibleCharacters++;
            yield return charDelay;
        }

        ShowCompleteDialogueText();
    }

    private void ShowCompleteDialogueText()
    {
        dialogueText.maxVisibleCharacters = characterCount;
        isTyping = false;
        onCompleteCallback?.Invoke();

        ClearCache();
    }

    private void ClearCache()
    {
        textToType = "";
        characterCount = 0;
        onCompleteCallback = null;
    }
    #endregion
}
