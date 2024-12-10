using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("BossAI")]
    public Transform pointA, pointB;
    private Vector3 CurrenTarget;
    public float speed = 5.0f;
    public float idleDuration = 1.0f;
    private bool isIdle = false;

    // For flip
    private bool isFacingRight = true;

    // For animation
    private enum MovementState { idle, run, attack };
    private MovementState _state;
    public Animator bossAnimator;

    // For attack
    public Transform player;
    public float attackRange = 5.0f;
    private bool isAttacked = false;
    

    // Lightning prefab
    public GameObject lightningPrefab;
    public float lightningCooldown = 5f;
    private float lightningTimer;
    private bool lightningUsed = false;

    void Start()
    {
        CurrenTarget = pointA.position;
        _state = MovementState.run;
        lightningTimer = lightningCooldown;
      
    }

    void Update()
    {
        // Check if the player is in range
        if (IsPlayerInRange())
        {
            HandleAttack();
        }

        else if (!isIdle)
        {
            BossAI();
        }

        HandleLightningCooldown();
    }

    public void BossAI()
    {
        // Check if the boss has reached the current target
        if (Vector3.Distance(transform.position, CurrenTarget) < 0.1f)
        {
            if (!isIdle)
            {
                StartCoroutine(IdleBeforeMoving());
            }
        }
        else
        {
            // Move towards the current target if not idle
            transform.position = Vector3.MoveTowards(transform.position, CurrenTarget, speed * Time.deltaTime);
            _state = MovementState.run;
        }

        // Update the animator state
        bossAnimator.SetInteger("state", (int)_state);
    }

    private IEnumerator IdleBeforeMoving()
    {
        isIdle = true;
        _state = MovementState.idle;
        bossAnimator.SetInteger("state", (int)_state);

        yield return new WaitForSeconds(idleDuration);

        // Switch target between pointA and pointB after idling
        CurrenTarget = CurrenTarget == pointA.position ? pointB.position : pointA.position;
        Flip();

        isIdle = false;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

   

    private void HandleAttack()
    {
        if (!isAttacked)
        {
            _state = MovementState.attack;
            bossAnimator.SetInteger("state", (int)_state);

            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacked = true;
        Strikelightning(); // Execute attack logic

        yield return new WaitForSeconds(1.0f); // Wait for the attack animation duration

        _state = MovementState.run; // Return to run animation if player is out of range
        bossAnimator.SetInteger("state", (int)_state);
        isAttacked = false;
    }

    private void Strikelightning()
    {
        if (!lightningUsed && lightningTimer >= lightningCooldown)
        {
            lightningTimer = 0;
            lightningUsed = true;
            GameObject lightning = Instantiate(lightningPrefab, player.position, Quaternion.identity);
            Destroy(lightning, 1.5f);
        }
    }

    private void HandleLightningCooldown()
    {
        if (lightningUsed)
        {
            lightningTimer += Time.deltaTime;
            if (lightningTimer >= lightningCooldown)
            {
                lightningUsed = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
