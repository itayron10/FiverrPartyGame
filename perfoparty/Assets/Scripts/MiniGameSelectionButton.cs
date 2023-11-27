using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameSelectionButton : EventButton
{
    [SerializeField] TextMeshProUGUI displayText;
    private int votes;

    private void Update()
    {
        displayText.text = votes.ToString();
    }

    public override void OnClick()
    {
        base.OnClick();
        votes++;
    }

}
