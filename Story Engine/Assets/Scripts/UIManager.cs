using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IEventSubscriber {

    GameObject talkButtonObject;
    GameObject dateActionButton;
	Button departConversationButton;
    GameObject askOnDateButton;
    GameObject mapButton;
	public GameObject dialoguePanel;
    public GameObject mainPanel;
	public GameObject mapPanel;
    public GameObject journalPanel;
    private GameObject menuPanel;
    private GameObject dialogueButtonPanel;
	private GameObject dialogueOptionsButtonPanel;
	private GameObject dateLocationButtonPanel;
    private GameObject mainPanelButtonsPanel;
    private GameObject dateButtonsPanel;
	private GameObject sequenceButtonsPanel;
    private GameObject dateLocationButton;
    private GameObject cutScenePanel;
    private GameObject characterPanel;

    private GameState myGameState;
    private DialogueManager myDialogueManager;
    private MapCartographer myMapCartographer;
    private ConversationTracker conversationTracker;
    private SceneCatalogue mySceneCatalogue;
    private TipManager myTipManager;
    private RelationshipCounselor myRelationshipCounselor;
	private CommandProcessor myCommandProcessor;
    private Timelord myTimelord;
    private EventQueue myEventQueue;
    private AudioConductor myAudioConductor;
    private AnimationMaestro myAnimationMaestro;

    private Text textPanel;
    private Text pastDatesText;
    private Text upcomingDatesText;
    private String cutSceneTextToWrite;
    public GameObject dateLocationButtonPrefab;

	bool mapEnabled;
    bool journalEnabled;
    
    void Start () {
        talkButtonObject = GameObject.Find("TalkButton");
		dateActionButton = GameObject.Find("DateActionButton");
		departConversationButton = GameObject.Find("Depart").GetComponent<Button>();
        askOnDateButton = GameObject.Find("AskOut");
		mapButton = GameObject.Find("MapButton");

        myGameState = GameObject.FindObjectOfType<GameState>();
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        myMapCartographer = GameObject.FindObjectOfType<MapCartographer>();
        myTimelord = GameObject.FindObjectOfType<Timelord>();
        conversationTracker = GameObject.FindObjectOfType<ConversationTracker>();
		mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
		myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
		myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();
        myTipManager = GameObject.FindObjectOfType<TipManager>();
        myEventQueue = GameObject.FindObjectOfType<EventQueue>();
        myAudioConductor = GameObject.FindObjectOfType<AudioConductor>();
        myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();

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

        textPanel = GameObject.Find("TextPanel").GetComponentInChildren<Text>();
        pastDatesText = GameObject.Find("PastDates").GetComponentInChildren<Text>();
        upcomingDatesText = GameObject.Find("UpcomingDates").GetComponentInChildren<Text>();

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
        BTN_toggleMenuPanel();
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
            if (!myGameState.hasGameBegun)
            {
                myCommandProcessor.createAndEnqueueChangeDialogueSequence(myTipManager.introText);
                dateLocationButton.GetComponentInChildren<Text>().text = "Exit " + mySceneCatalogue.getCurrentLocation().interiorName;
                myGameState.hasGameBegun = true;
            }
        }else if(currentState == GameState.gameStates.CUTSCENE)
        {
            cutScenePanel.GetComponentInChildren<Text>().text = cutSceneTextToWrite;
        }
        else if (currentState == GameState.gameStates.PROWL)
        {
            if (!mapEnabled && !journalEnabled)
            {
                describeLocation();
                myAnimationMaestro.placePotentialPartnersInUI( myDialogueManager.getAllCurrentLocalPresentConversationPartners() );
                updateSelectedPartnerUI();
            }
        }
        else if (currentState == GameState.gameStates.CONVERSATION)
        {
            //NOTHING HERE YET :D
        }else if (currentState == GameState.gameStates.DATEINTRO)
        {
            myAnimationMaestro.placePotentialPartnersInUI(new List<Character>() {
                myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimelord.getCurrentTimestep())
            });
            dateActionButton.GetComponentInChildren<Text>().text = mySceneCatalogue.getCurrentLocation().currentDateAction;
            setDescriptionText(mySceneCatalogue.getCurrentLocation().descriptionDate);
        }
        else if(currentState == GameState.gameStates.DATE)
        {
            myAnimationMaestro.placePotentialPartnersInUI(new List<Character>() {
                myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimelord.getCurrentTimestep())
            });
            dateActionButton.GetComponentInChildren<Text>().text = mySceneCatalogue.getCurrentLocation().currentDateAction;
        }
        else if (currentState == GameState.gameStates.DATEOUTRO)
        {
            dateActionButton.GetComponentInChildren<Text>().text = mySceneCatalogue.getCurrentLocation().currentDateAction;
            setDescriptionText("The results of my date went fine.");
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

    private void describeLocation()
    {
        setDescriptionText(mySceneCatalogue.getLocationDescription());
    }

    private string convertPastDatesToDateInfo(List<Date> allDates)
    {
        List<Date> pastDates = new List<Date>();
        foreach(Date d in allDates)
        {
            if(d.isOver)
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
            if (!d.isOver)
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

    internal void setCutSceneText(string textToWrite)
    {
        cutSceneTextToWrite = textToWrite;
    }

    internal void gameOver()
	{
        SceneManager.LoadScene("splash_game_over");
	}

	internal void showNeutralDescriptionText()
    {
		setDescriptionText(mySceneCatalogue.neutralResultForCurrentLocationDescription());
    }

    internal void abandonDateDescription()
    {
        setDescriptionText("Bye, lame.");
        dateActionButton.SetActive(false);
    }

    private void BTN_toggleMenuPanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
        }
    }

	public void BTN_toggleDialogueWindow(bool isDialoguing){
        if (!isDialoguing) {
            myDialogueManager.getPartnerAt(myDialogueManager.selectedPartner + 1).isPresent = false;
            myDialogueManager.getPartnerAt(myDialogueManager.selectedPartner + 1).returnTime = myTimelord.getCurrentTimestep() +1;
            myGameState.currentGameState = GameState.gameStates.PROWL;
            myDialogueManager.setConversationMode(isDialoguing);
            myDialogueManager.selectedPartner = -1;
        }
        else
        {
            Character selectedCharacter = myDialogueManager.getPartnerAt(myDialogueManager.selectedPartner + 1);
            if (selectedCharacter is DateableCharacter)
            {
                myDialogueManager.setConversationMode(isDialoguing);
                GameObject.FindObjectOfType<ConversationTracker>().beginConversation((DateableCharacter)selectedCharacter);
                myGameState.currentGameState = GameState.gameStates.CONVERSATION;
            }
            else
            {
                string dialogueString = myTipManager.getTip();
                myCommandProcessor.createAndEnqueueChangeDialogueSequence(new List<string>() { dialogueString });
            }
        }
    }

	public void enableAllButtons()
    {
		dialogueOptionsButtonPanel.SetActive(true);
    }

	public void enableOnlyBye(){
		dialogueOptionsButtonPanel.SetActive(false);
		departConversationButton.gameObject.SetActive(true);
        departConversationButton.enabled = true;
	}   

	public void resetDateButtons(){
		dateActionButton.SetActive(true);
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

    public void BTN_onLocationClick(int sceneNumber)
    {
        myMapCartographer.changeScene(sceneNumber);
        BTN_toggleMap();
        myAudioConductor.loadAndPlay(myAudioConductor.subwayCar);
        myEventQueue.queueEvent(new SceneChangeEvent());
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

    public void BTN_exitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void eventOccured(IGameEvent occurringEvent)
    {
        Debug.Log(occurringEvent.getEventType());
        if (occurringEvent.getEventType() == "TIMEEVENT") {
            myAnimationMaestro.fadeInCharacters(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
            foreach (DateableCharacter character in myDialogueManager.allDateableCharacters)
            {
                character.checkAndSetReturnToPresent(myTimelord.getCurrentTimestep());
            }
            
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
        }else if (occurringEvent.getEventType() == "DATESTARTEVENT")
        {
            myAnimationMaestro.fadeInCharacters(new List<Character>() {
                myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimelord.getCurrentTimestep())
            });
            myAnimationMaestro.fadeInCharacters(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
        }
        else if(occurringEvent.getEventType() == "DATEACTIONEVENT")
        {
            mySceneCatalogue.getCurrentLocation().setRandomDateAction();
        }
        
        
    }
}
