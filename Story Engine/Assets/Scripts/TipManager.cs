using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipManager : MonoBehaviour {
    VictoryCoach myVictoryCoach;
    SceneCatalogue mySceneCatalogue;
    private CommandBuilder myCommandBuilder;
    List<string> tips;
    List<string> locationReveals;
    private Location randomLocationToReveal;
    private List<string> introText;

    // Use this for initialization
    void Start () {
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myCommandBuilder = GameObject.FindObjectOfType<CommandBuilder>();

        tips = new List<string>();
        tips.Add("Try and figure out a girl's preferred location before asking them out. The chance to have a rewarding date will increase!");
        tips.Add("I'll reveal more locations on the map, the more experiences you have, so visit often!");
        tips.Add("Make sure not to linger in one location for too long. Peope will get creeped out and force you to leave.");

        locationReveals = new List<string>();
        locationReveals.Add("Have you visited the %location% yet?");
        locationReveals.Add("I heard that the %location% %verb% beautiful this time of year.");
        locationReveals.Add("Don't go to the %location%!");

        introText = new List<string>() {
        "Sitting at home is comfortable!..",
        "But something needs to change.",
        "It's time to get out there and meet someone! ... Developing a strong, intimate relationship over time will open a whole new side of life and a whole new me.",
        "...",
        "The problem with that is, the more time I spend with someone, the greater the chance there is of them falling in love with me.\nAnd as soon as that happens, I can't help but fall in love back!..",
        "Unfortunatley for me, relationships never seem to work out (for one reason or another). Every time I fall in love, eventually I'm left crushed and heartbroken. It's devestating!",
        "...",
        "But I have to get out there. I have to gain those life changing experiences that only a relationship can bring.",
        "And I have to do it without spending too much time with any one person. Until I'm ready, I can't let anyone fall in love with me!",
        "...",
        "I could start by making it to work on time. Let's go!"};
    }

    public void startGame()
    {
        myCommandBuilder.createAndEnqueueChangeDialogueSequence(introText);
        myCommandBuilder.build();
    }

    public string getTip()
    {
        if (shouldTeachLocation())
        {
            randomLocationToReveal = mySceneCatalogue.revealRandomUnknownLocation();
            return locationReveals[new System.Random().Next(0, locationReveals.Count)].Replace("%location%", randomLocationToReveal.locationName).Replace("%verb%", randomLocationToReveal.getVerb());
        }
        else
        {
            return tips[new System.Random().Next(0, tips.Count)];
        }
        
    }

    private bool shouldTeachLocation()
    {
        if(mySceneCatalogue.getKnownLocations().Count - 4 < myVictoryCoach.getNumberOfAchievedExperiences())
        {
            return true;
        }
        return false;
    }
}
