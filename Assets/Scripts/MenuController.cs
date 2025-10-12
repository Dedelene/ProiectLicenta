using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public SceneFader fader;
    public void StartGame()
    {
        fader.FadeToScene("Room1");
    }

    public void ContinueGame()
    {
        fader.FadeToScene("Room1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
