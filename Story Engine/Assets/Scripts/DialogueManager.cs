using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour, IEventSubscriber {

    public List<DateableCharacter> allDateableCharacters;
	public List<MinorCharacter> allMinorCharacters;
    public List<Character> allCharacters;
	public List<DialoguePiece> pieces;
	public List<Character> charactersPresent;
	public int selectedPartner;
	private bool conversationMode;

    private Timelord myTimeLord;
    private SceneCatalogue mySceneCatalogue;
    private RelationshipCounselor myRelationshipCounselor;
    private GameState myGameState;
    private CommandBuilder myCommandBuilder;
    private TipManager myTipManager;
    private EventQueue myEventQueue;

    public void registerDialogue(DialoguePiece piece){
		this.pieces.Add(piece);
	}

	private void Awake()
	{
		this.selectedPartner = -1;
		this.pieces = new List<DialoguePiece>();
		this.charactersPresent = new List<Character>();
	}

	void Start ()
    {
        myTimeLord = GameObject.FindObjectOfType<Timelord>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
        myGameState = GameObject.FindObjectOfType<GameState>();
        myCommandBuilder = GameObject.FindObjectOfType<CommandBuilder>();
        myTipManager = GameObject.FindObjectOfType<TipManager>();
        myEventQueue = GameObject.FindObjectOfType<EventQueue>();

        this.allDateableCharacters = new List<DateableCharacter>(this.GetComponents<DateableCharacter>());
        this.allMinorCharacters = new List<MinorCharacter>(this.GetComponents<MinorCharacter>());

        myEventQueue.subscribe(this);
        initializeAllCharacters();
        this.disburseCharacters(charactersToInclude: allCharacters, characterNamesToExclude: new List<string> { "kristie", "evan" });
    }

    void IEventSubscriber.eventOccurred(IGameEvent occurringEvent)
    {
        if (occurringEvent.getEventType() == "TIMEEVENT")
        {
            this.setAbsentCharactersToPresent();
        }
        selectedPartner = -1;
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

    public List<Character> getAllCurrentLocalPresentConversationPartners(){
		List<Character> toReturn = new List<Character>();
        int currentTimeOfDay = myTimeLord.getCurrentModulusTimestep();

        foreach (Character character in this.allCharacters)
        {
            if (this.isCharacterInTimeOfDay(character, currentTimeOfDay) && this.isCharacterPresentAtCurrentLocation(character, currentTimeOfDay) && character.isPresent)
            {
                toReturn.Add(character);
            }
        }
        charactersPresent = toReturn;
		return toReturn;
	}

	private bool isCharacterInTimeOfDay(Character character, int timeOfDayToCheck){
		return character.locations[timeOfDayToCheck].isActive && !myRelationshipCounselor.hasDateInFuture(character);
	}

	private bool isCharacterPresentAtCurrentLocation(Character character, int timeOfDayToCheck){
        if (character.locations[timeOfDayToCheck].locationName.ToLower().Equals(mySceneCatalogue.getCurrentSceneName().ToLower()) && character.locations[timeOfDayToCheck].isInside.Equals(mySceneCatalogue.getIsInInteriorScene())){
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

	public Character getCharacterForName(string name){
		foreach (Character chara in allCharacters){
			if (chara.givenName.ToLower() == name.ToLower()){
				return chara;
			}
		}
		return null;
	}

    public void disburseCharacters(List<Character> charactersToInclude = default(List<Character>), List<string> characterNamesToExclude = default(List<string>)){
		System.Random random = new System.Random();
        List<Location> knownLocations = mySceneCatalogue.getKnownLocations();

        foreach (Character chara in charactersToInclude){

            if ( characterNamesToExclude != null && characterNamesToExclude.Contains(chara.givenName.ToLower() )) { continue; }

            for ( int i = 0; i < 3; i++)
            {
                string destination;
                bool indoorDestination;
                bool toBeActive;

                while (true)
                {
                    destination = knownLocations[random.Next(knownLocations.Count)].locationName;
                    indoorDestination = random.Next(2) == 0 ? false : true;
                    toBeActive = random.Next(2) == 0 ? false : true;

                    if ((destination == "Residential District" && indoorDestination == true))
                    {
                        continue;
                    }
                    else
                    {
                        chara.locations[i].locationName = destination;
                        chara.locations[i].isInside = indoorDestination;
                        chara.locations[i].isActive = toBeActive;
                        break;
                    }
                }
            }
        }
    }

    public void checkCharacterRelocate()
    {
        List<Character> charsToRelocate = new List<Character>() { };
        foreach(Character ch in allCharacters){
            if (ch.relocationInterval != 0 && myTimeLord.timeStep % ch.relocationInterval == 0)
            {
                charsToRelocate.Add(ch);
            }
        }
        disburseCharacters(charactersToInclude: charsToRelocate);
    }

    public void endDialogue(bool isDialoguing)
    {
        this.getPartnerAt(this.selectedPartner + 1).isPresent = false;
        this.getPartnerAt(this.selectedPartner + 1).returnTime = myTimeLord.getCurrentTimestep() + 1;
        myGameState.currentGameState = GameState.gameStates.PROWL;
        this.setConversationMode(isDialoguing);
        this.selectedPartner = -1;
    }

    public void beginDialogue(bool isDialoguing)
    {
        Character selectedCharacter = this.getPartnerAt(this.selectedPartner + 1);
        if (selectedCharacter is DateableCharacter)
        {
            this.setConversationMode(isDialoguing);
            myGameState.currentGameState = GameState.gameStates.CONVERSATION;
            GameObject.FindObjectOfType<ConversationTracker>().beginConversation((DateableCharacter)selectedCharacter);
        }
        else
        {
            myCommandBuilder.createAndEnqueueChangeDialogueSequence(new List<string>() { myTipManager.getTip() });
            myCommandBuilder.build();
        }
    }

    private void setAbsentCharactersToPresent()
    {
        foreach (DateableCharacter character in allDateableCharacters)
        {
            character.checkAndSetReturnToPresent(myTimeLord.getCurrentTimestep());
        }
    }
}
