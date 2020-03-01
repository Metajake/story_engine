﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDriver : MonoBehaviour, IEventSubscriber {
	private EventQueue myEventQueue;
	private Button timeAdvanceButton;
	private Button toggleInteriorSceneButton;
	private DialogueManager myDialogueManager;
	private Timelord myTimelord;
	private AnimationMaestro myAnimationMaestro;
	private SceneCatalogue mySceneCatalogue;
	private UIManager myUIManager;
	private RelationshipCounselor myRelationshipCounselor;

	void Awake()
	{
		mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
	}

	// Use this for initialization
	void Start() {
		myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
		myTimelord = GameObject.FindObjectOfType<Timelord>();
		myAnimationMaestro = GameObject.FindObjectOfType<AnimationMaestro>();
		myUIManager = GameObject.FindObjectOfType<UIManager>();
		myRelationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
		myEventQueue = GameObject.FindObjectOfType<EventQueue>();

		timeAdvanceButton = GameObject.Find("TimeButton").GetComponent<Button>();
		toggleInteriorSceneButton = GameObject.Find("DateLocationButton").GetComponent<Button>();

		myEventQueue.subscribe(this);
	}

	public void eventOccured(IGameEvent occurringEvent)
	{
		Debug.Log("Driver " + occurringEvent.getEventType());
		if (occurringEvent.getEventType() == "TIMEEVENT")
		{
			foreach (DateableCharacter character in myDialogueManager.allDateableCharacters)
			{
				character.checkAndSetReturnToPresent(myTimelord.getCurrentTimestep());
			}

			myAnimationMaestro.fadeInCharacters(myDialogueManager.getAllCurrentLocalPresentConversationPartners());

			this.checkIfCharactersAndFadeInButtonUI(this.ActivateAdvanceTimeButton);
		}
		else if (occurringEvent.getEventType() == "LOCATIONEVENT")
		{
			myAnimationMaestro.fadeInCharacters(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
			myDialogueManager.selectedPartner = -1;
			this.updateToggleInteriorButtonUI();
			this.checkIfCharactersAndFadeInButtonUI(this.ActivateToggleInteriorSceneButton);
		}
		else if (occurringEvent.getEventType() == "DATESTARTEVENT")
		{
			myAnimationMaestro.fadeInCharacters(new List<Character>() {
				myRelationshipCounselor.getDatePartner(mySceneCatalogue.getCurrentLocation(), myTimelord.getCurrentTimestep())
			});
			//TODO Why is the following line there?
			myAnimationMaestro.fadeInCharacters(myDialogueManager.getAllCurrentLocalPresentConversationPartners());
		}
		else if (occurringEvent.getEventType() == "DATEACTIONEVENT")
		{
			mySceneCatalogue.getCurrentLocation().setRandomDateAction();
		}
	}

	private void checkIfCharactersAndFadeInButtonUI(Action buttonActivationFunction)
	{
		if (myDialogueManager.getAllCurrentLocalPresentConversationPartners().Count > 0)
		{
			StartCoroutine(myAnimationMaestro.delayGameCoroutine(0.6f, buttonActivationFunction));
		}
		else
		{
			buttonActivationFunction();
		}
	}

	private void updateToggleInteriorButtonUI()
	{
		if (!mySceneCatalogue.getIsInInteriorScene())
		{
			myUIManager.dateLocationButton.GetComponentInChildren<Text>().text = "Enter " + mySceneCatalogue.getCurrentLocation().interiorName;
			myUIManager.dateLocationButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/icon_enter");
		}
		else
		{
			myUIManager.dateLocationButton.GetComponentInChildren<Text>().text = "Exit " + mySceneCatalogue.getCurrentLocation().interiorName;
			myUIManager.dateLocationButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/icon_exit");
		}
	}
	public void ActivateToggleInteriorSceneButton()
	{
		toggleInteriorSceneButton.interactable = true;
	}

	public void ActivateAdvanceTimeButton()
	{
		timeAdvanceButton.interactable = true;
	}
}
