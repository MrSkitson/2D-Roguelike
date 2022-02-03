using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    //Alternate sprite to display after Wall has been attacked by player
public Sprite dmgSprite;
    //hit points for the wall
public int hp = 4;
//Store a component reference to the attached SpriteRenderer
private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        //Get a component reference to the SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer> ();
    }
    // Called when the player attacks a wall
    public void DamageWall (int loss)
    {
        //Set spriterenderer to the damaged wall sprite
        spriteRenderer.sprite = dmgSprite;
        //subtract loss from hit total
        hp -= loss;
        //If hit points are less than or equel to zero
        if(hp <= 0)
        //Desable the gameObject
        gameObject.SetActive(false);
    }
}
