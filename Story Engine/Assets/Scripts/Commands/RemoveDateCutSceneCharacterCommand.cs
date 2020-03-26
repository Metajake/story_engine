using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveDateCutSceneCharacterCommand : ICommand {

	public string characterToRemove;
	public string textToWrite;
	public float fadeDuration;
	private AnimationMaestro myAnimationMaestro;

	public RemoveDateCutSceneCharacterCommand(string characterName, string stringArg, float durationToFade = 0.6f)
	{
		myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
		characterToRemove = characterName;
		textToWrite = stringArg;
		fadeDuration = durationToFade;
	}

	public void execute(bool toFastForward)
	{

		List<Character> charList = GameObject.FindObjectOfType<CommandBuilder>().dateCutSceneCharList;
		int charListIndexToRemove = 0;
		Action removeCharFromCharList = () =>
		{
			GameObject.FindObjectOfType<CommandBuilder>().dateCutSceneCharList.RemoveAt(charListIndexToRemove);
			//If three char's in date cut scene and 2nd character leaves, 3rd character re-sorts to 2nd position, but doesn't have alpha
			for (int i = 0; i < GameObject.FindObjectOfType<CommandBuilder>().dateCutSceneCharList.Count; i++) {
				myAnimationMaestro.setImageColor(GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>(), new Color(255, 255, 255, 1));
			}
		};

		// For each character in present characters, if character is currently being summoned, fade in, otherwise snap in.
		//TODO: Maybe get rid of this for DateCutScene because charList doesn't rearrange like dialogueManager.getAllLocalCurrentEtc...
		for (int i = 0; i < charList.Count; i++)
		{
			if(charList[i].givenName.ToLower() == characterToRemove.ToLower())
			{
				charListIndexToRemove = i;
				if (!toFastForward)
				{
					myAnimationMaestro.fadeOutCharacterImage(i + 1, fadeDuration);
					myAnimationMaestro.StartCoroutine(myAnimationMaestro.delayGameCoroutine(fadeDuration, removeCharFromCharList));
				}
				else
				{
					myAnimationMaestro.setImageColor(GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>(), new Color(255, 255, 255, 0));
					GameObject.FindObjectOfType<CommandBuilder>().dateCutSceneCharList.RemoveAt(charListIndexToRemove);
				}
			}
		}

		myAnimationMaestro.writeDescriptionText(textToWrite, GameObject.Find("TextPanel").GetComponentInChildren<Text>());
	}

}
