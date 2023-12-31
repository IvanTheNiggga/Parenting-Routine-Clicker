using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    #region Appointed through the inspector
    [SerializeField] private AudioMixerGroup mixer;
    [SerializeField] private Dropdown fpsCapOption;
    [SerializeField] private Dropdown attackOption;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Text resetDataText;

    [SerializeField] private Panel panel;
    [SerializeField] private Clicker clicker;
    #endregion

    #region Variables
    private int confirmInt;
    private float volume;
    #endregion

    private void Start()
    {
        confirmInt = 0;
        LoadOptions();
    }

    public void DeleteData()
    {
        confirmInt++;

        if (confirmInt == 1)
        {
            Invoke(nameof(DisagreeToDelete), 3f);
            resetDataText.text = "Delete data??";
        }
        else if (confirmInt == 2)
        {
            CancelInvoke(nameof(DisagreeToDelete));
            Invoke(nameof(DisagreeToDelete), 3f);
            resetDataText.text = "Really?";
        }
        else if (confirmInt == 3)
        {
            CancelInvoke(nameof(DisagreeToDelete));
            DisagreeToDelete();
            clicker.ResetData();

            Miner washingMachine = GameObject.Find("Miner").GetComponent<Miner>();
            washingMachine.ResetMinerLoot();

            Utils.SetDataTime("LastEntranceTime", DateTime.UtcNow);

            EnemyManager enemyManager = GameObject.Find("ClickerManager").GetComponent<EnemyManager>();
            enemyManager.RespawnEnemy();
        }
    }

    void DisagreeToDelete()
    {
        resetDataText.text = "Delete data";
        confirmInt = 0;
    }

    public void SetDefaultCap()
    {
        fpsCapOption.value = 1;
        SetFpsCap();
    }

    public void SetFpsCap()
    {
        int frameRate = 0;

        switch (fpsCapOption.value)
        {
            case 0:
                frameRate = 30;
                break;
            case 1:
                frameRate = 60;
                break;
            case 2:
                frameRate = 90;
                break;
            case 3:
                frameRate = 144;
                break;
        }

        Application.targetFrameRate = frameRate;
    }

    public void SetAttackMode()
    {
        panel.clickMode = attackOption.value == 0 || attackOption.value == 1;
        panel.swapMode = attackOption.value == 0 || attackOption.value == 2;
    }

    public void ChangeVolume()
    {
        float volumeValue = volumeSlider.value;
        mixer.audioMixer.SetFloat("Master", volumeValue != volumeSlider.minValue ? volumeValue : -80f);
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetInt("fpsCapOption", fpsCapOption.value);
        PlayerPrefs.SetInt("attackOption", attackOption.value);
        PlayerPrefs.SetFloat("volumeSlider", volumeSlider.value);
    }

    public void LoadOptions()
    {
        fpsCapOption.value = PlayerPrefs.GetInt("fpsCapOption");
        SetFpsCap();

        attackOption.value = PlayerPrefs.GetInt("attackOption");
        SetAttackMode();

        volume = PlayerPrefs.GetFloat("volumeSlider");
        volumeSlider.value = volume;
        mixer.audioMixer.SetFloat("Master", volume);
    }
}