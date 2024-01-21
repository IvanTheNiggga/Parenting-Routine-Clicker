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
        interfaceSource.PlayOneShot(Warn[UnityEngine.Random.Range(0, Warn.Length)]);
    }
    public void PlayBuySound()
    {
        interfaceSource.PlayOneShot(buy);
    }
    public void PlayApplauseSound()
    {
        interfaceSource.PlayOneShot(xpUpgrade);
    }
    public void PlayCloseInventorySound()
    {
        interfaceSource.PlayOneShot(closeinv);
    }
    public void PlayOpenInventorySound()
    {
        interfaceSource.PlayOneShot(openinv);
    }
    public void PlayClickSound()
    {
        interfaceSource.PlayOneShot(click);
    }
}
