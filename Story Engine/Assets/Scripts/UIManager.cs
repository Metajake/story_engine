﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IEventSubscriber {

    private GameState myGameState;
    private CharacterManager myCharacterManager;
    private ConversationTracker conversationTracker;
    private SceneCatalogue mySceneCatalogue;
    private TipManager myTipManager;
    private RelationshipCounselor myRelationshipCounselor;
	private CommandProcessor myCommandProcessor;
    private Timecop myTimeCop;
    private EventQueue myEventQueue;
    private AnimationMaestro myAnimationMaestro;
    private InputOrganizer myInputOrganizer;
    private BackgroundSwapper myBackgroundSwapper;
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
    private String cutSceneTextToWrite;

    GameObject talkButtonObject;
    GameObject dateActionButton;
    Button departConversationButton;
    GameObject askOnDateButton;
    GameObject mapButton;

    public bool mapEnabled;
    public bool journalEnabled;

    void Awake()
    {
        myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();
        myTipManager = GameObject.FindObjectOfType<TipManager>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myGameState = GameObject.FindObjectOfType<GameState>();
    }
    
    void Start () {
        myCharacterManager = GameObject.FindObjectOfType<CharacterManager>();
        myTimeCop = GameObject.FindObjectOfType<Timecop>();
        conversationTracker = GameObject.FindObjectOfType<ConversationTracker>();
		myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
        myEventQueue = GameObject.FindObjectOfType<EventQueue>();
        myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
        myInputOrganizer = GameObject.FindObjectOfType<InputOrganizer>();
        myBackgroundSwapper = GameObject.FindObjectOfType<BackgroundSwapper>();
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();

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
            mainPanel.SetActive(true);
            characterPanel.gameObject.SetActive(true);
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
            myAnimationMaestro.setDescriptionText(cutSceneTextToWrite, textPanel);
            List<Character> sceneCharacters = new List<Character>() {
                myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimeCop.getCurrentTimestep())
            };
            sceneCharacters.AddRange(myCharacterManager.experienceActors);
            myAnimationMaestro.updateCharacterPanelSprites(sceneCharacters);
        }
        else if (currentState == GameState.gameStates.PROWL)
        {
            if (!mapEnabled && !journalEnabled)
            {
                myAnimationMaestro.updateLocationDescription();
                myAnimationMaestro.updateCharacterPanelSprites( myCharacterManager.getAllCurrentLocalPresentCharacters() );
                updateSelectedPartnerTalkButtonUI();
            }
        }
        else if (currentState == GameState.gameStates.CONVERSATION)
        {
            
        }else if (currentState == GameState.gameStates.DATEINTRO)
        {
            myAnimationMaestro.updateCharacterPanelSprites(new List<Character>() {
                myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimeCop.getCurrentTimestep())
            });
            dateActionButton.GetComponentInChildren<Text>().text = mySceneCatalogue.getCurrentLocation().currentDateAction;
            myAnimationMaestro.setDescriptionText(mySceneCatalogue.getCurrentLocation().descriptionDate, textPanel);
        }
        else if(currentState == GameState.gameStates.DATE)
        {
            myAnimationMaestro.updateCharacterPanelSprites(new List<Character>() {
                myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimeCop.getCurrentTimestep())
            });
            dateActionButton.GetComponentInChildren<Text>().text = mySceneCatalogue.getCurrentLocation().currentDateAction;
        }
        else if (currentState == GameState.gameStates.DATEOUTRO)
        {
            dateActionButton.GetComponentInChildren<Text>().text = mySceneCatalogue.getCurrentLocation().currentDateAction;
            myAnimationMaestro.setDescriptionText("The results of my date went fine.", textPanel);
        }
    }

    public void eventOccured(IGameEvent occurringEvent)
    {
        Debug.Log(occurringEvent.getEventType());
        if (occurringEvent.getEventType() == "TIMEEVENT")
        {
            myBackgroundSwapper.backgroundSky.sprite = BackgroundSwapper.createSpriteFromTex2D( myBackgroundSwapper.getNextEnvironmentBackground() );
            foreach (DateableCharacter character in myCharacterManager.allDateableCharacters)
            {
                character.checkAndSetReturnToPresent(myTimeCop.getCurrentTimestep());
            }

        }
        else if (occurringEvent.getEventType() == "LOCATIONEVENT")
        {
            //check past characters vs present characters
            //fadeout absent characters
            //change scene backgrounr
            //fade in new characters
            //myAnimationMaestro.fadeTo(myCharacterManager.getAllCurrentLocalPresentCharacters(), true);

            myCharacterManager.selectedPartner = -1;
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
            myAnimationMaestro.fadeTo(new List<Character>() {
                myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimeCop.getCurrentTimestep())
            }, true);
            myAnimationMaestro.fadeTo(myCharacterManager.getAllCurrentLocalPresentCharacters(), false);
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

    private void updateSelectedPartnerTalkButtonUI()
    {
        bool partners = myCharacterManager.charactersPresent.Count > 0;

        talkButtonObject.SetActive(partners);
        selectRandomTalkPartner(partners);
    }

    private void selectRandomTalkPartner(bool partners)
    {
        if (partners && myCharacterManager.selectedPartner < 0)
        {
            onPortraitClicked(new System.Random().Next(1, myCharacterManager.charactersPresent.Count + 1));
        }
    }

    //TODO MOVE TO INPUT MANAGER
    public void onPortraitClicked(int portraitNumber)
    {
        Character clickedCharacter = myCharacterManager.getPartnerAt(portraitNumber);
        if (clickedCharacter != null)
        {
            myCharacterManager.selectedPartner = portraitNumber - 1;
            talkButtonObject.GetComponentInChildren<Text>().text = "Talk to " + clickedCharacter.givenName;
        }
        //myAnimationMaestro.doFade(myCharacterManager.getAllCurrentLocalPresentCharacters(), false);
    }

    internal void gameOver()
	{
        SceneManager.LoadScene("splash_game_over");
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
