using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonDateCutSceneCharacterCommand : ICommand {

	public Character characterToSummon;
	public string textToWrite;
	public float fadeDuration;
	private AnimationMaestro myAnimationMaestro;

	public SummonDateCutSceneCharacterCommand(Character character, string stringArg, float durationToFade = 0.6f)
	{
		myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
		characterToSummon = character;
		textToWrite = stringArg;
		fadeDuration = durationToFade;
	}

	public void execute()
	{
		GameObject.FindObjectOfType<CommandBuilder>().dateCutSceneCharList.Add(characterToSummon);

		int timeOfDay = GameObject.FindObjectOfType<Timelord>().getCurrentModulusTimestep();
		characterToSummon.isPresent = true;
		characterToSummon.locations[timeOfDay].locationName = GameObject.FindObjectOfType<SceneCatalogue>().getCurrentSceneName();
		characterToSummon.locations[timeOfDay].isInside = GameObject.FindObjectOfType<SceneCatalogue>().getIsInInteriorScene();
		characterToSummon.locations[timeOfDay].isActive = true;

		List<Character> charList = GameObject.FindObjectOfType<CommandBuilder>().dateCutSceneCharList;

		// For each character in present characters, if character is currently being summoned, fade in, otherwise snap in.
		for (int i = 0; i < charList.Count; i++)
		{
			if(charList[i].givenName == characterToSummon.givenName)
			{
				myAnimationMaestro.fadeInCharacterImage(i + 1, fadeDuration);
			}
			else
			{
				myAnimationMaestro.setImageColor(GameObject.Find("Character " + (i+1) + " Portrait").GetComponent<Image>(), new Color(255, 255, 255, 1));
			}
		}

		myAnimationMaestro.writeDescriptionText(textToWrite, GameObject.Find("TextPanel").GetComponentInChildren<Text>());
	}

}
