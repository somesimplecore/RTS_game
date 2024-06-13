using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingNameMaker : MonoBehaviour
{
    private Dictionary<BuildingTag.buildingName, string> tagDict = new Dictionary<BuildingTag.buildingName, string>() {
    {BuildingTag.buildingName.warrior, "����"},    
    {BuildingTag.buildingName.shieldWarrior, "���������"},    
    {BuildingTag.buildingName.archery, "������"},    
    {BuildingTag.buildingName.rogue, "����"},    
    {BuildingTag.buildingName.catapult, "����������"},    
    {BuildingTag.buildingName.battleMage, "������ ���"},    
    {BuildingTag.buildingName.fireMage, "��� ����"},    
    {BuildingTag.buildingName.iceMage, "��� ����"},    
    {BuildingTag.buildingName.monk, "�����"},    
    {BuildingTag.buildingName.paladin, "�������"},    
    {BuildingTag.buildingName.pastor, "�������"},    
    };

    
    private void Start()
    {
        var buildingText = tagDict[GetComponent<BuildingTag>().tagName];
        GetComponentInChildren<TMP_Text>().text = buildingText;
    }
}
