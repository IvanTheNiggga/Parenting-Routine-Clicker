using UnityEngine;
using UnityEngine.UI;

public class MoneyItem : MonoBehaviour
{
    public AudioClip pickupCurrency;
    private AudioSource pcAudiosource;

    private RewardManager giveReward;

    public GameObject countTextObj;
    public Text countText;

    public float timeStart = 0;
    public double reward;
    public int count;

    private float yRot;

    private void Start()
    {
        pcAudiosource = GameObject.Find("PickupCurrencySource").GetComponent<AudioSource>();
        giveReward = GameObject.Find("ClickerManager").GetComponent<RewardManager>();
        reward = giveReward.KillReward;

        transform.localPosition = new Vector2(Random.Range(-115f, 115f), 400f);
        if (Random.Range(0, 2) <= 0)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        UpdateText();
        Invoke(nameof(Collect), 10f);
    }

    private void Update()
    {
        if (count > 1)
        { countTextObj.transform.rotation = Quaternion.Euler(0, 0, 0); }
    }

    public void UpdateText()
    {
        if (count > 1)
        { countText.text = count.ToString(); }
        else
        { countText.text = ""; }
    }

    public void Collect()
    {
        Destroy(gameObject);
        PlaySound();
        giveReward.GiveMeReward(count, reward);
    }

    public void OnMouseEnter()
    { Collect(); }

    void PlaySound()
    {
        pcAudiosource.pitch = Random.Range(0.9f, 1.15f);
        pcAudiosource.PlayOneShot(pickupCurrency);
    }

}
