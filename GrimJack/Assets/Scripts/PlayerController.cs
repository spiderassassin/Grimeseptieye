using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;
    public Animator animator;
    float turnSmoothVelocity;
    public float health = 100f;
    public bool inCircleofDeath = false;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Player only moves forward (no backward)
        bool isMovingForward = vertical > 0;

        // Rotate player based on horizontal input
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            float rotationSpeed = 120f; // adjust as needed
            transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);
        }

        if (isMovingForward)
        {
            animator.SetBool("isWalking", true);

            // Move forward relative to the camera's facing direction
            Vector3 camForward = cam.forward;
            camForward.y = 0f; // ignore camera tilt
            camForward.Normalize();

            Vector3 moveDir = camForward;
            controller.Move(moveDir * speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // Attack input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }



    public void Damage()
    {
        health = health - 10 * Time.deltaTime;
        if (health <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene(1);
    }
}
