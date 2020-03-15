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
		characterToSummon.isPresent = true;
		characterToSummon.locations[timeOfDay].locationName = GameObject.FindObjectOfType<SceneCatalogue>().getCurrentSceneName();
		characterToSummon.locations[timeOfDay].isInside = GameObject.FindObjectOfType<SceneCatalogue>().getIsInInteriorScene();
		characterToSummon.locations[timeOfDay].isActive = true;

		GameObject.FindObjectOfType<AnimationMaestro>().updatePotentialPartnersSprites(GameObject.FindObjectOfType<DialogueManager>().getAllCurrentLocalPresentConversationPartners());
		
		if (actorCount == 2)
		{
		    GameObject.FindObjectOfType<AnimationMaestro>().setImageColor(GameObject.Find("Character 2 Portrait").GetComponent<Image>(), new Color(255,255,255,1));
		}else if (actorCount == 3)
		{
		    GameObject.FindObjectOfType<AnimationMaestro>().setImageColor(GameObject.Find("Character 3 Portrait").GetComponent<Image>(), new Color(255,255,255,1));
		}

		GameObject.FindObjectOfType<AnimationMaestro>().fadeInCharacters(new List<Character>() {characterToSummon}, 0.75f);

		GameObject.FindObjectOfType<AnimationMaestro>().writeDescriptionText(textToWrite, GameObject.Find("TextPanel").GetComponentInChildren<Text>());
	}

}
