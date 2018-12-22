using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCoach : MonoBehaviour {

	public List<bool> hasAchievedExperience;
    private SceneCatalogue mySceneCatalogue;
    private DifficultyLevel nextGoal;

	// Use this for initialization
	void Start () {
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
		nextGoal = DifficultyLevel.EASY;
        hasAchievedExperience = new List<bool>();

        initializeExperienceList();

	}
	
	// Update is called once per frame
	void Update () {
        if(hasAchievedSomeExperiences()){
            goalAchieved(nextGoal);
            this.nextGoal += 3;
        }
		
    }

	private void initializeExperienceList(){
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
}
