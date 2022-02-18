using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ButtonScript : MonoBehaviour
{
    [SerializeField] private DialogueTrigger Katya;
    [SerializeField] private TextMeshProUGUI numberOfPressingText;
    private int numberOfPressing = 1000;
    
    public void ButtonPress()
    {
        numberOfPressing -= 1;
        numberOfPressingText.text = numberOfPressing.ToString();

        switch (numberOfPressing)
        {
            case 999:
                Katya.TriggerDialogue();
                break;
            case 975:
                DialogueManager.singleton.DisplayNextSentence();
                break;
            case 970:
                DialogueManager.singleton.DisplayNextSentence();
                break;
            case 965:
                DialogueManager.singleton.DisplayNextSentence();
                break;
            case 960:
                DialogueManager.singleton.DisplayNextSentence();
                break;
            case 955:
                DialogueManager.singleton.DisplayNextSentence();
                break;

        }
    }
}
