using UnityEngine;
using System.Collections;

public class SequenceEndCommand : ICommand
{
    private GameState myGameState;

    public SequenceEndCommand()
    {
        myGameState = GameObject.FindObjectOfType<GameState>();
    }

    public void execute()
    {
        myGameState.currentGameState = GameState.gameStates.PROWL;
    }
}
