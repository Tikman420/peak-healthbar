using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //movement stuff
    public Player player = new Player();

    //knocked stuff
    public Slider knockedOutSlider;
    public GameObject deathScreen;

    //statusbar stuff
    public Slider staminaSlider;
    public List<ScriptableCondition> statusTypes = new List<ScriptableCondition>();
    [SerializeField] private GameObject templateCondition;
    private StatusBar status = new StatusBar();

    public Transform cameraObject;
    private int collisionSize;

    //initialize stuff
    private void Start()
    {
        knockedOutSlider.maxValue = status.deathmanager.KnockoutTimer;
        knockedOutSlider.value = status.deathmanager.KnockoutTimer;

        status.deathmanager.KnockedOutSlider = knockedOutSlider;
        status.StaminaSlider = staminaSlider;
        status.statusTypes = statusTypes;

        player.controllerComponent = gameObject.GetComponent<CharacterController>();
        player.cameraObject = cameraObject;
        player.playerTransform = transform;
        player.status = status;

        ConditionFactory.templateCondition = templateCondition;

        Cursor.lockState = CursorLockMode.Locked;
    }

    //update input
    private void Update()
    {
        player.update(staminaSlider.maxValue <= 0);
    }

    //uodate conditions and state
    private void FixedUpdate()
    {
        //only continue if not in the dead state
        if (status.dead)
        {
            deathScreen.SetActive(true);
            return;
        }

        //update state
        status.update();

        //check for fall damage
        if (player.velocity.y - player.previousSpeed.y >= 8)
        {
            status.AddAmount("Damage", Mathf.RoundToInt(Mathf.Pow(player.previousSpeed.y, 2)/2));
        }

        //check for the other statuses
        var collisions = Physics.OverlapBox(transform.position, new Vector3(0.6f, 2.0f, 0.6f));
        if (collisions.Length > collisionSize)
        {
            foreach (var collision in collisions)
            {
                if (collision.gameObject.tag == "Untagged")
                {
                    continue;
                }

                status.AddVelocity(collision.gameObject.tag, player.previousSpeed);
                break;
            }
        }
        collisionSize = collisions.Length;

        player.previousSpeed = player.velocity;
    }
}
