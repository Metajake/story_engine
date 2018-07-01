using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conversation : MonoBehaviour {

	public Character speaker;
	public SpeechOption lastChosenOption;
	public bool greetedSoFar;
	public bool complimentedSoFar;
	public bool questionedSoFar;
	public bool demandedSoFar;
    public int opinion;
	public int complimentCount;
	public int questionCount;
	public int demandCount;

	private void Start()
	{
		this.lastChosenOption = SpeechOption.NONE;
        this.complimentCount = 0;
        this.questionCount = 0;
        this.demandCount = 0;
		this.opinion = 0;

	}

	public enum SpeechOption { NONE, INTRODUCTION, COMPLIMENT, QUESTION, DEMAND, ASK_ON_DATE };
}
