using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour

{
    public GameObject gameManager; //GameManager prefab to instantiate

    
    void Awake()
    {
        //Check if a GameManager has alrady been assigned to static varible or it's still null
        if (GameManager.instance == null)
        //Instantiate gameManger prefab
        Instantiate(gameManager);
    }

    
}
