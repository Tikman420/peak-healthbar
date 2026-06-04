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

        Cursor.lockState = CursorLockMode.Locked;

        ConditionFactory.templateCondition = templateCondition;
    }

    private void FixedUpdate()
    {
        player.update(staminaSlider.maxValue <= 0);

        if (status.dead)
        {
            deathScreen.SetActive(true);
            return;
        }

        status.update();

        if (player.velocity.y - player.previousSpeed.y >= 0.2)
        {
            status.AddAmount("Damage", Mathf.RoundToInt(Mathf.Pow(player.previousSpeed.y, 2)*100));
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

                status.Add(collisions[0].gameObject.tag, player.previousSpeed);
                break;
            }
        }
        collisionSize = collisions.Length;

        player.previousSpeed = player.velocity;
    }
}
