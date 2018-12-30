using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDialogueCommand : AbstractCommand {

	public string textToWrite;

	public override void execute()
	{
    	GameObject.FindObjectOfType<AnimationMaestro>().setDescriptionText(textToWrite, GameObject.Find("TextPanel").GetComponentInChildren<Text>());
	}
   
}
