using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public List<DateableCharacter> allDateableCharacters;
	public List<MinorCharacter> allMinorCharacters;
    public List<Character> allCharacters;
    public List<Character> experienceActors;

    public List<DialoguePiece> pieces;
	public List<Character> charactersPresent;
    private List<Character> allCharactersPresent;
	public int selectedPartner;
	private bool conversationMode;

    private Timecop myTimeLord;
    private SceneCatalogue mySceneCatalogue;
    private RelationshipCounselor myRelationshipCounselor;

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
        this.experienceActors = new List<Character>();

        initializeAllCharacters();

        myTimeLord = GameObject.FindObjectOfType<Timecop>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
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

    public List<Character> getAllCurrentLocalPresentConversationPartners(){
		List<Character> toReturn = new List<Character>();
        foreach (Character character in this.allCharacters)
        {
            if (isCharacterInTimeOfDay(character) && isCharacterPresentAtCurrentLocation(character) && character.checkIsPresent())
            {
                toReturn.Add(character);
            }
        }
        toReturn.AddRange(this.experienceActors);
        this.charactersPresent = toReturn;
		return toReturn;
	}

	private bool isCharacterInTimeOfDay(Character character){
		return character.activeTimes[myTimeLord.getCurrentModulusTimestep()] && !myRelationshipCounselor.hasDateInFuture(character);
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

	public DateableCharacter getDateableCharacterForName(string name){
		foreach (DateableCharacter chara in allDateableCharacters){
			if (chara.givenName.ToLower() == name.ToLower()){
				return chara;
			}
		}
		return null;
	}

    public Character getCharacterForName(string name)
    {
        foreach (Character chara in allCharacters)
        {
            if (chara.givenName.ToLower() == name.ToLower())
            {
                return chara;
            }
        }
        return null;
    }

    public void scatterCharacters(){
		System.Random random = new System.Random();
        List<Location> knownLocations = mySceneCatalogue.getKnownLocations();
        foreach (DateableCharacter chara in allDateableCharacters){
            string destination = knownLocations[random.Next(knownLocations.Count)].locationName;
            bool indoorDestination = random.Next(2) == 0 ? false : true;
            if (! (destination == "Residential District" && indoorDestination == true)){
                chara.currentSceneName = destination;
                chara.isInside = indoorDestination;
            }
        }
    }
}
