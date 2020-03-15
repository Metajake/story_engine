using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timelord : MonoBehaviour {
       
	public string[] timeNames;
	public int timeStep;
	public Text dayText;
    private DialogueManager myDialogueManager;
    private SceneCatalogue mySceneCatalogue;
    private CommandBuilder myCommandBuilder;
    private AnimationMaestro myAnimationMaestro;
    private VictoryCoach myVictoryCoach;
    private int creepAmount;
    private Location pastLocation;
    private bool pastInteriorStatus;

	// Use this for initialization
	void Start () {
		timeStep = 0;	
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myCommandBuilder = GameObject.FindObjectOfType<CommandBuilder>();
        myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();


    }

    // Update is called once per frame
    void Update () {
		int currentTimeStep = getCurrentTimestep();
		dayText.text = (currentTimeStep / timeNames.Length).ToString() + " " + getDayOfWeek(currentTimeStep / timeNames.Length);
    }

    //TODO This is very similar to SceneCatalogue.ToggleInteriorScene(). Refactor?
    public void checkCharactersToFadeAndAdvanceTime()
    {
        if (myDialogueManager.getAllCurrentLocalPresentConversationPartners().Count > 0)
        {
            myAnimationMaestro.fadeOutCharacters(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
            StartCoroutine(myAnimationMaestro.delayGameCoroutine(0.6f, this.advanceTimestep));
        }
        else
        {
            this.advanceTimestep();
        }
    }

	private void advanceTimestep(){
        timeStep++;

        myDialogueManager.checkCharacterRelocate();

        myVictoryCoach.checkQuestsCompleteAndQueueEvent(new EventTimeChange(), toBuild: false);

        if ( checkIfCreep() ) {
            relocatePlayerEvent();
        }

        if ( checkIfWeekEvent(timeStep) ) {
            weekEvent();
        }
        
        myAnimationMaestro.updatePotentialPartnersSprites(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
        
        //build the stuff in the conditionals, above
        myCommandBuilder.build();
    }

    private void relocatePlayerEvent()
    {
        mySceneCatalogue.setRandomKnownScene();
        myCommandBuilder.createAndEnqueueChangeDialogueSequence(new List<string>() { "Go somewhere else. Stop creeping around one location." });
    }

    private void weekEvent()
    {
        myCommandBuilder.createAndEnqueueChangeDialogueSequence(new List<string>() {
                "It's been another whole week. Time flies by when you're on this grind.",
                "I wonder where I'll meet people to talk to this week. It's a big city!"
        });
    }

    private bool checkIfWeekEvent(int timeStepToCheck)
    {
        if (timeStep % 21 == 0)
        { //if it's a multiple of 21 (aka Every 7 Days)
            return true;
        }
        return false;
    }

    private bool checkIfCreep()
    {
        Location currentLocation = mySceneCatalogue.getCurrentLocation();
        if (pastLocation != null && (currentLocation.interiorName != "Apartment" || !mySceneCatalogue.getIsInInteriorScene())){
            if (currentLocation.locationName == pastLocation.locationName && mySceneCatalogue.getIsInInteriorScene() == pastInteriorStatus)
            {
                creepAmount++;
            }
            else
            {
                creepAmount = 0;
            }
        }

        if (creepAmount >= 5)
        {
            creepAmount = 0;
            return true;
        }

        pastLocation = currentLocation;
        pastInteriorStatus = mySceneCatalogue.getIsInInteriorScene();
        return false;
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

    public string getTimeString(int timeStep)
    {
        return this.getDayOfWeekForTimeStep(timeStep) + " in the " + this.getTimeNameForTimeStep(timeStep);
    }
}
