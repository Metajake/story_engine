using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour {

    public string locationName;
    public string interiorName;
    public string neutralDateResultDescription;
    public string experienceDescription;
    public bool isDateScene;
    public bool isKnown;
    public string descriptionInterior;
    public string descriptionExterior;
    public string[] dateActions;
    public string currentDateAction;
    private System.Random random;

    // Use this for initialization
    void Start () {
        random = new System.Random();
        currentDateAction = dateActions[random.Next(dateActions.Length)];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setRandomDateAction()
    {
        currentDateAction = dateActions[random.Next(dateActions.Length)];
    }
}
