using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatComponent : MonoBehaviour {
    public VictoryCoach myVictoryCoach;
    private DialogueManager myDialogueManager;
    private SceneCatalogue mySceneCatalogue;
    private ConversationTracker myConversationTracker;
    private System.Random rn;

    // Use this for initialization
    void Start () {
        myVictoryCoach = GameObject.FindObjectOfType<VictoryCoach>();
        myDialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        mySceneCatalogue = GameObject.FindObjectOfType<SceneCatalogue>();
        myConversationTracker = GameObject.FindObjectOfType<ConversationTracker>();

        rn = new System.Random();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            myVictoryCoach.achieveNextExperience(false);
        }else if (Input.GetKeyDown(KeyCode.D))
        {
            DateableCharacter randomDateableChar = myDialogueManager.allDateableCharacters[rn.Next(myDialogueManager.allDateableCharacters.Count)];
            
            myDialogueManager.charactersPresent.Insert(0,randomDateableChar);
            myDialogueManager.selectedPartner = 0;
            myConversationTracker.beginConversation(randomDateableChar);
            List<Location> dateScenes = mySceneCatalogue.getDateScenes();
            Location randomDateLocation = dateScenes[rn.Next(dateScenes.Count)];
            myConversationTracker.scheduleDate(randomDateLocation);

            myDialogueManager.selectedPartner = -1;
        }else if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject.FindObjectOfType<UIManager>().gameOver();
        }
    }
}
