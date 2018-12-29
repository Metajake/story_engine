using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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


    private GameObject dateLocationButtonPanel;
    public GameObject dateLocationButtonPrefab;

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
    }

    // Update is called once per frame
    void Update () {
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
                string dialogueString = myTipManager.getTip();
                myCommandProcessor.createAndEnqueueChangeDialogueSequence(new List<string>() { dialogueString });
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

    public void BTN_toggleJournal()
    {
        myUIManager.pastDatesText.text = myUIManager.convertPastDatesToDateInfo(myRelationshipCounselor.getAllDates());
        myUIManager.upcomingDatesText.text = myUIManager.convertUpcomingDatesToDateInfo(myRelationshipCounselor.getAllDates());
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
