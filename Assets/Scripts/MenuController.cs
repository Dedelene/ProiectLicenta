using UnityEngine;
using System.IO;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    { 
        SceneFader.instance.FadeToScene("Room1");
    }

    public void ContinueGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "savegame.json");

        if (File.Exists(path))
        {

            GameManager.instance.LoadGame();
        }
        else
        {
            StartGame();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
