using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy inherits from MovingPObject, our base class
public class Enemy : MovingObject
{
    //The amount of food points to subtract from the player when attacking
    public int playerDamage;
    //Variable of type Animator to store a reference to the enemy's Animator component
    private Animator animator;
    //Transforn to attempt to move toward each turn
    private Transform target;
    //Bollean to determine whether or not ene,y shoulf skip turn or move this turn
    private bool skipMove;


    // Start is called before the first frame update
    void Start()
    {  //Reguster this enemy with our instance of GameManager by adding it to a list of Enemy
        //This allows the GameManager to issue movements commands
        GameManager.instance.AddEnemyToList (this);
        //Get and store a reference to the attached Animator component
        animator = GetComponent<Animator> ();
        //Find the Player using it's tag and store a reference to this transform component
        target = GameObject.FindGameObjectWithTag ("Player").transform;
        //Call the start fuction of our base class MovingObject
        base.Start ();
    }

    //Override function of MovingObject to include functionality needed for Enemy to skip turns
    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        //Check if skipMove is true, if so set ot to false and skip this turn
        if(skipMove)
        {
            skipMove = false;
            return;
        }
        //Call the AttenptMove fuction from MovingObject
        base.AttemptMove <T> (xDir, yDir);
        //Now that Enemy has moved. set skipMove to true to skip next move
        skipMove = true;
    }

    public void MoveEnemy ()
    {
        //Declare variables for X and Y axis move directions, the range from -1 to 1, this values allow us to choose bewtween the cardinal directions
        int xDir = 0;
        int yDir = 0;

        //If the difference in positions is approximately zero do the following
        if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
        //If the Y coordinate of the target's(player) position of greater than the Y coordinate of this enemy's position set Y direction 1(to move up). Is not, set it to -1(to move down)
        yDir = target.position.y > transform.position.y ? 1 : -1;
        //IF the difference in posotions is not  approximately zero  do the following
        else 
        //Check if target x position is greater than enemy's X position, if so set X to 1(move right), if not set  to -1( move left)
        xDir = target.position.x > transform.position.x ? 1 : -1;
        //Call the AttemptMove fuction and pass in the generic parametr Player, because Enemy is moving and expecting to potentially encounter a Player
        AttemptMove <Player> (xDir, yDir);

    }

    //OnCantMove is called if Enemy attempts to move into a space accupied by a Player, it overrides and takes a generic parametr T wich we use to pass  in the component we expect to encounter
    protected override void OnCantMove <T> (T component)
    {
        //Declare hitPlayer and set to equal the encountered component
        Player hitPlayer = component as Player;
        //Call the LoseFood fuction of hitPlayer passing it playerDamage, the amount of foodpoints to be subtracted
        hitPlayer.LoseFood (playerDamage);
        //Set the attack trigger of animator to trigger Ene,y attack animation
        animator.SetTrigger("enemyAttack")
    }
    
}
