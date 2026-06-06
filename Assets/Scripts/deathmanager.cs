using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//manage death
internal class deathmanager
{
    public Slider KnockedOutSlider;
    public float KnockoutTimer = 45;

    //update the death
    public bool updateDeath()
    {
        KnockedOutSlider.gameObject.SetActive(true);
        KnockoutTimer -= Time.deltaTime;
        KnockedOutSlider.value = KnockoutTimer;

        if (KnockoutTimer <= 0)
        {
            return true;
        }

        return false;
    }

    //reset to default
    public void resetDeath()
    {
        KnockedOutSlider.gameObject.SetActive(false);
        KnockoutTimer = KnockedOutSlider.maxValue;
        KnockedOutSlider.value = KnockoutTimer;
    }
}
