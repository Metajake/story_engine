using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCatalogue : MonoBehaviour {

    public string[] sceneNames;
	public string[] dateSceneNames;
	private int mySceneNumber;
	private bool isInDateScene;
	public string[] neutralResultDescriptions;
	public string[] experienceDescriptions;

   // Use this for initialization
	void Start () {
		mySceneNumber = 0;
		isInDateScene = true; // Start Player out in apartment
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void toggleDateScene(){
		isInDateScene = !isInDateScene;
	}

	public bool getIsInDateScene(){
		return isInDateScene;
	}

	public int getCurrentSceneNumberModulus(){
		return mySceneNumber;
	}
    
	public string getCurrentSceneName(){
		return sceneNames[mySceneNumber];
	}

	public void goToPreviousScene()
    {
        GameObject.FindObjectOfType<DialogueManager>().selectedPartner = -1;
        mySceneNumber--;
		if(mySceneNumber < 0){
			mySceneNumber = sceneNames.Length -1;
		}
    }

	public void goToNextScene(){
		GameObject.FindObjectOfType<DialogueManager>().selectedPartner = -1;
        mySceneNumber++;

		if (mySceneNumber == sceneNames.Length){
			mySceneNumber = 0;
		}
	}



	public string neutralResultForCurrentLocationDescription(){
		return neutralResultDescriptions[getCurrentSceneNumberModulus()];
	}

	public string currentExperienceDescription()
    {
        return experienceDescriptions[getCurrentSceneNumberModulus()];
    }

}
