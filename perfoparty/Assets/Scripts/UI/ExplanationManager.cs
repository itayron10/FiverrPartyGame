using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExplanationManager : MonoBehaviour
{
    [SerializeField] GameObject dialougeParant;
    [SerializeField] TextMeshProUGUI dialougeText;
    [SerializeField] DialougeSO dialougeSO;
    [SerializeField] Animator dialougeAnimator;
    [SerializeField] SoundScriptableObject explainingSound;
    [SerializeField] AudioSource audioSource;
    [SerializeField] string dialougeRunningName;
    [SerializeField] float delay;
    [HideInInspector] public bool isDialougeRunning = true;
    private SoundManager soundManager;
    

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        soundManager = FindObjectOfType<SoundManager>();
        StartCoroutine(WriteDialouge(dialougeSO));
    }

    public IEnumerator WriteDialouge(DialougeSO dialouge)
    {
        isDialougeRunning = true;
        dialougeAnimator.SetBool(dialougeRunningName, isDialougeRunning);
        soundManager.PlaySound(explainingSound, audioSource);

        foreach (string sentence in dialouge.GetSentences)
        {
            StartCoroutine(WriteSentence(sentence, dialouge.GetLetterWritingSpeed));
            yield return new WaitForSeconds((sentence.Length + 2) * dialouge.GetLetterWritingSpeed);
            yield return new WaitForSeconds(dialouge.GetDelayBetweenSentences);
        }

        soundManager.StopSound(audioSource);
        isDialougeRunning = false;
        dialougeAnimator.SetBool(dialougeRunningName, isDialougeRunning);
    }

    private IEnumerator WriteSentence(string sentence, float writingDelay)
    {
        dialougeText.text = string.Empty;
        for (int i = 0; i < sentence.Length; i++)
        {
            dialougeText.text += sentence[i];
            yield return new WaitForSeconds(writingDelay);
        }
    }
}
