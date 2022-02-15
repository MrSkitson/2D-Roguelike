using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    //Delay between starting new level, int seconds
    public float levelStartDelay = 2f;
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

    //Text to desplay current level number
    private Text levelText;
    //Image to block out level as levels are being set
    private GameObject levelImage;
    // Current level number, expressed in game as "Day"
   private int level = 1;
    //List of all Enemy units, used to issue them move commands
   private List<Enemy> enemies;
    //Boolean to cjeck if enemies are moving
   private bool enemiesMoving;
    //Boolean to check if we're setting up board, prevent Player from during setup
   private bool doingSetup;


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

    //This is called each time a scene is loaded
    void OnLevelWasLoaded(int index)
    {
        //Add one to our level number
        level++;
        //Call InitGame to initialize our level
        InitGame();
    }

    //Initialixes the game for each level
    void InitGame()
   {
       //While doingSetup is true the player can't move, prevent player from moving while title card is up
       doingSetup = true;
       //Get a reference to our image LevelImage by finding it by name 
       levelImage = GameObject.Find("LevelImage");
        //Get a reference to our text LevelText's text component by finding it by name and calling GetComponnent
       levelText = GameObject.Find("LevelText").GetComponent<Text>();
        //Set the text of levelText to the string "Day" and append the current level number
       levelText.text = "Day " + level;
        //Set levelImage to active blocking player's view of the game board during setup
       levelImage.SetActive(true);
        //Call the HideLevelImage function with a delay in seconds
        Invoke("HideLevelImage", levelStartDelay);
        //Clear ane Ene,y objects in our List to prepare for next level
       enemies.Clear();
       //Call the SetupScene function og the BoardManager script, pass it current level number
       boardScript.SetupScene(level);
   }

    //Hides black image used between levels
   private void HideLevelImage()
   {
       //Disable the levelImage gameObject
       levelImage.SetActive(false);
        //Set doingSetup to false allowing player to move again
       doingSetup = false;
   }

    public void GameOver()
    {   
        //Set levelText to display number of levels passed and game over message
        levelText.text = "After " + level + " days, you starved.";
        //Enable black background image gameObject
        levelImage.SetActive(true);
        //Disable this GameManager
        enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        //Check that playerTurn or enemiesMoving or doingSetup are not currently true
        if (playersTurn || enemiesMoving || doingSetup)
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
