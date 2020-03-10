using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBuilder : MonoBehaviour {
	Queue<ICommand> commandList;
	private GameState myGameState;
	private CommandProcessor myCommandProcessor;

	void Awake()
	{
		commandList = new Queue<ICommand>();
	}

	void Start()
	{
		myGameState = GameObject.FindObjectOfType<GameState>();
		myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();
	}

	public void createAndEnqueueChangeDialogueSequence(List<string> dialogues)
	{
		foreach (string dialogue in dialogues)
		{
			commandList.Enqueue(createChangeDialogueCommand(dialogue));
		}
	}

	internal void createAndEnqueueDateCutSceneSequence(List<string> sceneCuts, bool isEndGameCutscene)
	{
		foreach (string cut in sceneCuts)
		{
			commandList.Enqueue(createCutSceneCommand(cut));
		}

		if (isEndGameCutscene)
		{
			this.commandList.Enqueue(new GameClearCommand());
		}
	}

	internal void createAndEnqueueSummonCharacterSequence(Character characterToEnqueue, string stringToWrite)
	{
		commandList.Enqueue(new SummonCharacterCommand(characterToEnqueue, stringToWrite));
	}

	private ChangeDialogueCommand createChangeDialogueCommand(string dialogue)
	{
		ChangeDialogueCommand command = this.gameObject.AddComponent<ChangeDialogueCommand>();
		command.textToWrite = dialogue;
		return command;
	}

	private ChangeCutSceneCommand createCutSceneCommand(string cut)
	{
		ChangeCutSceneCommand command = this.gameObject.AddComponent<ChangeCutSceneCommand>();
		command.textToWrite = cut;
		return command;
	}

	public void build(GameState.gameStates stateToBuildIn = GameState.gameStates.COMMANDSEQUENCE)
	{
		myGameState.currentGameState = stateToBuildIn;
		this.commandList.Enqueue(new SequenceEndCommand(stateToBuildIn == GameState.gameStates.COMMANDSEQUENCE ? GameState.gameStates.PROWL : GameState.gameStates.DATEOUTRO));
		myCommandProcessor.setQueue(this.commandList);
		myCommandProcessor.executeNextCommand();
	}
}
