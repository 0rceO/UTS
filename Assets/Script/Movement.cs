using UnityEngine;

public class Movement : MonoBehaviour
{
    int Crops;
    public float speed = 5.0f;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    [SerializeField] Animator animator;
    Vector2 movement;
    bool isRecoiling = false;
    float recoilTimer = 0f;
    Vector2 recoilVelocity = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRecoiling)
        {
            recoilTimer -= Time.deltaTime;
            if (recoilTimer <= 0f)
            {
                isRecoiling = false;
                recoilVelocity = Vector2.zero;
            }
            // update animator while recoiling if desired
            animator.SetBool("isRunning", false);
            return;
        }

        float hz = Input.GetAxisRaw("Horizontal");
        float vr = Input.GetAxisRaw("Vertical");

        movement.x = hz * speed;
        movement.y = vr * speed;

    rb.linearVelocity = movement;

        if (hz < 0)
        {
            sr.flipX = true;
        }
        else if (hz > 0)
        {
            sr.flipX = false;
        }

        if (hz != 0 || vr != 0)
        {
            // animator.SetBool("isRunning", true);
            animator.PlayInFixedTime("Running");
        }
        else
        {
            // animator.SetBool("isRunning", false);
            animator.PlayInFixedTime("Idle");
        }
    }

    void FixedUpdate()
    {
        if (!isRecoiling)
        {
            rb.linearVelocity = movement;
        }
        else
            rb.linearVelocity = recoilVelocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Crops")
        {
            Crops += 1;
            Debug.Log("Crops Collected: " + Crops);
        }
        
        if (collision.gameObject.tag == "Enemy")
        {
            Vector2 fromAttacker = (Vector2)(transform.position - collision.transform.position);
            ApplyRecoil(fromAttacker.normalized, 7f, 0.25f);
            animator.PlayInFixedTime("Hit");
        }
    }

    public void ApplyRecoil(Vector2 dir, float force, float stunDuration)
    {
        recoilVelocity = dir.normalized * force;
        recoilTimer = stunDuration;
        isRecoiling = true;
        if (rb != null) rb.linearVelocity = recoilVelocity;
    }
}
