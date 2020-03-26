using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor : MonoBehaviour, ICommandProcessor {
	Queue<ICommand> commandList;

	public void goToSequenceEnd()
	{
		Debug.Log(this.commandList.Count);
		int stepsRematining = this.commandList.Count;
		for(int i = 0; i < stepsRematining - 1; i++)
		{
			this.executeNextCommand(toFastForward: true);
		}
	}

	public void executeNextCommand(bool toFastForward = false){
        if (commandList.Count > 0)
        {
            commandList.Dequeue().execute(toFastForward);
        }
		
	}

	void Awake ()
	{
		commandList = new Queue<ICommand>();
	}

	public void setQueue(Queue<ICommand> commandsToQueue)
	{
		this.commandList = commandsToQueue;
	}
}
