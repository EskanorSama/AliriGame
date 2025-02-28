using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    [Header ("Диалоги")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float LetterAppearanceTime = 0.1f;
    private Animator DialogBoxAnimator;
    [SerializeField] private Text NameText, DialogueText;
    [SerializeField]private TextMeshProUGUI Echo;
    private void Start()
    {
        DialogBoxAnimator = NameText.transform.parent.GetComponent<Animator>();
    }
    public void StartDisplayDialogue()
    {
        DialogBoxAnimator.SetBool("IsOpen", true);
        NameText.transform.parent.gameObject.SetActive(true);
    }
    public void ChangeNameInDialogue(string _name)
    {
        NameText.text = _name;
    }
    public void DisplayDialogueSentences(string sentence)
    {
        StartCoroutine(TypeSentences(sentence));
    }
    public void EndDisplayDialogue()
    {
        DialogBoxAnimator.SetBool("IsOpen", false);
    }
    private IEnumerator TypeSentences(string sentence)
    {
        DialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(LetterAppearanceTime);
        }
    }
    public void ShowEcho(int echo)
    {
        Echo.text = "Отголоски:" + echo.ToString();
    }
    private void OnEnable()
    {
        DialogueManager.EndedDialogueDisplay += EndDisplayDialogue;
        DialogueManager.DisplayDialogue += DisplayDialogueSentences;
        DialogueManager.DisplayName += ChangeNameInDialogue;
        DialogueManager.StartedDialogueDisplay += StartDisplayDialogue;
    }
    private void OnDisable()
    {
        DialogueManager.EndedDialogueDisplay -= EndDisplayDialogue;
        DialogueManager.DisplayDialogue -= DisplayDialogueSentences;
        DialogueManager.DisplayName -= ChangeNameInDialogue;
        DialogueManager.StartedDialogueDisplay -= StartDisplayDialogue;
    }
}
