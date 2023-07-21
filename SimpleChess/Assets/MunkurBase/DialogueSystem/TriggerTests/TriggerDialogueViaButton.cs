using EasyButtons;
using Munkur;
using UnityEngine;

public class TriggerDialogueViaButton : MonoBehaviour
{
    [Button]
    private void TriggerDialogue()
    {
        DialogueTrigger.Instance.TriggerDialogue();
    }
}
