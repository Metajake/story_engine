using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonCharacterCommand : ICommand {

	public Character characterToSummon;
	public string textToWrite;

	public SummonCharacterCommand(Character character, string stringArg = "")
	{
		characterToSummon = character;
		textToWrite = stringArg;
	}

	public void execute()
	{
		int timeOfDay = GameObject.FindObjectOfType<Timelord>().getCurrentModulusTimestep();

		characterToSummon.locations[timeOfDay].locationName = GameObject.FindObjectOfType<SceneCatalogue>().getCurrentSceneName();
		characterToSummon.locations[timeOfDay].isInside = GameObject.FindObjectOfType<SceneCatalogue>().getIsInInteriorScene();
		characterToSummon.locations[timeOfDay].isActive = true;

		GameObject.FindObjectOfType<AnimationMaestro>().fadeInCharacters(new List<Character>() {characterToSummon});

		GameObject.FindObjectOfType<AnimationMaestro>().writeDescriptionText(textToWrite, GameObject.Find("TextPanel").GetComponentInChildren<Text>());
	}

}
