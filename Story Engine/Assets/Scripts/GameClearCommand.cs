﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameClearCommand : ICommand
{
    public GameClearCommand()
    {
        
    }

    public void execute()
    {
        SceneManager.LoadScene("splash_game_clear");
    }
}
