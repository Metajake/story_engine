using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDialogueCommand : AbstractCommand {

	public string textToWrite;

	public override void execute()
	{
    	GameObject.FindObjectOfType<UIManager>().setDescriptionText(textToWrite);
	}
   
}
