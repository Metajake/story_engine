﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VictoryCoach : MonoBehaviour {

    public Dictionary<string, Experience> remainingExperiences;
    private DifficultyLevel nextGoal;
    private bool isIrresponsible;
    private List<Experience> achievedExperiences;
    private CommandBuilder myCommandBuilder;
    private SceneCatalogue mySceneCatalogue;
    private DialogueManager myDialogueManager;
    private Timelord myTimeLord;
    private EventQueue myEventQueue;

    private void Awake()
    {
        remainingExperiences = new Dictionary<string, Experience>();
        achievedExperiences = new List<Experience>();
        myCommandBuilder = GameObject.FindObjectOfType<CommandBuilder>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        myTimeLord = GameObject.FindObjectOfType<Timelord>();
        myEventQueue = GameObject.FindObjectOfType<EventQueue>();

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

        Character datePartner = GameObject.FindObjectOfType<RelationshipCounselor>().getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimeLord.getCurrentTimestep());
        if(toReturn.experienceName == "protect")
        {

            myCommandBuilder.createAndEnqueueAddCharToDateCutSceneCharListSequence(myDialogueManager.getCharacterForName(datePartner.givenName), "God, I'm having so much fun!");
            myCommandBuilder.createAndEnqueueChangeDialogueSequence(new List<string>(){
                "Things are going pretty good!",
                "I wonder if I should make my move?...",
                "............."
            });
            myCommandBuilder.createAndEnqueueSummonDateCutSceneCharacterSequence(myDialogueManager.getCharacterForName("chad"), "Hey buddy! What's going on here? I see you all are up to some fun!");
            myCommandBuilder.createAndEnqueueChangeDialogueSequence(new List<string>(){
                    "I respect you though, dog. And for that I'm gonna let you enjoy this one. Smell ya later...",
            }, isEndOfGame());
            myCommandBuilder.build(stateToBeginIn: GameState.gameStates.DATECUTSCENE, stateToEndIn: GameState.gameStates.DATEOUTRO);
        }
        else
        {
            myCommandBuilder.createAndEnqueueAddCharToDateCutSceneCharListSequence(myDialogueManager.getCharacterForName(datePartner.givenName), "God, I'm having so much fun!");
            myCommandBuilder.createAndEnqueueChangeDialogueSequence(new List<string>(toReturn.experienceCutSceneTexts), isEndOfGame());
            myCommandBuilder.build(stateToBeginIn: GameState.gameStates.DATECUTSCENE, stateToEndIn: GameState.gameStates.DATEOUTRO);
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
    public bool checkTutorialConditionsMet()
    {
        return tutorialComplete == false && mySceneCatalogue.getIsInInteriorScene() == true && mySceneCatalogue.getCurrentSceneName() == "City" && myTimeLord.getCurrentModulusTimestep() == 0;
    }

    public void playTutorialCommandSequence(bool toBuild)
    {
        //TODO Why can't I move the below algorithm to a Command Sequence?
        int currentCharacterCount = myDialogueManager.getAllCurrentLocalPresentConversationPartners().Count;
        for (int i = 0; i < currentCharacterCount; i++)
        {
            Character charToRemove = myDialogueManager.getPartnerAt(i + 1);
            charToRemove.returnTime = myTimeLord.getCurrentTimestep()+1;
            charToRemove.isPresent = false;
        }
        GameObject.FindObjectOfType<UIManager>().setMainPanelActive();

        myCommandBuilder.createAndEnqueueSummonCharacterSequence(myDialogueManager.getCharacterForName("evan"), "Welcome to the rat race.");
        myCommandBuilder.createAndEnqueueChangeDialogueSequence(new List<string>(){
                "Just kidding. It's not that bad. Here's how things work:",
                "Some people that you meet have regular schedules. Other characters will move around the city more often.",
        });
        myCommandBuilder.createAndEnqueueSummonCharacterSequence(myDialogueManager.getCharacterForName("chad"), "This is Chad, the city bully. He will force you out of locations.");
        myCommandBuilder.createAndEnqueueRemoveCharacterSequence("chad", "See ya!");
        myCommandBuilder.createAndEnqueueChangeDialogueSequence(new List<string>(){
            "Get strong enough to force him to leave.",
            "Meet your new coworker, Kristie Kerner."
        });
        myCommandBuilder.createAndEnqueueSummonCharacterSequence(myDialogueManager.getCharacterForName("kristie"), "Go ahead and introduce yourself.");
        if (toBuild)
        {
            myCommandBuilder.build();
        }
        this.tutorialComplete = true;
    }

    public void checkQuestsCompleteAndQueueEvent(IGameEvent eventToQueue, bool toBuild = true)
    {
        if (checkTutorialConditionsMet())
        {
            playTutorialCommandSequence(toBuild);
        }
        else
        {
            myEventQueue.queueEvent(eventToQueue);
        }
    }
}
