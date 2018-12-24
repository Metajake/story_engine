using System.Collections;
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

    public void createAndEnqueueChangeDialogueSequence(List<string> dialogues)
    {
        myGameState.currentGameState = GameState.gameStates.COMMANDSEQUENCE;
        foreach (string dialogue in dialogues)
        {
            commandList.Enqueue(createChangeDialogueCommand(dialogue));
        }
        this.commandList.Enqueue(new SequenceEndCommand());
        executeNextCommand();
    }
}
