using _Core._Scripts;
using UnityEngine;
using UnityEngine.UI;
//using UpgradeManager;

public class UpgradeText : MonoBehaviour
{
    public UpgradeManager manager;
    Text textField1;
    Text textField2;
    Text textField3;
    public string PickInfo;
    public string DrillInfo;
    public string BombInfo;

    // Start is called before the first frame update
    void Start()
    {
        textField1 = GameObject.Find("PickCost").GetComponent<Text>();
        textField2 = GameObject.Find("DrillCost").GetComponent<Text>();
        textField3 = GameObject.Find("BombCost").GetComponent<Text>();
    }

    public void changeText()
    {
        //text on pick axe
        if (manager.itemLevel == 0)
        {
            textField1.text = "Amethyist Pickaxe: \n DMG +1";
        }
        else if (manager.itemLevel == 1)
        {
            textField1.text = "Emerald Pickaxe: \n DMG +2";
        }
        else if (manager.itemLevel == 2)
        {
            textField1.text = "Ruby Pickaxe: \n DMG +3";
        }
        else if (manager.itemLevel == 3)
        { 
            textField1.text = "Diamond Pickaxe: \n DMG +4"; 
        }
        else { textField1.text = "MAX LEVEL"; }
//-----------------------------------------------------------------
//change text on drill box

        if (manager.itemLevel == 0)
        {
            textField2.text = "Drill Bot: \n total +1";
        }
        else if (manager.itemLevel == 1)
        {
            textField2.text = "Drill Bot: \n total +2";
        }
        else if (manager.itemLevel == 2)
        {
            textField2.text = "Drill Bot: \n total +3";
        }
        else { textField1.text = "MAX LEVEL"; }

 //-----------------------------------------------------------------
 //change text on bomb bot

        if (manager.itemLevel == 0)
        {
            textField3.text = "Bomb Bot: \n DMG +1";
        }
        else if (manager.itemLevel == 1)
        {
            textField3.text = "Bomb Bot: \n DMG +2";
        }
        else if (manager.itemLevel == 2)
        {
            textField3.text = "Bomb Bot: \n DMG +3";
        }
        else { textField1.text = "MAX LEVEL"; }
    }
    
}
