using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Player inherits from MovingObject, our bas class for objects that can move
public class Player : MovingObject
{
    //Delay time in seconds to restart level
    public int wallDamage = 1;
    //Number of points to add to player food points when picking up a food object
    public int pointsPerFood = 10;
    //Number of points to add to player food point when picking up soda object
    public int pointsPerSoda = 20;
    //How much damage a player does to a wall when chopping it
    public float restartLevelDelay = 1f;

    //Used to store a refrence to the Player's animator component
    private Animator animator;
    //Used to store player food points total during level
    private int food;

    // Start overrides the Start function
    protected override void Start()
    {
        //Get a component reference to the Player's animator component
        animator = GetComponent<Animator>();
        
        //Get the current food point total stored in GameManager.instance between levels
        food = GameManager.instance.playerFoodPoints;

        //Call the Start fuction of the MovingObject base class
        base.Start ();
    }

    //This function os called when the behaviour becomes disabled or inactive
    private void OnDisable ()
    {
        //When Player object is disabled, store the current local food total in GameManegr so it can re-loaded in next level
        GameManager.instance.playerFoodPoints = food;
    }


    // Update is called once per frame
    void Update()
    {
        //If it's not the player's turn, exit the function
        if(!GameManager.instance.playersTurn) return;
        
        int horizontal = 0;//USed to store the horizontal move diraction
        int vertical = 0; //USed to store the vertical move diraction

        //Get input from the maneger, round it to an integer and store in horizontal to set a axis move direction
        horizontal = (int) (Input.GetAxisRaw ("Horixontal"));
        //Get input from the manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int) (Input.GetAxisRaw ("Vertical"));

        //Check if moving horizontally, if so set vertical to zero
        if(horizontal !=0)
        {
            vertical = 0;
        }

        //Check if we have a non-zero value for horizontal or vertical
        if(horizontal != 0 || vertical != 0)
        {
            //Call Attempt Move passing in the generetic parametr Wall. Pass in horizontal and vertial as parametrs to specify the diractionto move Player in
            AttemptMove<Wall> (horizontal, vertical);
        }
        
    }

    //AttemtMove overrides on BAse class and takes a generic parametr T wich for Player will be of the type Wall, it also takes integers for x and y directions to move in
    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        //Every time player moves, substract from food points total
        food --;
        //Call the AttemptMove method of the vase clase, passing in the component T(Wall) and x and y direction to move 
        base.AttemptMove <T> (xDir, yDir);
        //Hit allows us to reference the result of the Linecast done in Move
        RaycastHit2D hit;
        //Since the player has moved and lost food points, check if the game has ended
        CheckIfGameOver ();
        //Since the playersTurn boolean of GameManager to false now that players turn is over
        GameManager.instance.playersTurn = false;
    }

    //Sent when another object enters a trigger collider attached to this object 
    private void OnTriggerEnter2D (Collider2D other)
    {
        //Check if the tag of the trigger collded with is Exit
        if(other.tag == "Exit")
        {
            //Invoke teh Respart fuction to start the next level with a delay
            Invoke ("Restart", restartLevelDelay);
            //Disable the player object since level is over
            enabled = false;
        } 
        //Check if the tag of the trigger collided with is Food
        else if (other.tag == "Food")
        {
            //Add pointsPerFood to the players current food total
            food += pointsPerFood;
            //Disable the food object the player collided with
            other.gameObject.SetActive (false);
        }
        //Check if the tah of the trigger collided with is Soda
        else if (other.tag == "Soda")
        {
            //Add pointsPerSoda to players food points total
            food += pointsPerSoda;
            //Disable the soda object the player collided with
            other.gameObject.SetActive (false);
        }
    }

    //Overrides the abstract function, it takes a generic parametrT wich in the case of Player is a Wall
    protected override void OnCantMove <T> (T component)
    {
        //Set hitWall to equal the component passed in as a parametr
        Wall hitWall = component as Wall;
        //Call the DamageWall function of the Wall we are hitting
        hitWall.DamageWall (wallDamage);
        //Set the attack trigger og yhe player's animation controller
        animator.SetTrigger ("PlayerChop");

    }

    //Restart reloads the scene when called
    private void Restart ()
    {
        Application.LoadLevel(Application.loadedLevel);
        //SceneManager.LoadScene (0);
    }

    //Called when an enemy attacks the player, it takes a parametr loss which specifoes how many pointes to lose
    public void LoseFood (int loss)
    {
        //Set the trigger for the player animator to transition animation
        animator.SetTrigger ("playerHit");
        //Subtract lost food points from players total
        food -= loss;
        //Check to see if game has ended
        CheckIfGameOver ();
    }

    //Check is the player os out of food points and if so, ends the game
    private void CheckIfGameOver()
    {
        //Check if food point total is less than or uqual to zero
        if(food <= 0)
        {
            //Call the GamveOver function 
            GameManager.instance.GameOver ();
        }

    }

}
