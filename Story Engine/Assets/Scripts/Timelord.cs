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
    private EventQueue myEventQueue;
    private CommandProcessor myCommandProcessor;
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
        myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();
        myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();

    }

    // Update is called once per frame
    void Update () {
		int currentTimeStep = getCurrentTimestep();
		dayText.text = (currentTimeStep / timeNames.Length).ToString() + " " + getDayOfWeek(currentTimeStep / timeNames.Length);
    }

	public void advanceTimestep(){
		myDialogueManager.selectedPartner = -1;
        timeStep++;
        if(timeStep % 21 == 0){ //if it's a multiple of 21 (aka Every 7 Days)
            myDialogueManager.scatterCharacters(); 
        }
        if (checkIfCreep())
        {
            mySceneCatalogue.setRandomKnownScene();
            myAnimationMaestro.updatePotentialPartnersSprites(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
            myCommandProcessor.createAndEnqueueChangeDialogueSequence(new List<string>() { "Go somewhere else. Stop creeping around one location." });
        }
        myEventQueue.queueEvent(new TimeChangeEvent());
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

    public int getWeek(){
        return this.timeStep / 21;
    }

    public string getTimeString(int timeStep)
    {
        return this.getDayOfWeekForTimeStep(timeStep) + " in the " + this.getTimeNameForTimeStep(timeStep);
    }
}
