using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatComponent : MonoBehaviour {
    public VictoryCoach myVictoryCoach;

    // Use this for initialization
    void Start () {
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            myVictoryCoach.achieveNextExperience(false);
        }
	}
}
