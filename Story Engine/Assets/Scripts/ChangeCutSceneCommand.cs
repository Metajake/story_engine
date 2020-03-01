using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCutSceneCommand : AbstractCommand {

	public string textToWrite;

	public override void execute()
	{
		GameObject.FindObjectOfType<UIManager>().cutSceneTextToWrite = textToWrite;
	}
   
}
