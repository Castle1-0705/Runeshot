using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CSoundControllerBackground : CSoundController<CSoundControllerBackground>
{
    
    private void Start()
    {
        AudioSource.outputAudioMixerGroup = CSoundManager.Instance.BackGround;
         
    }

    public static void PlayBackground(Scene scene, LoadSceneMode mode)
    { 
        PlayOtherBackGround(scene.name);
    }
}
