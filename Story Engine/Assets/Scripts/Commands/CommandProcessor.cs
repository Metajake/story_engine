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

	void Awake ()
	{
		commandList = new Queue<ICommand>();
	}

	public void setQueue(Queue<ICommand> commandsToQueue)
	{
		this.commandList = commandsToQueue;
	}
}
