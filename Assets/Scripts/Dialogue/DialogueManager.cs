using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    private DialogueTemplate currentDialogueTemplate;
    private int currentDialogueLineIndex;
    private Action onDialogueEndedCallback;
    
    [Header("Dialogue Input")]
    [SerializeField] private PlayerInput dialogueInput;
    
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private GameObject goToNextInstruction;

    [Header("Typewriter")]
    [SerializeField] private Typewriter typewriter;

    
    //Testing
    private void Update() {
        if(Input.GetKeyDown(KeyCode.X))
        {
            GoToNextDialogueLine();
        }
    }
    ////////////////
    private void Start() 
    {
        dialoguePanel.SetActive(false);
        dialogueInput.enabled = false;
    }

    public void StartDialogue(DialogueTemplate dialogueTemplate, Action onDialogueEndedCallback)
    {
        dialogueInput.enabled = true;

        currentDialogueTemplate = dialogueTemplate;
        currentDialogueLineIndex = 0;
        this.onDialogueEndedCallback = onDialogueEndedCallback;

        dialoguePanel.SetActive(true);
        ShowDialogueLine(currentDialogueTemplate.dialogueLines[currentDialogueLineIndex]);
    }

    public void GoToNextDialogueLine()
    {
        if(typewriter.IsTyping())
        {
            typewriter.AbortAndShowCompleteDialogueText();
            Debug.Log("abort and show");
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

        onDialogueEndedCallback?.Invoke();
        onDialogueEndedCallback = null;
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
        typewriter.OnTypeDialogue(dialogueText, () => ShowGoToNextLineInstruction());
    }

    private void ShowGoToNextLineInstruction()
    {
        goToNextInstruction.SetActive(true);
    }
}
