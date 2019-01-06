using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCoach : MonoBehaviour {

    public Dictionary<string, Experience> experiences;
    public List<bool> hasAchievedExperience;
    private SceneCatalogue mySceneCatalogue;
    private DifficultyLevel nextGoal;
    private bool isIrresponsible;

    private void Awake()
    {
        experiences = new Dictionary<string, Experience>();
    }

    // Use this for initialization
    void Start () {
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();

        foreach (Experience exp in this.GetComponents<Experience>())
        {
            experiences.Add(exp.experienceName, exp);
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
            toReturn = experiences["protect"];
            experiences.Remove("protect");
            isIrresponsible = false;
            return toReturn;
        }
        else
        {
            List<Experience> expList = new List<Experience>(experiences.Values);
            Experience toRemoveAndReturn = expList[random.Next(expList.Count)];
            experiences.Remove(toRemoveAndReturn.experienceName);
            return toRemoveAndReturn;
        }
    }
}
