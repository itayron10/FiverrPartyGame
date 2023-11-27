using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : EventButton
{
    public override void OnClick()
    {
        base.OnClick();
        Application.Quit();
    }
}
