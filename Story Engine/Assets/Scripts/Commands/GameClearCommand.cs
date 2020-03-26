using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameClearCommand : ICommand
{
    public GameClearCommand()
    {
        
    }

    public void execute(bool toFastForward)
    {
        SceneManager.LoadScene("splash_game_clear");
    }
}
