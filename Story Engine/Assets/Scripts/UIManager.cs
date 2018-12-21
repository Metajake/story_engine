using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IEventSubscriber {

    GameObject talkButtonObject;
	GameObject contextualActionButtonObject;
	Button byeButton;
    GameObject askOnDateButton;
    GameObject mapButton;
    GameObject journalButton;
    GameObject previousLocationButton;
	GameObject nextLocationButton;
	public GameObject dialoguePanel;
    public GameObject mainPanel;
	public GameObject mapPanel;
    public GameObject journalPanel;
    private GameState myGameState;
    private DialogueManager myDialogueManager;
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
    private GameObject characterPanel;
    private RelationshipCounselor myRelationshipCounselor;
    private VictoryCoach myVictoryCoach;
	private CommandProcessor myCommandProcessor;
    private Timelord myTimelord;
    private EventQueue myEventQueue;
    private Text textPanel;
    private Text pastDatesText;
    private Text upcomingDatesText;
    public GameObject dateLocationButtonPrefab;

	bool mapEnabled;
    bool journalEnabled;
    
    void Start () {
        talkButtonObject = GameObject.Find("TalkButton");
		contextualActionButtonObject = GameObject.Find("ContextActionButton");
		byeButton = GameObject.Find("Depart").GetComponent<Button>();
        askOnDateButton = GameObject.Find("AskOut");
        previousLocationButton = GameObject.Find("PreviousLocationButton");
        nextLocationButton = GameObject.Find("NextLocationButton");
		mapButton = GameObject.Find("MapButton");
        journalButton = GameObject.Find("JournalButton");

        myGameState = GameObject.FindObjectOfType<GameState>();
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        myMapCartographer = GameObject.FindObjectOfType<MapCartographer>();
        myTimelord = GameObject.FindObjectOfType<Timelord>();
        conversationTracker = GameObject.FindObjectOfType<ConversationTracker>();
		mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
		myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
		myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();
        myTipManager = GameObject.FindObjectOfType<TipManager>();
        myEventQueue = GameObject.FindObjectOfType<EventQueue>();

        dialogueButtonPanel = GameObject.Find("DialogueButtonPanel");
        dialogueOptionsButtonPanel = GameObject.Find("DialogueOptionsButtonPanel");
        dateLocationButtonPanel = GameObject.Find("LocationButtonPanel");
        mainPanelButtonsPanel = GameObject.Find("MainPanelButtonsPanel");
        dateButtonsPanel = GameObject.Find("DateButtonsPanel");
		sequenceButtonsPanel = GameObject.Find("SequenceButtonsPanel");
        characterPanel = GameObject.Find("CharacterPanel");

        textPanel = GameObject.Find("TextPanel").GetComponentInChildren<Text>();
        pastDatesText = GameObject.Find("PastDates").GetComponentInChildren<Text>();
        upcomingDatesText = GameObject.Find("UpcomingDates").GetComponentInChildren<Text>();

        myEventQueue.subscribe(this);

        dateLocationButtonPanel.SetActive(false);
		clearPotentialPartners();

		mapEnabled = false;
        journalEnabled = false;
    }
	
	void Update ()
	{
        enableComponentsForState(myGameState.currentGameState);
        populateComponentsForState(myGameState.currentGameState);
    }

    private void enableComponentsForState(GameState.gameStates currentState)
    {
        deactivateUIComponents();

        if(currentState == GameState.gameStates.COMMANDSEQUENCE)
        {
            mainPanel.SetActive(true);
            sequenceButtonsPanel.SetActive(true);
        }
        else if(currentState == GameState.gameStates.PROWL)
        {
            mainPanel.SetActive(true);
            mainPanelButtonsPanel.SetActive(true);
            mapButton.SetActive(!mySceneCatalogue.getIsInInteriorScene());
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
            askOnDateButton.SetActive(conversationTracker.canAskOnDateEnabled());
        }else if (currentState == GameState.gameStates.DATE)
        {
            mainPanel.SetActive(true);
            dateButtonsPanel.SetActive(myRelationshipCounselor.isAtDate);
        }
    }

    private void populateComponentsForState(GameState.gameStates currentState)
    {
        if (currentState == GameState.gameStates.COMMANDSEQUENCE)
        {
            if (!myGameState.hasGameBegun)
            {
                myCommandProcessor.createAndEnqueueChangeDialogueSequence(myTipManager.introText);
                myGameState.hasGameBegun = true;
            }
        }
        else if (currentState == GameState.gameStates.PROWL)
        {
            if (!mapEnabled && !journalEnabled)
            {
                placePotentialPartners( myDialogueManager.findConversationPartners() );
                updateSelectedPartnerUI();
            }
        }
        else if (currentState == GameState.gameStates.CONVERSATION)
        {
            
        }
        else if(currentState == GameState.gameStates.DATE)
        {
            placePotentialPartners( myDialogueManager.findConversationPartners() );
        }
    }

    private void deactivateUIComponents()
    {
        dialoguePanel.SetActive(false);
        journalPanel.SetActive(false);
        mapPanel.SetActive(false);
        mainPanel.SetActive(false);
        
        dateButtonsPanel.SetActive(false);
        mainPanelButtonsPanel.SetActive(false);
        sequenceButtonsPanel.SetActive(false);
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

    private string convertPastDatesToDateInfo(List<Date> allDates)
    {
        List<Date> pastDates = new List<Date>();
        foreach(Date d in allDates)
        {
            if(d.dateTime < myTimelord.getCurrentTimestep())
            {
                pastDates.Add(d);
            }
        }
        string allDatesInfo = stringifyAndSortDates(pastDates);
        return allDatesInfo;
    }

    private string convertUpcomingDatesToDateInfo(List<Date> allDates)
    {
        List<Date> upcomingDates = new List<Date>();
        foreach (Date d in allDates)
        {
            if (d.dateTime >= myTimelord.getCurrentTimestep())
            {
                upcomingDates.Add(d);
            }
        }
        string allDatesInfo = stringifyAndSortDates(upcomingDates);
        return allDatesInfo;
    }

    private string stringifyAndSortDates(List<Date> allDates)
    {
        allDates.Sort((x, y) => x.dateTime.CompareTo(y.dateTime));
        string allDatesInfo = "";
        foreach (Date d in allDates)
        {
            allDatesInfo += d.dateScene.interiorName + " " +  myTimelord.getTimeString(d.dateTime) + " " + d.character.givenName + "\n";
        }

        return allDatesInfo;
    }

    public void setDescriptionText(string toWrite){
		textPanel.text = toWrite;
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
		Character clickedCharacter = myDialogueManager.getPartnerAt(portraitNumber);
		if(clickedCharacter != null ){
			myDialogueManager.selectedPartner = portraitNumber - 1;
			talkButtonObject.GetComponentInChildren<Text>().text ="Talk to " + clickedCharacter.givenName;
		}
	}

	public void toggleDialogueWindow(bool isOn){
        if (!isOn) {
            myGameState.currentGameState = GameState.gameStates.PROWL;
            myDialogueManager.setConversationMode(isOn);
        }
        else
        {
            Character selectedCharacter = myDialogueManager.getPartnerAt(myDialogueManager.selectedPartner + 1);
            if (selectedCharacter is DateableCharacter)
            {
                myDialogueManager.setConversationMode(isOn);
                GameObject.FindObjectOfType<ConversationTracker>().beginConversation((DateableCharacter)selectedCharacter);
                myGameState.currentGameState = GameState.gameStates.CONVERSATION;
            }
            else
            {
                string dialogueString = myTipManager.getTip();
                myCommandProcessor.createAndExecuteChangeDialogueSequence(new List<string>() { dialogueString });
            }
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

                UnityAction buttonAction = () => BTN_scheduleDateForLocation(dateScenes[dateButtonIndex]);
                buttonObject.GetComponent<Button>().onClick.AddListener(buttonAction);

            }

        }
	}

	public void BTN_scheduleDateForLocation(Location dateLocation){
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

	public void BTN_toggleMap(){
        myMapCartographer.highlightCurrentLocation();
		this.mapEnabled = !this.mapEnabled;
	}

    public void BTN_toggleJournal()
    {
        pastDatesText.text = convertPastDatesToDateInfo(myRelationshipCounselor.getAllDates());
        upcomingDatesText.text = convertUpcomingDatesToDateInfo(myRelationshipCounselor.getAllDates());
        this.journalEnabled = !this.journalEnabled;
    }

    public void eventOccured(IGameEvent occurringEvent)
    {
        Debug.Log("Event Occurred");
        myDialogueManager.selectedPartner = -1;
    }
}
