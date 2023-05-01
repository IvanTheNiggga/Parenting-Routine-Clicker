using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixerGroup mixer;

    private Toggle fpsToggle;
    private Dropdown fpsCapOption;
    private Dropdown attackOption;
    private Slider volumeSlider;
    private Text text;

    private Panel panel;
    private Clicker clicker;
    private GameObject fpscounter;

    private int confirmInt;

    public float volume;

    void Start()
    {
        confirmInt = 0;

        fpscounter = GameObject.Find("FPSCounter");
        fpsToggle = GameObject.Find("==FPSToggle==").GetComponent<Toggle>();
        fpsCapOption = GameObject.Find("==TargetFPS==").GetComponent<Dropdown>();
        attackOption = GameObject.Find("==AttackMode==").GetComponent<Dropdown>();
        panel = GameObject.Find("Click Panel").GetComponent<Panel>();

        text = GameObject.Find("DeleteDataTEXT").GetComponent<Text>();

        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        volumeSlider = GameObject.Find("==VolumeSlider==").GetComponent<Slider>();

        LoadOptions();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Data manipulations
    public void DeleteData()
    {
        confirmInt++;

        if (confirmInt == 1)
        {
            Invoke(nameof(DisagreeToDelete), 3f);
            text.text = "Delete data??";
        }
        else if (confirmInt == 2)
        {
            CancelInvoke(nameof(DisagreeToDelete));
            Invoke(nameof(DisagreeToDelete), 3f);
            text.text = "Really?";
        }
        else if (confirmInt == 3)
        {
            CancelInvoke(nameof(DisagreeToDelete));
            DisagreeToDelete();
            clicker.ResetData();

            Washingmashine washingmashine = GameObject.Find("Washingmashine").GetComponent<Washingmashine>();
            washingmashine.DeleteWashingmashineIncome();

            EntranceTimeUtils.SetDataTime("LastEntranceTime", DateTime.UtcNow);

            EnemyManager enemyManager = GameObject.Find("ClickerManager").GetComponent<EnemyManager>();
            enemyManager.RespawnEnemy();
        }
    }

    void DisagreeToDelete()
    {
        text.text = "Delete data";
        confirmInt = 0;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Objects activation
    public void ShowElement(GameObject g)
    {
        g.SetActive(true);
    }
    public void HideElement(GameObject g)
    {
        g.SetActive(false);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Toggle FPS Counter
    public void ToggleFPSCounter()
    {
        fpscounter.SetActive(fpsToggle.isOn);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Set FPS cap
    public void SetDefaultCap()
    {
        fpsCapOption = GameObject.Find("==TargetFPS==").GetComponent<Dropdown>();
        fpsCapOption.value = 1;
        SetFpsCap();
    }
    public void SetFpsCap()
    {
        switch (fpsCapOption.value)
        {
            case 0:
                Application.targetFrameRate = 30;
                break;
            case 1:
                Application.targetFrameRate = 60;
                break;
            case 2:
                Application.targetFrameRate = 90;
                break;
            case 3:
                Application.targetFrameRate = 144;
                break;

        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Attack mode
    public void SetAttackMode()
    {
        switch (attackOption.value)
        {
            case 0:
                panel.clickMode = true;
                panel.swapMode = true;
                break;
            case 1:
                 panel.clickMode = true;
                 panel.swapMode = false;
                 break;
            case 2:
                panel.clickMode = false;
                panel.swapMode = true;
                break;
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Volume
    public void ChangeVolume()
    {
        if(volumeSlider.value != volumeSlider.minValue)
        {
            mixer.audioMixer.SetFloat("Master", volumeSlider.value);
        }
        else
        {
            mixer.audioMixer.SetFloat("Master", -80f);
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Save/Load
    public void SaveOptions()
    {
        PlayerPrefs.SetInt("fpsCapOption", fpsCapOption.value);

        PlayerPrefs.SetInt("fpsToggle", fpsToggle.isOn ? 1 : 0);

        PlayerPrefs.SetInt("attackOption", attackOption.value);

        PlayerPrefs.SetFloat("volumeSlider", volumeSlider.value);
    }

    public void LoadOptions()
    {
        fpsToggle.isOn = PlayerPrefs.GetInt("fpsToggle") == 1;
        fpscounter.SetActive(fpsToggle.isOn);

        fpsCapOption.value = PlayerPrefs.GetInt("fpsCapOption");
        SetFpsCap();

        attackOption.value = PlayerPrefs.GetInt("attackOption");
        SetAttackMode();

        volume = PlayerPrefs.GetFloat("volumeSlider");
        volumeSlider.value = volume;
        mixer.audioMixer.SetFloat("Master", volumeSlider.value);
    }
}
