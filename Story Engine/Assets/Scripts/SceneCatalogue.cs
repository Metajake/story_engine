using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCatalogue : MonoBehaviour, IKnownLocationsChangedObservable {

    public List<Location> locations;

	private int mySceneNumber;
	private bool isInInteriorScene;

    private List<IKnownLocationsChangedObserver> currentObservers;

    void Awake() {
        currentObservers = new List<IKnownLocationsChangedObserver>();
		locations = new List<Location>(this.gameObject.GetComponents<Location>());
    }

    void Start () {
        mySceneNumber = 6;
        isInInteriorScene = true; // Start Player out in apartment
	}
	
	void Update () {
		
	}

    public int getLocationCount(){
        return this.locations.Count;
    }

    public List<string> getLocationNames(){
        List<string> result = new List<string>();
        foreach(Location local in this.locations){
            result.Add(local.locationName);
        }
        return result;
    }

	public void toggleInteriorScene(){
		isInInteriorScene = !isInInteriorScene;
	}

	public bool getIsInInteriorScene(){
		return isInInteriorScene;
	}

    public Location getCurrentLocation(){
        return this.locations[mySceneNumber];
    }

	public int getCurrentSceneNumber(){
		return mySceneNumber;
	}
    
	public string getCurrentSceneName(){
        return locations[mySceneNumber].locationName;
	}

	public void setCurrentSceneNumber(int newSceneNumber){
		mySceneNumber = newSceneNumber;
	}

	public void goToPreviousScene()
    {
        GameObject.FindObjectOfType<DialogueManager>().selectedPartner = -1;
        mySceneNumber--;
		if(mySceneNumber < 0){
			mySceneNumber = locations.Count -1;
		}
    }

	public void goToNextScene(){
		GameObject.FindObjectOfType<DialogueManager>().selectedPartner = -1;
        mySceneNumber++;

		if (mySceneNumber == locations.Count){
			mySceneNumber = 0;
		}
	}

	public string neutralResultForCurrentLocationDescription(){
        return getCurrentLocation().neutralDateResultDescription;
	}

	public string currentExperienceDescription()
    {
        return getCurrentLocation().experienceDescription;
    }

	public List<Location> getDateScenes(){

        List<Location> dateScenes = new List<Location>();

		foreach (Location local in this.locations){
			if(local.isDateScene){
				dateScenes.Add(local);
			}
		}
		return dateScenes;
	}

    public List<string> getDateSceneNames(){
        List<string> dateLocationNames = new List<string>();
        foreach(Location local in this.locations){
            if(local.isDateScene){
                dateLocationNames.Add(local.interiorName);
            }
        }
        return dateLocationNames;
    }

	public void learnLocation(int locationToLearn){
		this.locations[locationToLearn].isKnown = true;
        Notify();
	}

	internal bool someLocationsObscured()
	{
		for (int i = 0; i < this.locations.Count; i++){
            if(!this.locations[i].isKnown){
				return true;
			}
		}
        return false;
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

    public List<Location> getKnownLocations(){
        List<Location> knownLocations = new List<Location>();
        foreach(Location local in this.locations){
            if(local.isKnown){
                knownLocations.Add(local);
            }
        }
        return knownLocations;
    }

    public List<string> getKnownDateSceneNames(){
        List<string> knownDateSceneNames = new List<string>();
        foreach (Location local in this.locations){
            if (local.isDateScene && local.isKnown){
                knownDateSceneNames.Add(local.interiorName);
            }
        }
        return knownDateSceneNames;
    }

    public Boolean isKnownDateLocation(string locationName){
        foreach(string sName in this.getKnownDateSceneNames()){
            if(sName == locationName){
                return true;
            }
        }
        return false;
    }
}
