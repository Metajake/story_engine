using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class InputOrganizer : MonoBehaviour {
    private UIManager myUIManager;
    private DialogueManager myDialogueManager;
    private GameState myGameState;
    private Timelord myTimelord;
    private TipManager myTipManager;
    private CommandProcessor myCommandProcessor;
    private SceneCatalogue mySceneCatalogue;
    private ConversationTracker myConversationTracker;
    private MapCartographer myMapCartographer;
    private AudioConductor myAudioConductor;
    private EventQueue myEventQueue;
    private RelationshipCounselor myRelationshipCounselor;
    private VictoryCoach myVictoryCoach;


    private GameObject dateLocationButtonPanel;
    public GameObject dateLocationButtonPrefab;

    bool oneClick = false;
    bool doubleClickTimerRunning;
    float timerForDoubleClick;
    float doubleClickDelay;

    private void Awake()
    {
        dateLocationButtonPanel = GameObject.Find("LocationButtonPanel");
    }

    // Use this for initialization
    void Start () {
        myUIManager = GameObject.FindObjectOfType<UIManager>();
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        myGameState = GameObject.FindObjectOfType<GameState>();
        myTimelord = GameObject.FindObjectOfType<Timelord>();
        myTipManager = GameObject.FindObjectOfType<TipManager>();
        myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myConversationTracker = GameObject.FindObjectOfType<ConversationTracker>();
        myMapCartographer = GameObject.FindObjectOfType<MapCartographer>();
        myAudioConductor = GameObject.FindObjectOfType<AudioConductor>();
        myEventQueue = GameObject.FindObjectOfType<EventQueue>();
        myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();

        doubleClickDelay = 0.6f;
    }

    // Update is called once per frame
    void Update ()
    {
    }

    private void checkDoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!oneClick)
            {
                //oneClick = true;
                timerForDoubleClick = Time.time;
            }
            else
            {
                oneClick = false;
                myCommandProcessor.createAndEnqueueChangeDialogueSequence(new List<string>() { "Slow Down." });
            }
        }
        if (oneClick)
        {
            if ((Time.time - timerForDoubleClick) > doubleClickDelay)
            {
                oneClick = false;
            }
        }
    }

    public void createDateLocationButtons()
    {

        List<string> dateSceneNames = mySceneCatalogue.getDateSceneNames();

        List<Location> dateScenes = mySceneCatalogue.getDateScenes();

        Button[] allButtons = dateLocationButtonPanel.GetComponentsInChildren<Button>();

        foreach (Button b in allButtons)
        {
            Destroy(b.gameObject);
        }

        //TODO: this checks for 9 total max date locations. Update this to accommodate any number of date locations.
        for (int j = 0; j < 3; j++)
        {

            for (int k = 0; k < 3; k++)
            {

                int dateButtonIndex = j * 3 + k;

                try {

                    if (!mySceneCatalogue.isKnownDateLocation(dateSceneNames[dateButtonIndex]))
                    {
                        continue;
                    }

                    GameObject buttonObject = GameObject.Instantiate(dateLocationButtonPrefab, dateLocationButtonPanel.transform);

                    buttonObject.transform.Translate(new Vector3(k * 140, j * 50));

                    buttonObject.GetComponentInChildren<Text>().text = dateSceneNames[dateButtonIndex];

                    UnityAction buttonAction = () => BTN_scheduleDateForLocation(dateScenes[dateButtonIndex]);
                    buttonObject.GetComponent<Button>().onClick.AddListener(buttonAction);

                } catch (ArgumentOutOfRangeException outOfRange) {
                    Debug.Log("Exception: " + outOfRange.Message);
                    continue;
                }

            }

        }
    }

    public void BTN_startGame()
    {
        myUIManager.startGame();
    }

    public void BTN_advanceTime()
    {
        myTimelord.advanceTimestep();
    }

    public void BTN_toggleMenuPanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            myUIManager.menuPanel.gameObject.SetActive(!myUIManager.menuPanel.gameObject.activeSelf);
        }
    }

    public void BTN_toggleDialogueWindow(bool isDialoguing)
    {
        if (!isDialoguing)
        {
            myDialogueManager.getPartnerAt(myDialogueManager.selectedPartner + 1).isPresent = false;
            myDialogueManager.getPartnerAt(myDialogueManager.selectedPartner + 1).returnTime = myTimelord.getCurrentTimestep() + 1;
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
                myCommandProcessor.createAndEnqueueChangeDialogueSequence(new List<string>() { myTipManager.getTip() });
            }
        }
    }

    public void BTN_scheduleDateForLocation(Location dateLocation)
    {
        myConversationTracker.scheduleDate(dateLocation);
    }

    public void BTN_onLocationClick(int sceneNumber)
    {
        myMapCartographer.changeScene(sceneNumber);
        BTN_toggleMap();
        myAudioConductor.loadAndPlay(myAudioConductor.subwayCar);
        myEventQueue.queueEvent(new SceneChangeEvent());
    }

    public void BTN_toggleMap()
    {
        myMapCartographer.highlightCurrentLocation();
        myUIManager.mapEnabled = !myUIManager.mapEnabled;
    }

    // TODO generate UI in a different place
    public void BTN_toggleJournal()
    {
        myUIManager.pastDatesText.text = myRelationshipCounselor.convertPastDatesToDateInfo(myRelationshipCounselor.getAllDates());
        myUIManager.upcomingDatesText.text = myRelationshipCounselor.convertUpcomingDatesToDateInfo(myRelationshipCounselor.getAllDates());
        myUIManager.experiencesText.text = myVictoryCoach.convertExperiencesToExperienceInfo();
        myUIManager.journalEnabled = !myUIManager.journalEnabled;
    }

    public void BTN_exitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
