using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonCharacterCommand : ICommand {

	public Character characterToSummon;
	public string textToWrite;
	public int actorCount;
	private AnimationMaestro myAnimationMaestro;

	public SummonCharacterCommand(Character character, int actorCountToAssign, string stringArg)
	{
		myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
		characterToSummon = character;
		actorCount = actorCountToAssign; 
		textToWrite = stringArg;
	}

	public void execute()
	{

		int timeOfDay = GameObject.FindObjectOfType<Timelord>().getCurrentModulusTimestep();
		characterToSummon.isPresent = true;
		characterToSummon.locations[timeOfDay].locationName = GameObject.FindObjectOfType<SceneCatalogue>().getCurrentSceneName();
		characterToSummon.locations[timeOfDay].isInside = GameObject.FindObjectOfType<SceneCatalogue>().getIsInInteriorScene();
		characterToSummon.locations[timeOfDay].isActive = true;

		List<Character> charList = GameObject.FindObjectOfType<DialogueManager>().getAllCurrentLocalPresentConversationPartners();
		
		for (int i = 0; i < charList.Count; i++)
		{
			if(charList[i].givenName == characterToSummon.givenName)
			{
				myAnimationMaestro.fadeInCharacterImage(i + 1, 0.75f);
			}
			else
			{
				myAnimationMaestro.setImageColor(GameObject.Find("Character " + (i+1) + " Portrait").GetComponent<Image>(), new Color(255, 255, 255, 1));
			}
		}

		myAnimationMaestro.writeDescriptionText(textToWrite, GameObject.Find("TextPanel").GetComponentInChildren<Text>());
	}

}
