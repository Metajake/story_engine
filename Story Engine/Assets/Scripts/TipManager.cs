using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipManager : MonoBehaviour {
    VictoryCoach myVictoryCoach;
    SceneCatalogue mySceneCatalogue;
    UIManager myUIManager;
    CommandProcessor myCommandProcessor;
    List<string> tips;
    List<string> locationReveals;

    public List<string> introText = new List<string>() {
        "Sitting at home is safe and comfortable...",
        "...But I need to grow up.",
        "... It's time to find a girlfriend. Things will change if I start dating.",
        "......",
        "The problem with that is: the more time I spend with a girl, the more chance there is of her falling in love with me...",
        "...But once that happens, it never works out for one reason or another, and I'm left heartbroken, crushed, and defeated. Life feels like a game over.",
        "I have to get out there, and have some new experiences without letting anyone fall in love with me. ... At least not until I've experienced everything that life has to offer. Let's go!"
    };

    // Use this for initialization
    void Start () {
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myUIManager = GameObject.FindObjectOfType<UIManager>();
        myCommandProcessor = GameObject.FindObjectOfType<CommandProcessor>();

        tips = new List<string>();
        tips.Add("Keep trying.");
        tips.Add("Learn about a girl's preferred location before asking them out. The chances of you getting laid will increase.");
        tips.Add("I'll reveal more locations on the map, the more experiences you have, so visit often!");

        locationReveals = new List<string>();
        locationReveals.Add("Have you visited the %location% yet?");
        locationReveals.Add("I heard that the %location% are beautiful this time of year.");
        locationReveals.Add("Don't go to the %location%!");
    }
	
	void Update () {
		
	}

    public string getTip()
    {
        if (shouldTeachLocation())
        {
            return locationReveals[new System.Random().Next(0, locationReveals.Count)].Replace("%location%", mySceneCatalogue.revealRandomUnknownLocation().locationName);
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
