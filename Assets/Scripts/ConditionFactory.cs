using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

internal static class ConditionFactory
{
    internal static GameObject templateCondition;

    public static ICondition AddCondition(ScriptableCondition condition, Transform parent, List<ICondition> conditions)
    {
        ICondition newCondition = condition.conditionType;
        newCondition.AcceptedTag = condition.acceptedTag;
        newCondition.deterioratingSpeed = condition.deterioratingSpeed;

        //visuals
        newCondition.StatusVisual = GameObject.Instantiate(templateCondition, parent);

        Image icon = newCondition.StatusVisual.transform.GetChild(0).GetComponent<Image>();
        icon.sprite = condition.icon;
        icon.color = condition.color;

        icon = newCondition.StatusVisual.transform.GetChild(1).GetComponent<Image>();
        icon.color = condition.color;

        //influencers
        foreach (ICondition otherCondition in conditions)
        {
            if (condition.InfluencerTag == otherCondition.AcceptedTag)
            {
                otherCondition.influence = newCondition;
                newCondition.influence = otherCondition;
                break;
            }
        }

        return newCondition;
    }
}