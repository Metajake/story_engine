using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeepCharacterInDateCutSceneCommand : ICommand {

	public Character characterToSummon;
	public string textToWrite;
	public float fadeDuration;
	private AnimationMaestro myAnimationMaestro;

	public KeepCharacterInDateCutSceneCommand(Character character, string stringArg)
	{
		myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
		characterToSummon = character;
		textToWrite = stringArg;
	}

	public void execute(bool toFastForward)
	{
		GameObject.FindObjectOfType<CommandBuilder>().dateCutSceneCharList.Add(characterToSummon);

		myAnimationMaestro.writeDescriptionText(textToWrite, GameObject.Find("TextPanel").GetComponentInChildren<Text>());
	}

}
