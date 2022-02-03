using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;   //Time it will take object to move, in seconds.
    public LayerMask blockingLayer; //Layer on which collision will be checked.
    private BoxCollider2D boxCollider;  //The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;           //The Rigidbody2D component attached to this object.
    private float inverseMoveTime;      //Used to make movement more efficient.


    //Protected, virtual functions can be overridden by inheriting classes.
    protected virtual void Start()
    {
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();
        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent<Rigidbody2D>();
        //By storing the reciprocal of the move time 
        inverseMoveTime = 1f / moveTime;
    }

        // Move returns bool value and takes parametrs for x/Y directions, and RaycastHid 2D to check collision
    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        //Store start position to move from
        Vector2 start = transform.position;
        //Calculate end position
        Vector2 end = start + new Vector2 (xDir, yDir);
        //Disable the boxcollider so that linecast doesn't hit this object's own collider
        boxCollider.enabled = false;
        //Cast a line from start point to end point cheking collision on blockinglayer
        hit = Physics2D.Linecast (start, end, blockingLayer);
        //Re-enable boxCollider after linecast
        boxCollider.enabled = true;

        //Check if anything was hit
        if(hit.transform == null)
        {
            //If nothing was hit, start method co-routine passing in the Vector2 end as destantion
            StartCoroutine (SmoothMovement (end));
            // Say that Move was successful
            return true;
        }
        //If smth was hit, move was unsuccessful 
        return false;
    }

    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to
    protected IEnumerator SmoothMovement (Vector3 end)
    {
        //Calculate the remaiming distance to movee based on the square magnitude of the dofference
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //while that distance is greater than a very small amount(almost Zero)
        while(sqrRemainingDistance > float.Epsilon)
        {
            //Found a new position proportonally closer to the end, based on the moveTime
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            //Call MovePosition on attached Rigidbody2D and move it
            rb2D.MovePosition (newPosition); 
            //Recalcelate the remaining distance after moving
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            //Return and  end the function
            yield return null;
        }
    }

    //The virtual keyword means AttemptMove can be overridden by inheriting abstract classes 
    // takes a generic parametr T to specify the type of component we expect our unit to interact with if blocked
    protected virtual void AttemptMove <T> (int xDir, int yDir)
    where T : Component
    {
        //Hit will store whatever our linecast hits when Move is called
        RaycastHit2D hit;
        //Set canMove to true if Move is was successful
        bool canMove = Move (xDir, yDir, out hit);
        
        //Check if nothing was hit by linecast
        if(hit.transform == null)
        //If nothing was hit, return and don't execute
        T hitComponent = hit.transform.GetComponent <T> ();

        //if canMove is false and hitCompomemt is not equel to null, meaning MovingObject is blocked and has hit smth
        if(!canMove && hitComponent != null)
        //Call the function and pass it hitComponent as a parameter
        OnCantMove (hitComponent);

    }

    //The abstract modifer OnCantMove will be overriden bt functions in the inheriring classes
     protected abstract void OnCantMove <T> (T component)
        where T : Component;
}
