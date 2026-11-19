using UnityEngine;
using UnityEngine.SceneManagement;

public class CTitleManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("BattleMap");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
