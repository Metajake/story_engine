using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TestTaker : MonoBehaviour {

	private Button runTestButton;
	private Button runConversationTestButton;
	private Text testResultsText;

	private SceneCatalogue mySceneCatalogue;
	private DialogueManager myDialogueManager;
	private ConversationTracker myConversationTracker;
	private GameState myGameState;
	private AnimationMaestro myAnimationMaestro;
	private EventQueue myEventQueue;

	public bool takingTest;
	private List<Location> allLocations;
	private List<DateableCharacter> allDateableCharacters;
	private TestEventSubscriber eventSubscriber;

	void Awake () {
		mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
		myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
		myConversationTracker = GameObject.FindObjectOfType<ConversationTracker>();
		myGameState = GameObject.FindObjectOfType<GameState>();
		myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
		myEventQueue = GameObject.FindObjectOfType<EventQueue>();

		runTestButton = GameObject.Find("RunTestButton").GetComponent<Button>();
		runConversationTestButton = GameObject.Find("ConversationButton").GetComponent<Button>();
		testResultsText = GameObject.Find("TestResults").GetComponent<Text>();

	}

	void Start()
	{
		runTestButton.onClick.AddListener(BTN_runTest);
		runConversationTestButton.onClick.AddListener(BTN_runConversationTest);
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
		DateableCharacter randomDateableChar = allDateableCharacters[new System.Random().Next(0, allDateableCharacters.Count -1)];
		myDialogueManager.charactersPresent.Add(randomDateableChar);
		myDialogueManager.selectedPartner = 0;

		testResultsText.text = "";
		testResultsText.text += "Random Character: " + randomDateableChar.givenName + "\n";
		myConversationTracker.beginConversation(randomDateableChar);
		testResultsText.text += "Current Reputation in this Conversation : " + myConversationTracker.currentConversation.speaker.reputation + "\n";
	}


	private void CheckEvents()
	{
		eventSubscriber = new TestEventSubscriber(testResultsText);
		myEventQueue.subscribe(eventSubscriber);
		myEventQueue.queueEvent(new SceneChangeEvent());
		myEventQueue.queueEvent(new TimeChangeEvent());
		myEventQueue.queueEvent(new DateActionEvent());
		//TODO MAKE THIS EVENT CLASS NAME CONSISTANT WITH THE REST OF THE EVENTS
		myEventQueue.queueEvent(new EventDateStart());
	}

	private void CheckStates()
	{
		foreach (Enum  value in Enum.GetValues(typeof(GameState.gameStates)))
		{
			testResultsText.text += "State: " + value.ToString() + " ok.\n";

		}
	}

}

public class TestEventSubscriber : MonoBehaviour, IEventSubscriber
{
	private Text toWriteTo;
	public TestEventSubscriber(Text toWriteTo){this.toWriteTo = toWriteTo;}
	public void eventOccured(IGameEvent occurringEvent){this.toWriteTo.text += "Event: " + occurringEvent.getEventType() + " ok.\n";}
}