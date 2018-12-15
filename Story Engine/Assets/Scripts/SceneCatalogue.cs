﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCatalogue : MonoBehaviour, IKnownLocationsChangedObservable {

    public List<Location> locations;

	private int mySceneNumber;
	private bool isInInteriorScene;
    private EventQueue myEventQueue;

    private List<IKnownLocationsChangedObserver> currentObservers;

    void Awake() {
        currentObservers = new List<IKnownLocationsChangedObserver>();
		locations = new List<Location>(this.gameObject.GetComponents<Location>());
    }

    void Start () {
        mySceneNumber = 6;
        isInInteriorScene = true; // Start Player out in apartment

        myEventQueue = GameObject.FindObjectOfType<EventQueue>();
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
        myEventQueue.queueEvent(new SceneChangeEvent());
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
        //CHANGE LOCATION EVENT
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


    public Location revealRandomUnknownLocation()
    {
        List<Location> unknownLocations = new List<Location>();

        foreach(Location local in this.locations)
        {
            if (!local.isKnown)
            {
                unknownLocations.Add(local);
            }
        }
        int randomUnknownLocationIndex = new System.Random().Next(0, unknownLocations.Count);

        return learnLocation(unknownLocations[randomUnknownLocationIndex]);
    }

    public Location learnLocation(Location location)
    {
        location.isKnown = true;
        Notify();
        return location;
    }

    public Location learnLocation(int locationToLearn){
		this.locations[locationToLearn].isKnown = true;
        Notify();
        return locations[locationToLearn];
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

    public List<string> getKnownLocationNames()
    {
        List<string> results = new List<string>();
        foreach (Location local in this.locations)
        {
            if (local.isKnown)
            {
                results.Add(local.locationName);
            }
        }
        return results;
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
