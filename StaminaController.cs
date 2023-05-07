using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina UI Elements")]

    [Header("Stamina UI Elements")]
    public Image staminaProgressUI;
    public CanvasGroup sliderCanvasGroup;

    private PlayerMovementAdvanced pma;

    private void Update()
    {
        staminaProgressUI.fillAmount = pma.stamina / pma.maxStamina;
        if (pma.stamina == 100)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
            Debug.Log(staminaProgressUI.fillAmount);
        }
    }
}
