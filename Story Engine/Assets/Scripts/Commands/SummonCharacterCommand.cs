using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonCharacterCommand : ICommand {

	public Character characterToSummon;
	public string textToWrite;
	public int actorCount;

	public SummonCharacterCommand(Character character, int actorCountToAssign, string stringArg)
	{
		characterToSummon = character;
		actorCount = actorCountToAssign; 
		textToWrite = stringArg;
	}

	public void execute()
	{

		int timeOfDay = GameObject.FindObjectOfType<Timelord>().getCurrentModulusTimestep();
		characterToSummon.locations[timeOfDay].locationName = GameObject.FindObjectOfType<SceneCatalogue>().getCurrentSceneName();
		characterToSummon.locations[timeOfDay].isInside = GameObject.FindObjectOfType<SceneCatalogue>().getIsInInteriorScene();
		characterToSummon.locations[timeOfDay].isActive = true;

		if(actorCount == 2)
		{
		    GameObject.FindObjectOfType<AnimationMaestro>().fadeInCharacterImage(2, 0.0f);
		}

		GameObject.FindObjectOfType<AnimationMaestro>().fadeInCharacters(new List<Character>() {characterToSummon}, 0.6f);

		GameObject.FindObjectOfType<AnimationMaestro>().writeDescriptionText(textToWrite, GameObject.Find("TextPanel").GetComponentInChildren<Text>());
	}

}
