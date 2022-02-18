using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager singleton { get; private set; }
    
    [SerializeField] private CanvasGroup canvGroup;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    
    [SerializeField] private float fadeDuration = 0.4f;

    private AudioSource _audioSource;
    private bool _faded = true;
    private Queue<string> sentences;
    private AudioClip sfx;
    void Start()
    {
        sentences = new Queue<string>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        singleton = this;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;
        sfx = dialogue.sentenceSFX;
        sentences.Clear();
        
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        Fade();
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(String sentence)
    {
        yield return new WaitForSeconds(0.4f);
        dialogueText.text = "";
        int k = 0;
        foreach (char letter in sentence.ToCharArray())
        {
            if (k % 2 == 0)
            {
                _audioSource.PlayOneShot(sfx);
            }
            dialogueText.text += letter;
            k += 1;
            yield return new WaitForSeconds(.05f);
        }

        yield return new WaitForSeconds(1.6f);
        DisplayNextSentence();
        
    }
    
    void EndDialogue()
    {
        Fade();
    }
    
    public void Fade()
    {
        StartCoroutine(DoFade(canvGroup, canvGroup.alpha, _faded ? 1 : 0));
        //Debug.Log(canvGroup.alpha);
        _faded = !_faded;
    }
    
    public IEnumerator DoFade(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;

        while (counter < fadeDuration)
        {
            Debug.Log(canvGroup.alpha);
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / fadeDuration);

            yield return null;
        }
        
    }
}
