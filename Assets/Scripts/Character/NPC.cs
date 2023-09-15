using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class NPC : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueTemplate dialogueTemplate;
    private StarterAssets.StarterAssetsInputs starterAssetsInputs;
    private PlayerInput playerInput;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag != "Player")
            return;

        starterAssetsInputs = other.GetComponent<StarterAssets.StarterAssetsInputs>();
        playerInput = other.GetComponent<PlayerInput>();
        starterAssetsInputs.InteractButtonPressed += StarterAssetsInputs_InteractButtonPressed;
        
        Debug.Log("enter: " + other.name);
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.tag != "Player")
            return;

        starterAssetsInputs.InteractButtonPressed -= StarterAssetsInputs_InteractButtonPressed;
        playerInput.enabled = true;
        Debug.Log("Exit: " + other.name);
    }

    private void StarterAssetsInputs_InteractButtonPressed()
    {
        Debug.Log("npc interacted");
        playerInput.enabled = false;
        dialogueManager.StartDialogue(dialogueTemplate, () => OnDialogueEnded());

    }

    private void OnDialogueEnded()
    {
        playerInput.enabled = true;
    }
}
