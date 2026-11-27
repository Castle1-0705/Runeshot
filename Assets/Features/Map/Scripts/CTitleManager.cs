using UnityEngine;
using UnityEngine.SceneManagement;

public class CTitleManager : MonoBehaviour
{
    public void StartGame()
    {
        CMapReturnData.Reset();
        
        SceneManager.LoadScene("BattleMap");
        Time.timeScale = 1.0f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
