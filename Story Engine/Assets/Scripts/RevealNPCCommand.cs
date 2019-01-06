using UnityEngine;

public class RevealNPCCommand : ICommand
{
    string NPCToReveal;
    CharacterManager myDialogueManager;
    VictoryCoach myVictoryCoach;
    
    public RevealNPCCommand(VictoryCoach victoryCoach, CharacterManager dialogueManager, string NPCName)
    {
        myVictoryCoach = victoryCoach;
        myDialogueManager = dialogueManager;
        NPCToReveal = NPCName;
    }

    public void execute()
    {
        Debug.Log("Hello Chad");
        myDialogueManager.experienceActors.Add(myDialogueManager.getCharacterForName( NPCToReveal ) );
    }
}