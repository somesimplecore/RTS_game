using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTag : MonoBehaviour
{
    public buildingName tagName;
    public enum buildingName
    {
        townHall,
        farm,
        woodcutter,
        stoneQuarry,
        mine,
        church,
        warrior,
        shieldWarrior,
        archery,
        rogue,
        catapult,
        battleMage,
        fireMage,
        iceMage,
        monk,
        paladin,
        pastor,
    }
}
