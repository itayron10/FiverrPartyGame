using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventButton : MonoBehaviour
{
    private Button button;

    void Start() => FindPrivateObjects();

    public virtual void FindPrivateObjects()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public virtual void OnClick()
    {
        // this method get called when the button is clicked
    }

}
