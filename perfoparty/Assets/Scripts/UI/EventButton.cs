using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventButton : MonoBehaviour
{
    [SerializeField] SoundScriptableObject clickSound;
    private SoundManager soundManager;
    private Button button;

    void Start() => FindPrivateObjects();

    private void OnLevelWasLoaded(int level)
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public virtual void FindPrivateObjects()
    {
        button = GetComponent<Button>();
        soundManager = FindObjectOfType<SoundManager>();
        button.onClick.AddListener(OnClick);
    }

    public virtual void OnClick()
    {
        // this method get called when the button is clicked
        soundManager.PlaySound(clickSound);
    }

}
