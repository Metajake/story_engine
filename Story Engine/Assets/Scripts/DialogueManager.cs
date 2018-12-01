﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public List<DateableCharacter> allDateableCharacters;
	public List<MinorCharacter> allMinorCharacters;
    public List<Character> allCharacters;
	public List<DialoguePiece> pieces;
	public List<Character> charactersPresent;
    private List<MinorCharacter> minorCharactersPresent;
    private List<Character> allCharactersPresent;
	public int selectedPartner;
	private bool conversationMode;

    private Timelord myTimeLord;
    private SceneCatalogue mySceneCatalogue;
    private RelationshipCounselor myRelationshipCounselor;
    //private CommandProcessor myCommandProcessor;
	private UIManager myUIManager;

	public void registerDialogue(DialoguePiece piece){
		this.pieces.Add(piece);
	}

	private void Awake()
	{
		this.selectedPartner = -1;
		this.pieces = new List<DialoguePiece>();
		this.charactersPresent = new List<Character>();
	}

	// Use this for initialization
	void Start ()
    {
        this.allDateableCharacters = new List<DateableCharacter>(this.GetComponents<DateableCharacter>());
        this.allMinorCharacters = new List<MinorCharacter>(this.GetComponents<MinorCharacter>());
        initializeAllCharacters();

        myTimeLord = GameObject.FindObjectOfType<Timelord>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
        myUIManager = GameObject.FindObjectOfType<UIManager>();

        findConversationPartners();
    }

    private void initializeAllCharacters()
    {
        this.allCharacters = new List<Character>();
        foreach (DateableCharacter dc in this.allDateableCharacters)
        {
            this.allCharacters.Add((Character)dc);
        }
        foreach(MinorCharacter mc in this.allMinorCharacters)
        {
            this.allCharacters.Add((Character)mc);
        }
    }

    void Update () {
	}
    
	public void updateCharacterUI()
	{
        charactersPresent = findConversationPartners();
		myUIManager.placePotentialPartners(charactersPresent);
	}
    
	public void updateSelectedPartnerUI(){
		bool partners = charactersPresent.Count > 0;
		myUIManager.partnersPresent(partners);

        if (partners && this.selectedPartner < 0)
        {
			myUIManager.onPortraitClicked(new System.Random().Next(1, charactersPresent.Count + 1));
        }
	}

	private List<Character> findConversationPartners(){

		List<Character> toReturn = new List<Character>();
        
		if(mySceneCatalogue.getIsInInteriorScene()){
            if(myRelationshipCounselor.isInDateMode()){
				toReturn.Add( 
    	            myRelationshipCounselor.datePartner(mySceneCatalogue.getCurrentLocation(), myTimeLord.getCurrentTimestep())
				);
				myRelationshipCounselor.isAtDate = true;
				return toReturn;
            }
        }

        foreach (Character character in this.allCharacters) {
			if(isCharacterInTimeOfDay(character) && isCharacterPresentAtCurrentLocation(character) && character.checkIsPresent()){
				toReturn.Add(character);
			}
		}
		return toReturn;
	}

	private bool isCharacterInTimeOfDay(Character character){
		return character.activeTimes[myTimeLord.getCurrentModulusTimestep()];
	}

	private bool isCharacterPresentAtCurrentLocation(Character character){
        if (character.currentSceneName.ToLower().Equals(mySceneCatalogue.getCurrentSceneName().ToLower()) && character.isInside.Equals(mySceneCatalogue.getIsInInteriorScene())){
			return true;
		}
		return false;
    }
    
	public Character getPartnerAt(int partnerNumber){
		return this.charactersPresent.Count >= partnerNumber ? this.charactersPresent[partnerNumber - 1] : null;
	}

	public bool getIsInConversationMode(){
		return this.conversationMode;	
	}

	public void setConversationMode(bool isOn){
		this.conversationMode = isOn;
	}

	public DialoguePiece getDialogueForTags(List<string> tags){
		List<string> desiredTags = new List<string>();
        DialoguePiece bestMatch = null;
        int nearnessOfBestMatch = Int32.MaxValue; 
		desiredTags.AddRange(tags);
        foreach (DialoguePiece piece in this.pieces)
        {
            if (piece.speaker.givenName == this.charactersPresent[selectedPartner].givenName)
            {
                if (piece.matchesExactly(desiredTags))
                {
                    return piece;
                }
                else if (piece.matchesPartially(desiredTags) > 0)
                {
                    bestMatch = piece.matchesPartially(desiredTags) < nearnessOfBestMatch ? piece : bestMatch;
                }
            }
        }
        return bestMatch;
	}

	public DateableCharacter getCharacterForName(string name){
		foreach (DateableCharacter chara in allDateableCharacters){
			if (chara.givenName.ToLower() == name.ToLower()){
				return chara;
			}
		}
		return null;
	}

    public void scatterCharacters(){
		System.Random random = new System.Random();
        foreach(DateableCharacter chara in allDateableCharacters){
            chara.currentSceneName = mySceneCatalogue.getLocationNames()[random.Next(12)];
            chara.isInside = random.Next(2) == 0 ? false : true;

        }
    }
}
