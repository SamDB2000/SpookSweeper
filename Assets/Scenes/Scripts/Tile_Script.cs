using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Script : MonoBehaviour
{
    public bool isMined = false;
    public Material materialIdle;
    public Material materialLightUp;
    public Material uncovered;
    public TextMesh displayText;
    public int ID;
    public int tilesPerRow;
    public List<Tile_Script> adjacentTiles;
    public int adjacentMines = 0;
    public string state = "idle";
    public bool covered = true;

    public Tile_Script tileUpper;
    public Tile_Script tileLower;
    public Tile_Script tileLeft;
    public Tile_Script tileRight;

    public Tile_Script tileUpperRight;
    public Tile_Script tileUpperLeft;
    public Tile_Script tileLowerRight;
    public Tile_Script tileLowerLeft;

    public Vector2Int coord; // the coordinates of the tile
    public delegate void TileClicked(Tile_Script t); // passes the event to the listeners of the delgate
    public event TileClicked tileClickedEvent; // creates the event

    public float shrinkRate; // the rate the player shrinks when dead
    public float speedRot; // speed of roation for DEATH

    // Start is called before the first frame update
    void Start()
    {
        initiateTiles();
        CountMines();
    }

    public void UncoverTile()
    {
        
        var mineText = Instantiate(displayText, new Vector3(transform.position.x, transform.position.y, transform.position.z - 5), displayText.transform.rotation);
        if (this.isMined)
        {
            mineText.text = "B";
        }
        else
        {
            mineText.text = adjacentMines.ToString();
            if (adjacentMines <= 0)
            {
                mineText.text = "";
            }
            covered = false;
            this.GetComponent<MeshRenderer>().material = uncovered;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp()
    {
        tileClickedEvent(this); // sends the object as the event
    }

    private void initiateTiles()
    {
        /* Gets all of the neighboring tiles and stores them */
        if (InBounds(Grid_Script.tilesAll, ID + tilesPerRow))                      tileUpper = Grid_Script.tilesAll[ID + tilesPerRow]; //sets tileUpper
        if (InBounds(Grid_Script.tilesAll, ID - tilesPerRow))                      tileLower = Grid_Script.tilesAll[ID - tilesPerRow]; //sets tileLower ... other lines do the same
        if (InBounds(Grid_Script.tilesAll, ID - 1) && ID % tilesPerRow != 0)       tileLeft = Grid_Script.tilesAll[ID - 1];
        if (InBounds(Grid_Script.tilesAll, ID + 1) && (ID + 1) % tilesPerRow != 0) tileRight = Grid_Script.tilesAll[ID + 1];

        if (InBounds(Grid_Script.tilesAll, ID + tilesPerRow + 1) && (ID + 1) % tilesPerRow != 0)    tileUpperRight = Grid_Script.tilesAll[ID + tilesPerRow + 1];
        if (InBounds(Grid_Script.tilesAll, ID + tilesPerRow - 1) &&  ID % tilesPerRow != 0)         tileUpperLeft = Grid_Script.tilesAll[ID + tilesPerRow - 1];
        if (InBounds(Grid_Script.tilesAll, ID - tilesPerRow + 1) && (ID + 1) % tilesPerRow != 0)    tileLowerRight = Grid_Script.tilesAll[ID - tilesPerRow + 1];
        if (InBounds(Grid_Script.tilesAll, ID - tilesPerRow - 1) &&  ID % tilesPerRow != 0)         tileLowerLeft = Grid_Script.tilesAll[ID - tilesPerRow - 1];
        /* InBounds function is the reason this doesn't vomit errors */

        // Add all of the other tiles into a list of adjacent tiles (if they exist)
        if (tileUpper) adjacentTiles.Add(tileUpper);
        if (tileLower) adjacentTiles.Add(tileLower);
        if (tileRight) adjacentTiles.Add(tileRight);
        if (tileLeft) adjacentTiles.Add(tileLeft);
        if (tileUpperRight) adjacentTiles.Add(tileUpperRight);
        if (tileUpperLeft) adjacentTiles.Add(tileUpperLeft);
        if (tileLowerRight) adjacentTiles.Add(tileLowerRight);
        if (tileLowerLeft) adjacentTiles.Add(tileLowerLeft);
    }

    private void OnMouseOver()
    {
        if (covered)
        {
            gameObject.GetComponent<MeshRenderer>().material = materialLightUp;
        }
        
    }

    private void OnMouseExit()
    {
        if (covered)
        {
            gameObject.GetComponent<MeshRenderer>().material = materialIdle;
        }
    }

    private bool InBounds(List<Tile_Script> tilesAll, int targetID)
    {
        if ((targetID < 0) || (targetID >= tilesAll.Count))
        {
            return false;
        }
        return true;
    }

    // Counts the surrounding mines and updates the displayText for the tile
    void CountMines()
    {
        adjacentMines = 0;

        foreach (Tile_Script currentTile in adjacentTiles)
        {
            if (currentTile.isMined) adjacentMines += 1;
        }
    }

    public void TileFall()
    {
        if (isMined)
        {
            transform.Rotate(Vector3.forward, 55 * Time.deltaTime * speedRot);
            transform.localScale *= shrinkRate;
            Debug.Log("ded");
        }
    }

}


