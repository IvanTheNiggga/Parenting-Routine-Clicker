using UnityEngine;

[System.Serializable]
public class SoundManager : MonoBehaviour
{
    public AudioSource interfaceSource;
    public AudioClip bruh;
    public AudioClip buy;
    public AudioClip xpUpgrade;
    public AudioClip openinv;
    public AudioClip closeinv;
    public void PlayBruhSound()
    {
        interfaceSource.PlayOneShot(bruh, 1);
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
}
