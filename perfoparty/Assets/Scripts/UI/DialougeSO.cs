using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Dialouge", menuName ="DialougeSystem/Dialouge")]
public class DialougeSO : ScriptableObject
{
    [SerializeField] float letterWritingSpeed;
    [SerializeField] float delayBetweenSentences;
    [TextArea(4, 20)]
    [SerializeField] string[] sentences;


    public string[] GetSentences => sentences;
    public float GetDelayBetweenSentences => delayBetweenSentences;
    public float GetLetterWritingSpeed => letterWritingSpeed;

}
