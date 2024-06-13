using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingNameMaker : MonoBehaviour
{
    private Dictionary<BuildingTag.buildingName, string> tagDict = new Dictionary<BuildingTag.buildingName, string>() {
    {BuildingTag.buildingName.warrior, "Воин"},    
    {BuildingTag.buildingName.shieldWarrior, "Щитоносец"},    
    {BuildingTag.buildingName.archery, "Лучник"},    
    {BuildingTag.buildingName.rogue, "Плут"},    
    {BuildingTag.buildingName.catapult, "Катапульта"},    
    {BuildingTag.buildingName.battleMage, "Боевой маг"},    
    {BuildingTag.buildingName.fireMage, "Маг огня"},    
    {BuildingTag.buildingName.iceMage, "Маг льда"},    
    {BuildingTag.buildingName.monk, "Монах"},    
    {BuildingTag.buildingName.paladin, "Паладин"},    
    {BuildingTag.buildingName.pastor, "Пастырь"},    
    };

    
    private void Start()
    {
        var buildingText = tagDict[GetComponent<BuildingTag>().tagName];
        GetComponentInChildren<TMP_Text>().text = buildingText;
    }
}
