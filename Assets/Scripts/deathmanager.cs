using System;
using System.Collections.Generic;
using UnityEngine;

internal class deathmanager
{
    float KnockoutTimer = 60;

    public void updateDeath()
    {
        Debug.Log("I am dying");
        KnockoutTimer -= Time.deltaTime;
    }

    public void resetDeath()
    {
        Debug.Log("I am ALIVE!");
    }
}
