using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateableCharacter : Character {

    public bool[] savedTimes;
	public bool isSubordinate;
	public int permanentOpinion;
    public int inLoveAmount;
	public bool isInLoveWithYou;
	public bool knowsYou;
	public int experienceCount;
    public int[] locationPreferences;
    public int tier;
    private VictoryCoach myVictoryCoach;

	// Use this for initialization
	void Start () {

        DialogueManager dm = GameObject.FindObjectOfType<DialogueManager>();
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
		isInLoveWithYou = false;
		experienceCount = 0;
		savedTimes = new bool[activeTimes.Length];
		activeTimes.CopyTo(savedTimes, 0);


		if(givenName.ToLower() == "kristie"){
            dm.registerDialogue(new DialoguePiece("Hi.",this).addTag("greeting"));
            dm.registerDialogue(new DialoguePiece("Fuck off!",this).addTag("greeting").addTag("annoyed"));

			dm.registerDialogue(new DialoguePiece("Nice to meet you.", this).addTag("introduction"));
            dm.registerDialogue(new DialoguePiece("Hi again!", this).addTag("introduction").addTag("known"));
			dm.registerDialogue(new DialoguePiece("I heard you the first time.", this).addTag("introduction").addTag("repeated"));

            dm.registerDialogue(new DialoguePiece("I'm busy. Come back later.",this).addTag("comebacklater"));
            dm.registerDialogue(new DialoguePiece("Sure! I'd love to!",this).addTag("date.yes"));
            dm.registerDialogue(new DialoguePiece("I'm busy... I have to baby-sit...",this).addTag("date.no"));
            dm.registerDialogue(new DialoguePiece("You'll have to catch me with it.",this).addTag("demand.yes").addTag("affectionate"));
            dm.registerDialogue(new DialoguePiece("Fine. Here you go.",this).addTag("demand.yes").addTag("unaffectionate"));
            dm.registerDialogue(new DialoguePiece("I'll come back and get it for you later.",this).addTag("demand.no").addTag("affectionate"));
            dm.registerDialogue(new DialoguePiece("Shove your stapler up your ass.",this).addTag("demand.no").addTag("unaffectionate"));

            dm.registerDialogue(new DialoguePiece("It's over there.",this).addTag("question"));
            dm.registerDialogue(new DialoguePiece("I'm not sure.",this).addTag("question").addTag("indifferent"));
			dm.registerDialogue(new DialoguePiece("I don't know you, and you already asked me that.",this).addTag("question").addTag("bothered"));
            dm.registerDialogue(new DialoguePiece("I'd be glad to help you.",this).addTag("question").addTag("helpful"));
			dm.registerDialogue(new DialoguePiece("It's over there.",this).addTag("question").addTag("willing"));

            dm.registerDialogue(new DialoguePiece("Thank you.",this).addTag("compliment"));
            dm.registerDialogue(new DialoguePiece("Thanks, I guess.",this).addTag("compliment").addTag("positive"));
            dm.registerDialogue(new DialoguePiece("I get it.",this).addTag("compliment").addTag("indifferent"));
            dm.registerDialogue(new DialoguePiece("You're making me blush!",this).addTag("compliment").addTag("hot"));
            dm.registerDialogue(new DialoguePiece("Why thank you.",this).addTag("compliment").addTag("warm"));
            dm.registerDialogue(new DialoguePiece("Oh, stop!",this).addTag("compliment").addTag("embarrassed"));

            dm.registerDialogue(new DialoguePiece("You're very demanding.",this).addTag("demand"));
            dm.registerDialogue(new DialoguePiece("Okay.",this).addTag("demand").addTag("subordinate"));
            dm.registerDialogue(new DialoguePiece("Maybe later.",this).addTag("demand").addTag("unwilling"));
            dm.registerDialogue(new DialoguePiece("Shove it up your ass.",this).addTag("demand").addTag("insubordinate"));
			dm.registerDialogue(new DialoguePiece("Yes sir.",this).addTag("demand").addTag("compliant"));
			dm.registerDialogue(new DialoguePiece("Get it yourself.",this).addTag("demand").addTag("incompliant"));

			dm.registerDialogue(new DialoguePiece("Um.. I have to go over there now.", this).addTag("departure").addTag("deflection"));
			dm.registerDialogue(new DialoguePiece("Um.. I have to be somewhere.", this).addTag("departure").addTag("time"));
		}else if (givenName.ToLower() == "tammy")
        {
            dm.registerDialogue(new DialoguePiece("HELLO.", this).addTag("greeting"));
            dm.registerDialogue(new DialoguePiece("Fuck off!", this).addTag("greeting").addTag("annoyed"));

            dm.registerDialogue(new DialoguePiece("Nice to meet you.", this).addTag("introduction"));
            dm.registerDialogue(new DialoguePiece("Hi again!", this).addTag("introduction").addTag("known"));
            dm.registerDialogue(new DialoguePiece("I heard you the first time.", this).addTag("introduction").addTag("repeated"));

            dm.registerDialogue(new DialoguePiece("I'm busy. Come back later.", this).addTag("comebacklater"));
            dm.registerDialogue(new DialoguePiece("Sure! I'd love to!", this).addTag("date.yes"));
            dm.registerDialogue(new DialoguePiece("I'm busy... I have to baby-sit...", this).addTag("date.no"));
            dm.registerDialogue(new DialoguePiece("You'll have to catch me with it.", this).addTag("demand.yes").addTag("affectionate"));
            dm.registerDialogue(new DialoguePiece("Fine. Here you go.", this).addTag("demand.yes").addTag("unaffectionate"));
            dm.registerDialogue(new DialoguePiece("I'll come back and get it for you later.", this).addTag("demand.no").addTag("affectionate"));
            dm.registerDialogue(new DialoguePiece("Shove your stapler up your ass.", this).addTag("demand.no").addTag("unaffectionate"));

            dm.registerDialogue(new DialoguePiece("It's over there.", this).addTag("question"));
            dm.registerDialogue(new DialoguePiece("I'm not sure.", this).addTag("question").addTag("indifferent"));
            dm.registerDialogue(new DialoguePiece("I don't know you, and you already asked me that.", this).addTag("question").addTag("bothered"));
            dm.registerDialogue(new DialoguePiece("I'd be glad to help you.", this).addTag("question").addTag("helpful"));
            dm.registerDialogue(new DialoguePiece("It's over there.", this).addTag("question").addTag("willing"));

            dm.registerDialogue(new DialoguePiece("Thank you.", this).addTag("compliment"));
            dm.registerDialogue(new DialoguePiece("Thanks, I guess.", this).addTag("compliment").addTag("positive"));
            dm.registerDialogue(new DialoguePiece("I get it.", this).addTag("compliment").addTag("indifferent"));
            dm.registerDialogue(new DialoguePiece("You're making me blush!", this).addTag("compliment").addTag("hot"));
            dm.registerDialogue(new DialoguePiece("Why thank you.", this).addTag("compliment").addTag("warm"));
            dm.registerDialogue(new DialoguePiece("Oh, stop!", this).addTag("compliment").addTag("embarrassed"));

            dm.registerDialogue(new DialoguePiece("You're very demanding.", this).addTag("demand"));
            dm.registerDialogue(new DialoguePiece("Okay.", this).addTag("demand").addTag("subordinate"));
            dm.registerDialogue(new DialoguePiece("Maybe later.", this).addTag("demand").addTag("unwilling"));
            dm.registerDialogue(new DialoguePiece("Shove it up your ass.", this).addTag("demand").addTag("insubordinate"));
            dm.registerDialogue(new DialoguePiece("Yes sir.", this).addTag("demand").addTag("compliant"));
            dm.registerDialogue(new DialoguePiece("Get it yourself.", this).addTag("demand").addTag("incompliant"));

            dm.registerDialogue(new DialoguePiece("Um.. I have to go over there now.", this).addTag("departure").addTag("deflection"));
            dm.registerDialogue(new DialoguePiece("Um.. I have to be somewhere.", this).addTag("departure").addTag("time"));
		}else if (givenName.ToLower() == "rebecca")
        {
            dm.registerDialogue(new DialoguePiece("HELLO.", this).addTag("greeting"));
            dm.registerDialogue(new DialoguePiece("Fuck off!", this).addTag("greeting").addTag("annoyed"));

            dm.registerDialogue(new DialoguePiece("Nice to meet you.", this).addTag("introduction"));
            dm.registerDialogue(new DialoguePiece("Hi again!", this).addTag("introduction").addTag("known"));
            dm.registerDialogue(new DialoguePiece("I heard you the first time.", this).addTag("introduction").addTag("repeated"));

            dm.registerDialogue(new DialoguePiece("I'm busy. Come back later.", this).addTag("comebacklater"));
            dm.registerDialogue(new DialoguePiece("Sure! I'd love to!", this).addTag("date.yes"));
            dm.registerDialogue(new DialoguePiece("I'm busy... I have to baby-sit...", this).addTag("date.no"));
            dm.registerDialogue(new DialoguePiece("You'll have to catch me with it.", this).addTag("demand.yes").addTag("affectionate"));
            dm.registerDialogue(new DialoguePiece("Fine. Here you go.", this).addTag("demand.yes").addTag("unaffectionate"));
            dm.registerDialogue(new DialoguePiece("I'll come back and get it for you later.", this).addTag("demand.no").addTag("affectionate"));
            dm.registerDialogue(new DialoguePiece("Shove your stapler up your ass.", this).addTag("demand.no").addTag("unaffectionate"));

            dm.registerDialogue(new DialoguePiece("It's over there.", this).addTag("question"));
            dm.registerDialogue(new DialoguePiece("I'm not sure.", this).addTag("question").addTag("indifferent"));
            dm.registerDialogue(new DialoguePiece("I don't know you, and you already asked me that.", this).addTag("question").addTag("bothered"));
            dm.registerDialogue(new DialoguePiece("I'd be glad to help you.", this).addTag("question").addTag("helpful"));
            dm.registerDialogue(new DialoguePiece("It's over there.", this).addTag("question").addTag("willing"));

            dm.registerDialogue(new DialoguePiece("Thank you.", this).addTag("compliment"));
            dm.registerDialogue(new DialoguePiece("Thanks, I guess.", this).addTag("compliment").addTag("positive"));
            dm.registerDialogue(new DialoguePiece("I get it.", this).addTag("compliment").addTag("indifferent"));
            dm.registerDialogue(new DialoguePiece("You're making me blush!", this).addTag("compliment").addTag("hot"));
            dm.registerDialogue(new DialoguePiece("Why thank you.", this).addTag("compliment").addTag("warm"));
            dm.registerDialogue(new DialoguePiece("Oh, stop!", this).addTag("compliment").addTag("embarrassed"));

            dm.registerDialogue(new DialoguePiece("You're very demanding.", this).addTag("demand"));
            dm.registerDialogue(new DialoguePiece("Okay.", this).addTag("demand").addTag("subordinate"));
            dm.registerDialogue(new DialoguePiece("Maybe later.", this).addTag("demand").addTag("unwilling"));
            dm.registerDialogue(new DialoguePiece("Shove it up your ass.", this).addTag("demand").addTag("insubordinate"));
            dm.registerDialogue(new DialoguePiece("Yes sir.", this).addTag("demand").addTag("compliant"));
            dm.registerDialogue(new DialoguePiece("Get it yourself.", this).addTag("demand").addTag("incompliant"));

            dm.registerDialogue(new DialoguePiece("Um.. I have to go over there now.", this).addTag("departure").addTag("deflection"));
            dm.registerDialogue(new DialoguePiece("Um.. I have to be somewhere.", this).addTag("departure").addTag("time"));
		}else if (givenName.ToLower() == "jill")
        {
            dm.registerDialogue(new DialoguePiece("HELLO.", this).addTag("greeting"));
            dm.registerDialogue(new DialoguePiece("Fuck off!", this).addTag("greeting").addTag("annoyed"));

            dm.registerDialogue(new DialoguePiece("Nice to meet you.", this).addTag("introduction"));
            dm.registerDialogue(new DialoguePiece("Hi again!", this).addTag("introduction").addTag("known"));
            dm.registerDialogue(new DialoguePiece("I heard you the first time.", this).addTag("introduction").addTag("repeated"));

            dm.registerDialogue(new DialoguePiece("I'm busy. Come back later.", this).addTag("comebacklater"));
            dm.registerDialogue(new DialoguePiece("Sure! I'd love to!", this).addTag("date.yes"));
            dm.registerDialogue(new DialoguePiece("I'm busy... I have to baby-sit...", this).addTag("date.no"));
            dm.registerDialogue(new DialoguePiece("You'll have to catch me with it.", this).addTag("demand.yes").addTag("affectionate"));
            dm.registerDialogue(new DialoguePiece("Fine. Here you go.", this).addTag("demand.yes").addTag("unaffectionate"));
            dm.registerDialogue(new DialoguePiece("I'll come back and get it for you later.", this).addTag("demand.no").addTag("affectionate"));
            dm.registerDialogue(new DialoguePiece("Shove your stapler up your ass.", this).addTag("demand.no").addTag("unaffectionate"));

            dm.registerDialogue(new DialoguePiece("It's over there.", this).addTag("question"));
            dm.registerDialogue(new DialoguePiece("I'm not sure.", this).addTag("question").addTag("indifferent"));
            dm.registerDialogue(new DialoguePiece("I don't know you, and you already asked me that.", this).addTag("question").addTag("bothered"));
            dm.registerDialogue(new DialoguePiece("I'd be glad to help you.", this).addTag("question").addTag("helpful"));
            dm.registerDialogue(new DialoguePiece("It's over there.", this).addTag("question").addTag("willing"));

            dm.registerDialogue(new DialoguePiece("Thank you.", this).addTag("compliment"));
            dm.registerDialogue(new DialoguePiece("Thanks, I guess.", this).addTag("compliment").addTag("positive"));
            dm.registerDialogue(new DialoguePiece("I get it.", this).addTag("compliment").addTag("indifferent"));
            dm.registerDialogue(new DialoguePiece("You're making me blush!", this).addTag("compliment").addTag("hot"));
            dm.registerDialogue(new DialoguePiece("Why thank you.", this).addTag("compliment").addTag("warm"));
            dm.registerDialogue(new DialoguePiece("Oh, stop!", this).addTag("compliment").addTag("embarrassed"));

            dm.registerDialogue(new DialoguePiece("You're very demanding.", this).addTag("demand"));
            dm.registerDialogue(new DialoguePiece("Okay.", this).addTag("demand").addTag("subordinate"));
            dm.registerDialogue(new DialoguePiece("Maybe later.", this).addTag("demand").addTag("unwilling"));
            dm.registerDialogue(new DialoguePiece("Shove it up your ass.", this).addTag("demand").addTag("insubordinate"));
            dm.registerDialogue(new DialoguePiece("Yes sir.", this).addTag("demand").addTag("compliant"));
            dm.registerDialogue(new DialoguePiece("Get it yourself.", this).addTag("demand").addTag("incompliant"));

            dm.registerDialogue(new DialoguePiece("Um.. I have to go over there now.", this).addTag("departure").addTag("deflection"));
            dm.registerDialogue(new DialoguePiece("Um.. I have to be somewhere.", this).addTag("departure").addTag("time"));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void noLocation(){
		for (int i = 0; i < this.activeTimes.Length; i++){
			this.activeTimes[i] = false;
		}
	}

    public int getPreferredLocation(){
        int preferredLocation = 0;
        for (int i = 0; i < locationPreferences.Length; i ++){
            if ( locationPreferences[i] == 2){
                preferredLocation = i;
            }
        }
        return preferredLocation;
    }

	public override bool checkIsPresent()
	{
		return isPresent && this.tier <= myVictoryCoach.getNumberOfAchievedExperiences();
	}
}
