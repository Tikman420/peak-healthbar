using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CharacterController controller;
    public Slider knockedOutSlider;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float sprintMultiplier = 2;

    public Slider staminaSlider;
    public List<ScriptableCondition> statusTypes = new List<ScriptableCondition>();

    public float mouseSensitivity = 100f;
    public Transform Camera;
    private StatusBar status = new StatusBar();
    private float xRotation = 0f;

    public Vector3 velocity;
    private Vector3 previousSpeed;
    private bool isgrounded = false;
    private int collisionSize;

    public GameObject deathScreen;

    private void Start()
    {
        knockedOutSlider.maxValue = status.deathmanager.KnockoutTimer;
        knockedOutSlider.value = status.deathmanager.KnockoutTimer;

        status.deathmanager.KnockedOutSlider = knockedOutSlider;
        status.StaminaSlider = staminaSlider;
        status.statusTypes = statusTypes;

        controller = gameObject.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        updateCamera();

        if (status.dead)
        {
            deathScreen.SetActive(true);
            return;
        }

        status.update();

        if (staminaSlider.maxValue <= 0)
        {
            return;
        }
        Controller();

        if (velocity.y - previousSpeed.y >= 0.2)
        {
            status.AddAmount("Damage", Mathf.RoundToInt((previousSpeed.y - velocity.y)*-10));
        }
        var collisions = Physics.OverlapBox(transform.position, new Vector3(0.5f, 2.0f, 0.5f));

        if (collisions.Length > collisionSize)
        {
            foreach (var collision in collisions)
            {
                if (collisions[0].gameObject.tag == "")
                {
                    continue;
                }

                status.Add(collisions[0].gameObject.tag, previousSpeed);
                break;
            }
        }
        collisionSize = collisions.Length;

        previousSpeed = velocity;
    }

    private void updateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Controller()
    {
        isgrounded = controller.collisionFlags.HasFlag(CollisionFlags.Below);


        if (isgrounded)
        {
            velocity.y = 0;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Sprint"))
        {
            status.isSprinting = true;
        }
        if (Input.GetButtonUp("Sprint"))
        {
            status.isSprinting = false;
        }

        if (status.isSprinting && status.health != 0 && (x != 0 || z != 0))
        {
            x *= sprintMultiplier;
            z *= sprintMultiplier;
        }
        else
        {
            status.isSprinting = false;
        }
            Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetButton("Jump") && isgrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isgrounded = false;
        }

        ///controller.Move(move * speed);

        velocity.x = move.x * speed;
        velocity.z = move.z * speed;
        velocity.y += gravity;

        controller.Move(velocity);
    }
}
