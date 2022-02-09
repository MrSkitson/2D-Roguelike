using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Delay between each Player turn
    public float turnDelay = .1f;
    //Static instance of GameManeger wich allows it to be accessed by other
    public static GameManager instance = null;
//Store a reference to our BoardManger wich will set up the level
    public BoardManager boardScript;
//Current playerFoodPoints score
    public int playerFoodPoints = 100;
//incapsulatuon 
    [HideInInspector] public bool playersTurn = true;
// Current level number, expressed in game as "Day"
   private int level = 3;
    //List of all Enemy units, used to issue them move commands
   private List<Enemy> enemies;
    //Boolean to cjeck if enemies are moving
   private bool enemiesMoving;


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
        //Get a component reference to attached BoardManager script
        enemies = new List<Enemy>();

        //Get a componenet reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();
        //Call the InitGame function to initializa the first level
        InitGame();
        
    }

    //Initialixes the game for each level
    void InitGame()
   {
        //Clear ane Ene,y objects in our List to prepare for next level
       enemies.Clear();
       //Call the SetupScene function og the BoardManager script, pass it current level number
       boardScript.SetupScene(level);
   }

    public void GameOver()
    {
        enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        //Check that playerTurn or enemiesMoving or doingSetup are not currently true
        if (playersTurn || enemiesMoving)
        //If eny of these are true, return and do not start MoveEnemies
        return;

        //Start moving enemies
        StartCoroutine(MoveEnemies());
        
    }

    //Call this to add the passed in Enemy to the List of Enemy objects
    public void AddEnemyToList(Enemy script)
    {
        //Add Enemy to List enemies
        enemies.Add(script);
    }

    //Coroutine to move enemies in sequence
    IEnumerator MoveEnemies()
    {
        //While enemiesMoving is true player is unable to move
        enemiesMoving = true;
        //Wait for tirnDelay seconds, defaults to .1 (100ms)
        yield return new WaitForSeconds(turnDelay);
        //If there are no enemies spawned (IE in first level)
        if(enemies.Count == 0)
        {
            //Wait for turnDelay seconds between moves, replaces delay caused by enemies moving there are none
            yield return new WaitForSeconds(turnDelay);
        }
        //Loop through List of Enemy objects
        for (int i = 0; i < enemies.Count; i++)
        {
            //Call the MoveEnemy function of Enemy at index i in th enemies List
            enemies[i].MoveEnemy ();
            //Wait for Enemy's moveTime before moving next Enemy
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        //Once Ene,ies are done moving, set playerTirn to true, so player can move
        playersTurn = true;
        //Enemies are done moving, set enemiesMoving to false
        enemiesMoving = false;
    }
}
