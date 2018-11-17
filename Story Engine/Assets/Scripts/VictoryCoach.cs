using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCoach : MonoBehaviour {

	public List<bool> hasAchievedExperience;
    private SceneCatalogue mySceneCatalogue;
    private DifficultyLevel nextGoal;
    private UIManager myUIManager;

	// Use this for initialization
	void Start () {
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myUIManager = GameObject.FindObjectOfType<UIManager>();
		nextGoal = DifficultyLevel.EASY;
        hasAchievedExperience = new List<bool>();

        initializeExperienceList();

	}
	
	// Update is called once per frame
	void Update () {
        if(hasAchievedSomeExperiences()){
            myUIManager.goalAchieved(nextGoal);
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
}
