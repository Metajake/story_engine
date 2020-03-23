using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class InputOrganizer : MonoBehaviour {
    private UIManager myUIManager;
    private DialogueManager myDialogueManager;
    private Timelord myTimeLord;
    private SceneCatalogue mySceneCatalogue;
    private ConversationTracker myConversationTracker;
    private MapCartographer myMapCartographer;
    private AudioConductor myAudioConductor;
    private RelationshipCounselor myRelationshipCounselor;
    private VictoryCoach myVictoryCoach;
    private TipManager myTipManager;
    private GameObject dateLocationButtonPanel;
    public GameObject dateLocationButtonPrefab;
    private Button timeAdvanceButton;
    private Button toggleInteriorSceneButton;

    private void Awake()
    {
        dateLocationButtonPanel = GameObject.Find("LocationButtonPanel");
    }

    // Use this for initialization
    void Start () {
        myUIManager = GameObject.FindObjectOfType<UIManager>();
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        myTimeLord = GameObject.FindObjectOfType<Timelord>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myConversationTracker = GameObject.FindObjectOfType<ConversationTracker>();
        myMapCartographer = GameObject.FindObjectOfType<MapCartographer>();
        myAudioConductor = GameObject.FindObjectOfType<AudioConductor>();
        myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
        myTipManager = GameObject.FindObjectOfType<TipManager>();

        timeAdvanceButton = GameObject.Find("TimeButton").GetComponent<Button>();
        toggleInteriorSceneButton = GameObject.Find("DateLocationButton").GetComponent<Button>();
    }

    public void createDateLocationButtons()
    {
        List<Location> dateScenes = mySceneCatalogue.getDateScenes();
        Button[] allButtons = dateLocationButtonPanel.GetComponentsInChildren<Button>();

        foreach (Button b in allButtons){ Destroy(b.gameObject);}

        for (int j = 0; j < 4; j++)
        {
            for (int k = 0; k < 4; k++)
            {
                int dateButtonIndex = j * 4 + k;
                if (dateButtonIndex > dateScenes.Count-1){return;}

                GameObject buttonObject = GameObject.Instantiate(dateLocationButtonPrefab, dateLocationButtonPanel.transform);

                float buttonWidth = buttonObject.GetComponentInChildren<RectTransform>().rect.width;
                float buttonHeight = buttonObject.GetComponentInChildren<RectTransform>().rect.height;

                buttonObject.GetComponentInChildren<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, k * buttonWidth, buttonWidth);
                buttonObject.GetComponentInChildren<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, j * buttonHeight, buttonHeight);

                buttonObject.GetComponentInChildren<Text>().text = dateScenes[dateButtonIndex].interiorName;

                UnityAction buttonAction = () => BTN_scheduleDateForLocation(dateScenes[dateButtonIndex]);
                buttonObject.GetComponent<Button>().onClick.AddListener(buttonAction);
            }
        }
    }

    public void BTN_startGame()
    {
        myUIManager.deactivateStartScreenPanel();
        myTipManager.startGame();
    }

    public void BTN_advanceTime()
    {
        timeAdvanceButton.interactable = false;
        myTimeLord.checkCharactersToFadeAndAdvanceTime();
    }

    public void BTN_toggleDialogueWindow(bool isDialoguing)
    {
        if (!isDialoguing)
        {
            myDialogueManager.endDialogue(isDialoguing);
        }
        else
        {
            myDialogueManager.beginDialogue(isDialoguing);
        }
    }

    public void BTN_scheduleDateForLocation(Location dateLocation)
    {
        myConversationTracker.scheduleDate(dateLocation);
        myUIManager.updateConversationUIAfterDateScheduled();
    }

    public void BTN_onLocationClick(int sceneNumber)
    {
        myMapCartographer.changeScene(sceneNumber);
        BTN_toggleMap();
        myAudioConductor.loadAndPlay(myAudioConductor.subwayCar);
        myVictoryCoach.checkQuestsCompleteAndQueueEvent(new EventSceneChange());
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

    public void BTN_toggleIntertiorScene()
    {
        toggleInteriorSceneButton.interactable = false;
        Action toggleInteriorAndTriggerEvent = () =>
        {
            mySceneCatalogue.toggleInteriorScene();
            myVictoryCoach.checkQuestsCompleteAndQueueEvent(new EventSceneChange());
        };
        myVictoryCoach.checkVictoryQuestCompleteAndDelayActionIfCharactersPresent(toggleInteriorAndTriggerEvent);
    }

    public void BTN_leaveDate()
    {
        myRelationshipCounselor.leaveDate();
    }

    public void BTN_dateAction()
    {
        myRelationshipCounselor.act();
    }

    public void BTN_characterClicked(int portraitNumber)
    {
        Character clickedCharacter = myDialogueManager.getPartnerAt(portraitNumber);
        if (clickedCharacter != null)
        {
            myDialogueManager.selectedPartner = portraitNumber - 1;
            myUIManager.talkButtonObject.GetComponentInChildren<Text>().text = "Talk to " + clickedCharacter.givenName;
        }
    }
}
