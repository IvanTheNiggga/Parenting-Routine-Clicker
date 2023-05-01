using UnityEngine;
using UnityEngine.UI;

public class DroppedItem : MonoBehaviour
{
    public AudioClip pickupPC;

    public int count;
    public int stage;
    public int index;

    AudioSource CurrencyAudiosource;

    public Image ico;
    public Text countText;
    public Text countTextObj;

    public Inventory inventory;

    private void Start()
    {
        countText.text = "";
        transform.position = new Vector2(Random.Range(-115f, 115f), 300f);

        Clicker clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        inventory = GameObject.Find("ClickerManager").GetComponent<Inventory>();
        StagesManager stagesManager = GameObject.Find("ClickerManager").GetComponent<StagesManager>();

        ico = GetComponent<Image>();
        ico.sprite = stagesManager.StagesDataBase[stagesManager.StageIndex].itemsDataBase[index].ico;

        CurrencyAudiosource = GameObject.Find("PickupCurrencySource").GetComponent<AudioSource>();

        UpdateText();
        Invoke(nameof(Collect), 10f);
    }

    private void Update()
    { countTextObj.transform.rotation = Quaternion.Euler(0, 0, 0); }

    public void UpdateText()
    { if(count > 1) { countText.text = count.ToString(); } }

    public void OnMouseEnter()
    { Collect(); }

    public void Collect()
    {
        PlaySound();
        inventory.AddItem(stage, index, count);
        Destroy(gameObject);
    }
    void PlaySound()
    {
        CurrencyAudiosource.pitch = Random.Range(0.9f, 1.15f);
        CurrencyAudiosource.PlayOneShot(pickupPC, 2f);
    }
}
