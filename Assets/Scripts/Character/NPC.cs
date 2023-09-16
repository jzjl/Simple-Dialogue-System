using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class NPC : MonoBehaviour
{
    #region Fields
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueTemplate dialogueTemplate;
    [SerializeField] private GameObject instructionGO;
    [SerializeField] private Transform playerCamRoot;
    private StarterAssets.StarterAssetsInputs starterAssetsInputs;
    private PlayerInput playerInput;
    #endregion


    #region Methods

    #region Unity Event Methods
    private void Start() 
    {
        instructionGO.SetActive(false);
    }

    private void Update()
    {
        instructionGO.transform.forward = playerCamRoot.forward;
    }
    #endregion

    #region Collider Event Methods
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag != "Player")
            return;

        starterAssetsInputs = other.GetComponent<StarterAssets.StarterAssetsInputs>();
        playerInput = other.GetComponent<PlayerInput>();
        starterAssetsInputs.InteractButtonPressed += StarterAssetsInputs_InteractButtonPressed;

        instructionGO.SetActive(true);
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.tag != "Player")
            return;

        starterAssetsInputs.InteractButtonPressed -= StarterAssetsInputs_InteractButtonPressed;
        playerInput.enabled = true;

        instructionGO.SetActive(false);
    }
    #endregion

    private void StarterAssetsInputs_InteractButtonPressed()
    {
        playerInput.enabled = false;
        dialogueManager.DialogueEnded += OnDialogueEnded;
        dialogueManager.StartDialogue(dialogueTemplate);

    }

    private void OnDialogueEnded()
    {
        dialogueManager.DialogueEnded -= OnDialogueEnded;
        playerInput.enabled = true;
    }
    #endregion
}
