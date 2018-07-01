using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public List<Character> allCharacters;
	public List<DialoguePiece> pieces;
	public List<Character> potentialPartners;
	public int selectedPartner;
	private bool conversationMode;

    private Timelord myTimeLord;
    private SceneCatalogue mySceneCatalogue;
    private RelationshipCounselor myRelationshipCounselor;
    private CommandProcessor myCommandProcessor;
	private UIManager myUIManager;

	public void registerDialogue(DialoguePiece piece){
		this.pieces.Add(piece);
	}

	private void Awake()
	{
		this.selectedPartner = -1;
		this.pieces = new List<DialoguePiece>();
		this.potentialPartners = new List<Character>();
	}

	// Use this for initialization
	void Start () {
		this.allCharacters = new List<Character>(this.GetComponents<Character>());
		myTimeLord = GameObject.FindObjectOfType<Timelord>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
        myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();
		myUIManager = GameObject.FindObjectOfType<UIManager>();

		findConversationPartners();
    }
    
    // Update is called once per frame
    void Update () {
	}
    
	public void updateCharacterUI()
	{
		potentialPartners = findConversationPartners();
		myUIManager.placePotentialPartners(potentialPartners);
	}
    
	public void updateSelectedPartnerUI(){
		bool partners = potentialPartners.Count > 0;
		myUIManager.partnersPresent(partners);

        if (partners && this.selectedPartner < 0)
        {
			myUIManager.onPortraitClicked(new System.Random().Next(1, potentialPartners.Count + 1));
        }
	}

	private List<Character> findConversationPartners(){

		List<Character> toReturn = new List<Character>();
        
		if(mySceneCatalogue.getIsInDateScene()){
            if(myRelationshipCounselor.isInDateMode()){
				toReturn.Add( 
    	            myRelationshipCounselor.datePartner(mySceneCatalogue.getCurrentSceneNumberModulus(), myTimeLord.getCurrentTimestep())
				);
				myRelationshipCounselor.isAtDate = true;
            }
			return toReturn;
        }

		foreach(Character character in this.allCharacters){
			if(isCharacterInTimeOfDay(character) && isCharacterPresentAtCurrentLocation(character)){
				toReturn.Add(character);
			}
		}
		return toReturn;
	}

	private bool isCharacterInTimeOfDay(Character character){
		return character.activeTimes[myTimeLord.getCurrentModulusTimestep()];
	}

	private bool isCharacterPresentAtCurrentLocation(Character character){
		if(character.currentSceneName.ToLower().Equals(mySceneCatalogue.getCurrentSceneName().ToLower())){
			return true;
		}
		return false;
    }

	public Character getPartnerAt(int partnerNumber){
		return this.potentialPartners.Count >= partnerNumber ? this.potentialPartners[partnerNumber - 1] : null;
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
            if (piece.speaker.givenName == this.potentialPartners[selectedPartner].givenName)
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

	public Character getCharacterForName(string name){
		foreach (Character chara in allCharacters){
			if (chara.givenName.ToLower() == name.ToLower()){
				return chara;
			}
		}
		return null;
	}

}
