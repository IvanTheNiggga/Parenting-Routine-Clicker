using UnityEngine;
using UnityEngine.UI;


public class ClickObj : MonoBehaviour
{
    private Clicker clicker;

    public Text text;
    private Transform clickParent;

    private Vector2 vector;
    public float acceleration;
    public Rigidbody2D rb;

    public bool big;


    private void Start()
    {
        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        text.text = FormatNumsHelper.FormatNumF1(clicker.CurrDealedDamage);
        if (clicker.Damage < clicker.CurrDealedDamage && big == true)
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
