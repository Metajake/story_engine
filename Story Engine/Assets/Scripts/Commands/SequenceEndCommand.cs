using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SequenceEndCommand : ICommand
{
    private readonly GameState.gameStates toEndIn;
    private GameState myGameState;
    private CommandBuilder myCommandBuilder;

    public SequenceEndCommand(GameState.gameStates toEndIn)
    {
        myGameState = GameObject.FindObjectOfType<GameState>();
        myCommandBuilder = GameObject.FindObjectOfType<CommandBuilder>();
        this.toEndIn = toEndIn;
    }

    public void execute(bool toFastForward)
    {
        myCommandBuilder.dateCutSceneCharList = new List<Character>() { };

        myGameState.currentGameState = toEndIn;
        if (toEndIn == GameState.gameStates.PROWL)
        {
            GameObject.FindObjectOfType<EventQueue>().queueEvent(new EventSequenceEnd());
        }
    }
}
