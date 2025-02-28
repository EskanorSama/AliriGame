using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> Senteces;
//    [SerializeField] private Text NameText, DialogueText;
  //  private Animator DialogBoxAnimator;
    //[Range(0.01f,0.2f)]
   // [SerializeField] private float LetterAppearanceTime = 0.1f;
    private Dialogue _dialogue;
    public static event Action StartedDialogueDisplay;
    public static event Action EndedDialogueDisplay;
    public static event Action<string> DisplayName;
    public static event Action<string> DisplayDialogue;


    private void Start()
    {
     //   DialogBoxAnimator = NameText.transform.parent.GetComponent<Animator>();
        Senteces = new Queue<string>(); 
    }
    public void StartDialogue(Dialogue dialogue)
    {
        if (!dialogue.PlayOnce)
        {
            _dialogue = dialogue;
            _dialogue.Change = false;
            dialogue.Started = true;
            Player.Instance.Freeze();
            StartedDialogueDisplay?.Invoke();
           // DialogBoxAnimator.SetBool("IsOpen", true);
           // NameText.transform.parent.gameObject.SetActive(true);
         //   NameText.text = dialogue.Name;
            Senteces.Clear();

            foreach (string sentence in dialogue.Sentences)
            {
                Senteces.Enqueue(sentence);
            }
            DisplayNextSentence();
        }
    }
     public void DisplayNextSentence()
    {
        if(Senteces.Count == 0)
        {
            EndDialogue();
            return;
        }
        if (!_dialogue.Monologue)
        {
            _dialogue.Change = !_dialogue.Change;
            if (_dialogue.Change)
            {
                DisplayName?.Invoke(_dialogue.Name);
            }
            else
            {
                DisplayName?.Invoke(_dialogue.MainName);
            }
        }
        string sentence = Senteces.Dequeue();
        StopAllCoroutines();
        DisplayDialogue?.Invoke(sentence);
       // StartCoroutine(TypeSentences(sentence));
    }
 /*   private IEnumerator TypeSentences(string sentence)
    {
        DialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(LetterAppearanceTime);
        }
    }*/
    private void EndDialogue()
    {
        if (!_dialogue.Cyclic) _dialogue.PlayOnce = true;
        _dialogue.Started = false;
        EndedDialogueDisplay?.Invoke();
       // DialogBoxAnimator.SetBool("IsOpen", false);
        Player.Instance.UnFreeze();
    }
}