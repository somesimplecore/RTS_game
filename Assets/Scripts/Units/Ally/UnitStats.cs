using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Stats")]
public class UnitStats : ScriptableObject
{
    public float health;

    public float damage;
    public float attackSpeed;
    public float attackRange;
    public DamageType firstDamageType;

    public float moveSpeed;

    public enum DamageType
    {
        none,
        physical,
        magical,
        holy
    }

}
