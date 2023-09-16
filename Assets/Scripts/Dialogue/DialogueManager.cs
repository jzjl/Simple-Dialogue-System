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
    private void OnContinue(InputValue value)
    {
        GoToNextDialogueLine();
    }
    #endregion

    public void StartDialogue(DialogueTemplate dialogueTemplate)
    {
        dialogueInput.enabled = true;

        currentDialogueTemplate = dialogueTemplate;
        currentDialogueLineIndex = 0;

        dialoguePanel.SetActive(true);
        ShowDialogueLine(currentDialogueTemplate.dialogueLines[currentDialogueLineIndex]);
    }

    private void GoToNextDialogueLine()
    {
        if(typewriter.IsTyping())
        {
            typewriter.AbortAndShowCompleteDialogueText();
            return;
        }

        currentDialogueLineIndex++;
        if(currentDialogueLineIndex >= currentDialogueTemplate.dialogueLines.Count) 
        {
            EndDialogue();
            return;
        }

        ShowDialogueLine(currentDialogueTemplate.dialogueLines[currentDialogueLineIndex]);
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueInput.enabled = false;
        DialogueEnded?.Invoke();
    }

    private void ShowDialogueLine(DialogueLine dialogueLine)
    {
        goToNextInstruction.SetActive(false);
        SetCharacterName(dialogueLine.characterName);
        TypeDialogueText(dialogueLine.dialogueText);
    }

    private void SetCharacterName(string characterName)
    {
        characterNameText.SetText(characterName);
    }

    private void TypeDialogueText(string dialogueText)
    {
        typewriter.TypingCompleted += OnTypingCompleted;
        typewriter.OnTypeDialogue(dialogueText);
    }

    private void OnTypingCompleted()
    {
        typewriter.TypingCompleted -= OnTypingCompleted;
        ShowGoToNextLineInstruction();
    }

    private void ShowGoToNextLineInstruction()
    {
        goToNextInstruction.SetActive(true);
    }
    #endregion
}
