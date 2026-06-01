using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class deathmanager
{
    public Slider KnockedOutSlider;
    public float KnockoutTimer = 45;

    public bool updateDeath()
    {
        Debug.Log("I am dying");

        KnockedOutSlider.gameObject.SetActive(true);
        KnockoutTimer -= Time.deltaTime;
        KnockedOutSlider.value = KnockoutTimer;

        if (KnockoutTimer <= 0)
        {
            return true;
        }

        return false;
    }

    public void resetDeath()
    {
        KnockedOutSlider.gameObject.SetActive(false);
        KnockoutTimer = KnockedOutSlider.maxValue;
        KnockedOutSlider.value = KnockoutTimer;
    }
}
