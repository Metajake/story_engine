using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor : MonoBehaviour {

	Queue<AbstractCommand> commandList;
	bool hasExecutedFirstCommands;

	public bool isInSequence(){
		return commandList.Count > 0;
	}
    
	public void executeNextCommand(){
		commandList.Dequeue().execute();
	}

	// Use this for initialization
	void Start ()
	{
		commandList = new Queue<AbstractCommand>();
		populateCommands();
		hasExecutedFirstCommands = false;
	}

	// Update is called once per frame
	void Update () {
		if(!hasExecutedFirstCommands){
			executeNextCommand();
			hasExecutedFirstCommands = true;
		}
	}

    private void populateCommands()
    {
        ChangeDialogueCommand command001 = this.gameObject.AddComponent<ChangeDialogueCommand>();
        command001.textToWrite = "Sitting at home is safe and comfortable...";
        commandList.Enqueue(command001);

        ChangeDialogueCommand command002 = this.gameObject.AddComponent<ChangeDialogueCommand>();
		command002.textToWrite = "...But I need to grow up.";
        commandList.Enqueue(command002);

		ChangeDialogueCommand command003 = this.gameObject.AddComponent<ChangeDialogueCommand>();
        command003.textToWrite = "... It's time to find a girlfriend. Things will change if I start dating.";
        commandList.Enqueue(command003);

		ChangeDialogueCommand command004 = this.gameObject.AddComponent<ChangeDialogueCommand>();
        command004.textToWrite = "......";
        commandList.Enqueue(command004);

		ChangeDialogueCommand command005 = this.gameObject.AddComponent<ChangeDialogueCommand>();
		command005.textToWrite = "The problem with that is: the more time I spend with a girl, the more chance there is of her falling in love with me...";
        commandList.Enqueue(command005);

		ChangeDialogueCommand command006 = this.gameObject.AddComponent<ChangeDialogueCommand>();
		command006.textToWrite = "...But once that happens, it never works out for one reason or another, and I'm left heartbroken, crushed, and defeated. Life feels like a game over.";
        commandList.Enqueue(command006);

		ChangeDialogueCommand command007 = this.gameObject.AddComponent<ChangeDialogueCommand>();
		command007.textToWrite = "I have to get out there, and have some new experiences without letting anyone fall in love with me. ... At least not until I've experienced everything that life has to offer. Let's go!";
        commandList.Enqueue(command007);
    }

}
