using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueTemplate", menuName = "Simple-Dialogue-System/DialogueTemplate")]
public class DialogueTemplate : ScriptableObject 
{
    public List<DialogueLine> dialogueLines;
}

[System.Serializable]
public struct DialogueLine
{
    public string characterName;
    [TextArea(1, 5)] public string dialogueText;
}

