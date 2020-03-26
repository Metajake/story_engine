using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveCharacterCommand : ICommand {

	public string characterToRemove;
	public string textToWrite;
	public float fadeDuration;
	private AnimationMaestro myAnimationMaestro;
	private DialogueManager myDialogueManager;
	private Timelord myTimeLord;

	public RemoveCharacterCommand(string characterName, string stringArg, float durationToFade)
	{
		myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
		myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
		myTimeLord = GameObject.FindObjectOfType<Timelord>();
		characterToRemove = characterName;
		textToWrite = stringArg;
		fadeDuration = durationToFade;
	}

	public void execute(bool toFastForward)
	{
		Action setCharacterPresentFalse = () =>
		{
			myDialogueManager.getCharacterForName(characterToRemove).isPresent = false;
			myDialogueManager.getCharacterForName(characterToRemove).returnTime = myTimeLord.getCurrentTimestep() + 1;
		};

		List<Character> charList = GameObject.FindObjectOfType<DialogueManager>().getAllCurrentLocalPresentConversationPartners();

		for (int i = 0; i < charList.Count; i++)
		{
			if (charList[i].givenName.ToLower() == characterToRemove.ToLower())
			{
				if (!toFastForward)
				{
					myAnimationMaestro.fadeOutCharacterImage(i + 1, fadeDuration);
					myAnimationMaestro.StartCoroutine(myAnimationMaestro.delayGameCoroutine(fadeDuration, setCharacterPresentFalse));
				}
				else
				{
					myAnimationMaestro.setImageColor(GameObject.Find("Character " + (i + 1) + " Portrait").GetComponent<Image>(), new Color(255, 255, 255, 0));
					myDialogueManager.getCharacterForName(characterToRemove).isPresent = false;
					myDialogueManager.getCharacterForName(characterToRemove).returnTime = myTimeLord.getCurrentTimestep() + 1;
				}
			}
		}
		myAnimationMaestro.writeDescriptionText(textToWrite, GameObject.Find("TextPanel").GetComponentInChildren<Text>());
	}

}
