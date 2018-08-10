using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timelord : MonoBehaviour {
       
	public string[] timeNames;
	public int timeStep;
	public Text dayText;
    private DialogueManager myDialogueManager;

	// Use this for initialization
	void Start () {
		timeStep = 0;	
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
	}
	
	// Update is called once per frame
	void Update () {
		int currentTimeStep = getCurrentTimestep();
		dayText.text = (currentTimeStep / timeNames.Length).ToString() + " " + getDayOfWeek(currentTimeStep / timeNames.Length);
    }

	public void advanceTimestep(){
		GameObject.FindObjectOfType<DialogueManager>().selectedPartner = -1;
        timeStep++;
        if(timeStep % 21 == 0){ //if it's a multiple of 21
            myDialogueManager.ninjaVanish(); 
        }
	}

	public int getCurrentModulusTimestep()
    {
		return timeStep % timeNames.Length;
    }

	public int getCurrentTimestep()
    {
        return timeStep;
    }

	private string getDayOfWeek(int day){
		switch(day % 7){
			case 0:
				return "Sunday";
			case 1:
                return "Monday";
			case 2:
                return "Tuesday";
			case 3:
                return "Wednesday";
			case 4:
                return "Thursday";
			case 5:
                return "Friday";
            case 6:
                return "Saturday";
			default:
				return "ERROR";
		}
	}

	public int getTimeStepsPerDay(){
		return this.timeNames.Length;
	}

	public string getDayOfWeekForTimeStep(int timeStep){
		return getDayOfWeek(timeStep / getTimeStepsPerDay());
	}

	public string getTimeNameForTimeStep(int timeStep){
		return timeNames[timeStep % timeNames.Length];
	}

    public int getWeek(){
        return this.timeStep / 21;
    }
}
