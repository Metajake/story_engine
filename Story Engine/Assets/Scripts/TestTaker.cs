using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TestTaker : MonoBehaviour, IEventSubscriber {

	private SceneCatalogue mySceneCatalogue;
	private DialogueManager myDialogueManager;
	private ConversationTracker myConversationTracker;
	//private GameState myGameState;
	//private AnimationMaestro myAnimationMaestro;
	private EventQueue myEventQueue;
	private MapCartographer myMapCartographer;
	private Timelord myTimeLord;
	private Button runTestButton;
	private Button runConversationTestButton;
	private Button locationCheckButton;
	private Text testResultsText;
	public bool takingTest;
	private System.Random rn;

	private List<Location> allLocations;
	private List<DateableCharacter> allDateableCharacters;

	void Awake () {
		mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
		myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
		myConversationTracker = GameObject.FindObjectOfType<ConversationTracker>();
		//myGameState = GameObject.FindObjectOfType<GameState>();
		//myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
		myEventQueue = GameObject.FindObjectOfType<EventQueue>();
		myMapCartographer = GameObject.FindObjectOfType<MapCartographer>();
		myTimeLord = GameObject.FindObjectOfType<Timelord>();

		runTestButton = GameObject.Find("RunTestButton").GetComponent<Button>();
		runConversationTestButton = GameObject.Find("ConversationButton").GetComponent<Button>();
		locationCheckButton = GameObject.Find("LocationCheck").GetComponent<Button>();
		testResultsText = GameObject.Find("TestResults").GetComponent<Text>();
	}

	void Start()
	{
		rn = new System.Random();

		myEventQueue.subscribe(this);

		runTestButton.onClick.AddListener(BTN_runTest);
		runConversationTestButton.onClick.AddListener(BTN_runConversationTest);
		locationCheckButton.onClick.AddListener(BTN_timeStepLocationCheck);
	}

	private void BTN_runTest()
	{
		allLocations = mySceneCatalogue.locations;
		allDateableCharacters = myDialogueManager.allDateableCharacters;

		testResultsText.text = "";
		testResultsText.text += "Test Initiated\n";
		testResultsText.text += "Location Count: "+ allLocations.Count + "\n";
		testResultsText.text += "Dateable Character Count: " + allDateableCharacters.Count + "\n";

		CheckStates();
		CheckEvents();
	}


	private void BTN_runConversationTest()
	{
		allDateableCharacters = myDialogueManager.allDateableCharacters;
		DateableCharacter randomDateableChar = allDateableCharacters[rn.Next(allDateableCharacters.Count)];
		myDialogueManager.charactersPresent.Add(randomDateableChar);
		myDialogueManager.selectedPartner = 0;

		testResultsText.text = "";
		testResultsText.text += "Random Character: " + randomDateableChar.givenName + "\n";
		myConversationTracker.beginConversation(randomDateableChar);
		testResultsText.text += "Current Reputation in this Conversation : " + myConversationTracker.currentConversation.speaker.reputation + "\n";
	}

	private void BTN_timeStepLocationCheck()
	{
		testResultsText.text = "";
		myTimeLord.timeStep++;
		testResultsText.text += "Time of Day : " + myTimeLord.getCurrentModulusTimestep() + "\n";

		allLocations = mySceneCatalogue.locations;
		myMapCartographer.changeScene(rn.Next(allLocations.Count));
		myEventQueue.queueEvent(new SceneChangeEvent());
		testResultsText.text += "Current Scene : " + mySceneCatalogue.getCurrentLocation().locationName + "\n";

		List<Character> allLocalChars = myDialogueManager.getAllCurrentLocalPresentConversationPartners();
		foreach(Character ch in allLocalChars)
			{
				testResultsText.text += "Present Character Ext: " + ch.givenName + "\n";
			}
	}

	private void CheckEvents()
	{
		
		myEventQueue.queueEvent(new SceneChangeEvent());
		myEventQueue.queueEvent(new TimeChangeEvent());
		myEventQueue.queueEvent(new DateActionEvent());
		//TODO MAKE THIS EVENT CLASS NAME CONSISTANT WITH THE REST OF THE EVENTS
		myEventQueue.queueEvent(new EventDateStart());
		//Destroy(eventSubscriber);
	}

	private void CheckStates()
	{
		foreach (Enum  value in Enum.GetValues(typeof(GameState.gameStates)))
		{
			testResultsText.text += "State: " + value.ToString() + " ok.\n";

		}
	}

	public void eventOccured(IGameEvent occurringEvent)
	{
		//testResultsText.text += "Event: " + occurringEvent.getEventType() + " ok.\n";
		if (occurringEvent.getEventType() == "LOCATIONEVENT")
		{
			testResultsText.text += "Test Location Event Occurred.\n";
			myDialogueManager.selectedPartner = -1;
			testResultsText.text += "Dialogue Manager selected partner returned to: " + myDialogueManager.selectedPartner + "\n";
		}
	}

}