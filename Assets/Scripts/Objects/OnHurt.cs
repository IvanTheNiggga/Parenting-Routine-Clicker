using UnityEngine;

public class OnHurt : MonoBehaviour
{
    #region Settings
    private const float MinPitch = 0.95f;
    private const float MaxPitch = 1.15f;

    private const int KnockbackAmount = 5;
    #endregion

    #region Local
    [Header("Hit Sound")]
    public AudioClip hit;
    private AudioSource hitSource;
    private SoundManager soundManager;
    private Enemy enemy;
    private ObjectMovement objectMovement;

    private float enemyScaleX;
    private float enemyScaleY;

    private bool isBoss;
    #endregion

    #region Init
    void Start()
    {
        hitSource = GameObject.Find("HitSource").GetComponent<AudioSource>();

        enemyScaleX = transform.lossyScale.x;
        enemyScaleY = transform.lossyScale.y;

        enemy = GetComponent<Enemy>();
        objectMovement = GetComponent<ObjectMovement>();

        isBoss = GameObject.Find("Boss") != null;
    }
    #endregion

    public void Kicked(bool onlySound)
    {
        hitSource.pitch = Random.Range(MinPitch, MaxPitch);
        hitSource.PlayOneShot(hit);
        if (onlySound) return;
        objectMovement.xMoveTo(
            transform.localPosition.x + Random.Range(-KnockbackAmount, KnockbackAmount), 
            1, 
            0.1f, 
            false);

        Invoke(nameof(SetBack), 0.05f);
    }
    public void Sabotaged()
    {
        soundManager.PlayBruhSound();

        objectMovement.xMoveTo(
            transform.localPosition.x + Random.Range(-KnockbackAmount, KnockbackAmount), 
            1, 
            0.1f, 
            false);

        Invoke(nameof(SetBack), 0.05f);
    }

    void SetBack()
    {
        objectMovement.xMoveTo(0, 1, 0.1f, false);
    }
}