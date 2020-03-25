using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuilder : MonoBehaviour {
	Queue<ICommand> commandList;
	private GameState myGameState;
	private CommandProcessor myCommandProcessor;
	public List<Character> dateCutSceneCharList;

	void Awake()
	{
		commandList = new Queue<ICommand>();
	}

	void Start()
	{
		myGameState = GameObject.FindObjectOfType<GameState>();
		myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();
	}

	public void createAndEnqueueChangeDialogueSequence(List<string> dialogues, bool isEndGameCutscene = false)
	{
		foreach (string dialogue in dialogues)
		{
			commandList.Enqueue(createChangeDialogueCommand(dialogue));
		}

		if (isEndGameCutscene)
		{
			this.commandList.Enqueue(new GameClearCommand());
		}
	}

	internal void createAndEnqueueSummonCharacterSequence(Character characterToEnqueue, string stringToWrite="", float fadeDurationToWrite=0.75f)
	{
		commandList.Enqueue(new SummonCharacterCommand(characterToEnqueue, stringToWrite, fadeDurationToWrite));
	}

	internal void createAndEnqueueRemoveCharacterSequence(string charNameToRemove, string stringToWrite="", float fadeDurationToWrite=0.75f)
	{
		commandList.Enqueue(new RemoveCharacterCommand(charNameToRemove, stringToWrite, fadeDurationToWrite));
	}

	internal void createAndEnqueueSummonDateCutSceneCharacterSequence(Character characterToEnqueue, string stringToWrite = "", float fadeDurationToWrite = 0.75f)
	{
		commandList.Enqueue(new SummonDateCutSceneCharacterCommand(characterToEnqueue, stringToWrite, fadeDurationToWrite));
	}

	private ChangeDialogueCommand createChangeDialogueCommand(string dialogue)
	{
		ChangeDialogueCommand command = this.gameObject.AddComponent<ChangeDialogueCommand>();
		command.textToWrite = dialogue;
		return command;
	}

	public void build(GameState.gameStates stateToBeginIn = GameState.gameStates.COMMANDSEQUENCE, GameState.gameStates stateToEndIn = GameState.gameStates.PROWL)
	{
		myGameState.currentGameState = stateToBeginIn;
		this.commandList.Enqueue(new SequenceEndCommand(stateToEndIn));
		myCommandProcessor.setQueue(this.commandList);
		myCommandProcessor.executeNextCommand();
	}
}
