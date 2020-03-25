using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCharacterToDateCutSceneCharListCommand : ICommand {

	public Character characterToSummon;
	public string textToWrite;
	public float fadeDuration;
	private AnimationMaestro myAnimationMaestro;

	public AddCharacterToDateCutSceneCharListCommand(Character character, string stringArg)
	{
		myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
		characterToSummon = character;
		textToWrite = stringArg;
	}

	public void execute()
	{
		GameObject.FindObjectOfType<CommandBuilder>().dateCutSceneCharList.Add(characterToSummon);

		int timeOfDay = GameObject.FindObjectOfType<Timelord>().getCurrentModulusTimestep();
		characterToSummon.isPresent = true;
		characterToSummon.locations[timeOfDay].locationName = GameObject.FindObjectOfType<SceneCatalogue>().getCurrentSceneName();
		characterToSummon.locations[timeOfDay].isInside = GameObject.FindObjectOfType<SceneCatalogue>().getIsInInteriorScene();
		characterToSummon.locations[timeOfDay].isActive = true;

		myAnimationMaestro.writeDescriptionText(textToWrite, GameObject.Find("TextPanel").GetComponentInChildren<Text>());
	}

}
