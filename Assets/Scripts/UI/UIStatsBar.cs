using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStatsBar : MonoBehaviour
{
    [SerializeField] Image HPBar;
    [SerializeField] Image MPBar;
    [SerializeField] TMP_Text HPText;
    [SerializeField] TMP_Text MPText;

    public void UpdateStatsBar(float currentHP, float currentMP, float maxHP, float maxMP)
    {
        HPBar.fillAmount = currentHP / maxHP;
        MPBar.fillAmount = currentMP / maxMP;
        HPText.text = currentHP + "/" + maxHP;
        MPText.text = currentMP + "/" + maxMP;
    }
}
