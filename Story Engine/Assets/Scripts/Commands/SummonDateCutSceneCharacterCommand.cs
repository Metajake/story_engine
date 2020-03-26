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

	public void execute(bool toFastForward)
	{
		GameObject.FindObjectOfType<CommandBuilder>().dateCutSceneCharList.Add(characterToSummon);

		List<Character> charList = GameObject.FindObjectOfType<CommandBuilder>().dateCutSceneCharList;

		// For each character in present characters, if character is currently being summoned, fade in, otherwise snap in.
		//TODO: Maybe get rid of this for DateCutScene because charList doesn't rearrange like dialogueManager.getAllLocalCurrentEtc...
		for (int i = 0; i < charList.Count; i++)
		{
			if(charList[i].givenName == characterToSummon.givenName)
			{
				myAnimationMaestro.fadeInCharacterImage(i + 1, toFastForward ? 0.0f : fadeDuration);
			}
			else
			{
				myAnimationMaestro.setImageColor(GameObject.Find("Character " + (i+1) + " Portrait").GetComponent<Image>(), new Color(255, 255, 255, 1));
			}
		}

		myAnimationMaestro.writeDescriptionText(textToWrite, GameObject.Find("TextPanel").GetComponentInChildren<Text>());
	}

}
