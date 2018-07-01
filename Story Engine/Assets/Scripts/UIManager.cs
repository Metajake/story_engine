﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    GameObject talkButtonObject;
	GameObject contextualActionButtonObject;
	Button byeButton;
    GameObject askOnDateButton;
	GameObject changeLocationButton;
	public GameObject dialoguePanel;
	public GameObject mainPanel;
	private DialogueManager dialogueManager;
	private ConversationTracker conversationTracker;
	private SceneCatalogue mySceneCatalogue;
    private GameObject dialogueButtonPanel;
	private GameObject dialogueOptionsButtonPanel;
	private GameObject dateLocationButtonPanel;
    private GameObject mainPanelButtonsPanel;
    private GameObject dateButtonsPanel;
	private GameObject sequenceButtonsPanel;
    private RelationshipCounselor myRelationshipCounselor;
    private VictoryCoach myVictoryCoach;
	private CommandProcessor myCommandProcessor;
	private Text sceneDescriptionText;

	// Use this for initialization
	void Start () {
        talkButtonObject = GameObject.Find("TalkButton");
		contextualActionButtonObject = GameObject.Find("ContextActionButton");
		byeButton = GameObject.Find("Depart").GetComponent<Button>();
        askOnDateButton = GameObject.Find("AskOut");
		changeLocationButton = GameObject.Find("LocationButton");

        dialogueManager = GameObject.FindObjectOfType<DialogueManager>();
		conversationTracker = GameObject.FindObjectOfType<ConversationTracker>();
		mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
		myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
		myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();

        dialogueButtonPanel = GameObject.Find("DialogueButtonPanel");
        dialogueOptionsButtonPanel = GameObject.Find("DialogueOptionsButtonPanel");
        dateLocationButtonPanel = GameObject.Find("LocationButtonPanel");
        mainPanelButtonsPanel = GameObject.Find("MainPanelButtonsPanel");
        dateButtonsPanel = GameObject.Find("DateButtonsPanel");
		sequenceButtonsPanel = GameObject.Find("SequenceButtonsPanel");
		sceneDescriptionText = GameObject.Find("SceneDescriptionPanel").GetComponentInChildren<Text>();

		dateLocationButtonPanel.SetActive(false);
		clearPotentialPartners();
	}
	
	// Update is called once per frame
	void Update ()
	{
		dialoguePanel.SetActive(dialogueManager.getIsInConversationMode());
		mainPanel.SetActive(!dialogueManager.getIsInConversationMode());

		askOnDateButton.SetActive(conversationTracker.canAskOnDateEnabled());
		changeLocationButton.SetActive(!mySceneCatalogue.getIsInDateScene());

		toggleButtons();

		if (myVictoryCoach.hasAchievedAllExperiences())
		{
			gameWon();
		}
	}

	public void setDescriptionText(string toWrite){
		sceneDescriptionText.text = toWrite;
	}


	private void toggleButtons()
	{
		sequenceButtonsPanel.SetActive(myCommandProcessor.isInSequence());
		mainPanelButtonsPanel.SetActive(!myRelationshipCounselor.isAtDate && !myCommandProcessor.isInSequence());
		dateButtonsPanel.SetActive(myRelationshipCounselor.isAtDate && !myCommandProcessor.isInSequence());
	}

	internal void gameOver()
	{
		setDescriptionText("She fell in love with you. As a result, you fell in love with her. She then dumped you, and you were left heartbroken. Game Over.");
	}

	public void placePotentialPartners(List<Character> potentialPartners)
    {
		if(myCommandProcessor.isInSequence()){
			return;
		}
        for (int i = 0; i < 3; i++)
        {
            Image partnerPortrait = GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>();
            Text partnerNameplate = GameObject.Find("Character " + (i + 1) + " NamePlate").GetComponent<Text>();
            if (i < potentialPartners.Count)
            {
                partnerPortrait.sprite = BackgroundSwapper.createSpriteFromTex2D(potentialPartners[i].image);
                partnerPortrait.color = new Color(partnerPortrait.color.r, partnerPortrait.color.g, partnerPortrait.color.b, 1);
                partnerNameplate.text = potentialPartners[i].givenName + " " + potentialPartners[i].surname;

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

	public void switchDialogueWindow(bool isOn){
		dialogueManager.setConversationMode(isOn);
		GameObject.FindObjectOfType<ConversationTracker>().beginConversation(dialogueManager.getPartnerAt(dialogueManager.selectedPartner + 1));
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

	private void updateLocationButtons(){
		for (int i = 0; i < mySceneCatalogue.dateSceneNames.Length; i++){
			Button sceneButton = GameObject.Find("Location" + (i + 1) + "Button").GetComponent<Button>();
			sceneButton.interactable = true;
			sceneButton.GetComponentInChildren<Text>().text = mySceneCatalogue.dateSceneNames[i];
		}
		int numberOfLocationButtons = GameObject.Find("LocationButtonPanel").transform.childCount;
		for (int j = mySceneCatalogue.dateSceneNames.Length; j < numberOfLocationButtons; j++ ){
			Button sceneButton = GameObject.Find("Location" + (j + 1) + "Button").GetComponent<Button>();
			sceneButton.interactable = false;
            sceneButton.GetComponentInChildren<Text>().text = "---";
		}
	}

	public void showLocationOptions(){
        this.dialogueButtonPanel.SetActive(false);
		this.dateLocationButtonPanel.SetActive(true);
		updateLocationButtons();
	}

	internal void hideLocationOptions()
	{
		this.dialogueButtonPanel.SetActive(true);
        this.dateLocationButtonPanel.SetActive(false);
    }

	public void gameWon(){
		Debug.Log("game won!!");
	}
}
