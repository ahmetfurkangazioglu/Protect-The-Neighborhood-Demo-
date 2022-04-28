using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QualitySelectSettings : MonoBehaviour
{
    public TMP_Dropdown QualityDropdown;
    public GameObject SettingsPanel;


    void Start()
    {
        if (PlayerPrefs.HasKey("Quality"))
        {
            int Quality= PlayerPrefs.GetInt("Quality");
            QualityDropdown.value = Quality;
            QualitySettings.SetQualityLevel(Quality);
        }
        else
        {
            PlayerPrefs.SetInt("Quality", 2);
            QualityDropdown.value = 2;
        }
    }

    public void SelectQuality(int index)
    {
        PlayerPrefs.SetInt("Quality", index);
        QualitySettings.SetQualityLevel(index);
    }

    public void CloseSettingsPanel()
    {
        SettingsPanel.SetActive(false);
    }
}
