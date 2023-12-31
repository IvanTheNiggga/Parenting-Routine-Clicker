using UnityEngine;
using UnityEngine.UI;


public class DamagePart : MonoBehaviour
{
    private Clicker clicker;

    public Text text;

    private Vector2 vector;
    public float acceleration;
    public Rigidbody2D rb;

    public bool big;


    private void Start()
    {
        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        text.text = "-" + NumFormat.FormatNumF1(clicker.CurrDealedDamage);
        if (big)
        {
            text.color = Color.red;
            text.fontSize += 15;
        }
        vector = new Vector2(Random.Range(-20f, 20f), Random.Range(10f, 50f));
        rb.AddForce(vector.normalized * acceleration, ForceMode2D.Impulse);

        Invoke(nameof(SelfDestroy), 1f);
    }

    void SelfDestroy()
    { Destroy(gameObject); }
}
