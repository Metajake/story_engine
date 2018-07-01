using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCoach : MonoBehaviour {

	public List<bool> hasAchievedExperience;
    private SceneCatalogue mySceneCatalogue;

	// Use this for initialization
	void Start () {
		mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
		hasAchievedExperience = new List<bool>();
		initializeExperienceList();
	}
	
	// Update is called once per frame
	void Update () {
		
    }

	private void initializeExperienceList(){
		for (int i = 0; i < mySceneCatalogue.sceneNames.Length; i ++){
			hasAchievedExperience.Add(false);
		}
	}

	public void achievedExperience(int currentSceneNumber){
		hasAchievedExperience[currentSceneNumber] = true;
	}

	public bool hasAchievedAllExperiences(){
		bool areAllTrue = true;

		foreach(bool b in hasAchievedExperience){
			if (b == false){
				areAllTrue = false;
			}
		}
		return areAllTrue;
	}
}
