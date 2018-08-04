using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationTracker : MonoBehaviour {

	public Conversation currentConversation;
	private List<Conversation> pastConversations;

	private Text dialogueText;
    private DialogueManager dialogueManager;
    private UIManager uiManager;
	private SceneCatalogue mySceneCatalogue;
    private Timelord timeLord;
    private RelationshipCounselor relationshipCounselor;

	public void Start()
	{
		dialogueText = GameObject.Find("DialogueText").GetComponent<Text>();
        dialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        uiManager = GameObject.FindObjectOfType<UIManager>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        timeLord = GameObject.FindObjectOfType<Timelord>();
        relationshipCounselor = GameObject.FindObjectOfType<RelationshipCounselor>();
	}

	public void beginConversation(Character speaker)
    {
        uiManager.enableAllButtons();
        List<string> greetingTags = new List<string>();
        greetingTags.Add("greeting");
        if (speaker.permanentOpinion < 0)
        {
            greetingTags.Add("annoyed");
        }
        dialogueText.text = dialogueManager.getDialogueForTags(greetingTags).dialogueContent;
		this.currentConversation = this.gameObject.AddComponent<Conversation>();
		this.currentConversation.speaker = speaker;
    }

	public void makeIntroduction()
    {
        List<string> tags = new List<string>();
        tags.Add("introduction");

		if (currentConversation.lastChosenOption == Conversation.SpeechOption.INTRODUCTION)
        {
			currentConversation.speaker.permanentOpinion -= 1;
            endConversation();
            return;
        }
		if (currentConversation.greetedSoFar)
        {
            tags.Add("repeated");
			currentConversation.opinion -= 1;
        }
		else if (!currentConversation.speaker.knowsYou)
        {
			currentConversation.opinion += 2;
        }
        else
        {
            tags.Add("known");
			currentConversation.opinion += 1;
        }

        dialogueText.text = dialogueManager.getDialogueForTags(tags).dialogueContent;
		currentConversation.lastChosenOption = Conversation.SpeechOption.INTRODUCTION;
		currentConversation.speaker.knowsYou = true;
		currentConversation.greetedSoFar = true;
    }

    public void askQuestion()
    {
        List<string> tags = new List<string>();
        tags.Add("question");

		if (currentConversation.lastChosenOption == Conversation.SpeechOption.QUESTION || currentConversation.complimentCount >= 2)
        {
			currentConversation.speaker.permanentOpinion -= 1;
            endConversation();
            return;
        }
		if (!currentConversation.speaker.knowsYou && !currentConversation.questionedSoFar)
        {
            tags.Add("indifferent");
			currentConversation.opinion += 0;
        }
		else if (!currentConversation.speaker.knowsYou && currentConversation.questionedSoFar)
        {
            currentConversation.opinion -= 1;
            tags.Add("bothered");
        }
        else if (currentConversation.speaker.knowsYou && !currentConversation.questionedSoFar)
        {
            // If has been greeted and has been complimented, in this conversation so far{
            //        difference = 3;
            //        cl("I'd love to help you.")
            //  }else{
            //        difference = 2;
            //        cl("It's over there.")
            //}
            currentConversation.opinion += 2;
            tags.Add("helpful");
        }
        else
        {
            // If HAS NOT been complimented, in this conversation so far{
            //    difference = 0;
            //    cl("I already told you where it is.")
            // }else{
            //        difference = 1;
            //        cl("I said, it's over there.")
            //}
            currentConversation.opinion += 1;
            tags.Add("willing");
        }

        dialogueText.text = dialogueManager.getDialogueForTags(tags).dialogueContent;
        currentConversation.lastChosenOption = Conversation.SpeechOption.QUESTION;
        this.currentConversation.questionedSoFar = true;
        this.currentConversation.questionCount++;
    }

    public void giveCompliment()
    {
        List<string> tags = new List<string>();
        tags.Add("compliment");

        if (currentConversation.lastChosenOption == Conversation.SpeechOption.COMPLIMENT || this.currentConversation.complimentCount >= 2)
        {
            currentConversation.speaker.permanentOpinion -= 1;
            endConversation();
            return;
        }
        if (!currentConversation.speaker.knowsYou && !currentConversation.complimentedSoFar)
        {
            currentConversation.opinion += 1;
            tags.Add("positive");
        }
		else if (!currentConversation.speaker.knowsYou && currentConversation.complimentedSoFar)
        {
            tags.Add("indifferent");
        }
		else if (currentConversation.speaker.knowsYou && !currentConversation.complimentedSoFar)
        {
            if (!currentConversation.questionedSoFar)
            {
                currentConversation.opinion += 2;
                tags.Add("hot");
            }
            else
            {
                currentConversation.opinion += 1;
                tags.Add("warm");
            }
        }
		else if (currentConversation.speaker.knowsYou && currentConversation.complimentedSoFar)
        {
            currentConversation.opinion += 1;
            tags.Add("embarrassed");
        }

        dialogueText.text = dialogueManager.getDialogueForTags(tags).dialogueContent;
        currentConversation.lastChosenOption = Conversation.SpeechOption.COMPLIMENT;
		this.currentConversation.complimentedSoFar = true;
		this.currentConversation.complimentCount++;
    }

    public void makeDemand()
    {
        List<string> tags = new List<string>();
        tags.Add("demand");

		if (currentConversation.lastChosenOption == Conversation.SpeechOption.DEMAND || this.currentConversation.demandCount >= 2)
        {
            currentConversation.speaker.permanentOpinion -= 1;
            endConversation();
            return;
        }
        if (!currentConversation.speaker.knowsYou && currentConversation.speaker.isSubordinate)
        {
			if (this.currentConversation.demandedSoFar)
            {
                currentConversation.opinion += 0;
                tags.Add("unwilling");

            }
            else
            {
                currentConversation.opinion += 1;
                tags.Add("subordinate");
            }
        }
        else if (!currentConversation.speaker.knowsYou && !currentConversation.speaker.isSubordinate)
        {
            currentConversation.opinion -= 1;
            tags.Add("insubordinate");
        }
        else if (currentConversation.speaker.knowsYou && currentConversation.speaker.isSubordinate)
        {
            currentConversation.opinion += 2;
            tags.Add("compliant");
        }
        else if (currentConversation.speaker.knowsYou && !currentConversation.speaker.isSubordinate)
        {
            currentConversation.opinion += 0;
            tags.Add("incompliant");
        }

		this.currentConversation.demandedSoFar = true;
        currentConversation.lastChosenOption = Conversation.SpeechOption.DEMAND;
        //this.thingsSaid.push("demand");
        this.currentConversation.demandCount++;
        dialogueText.text = dialogueManager.getDialogueForTags(tags).dialogueContent;
    }

    public bool canAskOnDateEnabled()
    {
		if(this.currentConversation != null){
			return this.currentConversation.opinion > 5;
		}else{
			return false;
		}
    }

    public void successfullyAskOnDate()
    {
		int random = 1;
		if(random > 0 && mySceneCatalogue.someLocationsObscured()){
            dialogueText.text = "I hear that the "+ mySceneCatalogue.sceneNames[revealLocation()] + " is beautiful this time of year! Where would you like to go?";
        }else{
			dialogueText.text = "Where would you like to go for our date?";
		}

        uiManager.showLocationOptions();
        this.currentConversation.lastChosenOption = Conversation.SpeechOption.ASK_ON_DATE;
    }

	public int revealLocation(){
		for (int i = 0; i < mySceneCatalogue.knownLocations.Length; i++){
			if(mySceneCatalogue.knownLocations[i] == false){
				mySceneCatalogue.learnLocation(i);
				return i;
			}
        }
		return -1;
	}

    public void scheduleDate(int location)
    {
        int randomDateTime = timeLord.getCurrentTimestep() + new System.Random().Next(0, 19);
        relationshipCounselor.createDate(location, randomDateTime, this.currentConversation.speaker);
        uiManager.hideLocationOptions();
        endConversation("Sounds good. see you " + timeLord.getDayOfWeekForTimeStep(randomDateTime) + " in the " + timeLord.getTimeNameForTimeStep(randomDateTime) + "!");

    }

    private void endConversation(string farewell = "")
    {
        List<string> tags = new List<string>();
        tags.AddRange(new List<string>() { "departure", "deflection" });
        dialogueText.text = farewell == "" ? dialogueManager.getDialogueForTags(tags).dialogueContent : farewell;

        uiManager.enableOnlyBye();
        this.currentConversation.opinion = 0;
    }

}
