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
        //Used in the Typewriter coroutine to add a delay between characters.
        charDelay = new WaitForSecondsRealtime(timePerCharacter);
    }
    #endregion
    
    /// <summary>
    /// Initiates the typing of a given text.
    /// </summary>
    /// <param name="textToDisplay">The text to be typed.</param>
    public void OnTypeDialogue(string textToDisplay)
    {
        StopAllCoroutines();

        textToType = textToDisplay;
        
        //Sets up the textMeshPro component for typing.
        dialogueText.SetText(textToType);
        dialogueText.ForceMeshUpdate();
        dialogueText.maxVisibleCharacters = 0;
        characterCount = dialogueText.textInfo.characterCount;

        StartCoroutine(TypewriterCoroutine());
    }

    /// <summary>
    /// Returns true if typing is still ongoing.
    /// </summary>
    public bool IsTyping()
    {
        return isTyping;
    }

    /// <summary>
    /// Stops the typing and reveals the completed dialogue text.
    /// </summary>
    public void AbortAndShowCompleteDialogueText()
    {
        StopAllCoroutines();
        ShowCompleteDialogueText();
    }

    /// <summary>
    /// Displays the dialogue text one character at a time. Supports RichText tags.
    /// </summary>
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

    /// <summary>
    /// Reveals the entire dialogue text and fires the TypingCompleted event.
    /// </summary>
    private void ShowCompleteDialogueText()
    {
        dialogueText.maxVisibleCharacters = characterCount;
        isTyping = false;
        TypingCompleted?.Invoke();

        ClearCache();
    }

    /// <summary>
    /// Clears variables used in typing.
    /// </summary>
    private void ClearCache()
    {
        textToType = "";
        characterCount = 0;
    }
    #endregion
}
