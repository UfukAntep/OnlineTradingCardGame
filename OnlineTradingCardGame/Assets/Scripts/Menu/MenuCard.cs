using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "MenuCard")]
public class MenuCard : ScriptableObject
{
    public new string name;

    public Sprite artwork;

    public int sceneNumber;

}
