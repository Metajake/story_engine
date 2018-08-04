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

	// Use this for initialization
	void Start () {
		scheduledDates = new List<Date>();
		myTimeLord = GameObject.FindObjectOfType<Timelord>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        uiManager = GameObject.FindObjectOfType<UIManager>();
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	internal void createDate(int dateLocation, int dateTime, Character speaker)
	{
		GameObject dateObject = new GameObject();
		Date date = dateObject.AddComponent<Date>();
        date.dateScene = dateLocation;
        date.dateTime = dateTime;
		date.character = speaker;
		this.scheduledDates.Add(date);
		speaker.noLocation();
	}
       
	public Character datePartner(int dateLocation, int dateTime){
		Character toReturn = null;
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
		var roll = roller.Next(0, 3);
       
		if(roll == 0){ //Falls in Love
		    uiManager.gameOver();
		}else if (roll == 1){
			uiManager.showNeutralDescriptionText();
		}else{
			uiManager.experienceDescription();
			myVictoryCoach.achievedExperience(mySceneCatalogue.getCurrentSceneNumber());
            
			foreach (Date date in this.scheduledDates)
            {
				if (date.dateTime == myTimeLord.getCurrentTimestep() && date.dateScene == mySceneCatalogue.getCurrentSceneNumber())
                {
					//date.character.activeTimes = date.character.savedTimes;
					date.character.savedTimes.CopyTo(date.character.activeTimes, 0);
					date.character.experienceCount++;
                }
            }
        }
	}

}
