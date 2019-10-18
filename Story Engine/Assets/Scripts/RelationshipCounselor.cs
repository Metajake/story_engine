using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipCounselor : MonoBehaviour {

	private List<Date> scheduledDates;
	private Timelord myTimelord;
    private SceneCatalogue mySceneCatalogue;
	private UIManager myUIManager;
    private EventQueue myEventQueue;
    private AnimationMaestro myAnimationMaestro;
	public bool isAtDate;
	public VictoryCoach myVictoryCoach;
    private GameState myGameState;
    public int loveChanceIncrement;

    private Dictionary<String, Dictionary<int, int>> actionLikelihoodMatrix;
    private AudioConductor myAudioConductor;

    void Start ()
    {
		scheduledDates = new List<Date>();
		myTimelord = GameObject.FindObjectOfType<Timelord>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myUIManager = GameObject.FindObjectOfType<UIManager>();
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
        myGameState = GameObject.FindObjectOfType<GameState>();
        myEventQueue = GameObject.FindObjectOfType<EventQueue>();
        myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
        myAudioConductor = FindObjectOfType<AudioConductor>();
        
        ConstructDateLikelihoods();
    }

    private void ConstructDateLikelihoods()
    {
        actionLikelihoodMatrix = new Dictionary<String, Dictionary<int, int>>();

        Dictionary<int, int> neutralReactions = new Dictionary<int, int>();
        neutralReactions.Add(-1, 25);
        neutralReactions.Add(-0, 35);
        neutralReactions.Add(1, 45);
        Dictionary<int, int> leaveReactions = new Dictionary<int, int>();
        leaveReactions.Add(-1, 55);
        leaveReactions.Add(0, 25);
        leaveReactions.Add(1, 5);
        //TODO remove "experience" dictionary because I don't think that it get's used anywhere.
        Dictionary<int, int> expReactions = new Dictionary<int, int>();
        expReactions.Add(-1, 20);
        expReactions.Add(-0, 30);
        expReactions.Add(1, 40);
        actionLikelihoodMatrix.Add("neutral", neutralReactions);
        actionLikelihoodMatrix.Add("leave", leaveReactions);
        actionLikelihoodMatrix.Add("experience", expReactions);
    }

	void Update ()
    {
        expireOldDates();
        checkStartDate();
    }

    private void expireOldDates()
    {
        foreach (Date d in getAllDates())
        {
            if (d.dateTime < myTimelord.getCurrentTimestep())
            {
                d.isOver = true;
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
	}

    private void checkStartDate()
    {
        if (myGameState.currentGameState == GameState.gameStates.PROWL)
        {
            if (mySceneCatalogue.getIsInInteriorScene())
            {
                if (hasDateAtPresentTimeInPresentLocationAndDateNotOver())
                {
                    isAtDate = true;
                    myAudioConductor.startMusic(myAudioConductor.dateMusic);
                    myGameState.currentGameState = GameState.gameStates.DATEINTRO;
                    myEventQueue.queueEvent(new EventDateStart());
                }
            }
        }
    }

    public DateableCharacter getDatePartner(Location dateLocation, int dateTime){
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

	public bool hasDateAtPresentTimeInPresentLocationAndDateNotOver(){
        return getCurrentDateFromScheduledDateList() != null;
	}

    public Date getCurrentDateFromScheduledDateList(){
        foreach (Date date in this.scheduledDates){
            if (date.dateTime == myTimelord.getCurrentTimestep() && date.dateScene == mySceneCatalogue.getCurrentLocation() && !date.isOver)
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

    public string convertPastDatesToDateInfo(List<Date> allDates)
    {
        List<Date> pastDates = new List<Date>();
        foreach (Date d in allDates)
        {
            if (d.isOver)
            {
                pastDates.Add(d);
            }
        }
        string allDatesInfo = stringifyAndSortDates(pastDates);
        return allDatesInfo;
    }

    public string convertUpcomingDatesToDateInfo(List<Date> allDates)
    {
        List<Date> upcomingDates = new List<Date>();
        foreach (Date d in allDates)
        {
            if (!d.isOver)
            {
                upcomingDates.Add(d);
            }
        }
        string allDatesInfo = stringifyAndSortDates(upcomingDates);
        return allDatesInfo;
    }

    // TODO Separate into two methods
    private string stringifyAndSortDates(List<Date> allDates)
    {
        allDates.Sort((x, y) => x.dateTime.CompareTo(y.dateTime));
        string allDatesInfo = "";
        foreach (Date d in allDates)
        {
            allDatesInfo += d.dateScene.interiorName + " " + myTimelord.getTimeString(d.dateTime) + " " + d.character.givenName + "\n";
        }

        return allDatesInfo;
    }

    public bool hasDateInFuture(Character character) {
        foreach (Date date in getAllDates()){
            if(date.dateTime >= myTimelord.getCurrentTimestep() && (date.character.givenName == character.givenName))
            {
                return true;
            }
        }
        return false;
    }

	public void leaveDate(){
		isAtDate = false;
        myGameState.currentGameState = GameState.gameStates.PROWL;
		mySceneCatalogue.toggleInteriorScene();
        myUIManager.resetDateButtons();
        myAudioConductor.fadeOutCurrentMusic();
        getCurrentDateFromScheduledDateList().isOver = true;
	}

	public void act(){
        if(myGameState.currentGameState == GameState.gameStates.DATEINTRO)
        {
            myGameState.currentGameState = GameState.gameStates.DATE;
        }

		var roller = new System.Random();
		var roll = roller.Next(0, 100);

        DateableCharacter she = getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimelord.getCurrentTimestep());

        Debug.Log("Roll: " + roll + " In love amount: " + she.inLoveAmount * this.loveChanceIncrement);

        if (roll < she.inLoveAmount*this.loveChanceIncrement) {
    		myUIManager.gameOver();
        }

        she.inLoveAmount ++;

        int leavePercentageForLocation = actionLikelihoodMatrix["leave"][she.locationPreferences[mySceneCatalogue.getCurrentSceneNumber()]];
        //int leavePercentageForLocation = 0; //FOR DEBUGGING

        if (roll <= leavePercentageForLocation){
            getCurrentDateFromScheduledDateList().isAbandoned = true;
            myAnimationMaestro.abandonDateDescription();
        }
        else if (roll <= leavePercentageForLocation + actionLikelihoodMatrix["neutral"][she.locationPreferences[mySceneCatalogue.getCurrentSceneNumber()]]){
            myAnimationMaestro.showNeutralDescriptionText();
            Debug.Log("Contextual pre-programmed neutral location description (which we will eventually do).");
        }else{
            getCurrentDateFromScheduledDateList().experienceAchieved = true;
            myVictoryCoach.achieveNextExperience(true);
        }
        myEventQueue.queueEvent(new DateActionEvent());
	}

}
