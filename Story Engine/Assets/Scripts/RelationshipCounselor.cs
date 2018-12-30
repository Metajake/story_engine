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
    private CommandProcessor myCommandProcessor;
    public int loveChanceIncrement;

    private Dictionary<String, Dictionary<int, int>> actionLikelihoodMatrix;
    
	void Start ()
    {
		scheduledDates = new List<Date>();
		myTimelord = GameObject.FindObjectOfType<Timelord>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myUIManager = GameObject.FindObjectOfType<UIManager>();
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
        myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();
        myGameState = GameObject.FindObjectOfType<GameState>();
        myEventQueue = GameObject.FindObjectOfType<EventQueue>();
        myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();

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

        Debug.Log("Roll: " + roll);
        Debug.Log("In love amount: " + she.inLoveAmount*this.loveChanceIncrement);
        if (roll < she.inLoveAmount*this.loveChanceIncrement) {
    		myUIManager.gameOver();
        }

        she.inLoveAmount ++;

        //int leavePercentageForLocation = actionLikelihoodMatrix["leave"][she.locationPreferences[mySceneCatalogue.getCurrentSceneNumber()]];
        int leavePercentageForLocation = 0; //FOR DEBUGGING

        if (roll <= leavePercentageForLocation){
            myAnimationMaestro.abandonDateDescription();
            Debug.Log("You got ditched, you lame. Enjoy watching porn at home, alone.");
        }else if (roll <= leavePercentageForLocation + actionLikelihoodMatrix["neutral"][she.locationPreferences[mySceneCatalogue.getCurrentSceneNumber()]]){
            myAnimationMaestro.showNeutralDescriptionText();
            Debug.Log("Contextual pre-programmed neutral location description (which we will eventually do).");
        }else{
            getCurrentDateFromScheduledDateList().experienceAchieved = true;
            myVictoryCoach.achievedExperience(mySceneCatalogue.getCurrentSceneNumber());
            Experience currentExp = myVictoryCoach.getNextExperience();
            myCommandProcessor.createAndEnqueueCutSceneSequence(new List<string>(currentExp.experienceCutSceneTexts));
        }
        myEventQueue.queueEvent(new DateActionEvent());
	}

}
