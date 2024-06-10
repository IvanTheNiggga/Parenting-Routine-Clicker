using UnityEngine;
using UnityEngine.UI;

public class DroppedItem : MonoBehaviour
{
    private AudioSource CurrencyAudiosource;
    [SerializeField] private AudioClip pickupSound;
    private Inventory inventory;

    public ItemPattern itemPattern;
    public int Count;

    [SerializeField] private Image ico;
    [SerializeField] private Text countText;


    private void Start()
    {
        countText.text = "";
        transform.localPosition = new Vector2(Random.Range(-115f, 115f), 500f);

        Clicker clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        inventory = GameObject.Find("ClickerManager").GetComponent<Inventory>();
        StagesManager stagesManager = GameObject.Find("ClickerManager").GetComponent<StagesManager>();

        ico.sprite = itemPattern.ico;

        CurrencyAudiosource = GameObject.Find("PickupCurrencySource").GetComponent<AudioSource>();

        UpdateText();
        Invoke(nameof(Collect), 10f);
    }

    private void Update()
    {
        if (Count > 1)
        { countText.transform.rotation = Quaternion.Euler(0, 0, 0); }
    }

    public void UpdateText()
    {
        if (Count > 1)
        { countText.text = Count.ToString(); }
    }

    public void OnMouseEnter() { Collect(); }

    public void Collect()
    {
        PlaySound();
        inventory.AddItem(itemPattern, Count);
        Destroy(gameObject);
    }
    void PlaySound()
    {
        CurrencyAudiosource.pitch = Random.Range(0.9f, 1.15f);
        CurrencyAudiosource.PlayOneShot(pickupSound, 4f);
    }
}
