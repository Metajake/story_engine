using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor : MonoBehaviour, ICommandProcessor {

	Queue<ICommand> commandList;

	public bool isInSequence(){
		return commandList.Count > 0;
	}
    
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
        executeNextCommand();
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

    public void createAndEnqueueListOfChangeDialogueCommands(List<string> dialogues)
    {
        foreach(string dialogue in dialogues)
        {
            createAndEnqueueChangeDialogueCommand(dialogue);
        }
    }

    public void doSequence(List<ICommand> commandSequence)
    {
        foreach(ICommand command in commandSequence)
        {
            commandList.Enqueue(command);
        }
        executeNextCommand();
    }

    public void createAndExecuteChangeDialogueSequence(List<string> sequence)
    {
        List<ICommand> commands = new List<ICommand>();

        foreach (string item in sequence) {
            commands.Add(createChangeDialogueCommand(item));
        }
        commands.Add(new SequenceEndCommand());
        this.doSequence(commands);
    }
}
