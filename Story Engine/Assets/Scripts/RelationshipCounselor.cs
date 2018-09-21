using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipCounselor : MonoBehaviour {

	private List<Date> scheduledDates;
	private Timelord myTimeLord;
    private SceneCatalogue mySceneCatalogue;
	private UIManager uiManager;
	public bool isAtDate;
	public VictoryCoach myVictoryCoach;

    private Dictionary<String, Dictionary<int, int>> actionLikelihoodMatrix;

	// Use this for initialization
	void Start ()
    {
		scheduledDates = new List<Date>();
		myTimeLord = GameObject.FindObjectOfType<Timelord>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        uiManager = GameObject.FindObjectOfType<UIManager>();
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();

        ConstructDateLikelihoods();
    }

    private void ConstructDateLikelihoods()
    {
        actionLikelihoodMatrix = new Dictionary<String, Dictionary<int, int>>();

        Dictionary<int, int> neutralReactions = new Dictionary<int, int>();
        neutralReactions.Add(-2, 20);
        neutralReactions.Add(-1, 25);
        neutralReactions.Add(-0, 35);
        neutralReactions.Add(1, 45);
        neutralReactions.Add(2, 30);
        Dictionary<int, int> leaveReactions = new Dictionary<int, int>();
        leaveReactions.Add(-2, 70);
        leaveReactions.Add(-1, 55);
        leaveReactions.Add(0, 35);
        leaveReactions.Add(1, 20);
        leaveReactions.Add(2, 10);
        Dictionary<int, int> expReactions = new Dictionary<int, int>();
        expReactions.Add(-2, 10);
        expReactions.Add(-1, 20);
        expReactions.Add(-0, 30);
        expReactions.Add(1, 40);
        expReactions.Add(2, 60);
        actionLikelihoodMatrix.Add("neutral", neutralReactions);
        actionLikelihoodMatrix.Add("leave", leaveReactions);
        actionLikelihoodMatrix.Add("experience", expReactions);
    }

	// Update is called once per frame
	void Update () {
	}

	internal void createDate(int dateLocation, int dateTime, DateableCharacter speaker)
	{
		GameObject dateObject = new GameObject();
		Date date = dateObject.AddComponent<Date>();
        date.dateScene = dateLocation;
        date.dateTime = dateTime;
		date.character = speaker;
		this.scheduledDates.Add(date);
		speaker.noLocation();
	}
       
	public DateableCharacter datePartner(int dateLocation, int dateTime){
		DateableCharacter toReturn = null;
        foreach (Date date in this.scheduledDates)
        {
            if (date.dateTime == dateTime && date.dateScene == dateLocation)
            {
                toReturn = date.character;
            }
        }
        return toReturn;
	}

	public bool isInDateMode(){
		bool toReturn = false;
        foreach (Date date in this.scheduledDates){
			if(date.dateTime == myTimeLord.getCurrentTimestep() && date.dateScene == mySceneCatalogue.getCurrentSceneNumber()){
                toReturn = true;
            }
        }
        return toReturn;
	}

	public void leaveDate(){
		isAtDate = false;
		mySceneCatalogue.toggleInteriorScene();
		uiManager.resetDateButtons();
	}

	public void act(){
		//Roll Dice
		var roller = new System.Random();
		var roll = roller.Next(0, 100);

        DateableCharacter she = datePartner(mySceneCatalogue.getCurrentSceneNumber(), myTimeLord.getCurrentTimestep());

        if(roller.Next(0, 20) == 0){ //5% chance to fall in love
    		uiManager.gameOver();
        }

        int leavePercentageForLocation = actionLikelihoodMatrix["leave"][she.locationPreferences[mySceneCatalogue.getCurrentSceneNumber()]];

        if(roll <= leavePercentageForLocation){
            uiManager.abandonDateDescription();
            Debug.Log("You got ditched, you lame. Enjoy watching porn at home, alone.");
        }else if (roll <= leavePercentageForLocation + actionLikelihoodMatrix["neutral"][she.locationPreferences[mySceneCatalogue.getCurrentSceneNumber()]]){
            uiManager.showNeutralDescriptionText();
            Debug.Log("Contextual pre-programmed neutral location description (which we will eventually do).");
        }else{
            uiManager.experienceDescription();
            myVictoryCoach.achievedExperience(mySceneCatalogue.getCurrentSceneNumber());
        }

        foreach (Date date in this.scheduledDates){
			if (date.dateTime == myTimeLord.getCurrentTimestep() && date.dateScene == mySceneCatalogue.getCurrentSceneNumber())
			{
				date.character.savedTimes.CopyTo(date.character.activeTimes, 0);
				date.character.experienceCount++; // WE"RE NOT USING THIS YET :))
			}
		}
	}

}
