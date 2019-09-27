using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IEventSubscriber {

    private GameState myGameState;
    private DialogueManager myDialogueManager;
    private ConversationTracker conversationTracker;
    private SceneCatalogue mySceneCatalogue;
    private TipManager myTipManager;
    private RelationshipCounselor myRelationshipCounselor;
	private CommandProcessor myCommandProcessor;
    private Timelord myTimelord;
    private EventQueue myEventQueue;
    private AnimationMaestro myAnimationMaestro;
    private InputOrganizer myInputOrganizer;
    private VictoryCoach myVictoryCoach;

    public GameObject dialoguePanel;
    public GameObject mainPanel;
    public GameObject mapPanel;
    public GameObject journalPanel;
    public GameObject menuPanel;
    private GameObject dialogueButtonPanel;
    private GameObject dialogueOptionsButtonPanel;
    private GameObject dateLocationButtonPanel;
    private GameObject mainPanelButtonsPanel;
    private GameObject dateButtonsPanel;
    private GameObject sequenceButtonsPanel;
    private GameObject dateLocationButton;
    private GameObject cutScenePanel;
    private GameObject characterPanel;
    private GameObject startScreenPanel;

    private Text textPanel;
    public Text pastDatesText;
    public Text upcomingDatesText;
    public Text experiencesText;
    private String cutSceneTextToWrite;

    GameObject talkButtonObject;
    GameObject dateActionButton;
    Button departConversationButton;
    GameObject askOnDateButton;
    GameObject mapButton;

    public bool mapEnabled;
    public bool journalEnabled;

    private List<Character> previouslyPresentCharacters;
    
    void Awake()
    {
        myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();
        myTipManager = GameObject.FindObjectOfType<TipManager>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myGameState = GameObject.FindObjectOfType<GameState>();
    }
    
    void Start ()
    {
        previouslyPresentCharacters = new List<Character>();
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        myTimelord = GameObject.FindObjectOfType<Timelord>();
        conversationTracker = GameObject.FindObjectOfType<ConversationTracker>();
		myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
        myEventQueue = GameObject.FindObjectOfType<EventQueue>();
        myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
        myInputOrganizer = GameObject.FindObjectOfType<InputOrganizer>();


        dialogueButtonPanel = GameObject.Find("DialogueButtonPanel");
        dialogueOptionsButtonPanel = GameObject.Find("DialogueOptionsButtonPanel");
        dateLocationButtonPanel = GameObject.Find("LocationButtonPanel");
        mainPanelButtonsPanel = GameObject.Find("MainPanelButtonsPanel");
        dateButtonsPanel = GameObject.Find("DateButtonsPanel");
		sequenceButtonsPanel = GameObject.Find("SequenceButtonsPanel");
        dateLocationButton = GameObject.Find("DateLocationButton");
        menuPanel = GameObject.Find("MenuPanel");
        cutScenePanel = GameObject.Find("CutScenePanel");
        characterPanel = GameObject.Find("CharacterPanel");
        startScreenPanel = GameObject.Find("StartScreenPanel");

        textPanel = GameObject.Find("TextPanel").GetComponentInChildren<Text>();
        pastDatesText = GameObject.Find("PastDates").GetComponentInChildren<Text>();
        upcomingDatesText = GameObject.Find("UpcomingDates").GetComponentInChildren<Text>();
        experiencesText = GameObject.Find("ExperiencesList").GetComponentInChildren<Text>();

        talkButtonObject = GameObject.Find("TalkButton");
        dateActionButton = GameObject.Find("DateActionButton");
        departConversationButton = GameObject.Find("Depart").GetComponent<Button>();
        askOnDateButton = GameObject.Find("AskOut");
        mapButton = GameObject.Find("MapButton");

        myEventQueue.subscribe(this);

        dateLocationButtonPanel.SetActive(false);
		myAnimationMaestro.clearPotentialPartners();

		mapEnabled = false;
        journalEnabled = false;
        menuPanel.gameObject.SetActive(false);
    }
	
	void Update ()
	{
        enableComponentsForState(myGameState.currentGameState);
        populateComponentsForState(myGameState.currentGameState);
        myInputOrganizer.BTN_toggleMenuPanel();
    }

    private void deactivateUIComponents()
    {
        dialoguePanel.SetActive(false);
        journalPanel.SetActive(false);
        mapPanel.SetActive(false);
        mainPanel.SetActive(false);
        cutScenePanel.SetActive(false);
        characterPanel.gameObject.SetActive(false);

        dateButtonsPanel.SetActive(false);
        mainPanelButtonsPanel.SetActive(false);
        sequenceButtonsPanel.SetActive(false);
    }

    private void enableComponentsForState(GameState.gameStates currentState)
    {
        deactivateUIComponents();

        if(currentState == GameState.gameStates.COMMANDSEQUENCE)
        {
            mainPanel.SetActive(true);
            sequenceButtonsPanel.SetActive(true);
            characterPanel.gameObject.SetActive(true);
        }
        else if(currentState == GameState.gameStates.CUTSCENE)
        {
            cutScenePanel.SetActive(true);
        }
        else if(currentState == GameState.gameStates.PROWL)
        {
            mainPanel.SetActive(true);
            mainPanelButtonsPanel.SetActive(true);
            mapButton.SetActive(!mySceneCatalogue.getIsInInteriorScene());
            characterPanel.gameObject.SetActive(true);
            if (this.mapEnabled)
            {
                mainPanel.SetActive(false);
                mapPanel.SetActive(true);
            }else if (this.journalEnabled)
            {
                mainPanel.SetActive(false);
                journalPanel.SetActive(true);
            }
        }else if (currentState == GameState.gameStates.CONVERSATION)
        {
            dialoguePanel.SetActive(true);
            characterPanel.gameObject.SetActive(true);
            askOnDateButton.SetActive(conversationTracker.canAskOnDateEnabled());
        }else if(currentState == GameState.gameStates.DATEINTRO)
        {
            enableDateComponents();
        }
        else if (currentState == GameState.gameStates.DATE)
        {
            enableDateComponents();
        }
        else if (currentState == GameState.gameStates.DATEOUTRO)
        {
            enableDateComponents();
            characterPanel.gameObject.SetActive(false);
        }
    }

    private void populateComponentsForState(GameState.gameStates currentState)
    {
        if (currentState == GameState.gameStates.COMMANDSEQUENCE)
        {
            
        }else if(currentState == GameState.gameStates.CUTSCENE)
        {
            cutScenePanel.GetComponentInChildren<Text>().text = cutSceneTextToWrite;
        }
        else if (currentState == GameState.gameStates.PROWL)
        {
            if (!mapEnabled && !journalEnabled)
            {
                this.updateLocationDescription();
                myAnimationMaestro.updatePotentialPartnersSprites( myDialogueManager.getAllCurrentLocalPresentConversationPartners() );
                updateSelectedPartnerUI();
            }
        }
        else if (currentState == GameState.gameStates.CONVERSATION)
        {
            
        }else if (currentState == GameState.gameStates.DATEINTRO)
        {
            myAnimationMaestro.updatePotentialPartnersSprites(new List<Character>() {
                myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimelord.getCurrentTimestep())
            });
            dateActionButton.GetComponentInChildren<Text>().text = mySceneCatalogue.getCurrentLocation().currentDateAction;
            myAnimationMaestro.writeDescriptionText(mySceneCatalogue.getCurrentLocation().descriptionDate, textPanel);
        }
        else if(currentState == GameState.gameStates.DATE)
        {
            myAnimationMaestro.updatePotentialPartnersSprites(new List<Character>() {
                myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimelord.getCurrentTimestep())
            });
            dateActionButton.GetComponentInChildren<Text>().text = mySceneCatalogue.getCurrentLocation().currentDateAction;
        }
        else if (currentState == GameState.gameStates.DATEOUTRO)
        {
            dateActionButton.GetComponentInChildren<Text>().text = mySceneCatalogue.getCurrentLocation().currentDateAction;
            myAnimationMaestro.writeDescriptionText("One good date can change your life.", textPanel);
        }
    }

    public void eventOccured(IGameEvent occurringEvent)
    {
        Debug.Log(occurringEvent.getEventType());
        if (occurringEvent.getEventType() == "TIMEEVENT")
        {
            myAnimationMaestro.fadeInCharacters(myDialogueManager.getAllCurrentLocalPresentConversationPartners().Except(this.previouslyPresentCharacters).ToList());
            foreach (DateableCharacter character in myDialogueManager.allDateableCharacters)
            {
                character.checkAndSetReturnToPresent(myTimelord.getCurrentTimestep());
            }

            this.previouslyPresentCharacters = myDialogueManager.getAllCurrentLocalPresentConversationPartners();
        }
        else if (occurringEvent.getEventType() == "LOCATIONEVENT")
        {
            myAnimationMaestro.fadeInCharacters(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
            myDialogueManager.selectedPartner = -1;
            if (!mySceneCatalogue.getIsInInteriorScene())
            {
                dateLocationButton.GetComponentInChildren<Text>().text = "Enter " + mySceneCatalogue.getCurrentLocation().interiorName;
            }
            else
            {
                dateLocationButton.GetComponentInChildren<Text>().text = "Exit " + mySceneCatalogue.getCurrentLocation().interiorName;
            }
        }
        else if (occurringEvent.getEventType() == "DATESTARTEVENT")
        {
            myAnimationMaestro.fadeInCharacters(new List<Character>() {
                myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimelord.getCurrentTimestep())
            });
            myAnimationMaestro.fadeInCharacters(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
        }
        else if (occurringEvent.getEventType() == "DATEACTIONEVENT")
        {
            mySceneCatalogue.getCurrentLocation().setRandomDateAction();
        }
    }

    private void enableDateComponents()
    {
        mainPanel.SetActive(true);
        characterPanel.gameObject.SetActive(true);
        dateButtonsPanel.SetActive(myRelationshipCounselor.isAtDate);
        dateActionButton.SetActive(!myRelationshipCounselor.getCurrentDateFromScheduledDateList().experienceAchieved);
    }

    private void updateSelectedPartnerUI()
    {
        bool partners = myDialogueManager.charactersPresent.Count > 0;

        talkButtonObject.SetActive(partners);

        if (partners && myDialogueManager.selectedPartner < 0)
        {
            onPortraitClicked(new System.Random().Next(1, myDialogueManager.charactersPresent.Count + 1));
        }
    }

    public void onPortraitClicked(int portraitNumber)
    {
        Character clickedCharacter = myDialogueManager.getPartnerAt(portraitNumber);
        if (clickedCharacter != null)
        {
            myDialogueManager.selectedPartner = portraitNumber - 1;
            talkButtonObject.GetComponentInChildren<Text>().text = "Talk to " + clickedCharacter.givenName;
        }
    }

    private void updateLocationDescription()
    {
        myAnimationMaestro.writeDescriptionText(mySceneCatalogue.getLocationDescription(), textPanel);
    }

    internal void gameOver()
	{
        SceneManager.LoadScene("splash_game_over");
	}

    internal void gameClear()
    {
        SceneManager.LoadScene("splash_game_clear");
    }

    internal void startGame()
    {
        startScreenPanel.SetActive(false);
        myCommandProcessor.createAndEnqueueChangeDialogueSequence(myTipManager.introText);
        dateLocationButton.GetComponentInChildren<Text>().text = "Exit " + mySceneCatalogue.getCurrentLocation().interiorName;
        myGameState.hasGameBegun = true;
    }

	public void enableOnlyBye(){
		dialogueOptionsButtonPanel.SetActive(false);
		departConversationButton.gameObject.SetActive(true);
        departConversationButton.enabled = true;
	}   

	public void resetDateButtons(){
		dateActionButton.SetActive(true);
	}

	public void showLocationOptions(){
        this.dialogueButtonPanel.SetActive(false);
		this.dateLocationButtonPanel.SetActive(true);
		myInputOrganizer.createDateLocationButtons();
	}

	internal void hideLocationOptions()
	{
		this.dialogueButtonPanel.SetActive(true);
        this.dateLocationButtonPanel.SetActive(false);
    }

    public void activateDialogueButtons()
    {
        dialogueOptionsButtonPanel.SetActive(true);
    }

internal void updateCutSceneTextContent(string textToWrite)
    {
        cutSceneTextToWrite = textToWrite;
    }
}
