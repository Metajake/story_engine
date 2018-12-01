using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TextDelayer : MonoBehaviour
{

    public Text displayDialogueBox;
    public float delayPerCharacter;
    protected string _text;
    protected int framesSinceTextChanged;
    public string Text
    {
        set
        {
            if (value == _text)
            {
                return;
            }
            else
            {
                framesSinceTextChanged = 0;
                _text = value;
                return;
            }
        }
        get
        {
            int charactersToShow = (int)(framesSinceTextChanged / delayPerCharacter);
            return _text.Substring(0, Math.Min(charactersToShow, _text.Length));
        }
    }

    // Use this for initialization
    void Start()
    {
        framesSinceTextChanged = 0;
        _text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if( !displayDialogueBox.text.Contains(Text)){
            Text = displayDialogueBox.text;   
		}
        displayDialogueBox.text = Text;
        framesSinceTextChanged++;
    }
}