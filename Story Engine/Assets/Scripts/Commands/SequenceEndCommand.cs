using UnityEngine;
using System.Collections;

public class SequenceEndCommand : ICommand
{
    private readonly GameState.gameStates toEndIn;
    private GameState myGameState;

    public SequenceEndCommand(GameState.gameStates toEndIn)
    {
        myGameState = GameObject.FindObjectOfType<GameState>();
        this.toEndIn = toEndIn;
    }

    public void execute()
    {
        myGameState.currentGameState = toEndIn;
        if (toEndIn == GameState.gameStates.PROWL)
        {
            GameObject.FindObjectOfType<EventQueue>().queueEvent(new EventSequenceEnd());
        }
    }
}
