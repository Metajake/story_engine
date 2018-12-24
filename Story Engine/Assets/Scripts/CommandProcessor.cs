using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor : MonoBehaviour, ICommandProcessor {

	Queue<ICommand> commandList;
    
	public void executeNextCommand(){
        if (commandList.Count > 0)
        {
            commandList.Dequeue().execute();
        }
		
	}

    public void executeAllCommands()
    {
        while(commandList.Count > 0)
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
        
    }

    void Update () {

	}

    public void createAndEnqueueChangeDialogueCommand(string dialogue)
    {
        ChangeDialogueCommand command = createChangeDialogueCommand(dialogue);
        commandList.Enqueue(command);
    }

    private ChangeDialogueCommand createChangeDialogueCommand(string dialogue)
    {
        ChangeDialogueCommand command = this.gameObject.AddComponent<ChangeDialogueCommand>();
        command.textToWrite = dialogue;
        return command;
    }

    public void createAndEnqueueChangeDialogueSequence(List<string> dialogues)
    {
        foreach(string dialogue in dialogues)
        {
            createAndEnqueueChangeDialogueCommand(dialogue);
        }
        this.commandList.Enqueue(new SequenceEndCommand());
        executeNextCommand();
    }

    public void doSequence(List<ICommand> commandSequence)
    {
        foreach(ICommand command in commandSequence)
        {
            commandList.Enqueue(command);
        }
        executeAllCommands();
    }
}
