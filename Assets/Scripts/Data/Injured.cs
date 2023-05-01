using UnityEngine;

public class Injured : MonoBehaviour
{
    [Header("Hit Sound")]
    public AudioClip hit;
    private AudioSource hitSource;
    private Enemy enemy;
    private ObjectMovement objectMovement;
    private SpriteRenderer thisSR;

    [Header("Enemy or Boss Sprites")]
    public Sprite FullHP;
    public Sprite[] injured;

    private float enemyScaleX;
    private float enemyScaleY;

    public Vector2 startPos;
    void Start()
    {
        hitSource = GameObject.Find("HitSource").GetComponent<AudioSource>();

        enemyScaleX = transform.lossyScale.x;
        enemyScaleY = transform.lossyScale.y;

        enemy = GetComponent<Enemy>();
        objectMovement = GetComponent<ObjectMovement>();
        thisSR = GetComponent<SpriteRenderer>();

        thisSR.sprite = FullHP;
    }

    void SetBack()
    {
        objectMovement.MoveTo( startPos, 1, 0.1f, false);
    }

    public void SetInjured()
    {
        if (injured.Length > 1)
        {
            if (enemy.HP < enemy.startHp)
            {
                double district = enemy.startHp / injured.Length;
                thisSR.sprite = injured[Mathf.FloorToInt((float)(enemy.HP / district))];
            }
            else
            {
                thisSR.sprite = FullHP;
            }
        }
    }

    void Knockback(int i)
    {
        objectMovement.MoveTo( new Vector2(transform.localPosition.x + Random.Range(-i, i), transform.localPosition.y + Random.Range(-i, i)), 1, 0.1f, false);
        Invoke(nameof(SetBack), 0.05f);
    }

    public void Kicked()
    {
        if (GameObject.Find("Boss") == null)
        {
            hitSource.pitch = Random.Range(0.9f, 1.15f);
            hitSource.PlayOneShot(hit, 3f);

            Knockback(5);
        }
        else
        {
            KickedBoss();
        }
    }
    public void KickedCrit()
    {
        if (GameObject.Find("Boss") == null)
        {
            hitSource.pitch = Random.Range(0.9f, 1.15f);
            hitSource.PlayOneShot(hit, 3f);

            Knockback(10);
        }
        else
        {
            KickedCritBoss();
        }
    }
    public void KickedBoss()
    {
        hitSource.pitch = Random.Range(0.9f, 1.15f);
        hitSource.PlayOneShot(hit, 3f);

        Knockback(5);
    }
    public void KickedCritBoss()
    {
        hitSource.pitch = Random.Range(0.9f, 1.15f);
        hitSource.PlayOneShot(hit, 3f);

        Knockback(10);
    }

}
