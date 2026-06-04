using UnityEngine;

public class Player
{
    //controller settings
    public const float speed = 0.2f;
    public const float gravity = -0.01962f;
    public const float jumpHeight = 1f;
    public const float sprintMultiplier = 1.5f;

    public const float mouseSensitivity = 10f;

    //other stuff
    public CharacterController controllerComponent;
    internal StatusBar status;
    public Transform cameraObject;
    public Transform playerTransform;

    private bool isgrounded;
    private float xRotation = 0f;
    public Vector3 velocity;
    public Vector3 previousSpeed;

    public void update(bool isKnocked)
    {
        updateCamera();

        if (isKnocked)
        {
            return;
        }
        controller();
    }
    
    private void updateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraObject.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX);
    }

    private void controller()
    {
        isgrounded = controllerComponent.collisionFlags.HasFlag(CollisionFlags.Below);


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
        Vector3 move = playerTransform.right * x + playerTransform.forward * z;

        if (Input.GetButton("Jump") && isgrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isgrounded = false;
        }

        ///controller.Move(move * speed);

        velocity.x = move.x * speed;
        velocity.z = move.z * speed;
        velocity.y += gravity;

        controllerComponent.Move(velocity);
    }
}
