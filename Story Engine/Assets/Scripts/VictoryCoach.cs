using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VictoryCoach : MonoBehaviour, IEventSubscriber {

    public Dictionary<string, Experience> remainingExperiences;
    private DifficultyLevel nextGoal;
    private bool isIrresponsible;
    private List<Experience> achievedExperiences;
    private CommandBuilder myCommandBuilder;
    private SceneCatalogue mySceneCatalogue;
    private EventQueue myEventQueue;
    private DialogueManager myDialogueManager;
    private Timelord myTimeLord;

    private void Awake()
    {
        remainingExperiences = new Dictionary<string, Experience>();
        achievedExperiences = new List<Experience>();
        myCommandBuilder = GameObject.FindObjectOfType<CommandBuilder>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myEventQueue = GameObject.FindObjectOfType<EventQueue>();
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        myTimeLord = GameObject.FindObjectOfType<Timelord>();
    }

    // Use this for initialization
    void Start () {

        foreach (Experience exp in this.GetComponents<Experience>())
        {
            // TODO Experience Name is duplicate Data
            remainingExperiences.Add(exp.experienceName, exp);
        }
        
        nextGoal = DifficultyLevel.EASY;

        isIrresponsible = true;
        myEventQueue.subscribe(this);
	}
	
	// Update is called once per frame
	void Update () {
        if(hasAchievedSomeExperiences()){
            goalAchieved(nextGoal);
            this.nextGoal += 3;
        }
		
    }

    public bool hasAchievedSomeExperiences(){
        return getNumberOfAchievedExperiences() >= (int) nextGoal;
	}

    public int getNumberOfAchievedExperiences(){
        return achievedExperiences.Count();
    }

    private void goalAchieved(DifficultyLevel levelAchieved)
    {
        if (levelAchieved == DifficultyLevel.HARD)
        {
            Debug.Log("You have ascended to the ultimate form of human being. Your love is free. You are at one with everyone.");
        }
        else
        {
            Debug.Log("Goal " + levelAchieved + " Attained!! ... But there are more experiences to be had!");
        }
    }

    public void achieveNextExperience(bool playCutscene)
    {
        System.Random random = new System.Random();
        Experience toReturn;
        if (isIrresponsible)
        {
            toReturn = remainingExperiences["responsibility"];
            remainingExperiences.Remove("responsibility");
            isIrresponsible = false;
        }
        else if(isEndOfGame()){
            toReturn = remainingExperiences["create"];
            remainingExperiences.Remove("create");
        }
        else
        {
            List<Experience> expList = getExperiencesExceptFinal();
            Experience toRemoveAndReturn = expList[random.Next(expList.Count)];
            remainingExperiences.Remove(toRemoveAndReturn.experienceName);
            toReturn = toRemoveAndReturn;
        }
        achievedExperiences.Add(toReturn);

        if (playCutscene)
        {
            myCommandBuilder.createAndEnqueueDateCutSceneSequence(new List<string>(toReturn.experienceCutSceneTexts), isEndOfGame() );
            myCommandBuilder.build(GameState.gameStates.CUTSCENE);
        }
    }

    private bool isEndOfGame()
    {
        List<Experience> expList = new List<Experience>(remainingExperiences.Values);
        return expList.Count <= 1;
    }

    private List<Experience> getExperiencesExceptFinal()
    {
        List<Experience> expList = new List<Experience>(remainingExperiences.Values);
        expList = new List<Experience>( expList.Where( exp => exp.experienceName != "create") );
        return expList;
    }

    public string convertExperiencesToExperienceInfo()
    {
        string achievedExperienceInfo = "";
        foreach (Experience e in achievedExperiences)
        {
            achievedExperienceInfo += e.experienceName + "\n";
        }

        for(int i = achievedExperiences.Count() + 1 ; i <= remainingExperiences.Count() + achievedExperiences.Count(); i++) {
            achievedExperienceInfo += i + ". \n";
        }

        return achievedExperienceInfo;
    }

    private bool tutorialComplete = false;
    void IEventSubscriber.eventOccurred(IGameEvent occurringEvent)
    {
        if (
            mySceneCatalogue.getCurrentSceneName() == "City"
            && mySceneCatalogue.getIsInInteriorScene() == true
            && myTimeLord.getCurrentModulusTimestep() == 0
            && tutorialComplete == false
        ){
            myCommandBuilder.createAndEnqueueSummonCharacterSequence(myDialogueManager.getCharacterForName("evan"), 1, "Welcome to the rat race.");
            myCommandBuilder.createAndEnqueueChangeDialogueSequence(new List<string>(){
                "Just kidding. It's not that bad. Here's how things work:",
                "Some people that you meet have regular schedules. Other characters will move around the city more often.",
                "Meet your new coworker, Kristie Yamaguchi."
            });
            myCommandBuilder.createAndEnqueueSummonCharacterSequence(myDialogueManager.getCharacterForName("kristie"), 2, "Go ahead and introduce yourself.");
            myCommandBuilder.build();
            tutorialComplete = true;
        }
    }
}
