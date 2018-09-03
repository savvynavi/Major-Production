using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour {

  private static FloatingText popupText;            //working
    private static FloatingText damageEnemyText;    //tests for various different anims
    private static FloatingText damageAllyText;
    private static FloatingText missText;
    private static FloatingText healEnemyText;
    private static FloatingText healAllyText;
    private static GameObject canvas;

   
    // In controller for combat scene, need to call following functions on start.  
    public static void DamageEnemy() 
    {
        canvas = GameObject.Find("Canvas");
        if (!damageEnemyText)
            damageEnemyText = Resources.Load<FloatingText>("Prefabs/FeedbackText/ParentObjects/DamageEnemyTextParent");
    }
    public static void DamageAlly()
    {
        canvas = GameObject.Find("Canvas");
        if (!damageAllyText)
            damageAllyText = Resources.Load<FloatingText>("Prefabs/FeedbackText/ParentObjects/DamageAllyTextParent");
    }
    public static void Miss()
    {
        canvas = GameObject.Find("Canvas");
        if (!missText)
            missText = Resources.Load<FloatingText>("Prefabs/FeedbackText/ParentObjects/MissTextParent");
    }
    public static void HealEnemy()
    {
        canvas = GameObject.Find("Canvas");
        if (!healEnemyText)
            healEnemyText = Resources.Load<FloatingText>("Prefabs/FeedbackText/ParentObjects/HealEnemyTextParent");
    }
    public static void HealAlly()
    {
        canvas = GameObject.Find("Canvas");
        if (!healAllyText)
            healAllyText = Resources.Load<FloatingText>("Prefabs/FeedbackText/ParentObjects/HealAllyTextParent");
    }




    // In script that calculates effect, need the following line:
    //FloatingTextController.Create[RELEVANT]Text(amount.ToString(), transform);

    public static void CreateDamageEnemyText(string text, Vector2 location) { CreateText(text, location, damageEnemyText);  }
    public static void CreateDamageAllyText(string text, Vector2 location) { CreateText(text, location, damageAllyText); }
    public static void CreateMissText(string text, Vector2 location) { CreateText(text, location, missText); }
    public static void CreateHealEnemyText(string text, Vector2 location) { CreateText(text, location, healEnemyText); }
    public static void CreateHealAllyText(string text, Vector2 location) { CreateText(text, location, healAllyText); }



    public static void CreateText(string text, Vector2 location, FloatingText prefab)
    {

        if (prefab == null)
        {
            Debug.Log("null prefab");
            return;
        }
        // clone the game object, make it a sibling of the prefab
        GameObject go = Instantiate(prefab.gameObject);
		//go.transform.parent = prefab.transform.parent;
		go.transform.SetParent(prefab.transform.parent);
        go.SetActive(true);
        FloatingText instance = go.GetComponent<FloatingText>();

		Vector2 randOffset = new Vector2(Random.Range(-.1f, .1f), Random.Range(-.1f, .1f));
		Vector2 screenPosition = new Vector2(location.x, location.y) + randOffset;



        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
        instance.SetText(text);
    }
}
