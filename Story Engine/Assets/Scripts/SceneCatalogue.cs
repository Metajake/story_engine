using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCatalogue : MonoBehaviour, IKnownLocationsChangedObservable {

    public string[] sceneNames;
	public string[] dateSceneNames; // CHANGE THIS TO INTERNIOR SCENE NEANMES
	private int mySceneNumber;
	private bool isInInteriorScene;
	public string[] neutralResultDescriptions;
	public string[] experienceDescriptions;
    public bool[] isDateScene;
    public bool[] knownLocations;

    private List<IKnownLocationsChangedObserver> currentObservers;

    void Awake() {
        currentObservers = new List<IKnownLocationsChangedObserver>();
    }

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

	public int getCurrentSceneNumber(){
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
		return neutralResultDescriptions[getCurrentSceneNumber()];
	}

	public string currentExperienceDescription()
    {
        return experienceDescriptions[getCurrentSceneNumber()];
    }

	public Dictionary<string,int> getDateScenes(){

		Dictionary<string, int> dateScenes = new Dictionary<string, int>();

		for (int i = 0; i < dateSceneNames.Length; i++){
			if(isDateScene[i] && knownLocations[i]){
				dateScenes.Add(dateSceneNames[i], i);
			}
		}
		return dateScenes;
	}

	public void learnLocation(int locationToLearn){
		this.knownLocations[locationToLearn] = true;
        Notify();
	}

	internal bool someLocationsObscured()
	{
		bool toReturn = false;
		for (int i = 0; i < this.knownLocations.Length; i++){
			if(!this.knownLocations[i]){
				toReturn = true;
			}
		}
		return toReturn;
	}

    public void Subscribe(IKnownLocationsChangedObserver observer)
    {
        this.currentObservers.Add(observer);
    }

    public void Notify()
    {
        foreach (IKnownLocationsChangedObserver observer in currentObservers){
            observer.BeNotifiedOfLocationChange();
        }
    }

    public void Unsubscribe(IKnownLocationsChangedObserver observer)
    {
        throw new NotImplementedException();
    }
}
