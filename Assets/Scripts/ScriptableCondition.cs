using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Condition", menuName = "StatusBar/Condition")]
public class ScriptableCondition : ScriptableObject
{
    public string acceptedTag;
    public Sprite icon;
    public Color color;
    public int deterioratingSpeed = 0;
    public Condition conditionType = new Condition();
}
