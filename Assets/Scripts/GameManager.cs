using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<ICondition> statusconditions = new List<ICondition>();
    deathmanager deathmanager;

    public static int health = 1000;

    // Update is called once per frame
    void FixedUpdate()
    {
        int statusTotal = 0;
        foreach (ICondition condition in statusconditions)
        {
            condition.Update();
            statusTotal += condition.Amount;
        }

        if (statusTotal >= health)
        {
            deathmanager.updateDeath();
        }
    }
}
