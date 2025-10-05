using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public enum MotionMode { Straight, ZigZag, CircleSpiral, CrissCross, Rando }
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


    // ultimate random stuff



    void Awake()
    {
        // normalize direction; ignore y axis because it needs to be on the ground anyway
        dirXZ = new Vector3(initialDirectionXZ.x, 0f, initialDirectionXZ.z);
        dirXZ.Normalize();
    }

    void FixedUpdate()
    {
        if(isDead == false)
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

            // time (in seconds) between steps
            float dt = Time.fixedDeltaTime;

            UpdateDirectionByMode(center, dt);

            Vector3 pos = rb.position;

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

}

