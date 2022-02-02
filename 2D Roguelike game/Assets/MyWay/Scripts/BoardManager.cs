using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// Tells Random to use  th Unity Engine random number generator
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    // use Serializable give us encapsulation
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;         //Number of columns in game board     
    public int rows = 8;            //Number of rows in game board        
    public Count wallCount = new Count(5,9);            //Lower and upper limit for our random number of walls per level.             
    public Count foodCount =new Count(1,5);             //Lower and upper limit for our random number of food per level. 
    public GameObject exit;         //Prefab to spawn for exit  
    public GameObject[] floorTiles;         //Array of floor prefabs
    public GameObject[] wallTiles;           //Array of wall  prefabs
    public GameObject[] foodTiles;          //Array of food prefabs
    public GameObject[] enemyTiles;         //Array of enemy prefabs
    public GameObject[] outerWallTiles;         //Array of outer tile prefabs

    //A varible to store a reference to the transform of our Board object
    private Transform boardHolder;
    // A list of possible locations to place tiles.
    private List <Vector3> gridPositions = new List<Vector3> ();

//Clears out list gridPositions and prepares it to generate a new board
    void InitialisateList ()
    {
        // clear list gridPositions
        gridPositions.Clear ();
        //Loop through x axis 
        for(int x = 1; x < columns - 1; x++)
        {
            //Within each colum, Loop through y axis
            for(int y = 1; y < rows - 1; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position
                gridPositions.Add (new Vector3(x, y, 0f));
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}