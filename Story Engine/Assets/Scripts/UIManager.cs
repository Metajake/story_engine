﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    private GameState myGameState;
    private DialogueManager myDialogueManager;
    private ConversationTracker conversationTracker;
    private SceneCatalogue mySceneCatalogue;
    private TipManager myTipManager;
    private RelationshipCounselor myRelationshipCounselor;
	private CommandProcessor myCommandProcessor;
    private Timelord myTimelord;
    private AnimationMaestro myAnimationMaestro;
    private InputOrganizer myInputOrganizer;

    public GameObject dialoguePanel;
    public GameObject mainPanel;
    public GameObject mapPanel;
    public GameObject journalPanel;
    public GameObject menuPanel;
    private GameObject dialogueButtonPanel;
    private GameObject dialogueOptionsPanel;
    private GameObject dateLocationButtonPanel;
    private GameObject mainPanelButtonsPanel;
    private GameObject dateButtonsPanel;
    private GameObject sequenceButtonsPanel;
    private GameObject cutScenePanel;
    private GameObject characterPanel;
    private GameObject startScreenPanel;

    private Text textPanel;
    public Text pastDatesText;
    public Text upcomingDatesText;
    public Text experiencesText;
    public String cutSceneTextToWrite;

    GameObject talkButtonObject;
    GameObject dateActionButton;
    Button departConversationButton;
    GameObject askOnDateButton;
    GameObject mapButton;
    public GameObject dateLocationButton;

    public bool mapEnabled;
    public bool journalEnabled;
    
    void Awake()
    {
        myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();
        myTipManager = GameObject.FindObjectOfType<TipManager>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myGameState = GameObject.FindObjectOfType<GameState>();
    }
    
    void Start ()
    {
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        myTimelord = GameObject.FindObjectOfType<Timelord>();
        conversationTracker = GameObject.FindObjectOfType<ConversationTracker>();
		myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
        myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
        myInputOrganizer = GameObject.FindObjectOfType<InputOrganizer>();


        dialogueButtonPanel = GameObject.Find("DialogueButtonPanel");
        dialogueOptionsPanel = GameObject.Find("DialogueOptionsButtonPanel");
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

        dateActionButton = GameObject.Find("DateActionButton");
        departConversationButton = GameObject.Find("Depart").GetComponent<Button>();
        askOnDateButton = GameObject.Find("AskOut");
        mapButton = GameObject.Find("MapButton");

		mapEnabled = false;
        journalEnabled = false;
        menuPanel.gameObject.SetActive(false);
        dateLocationButtonPanel.SetActive(false);
		myAnimationMaestro.clearPotentialPartners();
    }
	
	void Update ()
	{
        updateUIComponentsForState(myGameState.currentGameState);
        updateUIFromInput();
    }

    private void updateUIComponentsForState(GameState.gameStates currentState)
    {
        this.deactivateUIComponents();

        if(currentState == GameState.gameStates.COMMANDSEQUENCE)
        {
            mainPanel.SetActive(true);
            sequenceButtonsPanel.SetActive(true);
            characterPanel.gameObject.SetActive(true);
        }
        else if(currentState == GameState.gameStates.CUTSCENE)
        {
            cutScenePanel.SetActive(true);
            cutScenePanel.GetComponentInChildren<Text>().text = cutSceneTextToWrite;
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

            if (!mapEnabled && !journalEnabled)
            {
                this.updateLocationDescription();
                myAnimationMaestro.updatePotentialPartnersSprites(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
                myInputOrganizer.updateSelectedPartnerButtonUI();
            }
        }
        else if (currentState == GameState.gameStates.CONVERSATION)
        {
            dialoguePanel.SetActive(true);
            characterPanel.gameObject.SetActive(true);
            askOnDateButton.SetActive(conversationTracker.canAskOnDateEnabled());
        }else if(currentState == GameState.gameStates.DATEINTRO)
        {
            enableDateComponents();
            myAnimationMaestro.updatePotentialPartnersSprites(new List<Character>() {
                myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimelord.getCurrentTimestep())
            });
            dateActionButton.GetComponentInChildren<Text>().text = mySceneCatalogue.getCurrentLocation().currentDateAction;
            myAnimationMaestro.writeDescriptionText(mySceneCatalogue.getCurrentLocation().descriptionDate, textPanel);
        }
        else if (currentState == GameState.gameStates.DATE)
        {
            enableDateComponents();
            myAnimationMaestro.updatePotentialPartnersSprites(new List<Character>() {
                myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimelord.getCurrentTimestep())
            });
            dateActionButton.GetComponentInChildren<Text>().text = mySceneCatalogue.getCurrentLocation().currentDateAction;
        }
        else if (currentState == GameState.gameStates.DATEOUTRO)
        {
            enableDateComponents();
            characterPanel.gameObject.SetActive(false);
            dateActionButton.GetComponentInChildren<Text>().text = mySceneCatalogue.getCurrentLocation().currentDateAction;
            myAnimationMaestro.writeDescriptionText("One good date can change your life.", textPanel);
        }
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

    private void enableDateComponents()
    {
        mainPanel.SetActive(true);
        characterPanel.gameObject.SetActive(true);
        dateButtonsPanel.SetActive(myRelationshipCounselor.isAtDate);
        dateActionButton.SetActive(!getDateAbandonedOrExperienced());
    }

    private bool getDateAbandonedOrExperienced()
    {
        bool toReturn = false;
        if (myRelationshipCounselor.getCurrentDateFromScheduledDateList().experienceAchieved || myRelationshipCounselor.getCurrentDateFromScheduledDateList().isAbandoned)
        {
            toReturn = true;
        }
        return toReturn;
    }

    private void updateLocationDescription()
    {
        myAnimationMaestro.writeDescriptionText(mySceneCatalogue.getLocationDescription(), textPanel);
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
		dialogueOptionsPanel.SetActive(false);
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

    public void activateDialogueOptionsUI()
    {
        dialogueOptionsPanel.SetActive(true);
    }

    private void updateUIFromInput()
    {
        //Handle Game Menu UI
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
        }
    }

}
