using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCatalogue : MonoBehaviour {

    public string[] sceneNames;
	public string[] dateSceneNames; // CHANGE THIS TO INTERNIOR SCENE NEANMES
	private int mySceneNumber;
	private bool isInInteriorScene;
	public string[] neutralResultDescriptions;
	public string[] experienceDescriptions;
	public bool[] isDateScene;

   // Use this for initialization
	void Start () {
		mySceneNumber = 6;
		isInInteriorScene = true; // Start Player out in apartment
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void toggleInteriorScene(){
		isInInteriorScene = !isInInteriorScene;
	}

	public bool getIsInInteriorScene(){
		return isInInteriorScene;
	}

	public int getCurrentSceneNumberModulus(){
		return mySceneNumber;
	}
    
	public string getCurrentSceneName(){
		return sceneNames[mySceneNumber];
	}

	public void setCurrentSceneNumber(int newSceneNumber){
		mySceneNumber = newSceneNumber;
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

	public Dictionary<string,int> getDateScenes(){

		Dictionary<string, int> dateScenes = new Dictionary<string, int>();

		for (int i = 0; i < dateSceneNames.Length; i++){
			if(isDateScene[i]){
				dateScenes.Add(dateSceneNames[i], i);
			}
		}
		return dateScenes;
	}

}
