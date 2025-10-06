using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public enum MotionMode { Straight, ZigZag, CircleSpiral, CrissCross, Rando, CrackCocaine }
    public MotionMode motion = MotionMode.Straight;

    public Animator animator;

    public bool isDamaged = false;
    public bool isDead = false;
    float damage_duation = 2f;

    public CapsuleCollider circle;
    public float speed = 3f;

    public Vector3 initialDirectionXZ = new Vector3(1, 0, 0);
    public bool faceMovement = true;
    public Rigidbody rb;

    public float HP = 100f;

    private Vector3 dirXZ;    // unit vector on XZ plane

    private float modeTimer;     // general timer for mode switching

    // zigzag stuff
    public float zigZagTurnRate = 200f;    // degrees per second
    public float zigZagSwitchTime = 1f;  // time before flipping turn direction
    private int zigSign = 1;     // +1 / -1 for zigzag turning

    // criss-cross stuff


    // circle-spiral stuff
    public float spiralAmount = 0.5f;      // strength of in/out push (0..1ish)
    public float spiralSwitchTime = 1.2f;  // seconds between flipping in <-> out
    private float spiralTimer = 0f;
    private int spiralSign = +1;           // +1 = inward, -1 = outward

    // ultimate random stuff
    public float targetReachDistance = 0.3f;   // how close is "arrived"
    public float edgeBuffer = 0.2f;            // don't pick targets right on the rim
    private Vector3 randomTarget;              // current random destination
    private bool hasRandomTarget = false;


    // crackCOCIANE BRO
    public float circleTurnRate = 30f;     // degrees per second (positive = CCW)

    // cached world-space radius for use inside UpdateDirectionByMode (replaces GetWorldRadius)
    private float worldRadius;



    void Awake()
    {
        // normalize direction; ignore y axis because it needs to be on the ground anyway
        dirXZ = new Vector3(initialDirectionXZ.x, 0f, initialDirectionXZ.z);
        dirXZ.Normalize();
    }

    void FixedUpdate()
    {
        if (isDead == false)
        {
            if (damage_duation < 2f)
            {
                damage_duation += Time.deltaTime;
            }
            else
            {
                if (isDamaged == true)
                {
                    isDamaged = false;
                    animator.SetBool("hurt", false);
                }
            }
            // get world-space center / radius of the circle
            Vector3 center = circle.transform.TransformPoint(circle.center);

            float maxScale = circle.transform.lossyScale.x; // they're all scaled the same anyway - lossyscale because its inconsistent
            float radius = circle.radius * maxScale;
            worldRadius = radius; // cache for modes that need radius inside UpdateDirectionByMode

            // time (in seconds) between steps
            float dt = Time.fixedDeltaTime;

            if (motion == MotionMode.Rando && !hasRandomTarget)
            {
                float y = transform.position.y;
                randomTarget = PickRandomTarget(center, radius, y);
                hasRandomTarget = true;
            }

            UpdateDirectionByMode(center, dt);

            Vector3 pos = rb ? rb.position : transform.position;

            // propose next position
            Vector3 step = dirXZ * speed * dt;
            Vector3 nextPos = pos + step;
            // if we'd go outside the circle, correct the direction and snap to the edge
            Vector3 toNext = nextPos - center;
            float distXZ = new Vector2(toNext.x, toNext.z).magnitude;

            if (distXZ > radius)
            {
                // function that determines whatever its supposed to do
                dirXZ = (center - pos);
                dirXZ.y = 0f;
                dirXZ.Normalize();
            }

            transform.position = nextPos;
            // face direction of movement
            if (faceMovement)
            {
                transform.forward = new Vector3(dirXZ.x, 0f, dirXZ.z);
            }
        }


    }

    private void UpdateDirectionByMode(Vector3 center, float dt)
    {
        switch (motion)
        {
            case MotionMode.Straight:
                // do nothing
                break;



            case MotionMode.ZigZag:
                modeTimer += dt;

                // rotate the direction left / right
                dirXZ = RotateXZ(dirXZ, zigZagTurnRate * zigSign * dt);
                if (modeTimer >= zigZagSwitchTime)
                {
                    zigSign = -zigSign;
                    modeTimer = 0f;
                }

                break;




            case MotionMode.CircleSpiral:
                // time-keeping for simple flip-flop in/out
                spiralTimer += dt;
                if (spiralTimer >= spiralSwitchTime)
                {
                    spiralSign = -spiralSign;
                    spiralTimer = 0f;
                }

                // radial (inward) and tangent (perpendicular anti) from our current position
                Vector3 toCenter = center - transform.position;
                toCenter.y = 0f;
                if (toCenter.sqrMagnitude < 1e-6f) toCenter = -dirXZ; // fallback

                Vector3 radial = toCenter.normalized;         // inward
                Vector3 tangent = new Vector3(-radial.z, 0f, radial.x); // 90° anti

                // mostly tangent (orbit) + fixed inward/outward push that flips sign
                dirXZ = (tangent + radial * (spiralAmount * spiralSign));
                dirXZ.y = 0f;
                dirXZ.Normalize();
                break;



            case MotionMode.CrackCocaine:
                // Build a tangent direction around the circle at our current position
                toCenter = center - transform.position;
                toCenter.y = 0f;
                if (toCenter.sqrMagnitude < 1e-6f) toCenter = -dirXZ; // fallback

                radial = toCenter.normalized;                 // points inward
                tangent = RotateXZ(radial, +90f);             // anti tangent

                // Turn direction along the tangent at circleTurnRate
                RotateVectorTowards(ref tangent, circleTurnRate * dt);

                // Combine: tangent + a bit of in/out
                dirXZ = (tangent + radial);
                dirXZ.y = 0f;
                dirXZ.Normalize();
                break;




            case MotionMode.Rando:
                {
                    // If no target yet (or was cleared), pick one
                    if (!hasRandomTarget)
                    {
                        randomTarget = PickRandomTarget(center, worldRadius, transform.position.y);
                        hasRandomTarget = true;
                    }

                    // Head toward the target
                    Vector3 toTarget = randomTarget - transform.position;
                    toTarget.y = 0f;

                    // If we arrived (or target is degenerate), pick a new one
                    if (toTarget.sqrMagnitude < targetReachDistance * targetReachDistance || toTarget.sqrMagnitude < 1e-6f)
                    {
                        randomTarget = PickRandomTarget(center, worldRadius, transform.position.y);
                        toTarget = randomTarget - transform.position;
                        toTarget.y = 0f;
                    }

                    dirXZ = toTarget.normalized;
                    break;
                }


        }
    }

    public void TakeDamage(float amount)
    {
        HP -= amount;
        if (HP <= 0f)
        {

            Die();
        }
        else
        {
            isDamaged = true;
            damage_duation = 0f;
            animator.SetBool("hurt", true);
            // play hit sound or something?
        }
    }

    private void Die()
    {
        animator.SetBool("die", true);
        isDead = true;
        // insert death noises
        // kill kill kill kill
        //Destroy(gameObject);
    }




    // rotate any XZ vector by degrees around Y
    private static Vector3 RotateXZ(Vector3 v, float degrees)
    {
        return (Quaternion.Euler(0f, degrees, 0f) * v).normalized;
    }

    private void RotateVectorTowards(ref Vector3 v, float degrees)
    {
        v = RotateXZ(v, degrees);
    }

    private Vector3 PickRandomTarget(Vector3 center, float radius, float y)
    {
        // Random.insideUnitCircle -> point in unit disk; scale by (radius - edgeBuffer)
        Vector2 p = Random.insideUnitCircle * Mathf.Max(0f, radius - edgeBuffer);
        return new Vector3(center.x + p.x, y, center.z + p.y);
    }


}
