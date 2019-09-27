using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VictoryCoach : MonoBehaviour {

    public Dictionary<string, Experience> remainingExperiences;
    public List<bool> hasAchievedExperience;
    private SceneCatalogue mySceneCatalogue;
    private DifficultyLevel nextGoal;
    private bool isIrresponsible;
    private List<Experience> achievedExperiences;

    private void Awake()
    {
        remainingExperiences = new Dictionary<string, Experience>();
        achievedExperiences = new List<Experience>();
    }

    // Use this for initialization
    void Start () {
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();

        foreach (Experience exp in this.GetComponents<Experience>())
        {
            // TODO Experience Name is duplicate Data
            remainingExperiences.Add(exp.experienceName, exp);
        }

        hasAchievedExperience = new List<bool>();

        nextGoal = DifficultyLevel.EASY;

        isIrresponsible = true;

        initializeLocationExperienceChecklist();

	}
	
	// Update is called once per frame
	void Update () {
        if(hasAchievedSomeExperiences()){
            goalAchieved(nextGoal);
            this.nextGoal += 3;
        }
		
    }
    
	private void initializeLocationExperienceChecklist(){
        for (int i = 0; i < mySceneCatalogue.getLocationCount(); i ++){
			hasAchievedExperience.Add(false);
		}
	}

	public void achievedExperience(int currentSceneNumber){
		hasAchievedExperience[currentSceneNumber] = true;
	}

    public bool hasAchievedSomeExperiences(){
        return getNumberOfAchievedExperiences() >= (int) nextGoal;
	}

    public int getNumberOfAchievedExperiences(){
        int numberOfExperiences = 0;

        foreach (bool b in hasAchievedExperience)
        {
            if (b)
            {
                numberOfExperiences++;
            }
        }
        return numberOfExperiences;
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

    public Experience getNextExperience()
    {
        System.Random random = new System.Random();
        Experience toReturn;
        if (isIrresponsible)
        {
            toReturn = remainingExperiences["responsibility"];
            remainingExperiences.Remove("responsibility");
            isIrresponsible = false;
        }
        else if(IsReadyToCreate()){
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
        return toReturn;
    }

    private bool IsReadyToCreate()
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
}
