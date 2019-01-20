using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScroller : MonoBehaviour
{

	public Text _textComponentToScroll;
	public int _timeScrollingStart;
	public string _fullTextValue;
	public int showingCharacters;
	
	public int framesPerCharacter = 4;

	void Awake()
	{
		_fullTextValue = "";
		_textComponentToScroll.text = "";
		_timeScrollingStart = 0;
	}
    
	void Update()
	{
		showingCharacters = Math.Min(_fullTextValue.Length, (Time.frameCount - _timeScrollingStart) / framesPerCharacter);
		_textComponentToScroll.text = _fullTextValue.Substring(0, showingCharacters);

	}
    

	public void SetText(string textToScroll)
	{
		if (_fullTextValue == textToScroll) return;
		_fullTextValue = textToScroll;
		_timeScrollingStart = Time.frameCount;
	}
}