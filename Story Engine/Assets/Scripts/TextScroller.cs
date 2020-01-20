using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScroller : MonoBehaviour
{

	private string fullTextValue;
	private float showingCharacters;
	
	public Text textComponentToScroll;
	public float scrollingSpeed = 1f;

	void Awake()
	{
		fullTextValue = "";
		textComponentToScroll.text = "";
	}
    
	void Update()
	{
		showingCharacters = Math.Min(fullTextValue.Length, showingCharacters + Time.deltaTime * scrollingSpeed);
		textComponentToScroll.text = fullTextValue.Substring(0, (int)showingCharacters);

	}
    

	public void SetText(string textToScroll)
	{
		if (fullTextValue == textToScroll) return;
		showingCharacters = 0;
		fullTextValue = textToScroll;
	}
}