using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDialogueCommand : AbstractCommand {

	public string textToWrite;

	public override void execute(bool toFastForward)
	{
    	GameObject.FindObjectOfType<AnimationMaestro>().writeDescriptionText(textToWrite, GameObject.Find("TextPanel").GetComponentInChildren<Text>());
	}
}
