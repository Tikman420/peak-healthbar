using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class deathmanager
{
    public Slider KnockedOutSlider;
    public float KnockoutTimer = 60;

    public bool updateDeath()
    {
        Debug.Log("I am dying");

        KnockedOutSlider.gameObject.SetActive(true);
        KnockoutTimer -= Time.deltaTime;
        return false;
    }

    public void resetDeath()
    {
        //Debug.Log("I am ALIVE!");
        KnockedOutSlider.gameObject.SetActive(false);
        KnockoutTimer = KnockedOutSlider.maxValue;
    }
}
