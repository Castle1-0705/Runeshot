using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COptionSound : MonoBehaviour
{
    [SerializeField] GameObject _soundPenel;

    public void ViewSoundOption()
    {
        _soundPenel.SetActive(true);
    }
}
