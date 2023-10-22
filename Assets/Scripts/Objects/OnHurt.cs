using UnityEngine;

public class OnHurt : MonoBehaviour
{
    [Header("Hit Sound")]
    public AudioClip hit;
    private AudioSource hitSource;
    private Enemy enemy;
    private ObjectMovement objectMovement;

    private float enemyScaleX;
    private float enemyScaleY;

    public Vector2 startPos;

    private bool isBoss;

    private const float MinPitch = 0.9f;
    private const float MaxPitch = 1.15f;

    private const int KnockbackAmount = 5;

    void Start()
    {
        hitSource = GameObject.Find("HitSource").GetComponent <AudioSource>();

        enemyScaleX = transform.lossyScale.x;
        enemyScaleY = transform.lossyScale.y;

        enemy = GetComponent<Enemy>();
        objectMovement = GetComponent<ObjectMovement>();

        isBoss = GameObject.Find("Boss") != null;
    }

    void SetBack()
    {
        objectMovement.MoveTo(startPos, 1, 0.1f, false);
    }

    void Knockback(int i)
    {
        objectMovement.MoveTo(new Vector2(transform.localPosition.x + Random.Range(-i, i), transform.localPosition.y + Random.Range(-i, i)), 1, 0.1f, false);
        Invoke(nameof(SetBack), 0.05f);
    }

    private void PlayHitSound()
    {
        hitSource.pitch = Random.Range(MinPitch, MaxPitch);
        hitSource.PlayOneShot(hit, 3f);
    }

    public void Kicked()
    {
        if (!isBoss)
        {
            PlayHitSound();
            Knockback(KnockbackAmount);
        }
        else
        {
            KickedBoss();
        }
    }

    public void KickedCrit()
    {
        if (!isBoss)
        {
            PlayHitSound();
            Knockback(KnockbackAmount * 2); // Increased knockback for critical hit
        }
        else
        {
            KickedCritBoss();
        }
    }

    public void KickedBoss()
    {
        PlayHitSound();
        Knockback(KnockbackAmount);
    }

    public void KickedCritBoss()
    {
        PlayHitSound();
        Knockback(KnockbackAmount * 2); // Increased knockback for critical hit
    }
}