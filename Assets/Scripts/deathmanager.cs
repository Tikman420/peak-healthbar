using UnityEngine;
using UnityEngine.UI;

//manage death
internal class DeathManager
{
    public Slider knockedOutSlider;
    public float knockoutTimer = 45;

    //update the death
    public bool UpdateDeath()
    {
        knockedOutSlider.gameObject.SetActive(true);
        knockoutTimer -= Time.deltaTime;
        knockedOutSlider.value = knockoutTimer;

        if (knockoutTimer <= 0)
        {
            return true;
        }

        return false;
    }

    //reset to default
    public void ResetDeath()
    {
        knockedOutSlider.gameObject.SetActive(false);
        knockoutTimer = knockedOutSlider.maxValue;
        knockedOutSlider.value = knockoutTimer;
    }
}
