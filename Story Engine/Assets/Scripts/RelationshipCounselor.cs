﻿using System;
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
    private GameState myGameState;
    public int loveChanceIncrement;

    private Dictionary<String, Dictionary<int, int>> actionLikelihoodMatrix;
    
	void Start ()
    {
		scheduledDates = new List<Date>();
		myTimeLord = GameObject.FindObjectOfType<Timelord>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        uiManager = GameObject.FindObjectOfType<UIManager>();
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
        myGameState = GameObject.FindObjectOfType<GameState>();

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

	void Update () {
        foreach (Date date in this.scheduledDates)
        {
            if (date.dateTime < myTimeLord.getCurrentTimestep())
            {
                date.character.savedTimes.CopyTo(date.character.activeTimes, 0);
            }
        }
    }

	internal void createDate(Location dateLocation, int dateTime, DateableCharacter speaker)
	{
		GameObject dateObject = new GameObject();
		Date date = dateObject.AddComponent<Date>();
        date.dateScene = dateLocation;
        date.dateTime = dateTime;
        date.character = speaker;
        date.isOver = false;
		this.scheduledDates.Add(date);
		speaker.noLocation();
	}
       
	public DateableCharacter datePartner(Location dateLocation, int dateTime){
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
        return getCurrentDate() != null;
	}

    public Date getCurrentDate(){
        foreach (Date date in this.scheduledDates){
            if (date.dateTime == myTimeLord.getCurrentTimestep() && date.dateScene == mySceneCatalogue.getCurrentLocation() && !date.isOver)
            {
                return date;
            }
        }
        return null;
    }

    public List<Date> getAllDates()
    {
        return new List<Date>(this.scheduledDates);
    }

	public void leaveDate(){
		isAtDate = false;
        myGameState.currentGameState = GameState.gameStates.PROWL;
		mySceneCatalogue.toggleInteriorScene();
		uiManager.resetDateButtons();
        getCurrentDate().isOver = true;
	}

	public void act(){
		var roller = new System.Random();
		var roll = roller.Next(0, 100);

        DateableCharacter she = datePartner(mySceneCatalogue.getCurrentLocation(), myTimeLord.getCurrentTimestep());

        Debug.Log("Roll: " + roll);
        Debug.Log("In love amount: " + she.inLoveAmount*this.loveChanceIncrement);
        if (roll < she.inLoveAmount*this.loveChanceIncrement) {
    		uiManager.gameOver();
        }

        she.inLoveAmount ++;

        //int leavePercentageForLocation = actionLikelihoodMatrix["leave"][she.locationPreferences[mySceneCatalogue.getCurrentSceneNumber()]];
        int leavePercentageForLocation = 0; //FOR DEBUGGING

        if (roll <= leavePercentageForLocation){
            uiManager.abandonDateDescription();
            Debug.Log("You got ditched, you lame. Enjoy watching porn at home, alone.");
        }else if (roll <= leavePercentageForLocation + actionLikelihoodMatrix["neutral"][she.locationPreferences[mySceneCatalogue.getCurrentSceneNumber()]]){
            uiManager.showNeutralDescriptionText();
            Debug.Log("Contextual pre-programmed neutral location description (which we will eventually do).");
        }else{
            uiManager.experienceDescription();
            myVictoryCoach.achievedExperience(mySceneCatalogue.getCurrentSceneNumber());
        }
        
	}

}
