using System;
using UnityEngine;

[System.Serializable]
public class SoundManager : MonoBehaviour
{
    public AudioSource interfaceSource;
    public AudioClip[] Warn;
    public AudioClip buy;
    public AudioClip xpUpgrade;
    public AudioClip openinv;
    public AudioClip closeinv;
    public AudioClip click;
    public void PlayBruhSound()
    {
        interfaceSource.PlayOneShot(Warn[UnityEngine.Random.Range(0, Warn.Length)], 1);
    }
    public void PlayBuySound()
    {
        interfaceSource.PlayOneShot(buy, 1);
    }
    public void PlayApplauseSound()
    {
        interfaceSource.PlayOneShot(xpUpgrade, 3);
    }
    public void PlayCloseInventorySound()
    {
        interfaceSource.PlayOneShot(closeinv, 2f);
    }
    public void PlayOpenInventorySound()
    {
        interfaceSource.PlayOneShot(openinv, 2f);
    }
    public void PlayClickSound()
    {
        interfaceSource.PlayOneShot(click, 1f);
    }
}
