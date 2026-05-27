using UnityEngine;

internal static class ConditionFactory
{
    public static ICondition AddCondition<T>(Sprite sprite, Color color, string acceptedTag) where T : ICondition, new()
    {
        ICondition condition = new T(); 
        condition.Icon = sprite;
        condition.Color = color;
        condition.AcceptedTag = acceptedTag;
        return condition;
    }

    public static ICondition AddCondition(ScriptableCondition condition)
    {
        ICondition newCondition = condition.conditionType;
        newCondition.Icon = condition.icon;
        newCondition.Color = condition.color;
        newCondition.AcceptedTag = condition.acceptedTag;
        return newCondition;
    }
}