using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
    public enum gameStates { PROWL, DATE, CUTSCENE, COMMANDSEQUENCE, TIPCONVERSATION, CONVERSATION };
    public gameStates currentGameState;
    public bool hasGameBegun;
    public bool hasLearnedHowToDate;

	// Use this for initialization
	void Start () {
        currentGameState = gameStates.COMMANDSEQUENCE;
        hasGameBegun = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
