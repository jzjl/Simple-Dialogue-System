using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Typewriter : MonoBehaviour
{
    #region Fields
    public event Action TypingCompleted;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float timePerCharacter = 0.02f;
    private WaitForSecondsRealtime charDelay;

    private string textToType;
    private int characterCount;
    private bool isTyping;
    #endregion


    #region Methods

    #region Unity Event Methods
    public void Start()
    {
        charDelay = new WaitForSecondsRealtime(timePerCharacter);
    }
    #endregion
    
    public void OnTypeDialogue(string textToDisplay)
    {
        StopAllCoroutines();

        textToType = textToDisplay;

        dialogueText.SetText(textToType);
        dialogueText.ForceMeshUpdate();
        dialogueText.maxVisibleCharacters = 0;
        characterCount = dialogueText.textInfo.characterCount;

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

    //This function displays the dialogue text one character at a time. Supports RichText tags.
    private IEnumerator TypewriterCoroutine()
    {
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
        TypingCompleted?.Invoke();

        ClearCache();
    }

    private void ClearCache()
    {
        textToType = "";
        characterCount = 0;
    }
    #endregion
}
