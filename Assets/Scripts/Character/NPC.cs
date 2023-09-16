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
        //Ensures that the instruction text is always facing the camera.
        instructionGO.transform.forward = playerCamRoot.forward;
    }
    #endregion

    #region Collider Event Methods
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag != "Player")
            return;

        //Get references to the player character's input scripts when the player enters the trigger region. Subscribes to the interact-button-pressed event.
        starterAssetsInputs = other.GetComponent<StarterAssets.StarterAssetsInputs>();
        playerInput = other.GetComponent<PlayerInput>();
        starterAssetsInputs.InteractButtonPressed += OnInteractButtonPressed;

        //Activates instruction above the NPC.
        instructionGO.SetActive(true);
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.tag != "Player")
            return;

        //When the player exits the trigger region, unsubscribes from the interact-button-pressed event and renable the player's inputs.
        starterAssetsInputs.InteractButtonPressed -= OnInteractButtonPressed;
        playerInput.enabled = true;

        //Deactives the instruction above the NPC.
        instructionGO.SetActive(false);
    }
    #endregion

    /// <summary>
    /// Gets called when the player presses the "Interact" button as mapped in the Starter Assets input action asset. Initiates a dialogue with the NPC.
    /// </summary>
    private void OnInteractButtonPressed()
    {
        playerInput.enabled = false;
        dialogueManager.DialogueEnded += OnDialogueEnded;
        dialogueManager.StartDialogue(dialogueTemplate);
    }

    /// <summary>
    /// Gets called when the dialogue has ended. Reenables the player's inputs.
    /// </summary>
    private void OnDialogueEnded()
    {
        dialogueManager.DialogueEnded -= OnDialogueEnded;
        playerInput.enabled = true;
    }
    #endregion
}
