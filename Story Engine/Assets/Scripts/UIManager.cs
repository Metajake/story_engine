﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    GameObject talkButtonObject;
	GameObject contextualActionButtonObject;
	Button byeButton;
    GameObject askOnDateButton;
    GameObject mapButton;
    GameObject previousLocationButton;
	GameObject nextLocationButton;
	public GameObject dialoguePanel;
    public GameObject mainPanel;
	public GameObject mapPanel;
	private DialogueManager dialogueManager;
	private MapCartographer myMapCartographer;
	private ConversationTracker conversationTracker;
	private SceneCatalogue mySceneCatalogue;
    private TipManager myTipManager;
    private GameObject dialogueButtonPanel;
	private GameObject dialogueOptionsButtonPanel;
	private GameObject dateLocationButtonPanel;
    private GameObject mainPanelButtonsPanel;
    private GameObject dateButtonsPanel;
	private GameObject sequenceButtonsPanel;
    private RelationshipCounselor myRelationshipCounselor;
    private VictoryCoach myVictoryCoach;
	private CommandProcessor myCommandProcessor;
	private Text textPanel;
    public GameObject dateLocationButtonPrefab;

	bool mapEnabled;

	// Use this for initialization
	void Start () {
        talkButtonObject = GameObject.Find("TalkButton");
		contextualActionButtonObject = GameObject.Find("ContextActionButton");
		byeButton = GameObject.Find("Depart").GetComponent<Button>();
        askOnDateButton = GameObject.Find("AskOut");
        previousLocationButton = GameObject.Find("PreviousLocationButton");
        nextLocationButton = GameObject.Find("NextLocationButton");
		mapButton = GameObject.Find("MapButton");

        dialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        myMapCartographer = GameObject.FindObjectOfType<MapCartographer>();
		conversationTracker = GameObject.FindObjectOfType<ConversationTracker>();
		mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
		myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
		myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();
        myTipManager = GameObject.FindObjectOfType<TipManager>();

        dialogueButtonPanel = GameObject.Find("DialogueButtonPanel");
        dialogueOptionsButtonPanel = GameObject.Find("DialogueOptionsButtonPanel");
        dateLocationButtonPanel = GameObject.Find("LocationButtonPanel");
        mainPanelButtonsPanel = GameObject.Find("MainPanelButtonsPanel");
        dateButtonsPanel = GameObject.Find("DateButtonsPanel");
		sequenceButtonsPanel = GameObject.Find("SequenceButtonsPanel");
        textPanel = GameObject.Find("TextPanel").GetComponentInChildren<Text>();

		dateLocationButtonPanel.SetActive(false);
		clearPotentialPartners();

		mapEnabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(this.mapEnabled){
			disableAllPanels();
            enableMapPanel();
			return;
		}

		if( !dialogueManager.getIsInConversationMode()){
			disableAllPanels();
			enableMainPanel();
			dialogueManager.updateCharacterUI();
			dialogueManager.updateSelectedPartnerUI();
		}

		dialoguePanel.SetActive(dialogueManager.getIsInConversationMode());
		mainPanel.SetActive(!dialogueManager.getIsInConversationMode());

		askOnDateButton.SetActive(conversationTracker.canAskOnDateEnabled());
		mapButton.SetActive(!mySceneCatalogue.getIsInInteriorScene());

		toggleButtons();

	}

	public void setDescriptionText(string toWrite){
		textPanel.text = toWrite;
	}


	private void toggleButtons()
	{
		sequenceButtonsPanel.SetActive(myCommandProcessor.isInSequence());
		mainPanelButtonsPanel.SetActive(!myRelationshipCounselor.isAtDate && !myCommandProcessor.isInSequence());
		dateButtonsPanel.SetActive(myRelationshipCounselor.isAtDate && !myCommandProcessor.isInSequence());
	}

	internal void gameOver()
	{
        Application.LoadLevel("splash_game_over");
	}

    public void placePotentialPartners(List<Character> potentialConversationPartners)
    {
		if(myCommandProcessor.isInSequence()){
			return;
		}
        for (int i = 0; i < 3; i++)
        {
            Image partnerPortrait = GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>();
            Text partnerNameplate = GameObject.Find("Character " + (i + 1) + " NamePlate").GetComponent<Text>();
            if (i < potentialConversationPartners.Count)
            {
                partnerPortrait.sprite = BackgroundSwapper.createSpriteFromTex2D(potentialConversationPartners[i].image);
                partnerPortrait.color = new Color(partnerPortrait.color.r, partnerPortrait.color.g, partnerPortrait.color.b, 1);
                partnerNameplate.text = potentialConversationPartners[i].givenName + " " + potentialConversationPartners[i].surname;

            }
            else
			{
				disablePartnerSelectionUI(partnerPortrait, partnerNameplate);
			}
		}
	}

	private void clearPotentialPartners(){
		for (int i = 0; i < 3; i++){
			Image partnerPortrait = GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>();
            Text partnerNameplate = GameObject.Find("Character " + (i + 1) + " NamePlate").GetComponent<Text>();
			disablePartnerSelectionUI(partnerPortrait, partnerNameplate);
		}
	}

	private static void disablePartnerSelectionUI(Image partnerPortrait, Text partnerNameplate)
	{
		partnerPortrait.color = new Color(partnerPortrait.color.r, partnerPortrait.color.g, partnerPortrait.color.b, 0);
		partnerNameplate.text = "";
	}

	internal void showNeutralDescriptionText()
    {
		setDescriptionText(mySceneCatalogue.neutralResultForCurrentLocationDescription());
    }

	internal void experienceDescription(){
		setDescriptionText(mySceneCatalogue.currentExperienceDescription());
		contextualActionButtonObject.SetActive(false);
	}

    internal void abandonDateDescription()
    {
        setDescriptionText("Bye, lame.");
        contextualActionButtonObject.SetActive(false);
    }

	public void onPortraitClicked(int portraitNumber){
		Character clickedCharacter = dialogueManager.getPartnerAt(portraitNumber);
		if(clickedCharacter != null ){
			dialogueManager.selectedPartner = portraitNumber - 1;
			talkButtonObject.GetComponentInChildren<Text>().text ="Talk to " + clickedCharacter.givenName;
		}
	}

	public void partnersPresent(bool partners){
		if (myCommandProcessor.isInSequence())
        {
            return;
        }
		talkButtonObject.SetActive(partners);
	}

	public void toggleDialogueWindow(bool isOn){
        
        Character selectedCharacter = dialogueManager.getPartnerAt(dialogueManager.selectedPartner + 1);
        if(selectedCharacter is DateableCharacter)
        {
            dialogueManager.setConversationMode(isOn);
            GameObject.FindObjectOfType<ConversationTracker>().beginConversation((DateableCharacter)selectedCharacter);
        }
        else
        {
            //construct command
            string dialogueString = myTipManager.getTip();
            myCommandProcessor.createAndExecuteChangeDialogueSequence(new List<string>() { dialogueString });
        }
    }

	public void enableAllButtons()
    {
		dialogueOptionsButtonPanel.SetActive(true);
    }

	public void enableOnlyBye(){
		dialogueOptionsButtonPanel.SetActive(false);
		byeButton.gameObject.SetActive(true);
        byeButton.enabled = true;
	}   

	public void resetDateButtons(){
		contextualActionButtonObject.SetActive(true);
	}

    private void createDateLocationButtons(){

        List<string> dateSceneNames = mySceneCatalogue.getDateSceneNames();

        List<Location> dateScenes = mySceneCatalogue.getDateScenes();

        Button[] allButtons = dateLocationButtonPanel.GetComponentsInChildren<Button>();

        foreach (Button b in allButtons)
        {
            Destroy(b.gameObject);
        }

        for (int j = 0; j < 3; j++)
        {

            for (int k = 0; k < 3; k++)
            {

                int dateButtonIndex = j * 3 + k;

                if (!mySceneCatalogue.isKnownDateLocation(dateSceneNames[dateButtonIndex]))
                {
                    continue;
                }

                GameObject buttonObject = GameObject.Instantiate(dateLocationButtonPrefab, dateLocationButtonPanel.transform);

                buttonObject.transform.Translate(new Vector3(k * 140, j * 50));

                buttonObject.GetComponentInChildren<Text>().text = dateSceneNames[dateButtonIndex];

                UnityAction buttonAction = () => scheduleDateForLocation(dateScenes[dateButtonIndex]);
                buttonObject.GetComponent<Button>().onClick.AddListener(buttonAction);

            }

        }
	}

	public void scheduleDateForLocation(Location dateLocation){
		conversationTracker.scheduleDate(dateLocation);

	}

	public void showLocationOptions(){
        this.dialogueButtonPanel.SetActive(false);
		this.dateLocationButtonPanel.SetActive(true);
		createDateLocationButtons();
	}

	internal void hideLocationOptions()
	{
		this.dialogueButtonPanel.SetActive(true);
        this.dateLocationButtonPanel.SetActive(false);
    }

	public void goalAchieved(DifficultyLevel levelAchieved){
        if(levelAchieved == DifficultyLevel.HARD){
            Debug.Log("You have ascended to the ultimate form of human being. Your love is free. You are at one with everyone.");
        }else{
			Debug.Log("Goal " + levelAchieved + " Attained!! ... But there are more experiences to be had!");
        }
	}

	public void toggleMap(){
		this.mapEnabled = !this.mapEnabled;
	}

	private void enableMapPanel()
	{
        mapPanel.SetActive(true);
		//myMapCartographer.createLocationButtons();
    }

    private void enableMainPanel()
    {
		mainPanel.SetActive(true);
    }

	private void disableAllPanels()
	{
        mapPanel.SetActive(false);
        mainPanel.SetActive(false);
		dialoguePanel.SetActive(false);
	}

	private void enableAllPanels()
    {
        mapPanel.SetActive(true);
        mainPanel.SetActive(true);
        dialoguePanel.SetActive(true);
    }

	public bool getMapEnabled(){
		return this.mapEnabled;
	}
 
}
