using UnityEngine;

public class DismissNPCCommand : ICommand
{
    private string NPCToDismiss;
    DialogueManager myDialogueManager;
    VictoryCoach myVictoryCoach;

    public DismissNPCCommand(VictoryCoach victoryCoach, DialogueManager dialogueManager, string NPCName)
    {
        myVictoryCoach = victoryCoach;
        myDialogueManager = dialogueManager;
        NPCToDismiss = NPCName;
    }

    public void execute()
    {
        Debug.Log("Bye Chad");
        myDialogueManager.experienceActors.Remove(myDialogueManager.getCharacterForName(NPCToDismiss));
    }
}