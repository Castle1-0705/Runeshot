using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COptionExit : MonoBehaviour
{ 
    public void GameEnd()
    {
        Application.Quit();
    }

    [SerializeField] GameObject _soundPenel;

    public void ViewSoundOption()
    {
        _soundPenel.SetActive(true);
    }
}
