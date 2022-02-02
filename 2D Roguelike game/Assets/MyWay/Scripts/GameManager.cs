using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Static instance of GameManeger wich allows it to be accessed by other
    public static GameManager instance = null;
//Store a reference to our BoardManger wich will set up the level
   public BoardManager boardScript;
// Current level number, expressed in game as "Day"
   private int level = 3;


    // Awake is always called before any Start 
    void Awake()
    {
        //Check if instance alrady exists
        if(instance == null)
        // if not, set instance to this
        instance = this;
        //if instance alrady exists and it's not this
        else if (instance != this)
        // Then destroy this
        Destroy (gameObject);
        //Sets this to not be destroes when reloading scene
        DontDestroyOnLoad(gameObject);
        //Get a componenet reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();
        //Call the InitGame function to initializa the first level
        InitGame();
        
    }

    //Initialixes the game for each level
    void InitGame()
   {
       //Call the SetupScene function og the BoardManager script, pass it current level number
       boardScript.SetupScene(level);
   }

    // Update is called once per frame
    void Update()
    {
        
    }
}
