using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
    public enum gameStates { PROWL, DATE, DATEINTRO, DATEOUTRO, CUTSCENE, COMMANDSEQUENCE, CONVERSATION };
    public gameStates currentGameState;
    public bool hasLearnedHowToDate;

	// Use this for initialization
	void Start () {
        currentGameState = gameStates.COMMANDSEQUENCE;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
