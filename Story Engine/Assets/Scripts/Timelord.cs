﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timelord : MonoBehaviour {
       
	public string[] timeNames;
	public int timeStep;
	public Text dayText;
    private DialogueManager myDialogueManager;
    private SceneCatalogue mySceneCatalogue;
    private EventQueue myEventQueue;
    private CommandBuilder myCommandBuilder;
    private AnimationMaestro myAnimationMaestro;

    private int creepAmount;
    private Location pastLocation;
    private bool pastInteriorStatus;

	// Use this for initialization
	void Start () {
		timeStep = 0;	
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myEventQueue = GameObject.FindObjectOfType<EventQueue>();
        myCommandBuilder = GameObject.FindObjectOfType<CommandBuilder>();
        myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();

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

    //TODO See if we can rewrite the command sequences so that we don't have to make conditional calls in advanceTimestep
	private void advanceTimestep(){
		myDialogueManager.selectedPartner = -1;
        timeStep++;

        if (checkIfScatterCharacters(timeStep) && checkIfCreep())
        {
            scatterCharactersAndRelocatePlayerEvent();
            myEventQueue.queueEvent(new EventTimeChange());
        }
        else if (checkIfScatterCharacters(timeStep))
        {
            scatterCharactersEvent();
            myEventQueue.queueEvent(new EventTimeChange());
        }
        else if (checkIfCreep())
        {
            relocatePlayerEvent();
            myEventQueue.queueEvent(new EventTimeChange());
        }
        else
        {
            myEventQueue.queueEvent(new EventTimeChange());
        }
        
    }

    private void relocatePlayerEvent()
    {
        mySceneCatalogue.setRandomKnownScene();
        myAnimationMaestro.updatePotentialPartnersSprites(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
        myCommandBuilder.createAndEnqueueChangeDialogueSequence(new List<string>() { "Go somewhere else. Stop creeping around one location." });
        myCommandBuilder.build();
    }

    private void scatterCharactersEvent()
    {
        myDialogueManager.scatterCharacters();
        myAnimationMaestro.updatePotentialPartnersSprites(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
        myCommandBuilder.createAndEnqueueChangeDialogueSequence(new List<string>() {
                "It's been another whole week. Time flies by when you're really out here, on this grind.",
                "I wonder where I'll meet people to talk to this week. It's a big city!"
        });
        myCommandBuilder.build();
    }

    private void scatterCharactersAndRelocatePlayerEvent()
    {
        myDialogueManager.scatterCharacters();
        mySceneCatalogue.setRandomKnownScene();
        myAnimationMaestro.updatePotentialPartnersSprites(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
        myCommandBuilder.createAndEnqueueChangeDialogueSequence(new List<string>() {
                "Go somewhere else. Stop creeping around one location.",
                "It's been another whole week. Time flies by when you're really out here, on this grind.",
                "I wonder where I'll meet people to talk to this week. It's a big city!"
            });
        myCommandBuilder.build();
    }

    private bool checkIfScatterCharacters(int timeStepToCheck)
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

        if (creepAmount >= 7)
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
