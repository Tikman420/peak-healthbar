using UnityEngine;

public class Player
{
    //controller settings
    public const float speed = 6f;
    public const float gravity = -0.1962f;
    public const float jumpHeight = 30f;
    public const float sprintMultiplier = 1.5f;

    public const float mouseSensitivity = 200f;

    //other stuff, no need to touch
    internal StatusBar status;
    public CharacterController controllerComponent;
    public Transform cameraObject;
    public Transform playerTransform;

    private bool isGrounded;
    private float xRotation = 0f;
    public Vector3 velocity;
    public Vector3 previousSpeed;

    //update the controller
    public void update(bool isKnocked)
    {
        updateCamera();

        //only move if in the alive state
        if (isKnocked)
        {
            return;
        }
        controller();
    }
    
    //update camera rotation
    private void updateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraObject.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX * Time.deltaTime);
    }


    //update movement
    private void controller()
    {
        isGrounded = controllerComponent.collisionFlags.HasFlag(CollisionFlags.Below);

        if (isGrounded)
        {
            velocity.y = 0;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Sprinting
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

        //Jumping
        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isGrounded = false;
        }

        //apply movement
        Vector3 move = playerTransform.right * x + playerTransform.forward * z;

        velocity.x = move.x * speed;
        velocity.z = move.z * speed;
        velocity.y += gravity;

        controllerComponent.Move(velocity * Time.deltaTime);
    }
}