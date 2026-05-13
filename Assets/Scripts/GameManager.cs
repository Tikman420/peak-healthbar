using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CharacterController controller;
    public Slider KnockedOutSlider;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public float mouseSensitivity = 100f;
    public Transform Camera;
    StatusBar status = new StatusBar();
    float Xrotation = 0f;

    public Vector3 velocity;
    float previousSpeed;
    bool isgrounded = false;

    private void Start()
    {
        KnockedOutSlider.maxValue = status.deathmanager.KnockoutTimer;
        KnockedOutSlider.value = status.deathmanager.KnockoutTimer;

        status.deathmanager.KnockedOutSlider = KnockedOutSlider;

        controller = gameObject.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (status.dead)
        {
            return;
        }

        status.update();
        Controller();
        updateCamera();

        if (velocity.y - previousSpeed >= 0.2)
        {
            status.Add("Damage", Mathf.RoundToInt((previousSpeed - velocity.y)*-1));
        }
        previousSpeed = velocity.y;
    }

    private void updateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        Xrotation -= mouseY;
        Xrotation = Mathf.Clamp(Xrotation, -90f, 90f);

        Camera.localRotation = Quaternion.Euler(Xrotation, 0f, 0f);
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

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetButton("Jump") && isgrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isgrounded = false;
        }

        controller.Move(move * speed);

        velocity.y += gravity;

        controller.Move(velocity);
    }
}
