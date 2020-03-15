using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IEventSubscriber {

    private GameState myGameState;
    private DialogueManager myDialogueManager;
    private ConversationTracker myConversationTracker;
    private SceneCatalogue mySceneCatalogue;
    private RelationshipCounselor myRelationshipCounselor;
    private Timelord myTimelord;
    private AnimationMaestro myAnimationMaestro;
    private InputOrganizer myInputOrganizer;
    private EventQueue myEventQueue;
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

    GameObject dateActionButton;
    GameObject askOnDateButton;
    GameObject mapButton;
    public GameObject talkButtonObject;
    public GameObject dateLocationButton;
    private Button timeAdvanceButton;
    private Button toggleInteriorSceneButton;
    public bool mapEnabled;
    public bool journalEnabled;

    void Awake()
    {
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myGameState = GameObject.FindObjectOfType<GameState>();
    }

    void Start()
    {
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        myTimelord = GameObject.FindObjectOfType<Timelord>();
        myConversationTracker = GameObject.FindObjectOfType<ConversationTracker>();
        myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
        myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
        myInputOrganizer = GameObject.FindObjectOfType<InputOrganizer>();
        myEventQueue = GameObject.FindObjectOfType<EventQueue>();


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
        askOnDateButton = GameObject.Find("AskOut");
        mapButton = GameObject.Find("MapButton");
        talkButtonObject = GameObject.Find("TalkButton");
        timeAdvanceButton = GameObject.Find("TimeButton").GetComponent<Button>();
        toggleInteriorSceneButton = GameObject.Find("DateLocationButton").GetComponent<Button>();

        myEventQueue.subscribe(this);
        mapEnabled = false;
        journalEnabled = false;
        menuPanel.gameObject.SetActive(false);
        dateLocationButtonPanel.SetActive(false);
        myAnimationMaestro.clearPotentialPartners();
    }

    void Update()
    {
        updateUIComponentsForState(myGameState.currentGameState);
        updateUIFromInput();
    }

    private void updateUIComponentsForState(GameState.gameStates currentState)
    {
        this.deactivateUIComponents();

        if (currentState == GameState.gameStates.COMMANDSEQUENCE)
        {
            mainPanel.SetActive(true);
            characterPanel.gameObject.SetActive(true);
            sequenceButtonsPanel.SetActive(true);
            myAnimationMaestro.updatePotentialPartnersSprites(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
        }
        else if (currentState == GameState.gameStates.CUTSCENE)
        {
            updateUIForCutSceneState();
        }
        else if (currentState == GameState.gameStates.PROWL)
        {
            mainPanel.SetActive(true);
            mainPanelButtonsPanel.SetActive(true);
            mapButton.SetActive(!mySceneCatalogue.getIsInInteriorScene());
            characterPanel.gameObject.SetActive(true);

            toggleMainPanelIfUIEnabled(mapPanel, this.mapEnabled);
            toggleMainPanelIfUIEnabled(journalPanel, this.journalEnabled);

            if (mainPanel.activeSelf == true)
            {
                myAnimationMaestro.writeDescriptionText(mySceneCatalogue.getLocationDescription(), textPanel);
                myAnimationMaestro.updatePotentialPartnersSprites(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
                updateSelectedPartnerButtonUI();
                updateToggleInteriorButtonUI();
            }
        }
        else if (currentState == GameState.gameStates.CONVERSATION)
        {
            dialoguePanel.SetActive(true);
            characterPanel.gameObject.SetActive(true);
            askOnDateButton.SetActive(myConversationTracker.canAskOnDateEnabled());
        }
        else if (currentState == GameState.gameStates.DATEINTRO)
        {
            mainPanel.SetActive(true);
            enableDateComponents();
            characterPanel.gameObject.SetActive(true);
            myAnimationMaestro.updatePotentialPartnersSprites(new List<Character>() { myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimelord.getCurrentTimestep()) });
            myAnimationMaestro.writeDescriptionText(mySceneCatalogue.getCurrentLocation().descriptionDate, textPanel);
        }
        else if (currentState == GameState.gameStates.DATE)
        {
            mainPanel.SetActive(true);
            enableDateComponents();
            characterPanel.gameObject.SetActive(true);
            myAnimationMaestro.updatePotentialPartnersSprites(new List<Character>() { myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimelord.getCurrentTimestep()) });
        }
        else if (currentState == GameState.gameStates.DATEOUTRO)
        {
            mainPanel.SetActive(true);
            enableDateComponents();
            myAnimationMaestro.writeDescriptionText("One good date can change your life.", textPanel);
        }
    }

    private void toggleMainPanelIfUIEnabled(GameObject uiToReplace, bool isEnabled)
    {
        if (isEnabled)
        {
            mainPanel.SetActive(false);
            uiToReplace.SetActive(true);
        }
    }

    private void updateUIForCutSceneState()
    {
        cutScenePanel.SetActive(true);
        cutScenePanel.GetComponentInChildren<Text>().text = cutSceneTextToWrite;
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
        dateButtonsPanel.SetActive(myRelationshipCounselor.isAtDate);
        dateActionButton.SetActive(!myRelationshipCounselor.getDateAbandonedOrExperienced());
        dateActionButton.GetComponentInChildren<Text>().text = mySceneCatalogue.getCurrentLocation().currentDateAction;
    }

    internal void gameOver()
    {
        SceneManager.LoadScene("splash_game_over");
    }

    internal void deactivateStartScreenPanel()
    {
        startScreenPanel.SetActive(false);
    }

    public void resetDateButtons() {
        dateActionButton.SetActive(true);
    }

    public void showLocationOptions() {
        this.dialogueButtonPanel.SetActive(false);
        this.dateLocationButtonPanel.SetActive(true);
        myInputOrganizer.createDateLocationButtons();
    }

    public void activateDialogueOptionsUI()
    {
        dialogueOptionsPanel.SetActive(true);
    }

    private void updateUIFromInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
        }
    }

    public void updateSelectedPartnerButtonUI()
    {
        bool partners = myDialogueManager.charactersPresent.Count > 0;
        talkButtonObject.SetActive(partners);
        //If People present, Set Talk Button to random character in location
        if (partners && myDialogueManager.selectedPartner < 0)
        {
            myInputOrganizer.BTN_characterClicked(new System.Random().Next(1, myDialogueManager.charactersPresent.Count + 1));
        }
    }

    void IEventSubscriber.eventOccurred(IGameEvent occurringEvent)
    {
        if (occurringEvent.getEventType() == "TIMEEVENT")
        {
            myAnimationMaestro.fadeInCharacters(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
            reactivateButtonFadingInIfCharactersPresent(timeAdvanceButton, 0.6f);
        }
        else if (occurringEvent.getEventType() == "LOCATIONEVENT")
        {
            myAnimationMaestro.fadeInCharacters(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
            reactivateButtonFadingInIfCharactersPresent(toggleInteriorSceneButton, 0.6f);
        }
        else if (occurringEvent.getEventType() == "SEQUENCEENDEVENT")
        {
            reactivateButtonFadingInIfCharactersPresent(timeAdvanceButton, 0.0f);
            reactivateButtonFadingInIfCharactersPresent(toggleInteriorSceneButton, 0.0f);
        }
        else if (occurringEvent.getEventType() == "DATESTARTEVENT")
        {
            myAnimationMaestro.fadeInCharacters(new List<Character>() { myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimelord.getCurrentTimestep()) });
        }
        else if (occurringEvent.getEventType() == "DATEACTIONEVENT")
        {
            mySceneCatalogue.getCurrentLocation().setRandomDateAction();
        }
    }

    private void reactivateButtonFadingInIfCharactersPresent(Button buttonToSetInteractable, float fadeDuration)
    {
        if (myDialogueManager.getAllCurrentLocalPresentConversationPartners().Count > 0)
        {
            StartCoroutine(myAnimationMaestro.delayGameCoroutine(fadeDuration, () => { buttonToSetInteractable.interactable = true; }));
        }
        else
        {
            buttonToSetInteractable.interactable = true;
        }
    }

    private void updateToggleInteriorButtonUI()
    {
        if (!mySceneCatalogue.getIsInInteriorScene())
        {
            dateLocationButton.GetComponentInChildren<Text>().text = "Enter " + mySceneCatalogue.getCurrentLocation().interiorName;
            dateLocationButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/icon_enter");
        }
        else
        {
            dateLocationButton.GetComponentInChildren<Text>().text = "Exit " + mySceneCatalogue.getCurrentLocation().interiorName;
            dateLocationButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/icon_exit");
        }
    }

    public void updateConversationUIAfterDateScheduled()
    {
        this.dialogueButtonPanel.SetActive(true);
        this.dateLocationButtonPanel.SetActive(false);
        dialogueOptionsPanel.SetActive(false);
    }

    public void setMainPanelActive()
    {
        mainPanel.gameObject.SetActive(true);
    }
}
