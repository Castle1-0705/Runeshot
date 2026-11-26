using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CMouseOverTriggerBlockHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CInputDispatcher.Instance == null)
        {
            return;
        }

        CInputDispatcher.Instance.ToggleAction("TriggerWeapon", false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(CInputDispatcher.Instance == null)
        {
            return;
        }

        CInputDispatcher.Instance.ToggleAction("TriggerWeapon", true);
    }
}
