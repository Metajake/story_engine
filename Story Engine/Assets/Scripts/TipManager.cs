using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipManager : MonoBehaviour {
    VictoryCoach myVictoryCoach;
    SceneCatalogue mySceneCatalogue;
    List<string> tips;
    List<string> locationReveals;
    private Location randomLocationToReveal;

    public List<string> introText = new List<string>() {
        "Sitting at home is safe and comfortable...",
        "But things need to change. I need to change.",
        "It's time to get out there and meet someone. I will experience a whole new side of life if I start dating.",
        "...",
        "The one BIG problem with that? the more time I spend with someone, the greater the chance there is of them falling in love with me.",
        "...And as soon as that happens, I absolutely can't help but fall in love back.",
        "Unfortunatley, the relationship never seems work out for one reason or another. And I'm left crushed and heartbroken. It's a horrible ending.",
        "...",
        "But I have to get out there. I have to gain those life changing experiences that only a relationship can bring.",
        "And I have to do it without spending too much time with any one person. Until I'm ready, I can't let anyone fall in love with me!",
        "Let's go!"
    };

    // Use this for initialization
    void Start () {
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();

        tips = new List<string>();
        tips.Add("Try and figure out a girl's preferred location before asking them out. The chance to have a rewarding date will increase!");
        tips.Add("I'll reveal more locations on the map, the more experiences you have, so visit often!");
        tips.Add("Make sure not to linger in one location for too long. Peope will get creeped out and force you to leave.");

        locationReveals = new List<string>();
        locationReveals.Add("Have you visited the %location% yet?");
        locationReveals.Add("I heard that the %location% %verb% beautiful this time of year.");
        locationReveals.Add("Don't go to the %location%!");
    }
	
	void Update () {
		
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
