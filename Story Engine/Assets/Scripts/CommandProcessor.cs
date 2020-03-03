using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor : MonoBehaviour, ICommandProcessor {

    private GameState myGameState;

	Queue<ICommand> commandList;
    
	public void executeNextCommand(){
        if (commandList.Count > 0)
        {
            commandList.Dequeue().execute();
        }
		
	}

	void Awake ()
	{
		commandList = new Queue<ICommand>();
	}

    void Start()
    {
        myGameState = GameObject.FindObjectOfType<GameState>();
    }

    void Update () {

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

    public void createAndEnqueueChangeDialogueSequence(List<string> dialogues)
    {
        myGameState.currentGameState = GameState.gameStates.COMMANDSEQUENCE;
        foreach (string dialogue in dialogues)
        {
            commandList.Enqueue(createChangeDialogueCommand(dialogue));
        }
        this.commandList.Enqueue(new SequenceEndCommand(GameState.gameStates.PROWL));
        executeNextCommand();
    }

    internal void createAndEnqueueDateCutSceneSequence(List<string> sceneCuts, bool isFinalCutScene)
    {
        myGameState.currentGameState = GameState.gameStates.CUTSCENE;
        foreach (string cut in sceneCuts)
        {
            commandList.Enqueue(createCutSceneCommand(cut));
        }

        if (isFinalCutScene)
        {
            this.commandList.Enqueue(new GameClearCommand());
        }
        else
        {
            this.commandList.Enqueue(new SequenceEndCommand(GameState.gameStates.DATEOUTRO));
        }

        executeNextCommand();
    }
}
