using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    #region Fields
    public event Action DialogueEnded;

    private DialogueTemplate currentDialogueTemplate;
    private int currentDialogueLineIndex;
    
    [Header("Dialogue Input")]
    [SerializeField] private PlayerInput dialogueInput;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private GameObject goToNextInstruction;

    [Header("Typewriter")]
    [SerializeField] private Typewriter typewriter;
    #endregion


    #region Methods

    #region Unity Event Methods
    private void Start() 
    {
        dialoguePanel.SetActive(false);
        dialogueInput.enabled = false;
    }
    #endregion

    #region Player Input Callbacks
    /// <summary>
    /// Gets called when the player presses the "Continue" button as mapped in the Dialogue System input action asset. Proceeds to the next dialogue line.
    /// </summary>
    private void OnContinue(InputValue value)
    {
        GoToNextDialogueLine();
    }
    #endregion

    /// <summary>
    /// Initiates a given dialogue.
    /// </summary>
    /// <param name="dialogueTemplate">The scriptableObject that contains the dialogue lines</param>
    public void StartDialogue(DialogueTemplate dialogueTemplate)
    {
        dialogueInput.enabled = true;

        currentDialogueTemplate = dialogueTemplate;
        currentDialogueLineIndex = 0;

        dialoguePanel.SetActive(true);
        ShowDialogueLine(currentDialogueTemplate.dialogueLines[currentDialogueLineIndex]);
    }

    /// <summary>
    /// Initiates the next dialogue line if the current line has completed. If typing is still ongoing, aborts and shows entire line.
    /// </summary>
    private void GoToNextDialogueLine()
    {
        if(typewriter.IsTyping())
        {
            typewriter.AbortAndShowCompleteDialogueText();
            return;
        }

        //If all dialogue lines have been shown, calls EndDialogue.
        currentDialogueLineIndex++;
        if(currentDialogueLineIndex >= currentDialogueTemplate.dialogueLines.Count) 
        {
            EndDialogue();
            return;
        }

        ShowDialogueLine(currentDialogueTemplate.dialogueLines[currentDialogueLineIndex]);
    }

    /// <summary>
    /// Deactivates the dialogue panel and fires the DialogueEnded event.
    /// </summary>
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueInput.enabled = false;
        DialogueEnded?.Invoke();
    }

    /// <summary>
    /// Sets up the character name and initiates the typing of the dialogue line.
    /// </summary>
    /// <param name="dialogueLine">The data structure that contains the character name and dialogue text.</param>
    private void ShowDialogueLine(DialogueLine dialogueLine)
    {
        goToNextInstruction.SetActive(false);
        SetCharacterName(dialogueLine.characterName);
        TypeDialogueText(dialogueLine.dialogueText);
    }

    /// <summary>
    /// Sets the text of the character name UI.
    /// </summary>
    /// <param name="characterName">The name of the character.</param>
    private void SetCharacterName(string characterName)
    {
        characterNameText.SetText(characterName);
    }

    /// <summary>
    /// Initiates the typing of a given text.
    /// </summary>
    /// <param name="dialogueText">The text to type.</param>
    private void TypeDialogueText(string dialogueText)
    {
        typewriter.TypingCompleted += OnTypingCompleted;
        typewriter.OnTypeDialogue(dialogueText);
    }

    /// <summary>
    /// Gets called when typing has completed. Shows the continue-to-next-line instruction.
    /// </summary>
    private void OnTypingCompleted()
    {
        typewriter.TypingCompleted -= OnTypingCompleted;
        ShowGoToNextLineInstruction();
    }

    /// <summary>
    /// Activates the continue-to-next-line instruction UI gameObject.
    /// </summary>
    private void ShowGoToNextLineInstruction()
    {
        goToNextInstruction.SetActive(true);
    }
    #endregion
}
