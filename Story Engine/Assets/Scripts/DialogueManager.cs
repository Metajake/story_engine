using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public List<DateableCharacter> allCharacters;
	public List<MinorCharacter> allMinorCharacters;
	public List<DialoguePiece> pieces;
	public List<DateableCharacter> potentialPartners;
    private List<MinorCharacter> minorPartners;
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
		this.potentialPartners = new List<DateableCharacter>();
	}

	// Use this for initialization
	void Start () {
        this.allCharacters = new List<DateableCharacter>(this.GetComponents<DateableCharacter>());
		this.allMinorCharacters = new List<MinorCharacter>(this.GetComponents<MinorCharacter>());
		myTimeLord = GameObject.FindObjectOfType<Timelord>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
        //myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();
		myUIManager = GameObject.FindObjectOfType<UIManager>();

		findConversationPartners();
    }
    
    // Update is called once per frame
    void Update () {
	}
    
	public void updateCharacterUI()
	{
        potentialPartners = findConversationPartners();
		minorPartners = findMinorConversationPartners();
		myUIManager.placePotentialPartners(potentialPartners, minorPartners);
	}
    
	public void updateSelectedPartnerUI(){
		bool partners = potentialPartners.Count > 0;
		myUIManager.partnersPresent(partners);

        if (partners && this.selectedPartner < 0)
        {
			myUIManager.onPortraitClicked(new System.Random().Next(1, potentialPartners.Count + 1));
        }
	}

	private List<DateableCharacter> findConversationPartners(){

		List<DateableCharacter> toReturn = new List<DateableCharacter>();
        
		if(mySceneCatalogue.getIsInInteriorScene()){
            if(myRelationshipCounselor.isInDateMode()){
				toReturn.Add( 
    	            myRelationshipCounselor.datePartner(mySceneCatalogue.getCurrentSceneNumber(), myTimeLord.getCurrentTimestep())
				);
				myRelationshipCounselor.isAtDate = true;
				return toReturn;
            }
        }

		foreach(DateableCharacter character in this.allCharacters){
			if(isCharacterInTimeOfDay(character) && isCharacterPresentAtCurrentLocation(character)){
				toReturn.Add(character);
			}
		}
		return toReturn;
	}

    private List<MinorCharacter> findMinorConversationPartners()
    {

        List<MinorCharacter> toReturn = new List<MinorCharacter>();

        foreach (MinorCharacter character in this.allMinorCharacters){
            if(isCharacterInTimeOfDay(character) && isCharacterPresentAtCurrentLocation(character)){
                toReturn.Add(character);
            }
        }
        return toReturn;
    }

	private bool isCharacterInTimeOfDay(DateableCharacter character){
		return character.activeTimes[myTimeLord.getCurrentModulusTimestep()];
	}
    private bool isCharacterInTimeOfDay(MinorCharacter character)
    {
        return character.activeTimes[myTimeLord.getCurrentModulusTimestep()];
    }

	private bool isCharacterPresentAtCurrentLocation(DateableCharacter character){
        if (character.currentSceneName.ToLower().Equals(mySceneCatalogue.getCurrentSceneName().ToLower()) && character.isInside.Equals(mySceneCatalogue.getIsInInteriorScene())){
			return true;
		}
		return false;
    }
    private bool isCharacterPresentAtCurrentLocation(MinorCharacter character)
    {
        if (character.currentSceneName.ToLower().Equals(mySceneCatalogue.getCurrentSceneName().ToLower()) && character.isInside.Equals(mySceneCatalogue.getIsInInteriorScene()))
        {
            return true;
        }
        return false;
    }

	public DateableCharacter getPartnerAt(int partnerNumber){
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

	public DateableCharacter getCharacterForName(string name){
		foreach (DateableCharacter chara in allCharacters){
			if (chara.givenName.ToLower() == name.ToLower()){
				return chara;
			}
		}
		return null;
	}

    public void ninjaVanish(){
		System.Random random = new System.Random();
        foreach(DateableCharacter chara in allCharacters){
            chara.currentSceneName = mySceneCatalogue.sceneNames[random.Next(12)];
            chara.isInside = random.Next(2) == 0 ? false : true;

        }
    }
}
