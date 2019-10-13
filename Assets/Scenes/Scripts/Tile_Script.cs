using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Script : MonoBehaviour
{
    public bool isMined = false;
    public Material materialIdle;
    public Material materialLightUp;
    public Material uncovered;
    public TextMesh textPrefab;
    private TextMesh mineText;
    public int ID;
    public int tilesPerRow;
    public List<Tile_Script> adjacentTiles;
    public int adjacentMines = 0;
    public string state = "idle";
    public bool covered = true;
    public bool winTile = false;

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

    public bool isFlagged; // if the tile is flagged or not
    public GameObject flagPrefab; // the flag prefab
    private GameObject flag;
    

    // Start is called before the first frame update
    void Start()
    {
        initiateTiles();
        CountMines();
        flag = Instantiate(flagPrefab, transform.position + flagPrefab.transform.position, flagPrefab.transform.rotation);
        mineText = Instantiate(textPrefab, transform.position + textPrefab.transform.position, textPrefab.transform.rotation);
        mineText.GetComponent<MeshRenderer>().enabled = false;
        flag.GetComponent<SpriteRenderer>().enabled = isFlagged;
    }

    public void UncoverTile()
    {

        if (this.isMined)
        {
            mineText.text = "B";
        }
        else
        {
            mineText.text = adjacentMines.ToString();
            covered = false;
            if (adjacentMines == 0)
            {
                mineText.text = "";
                UncoverAdjacentTiles();
            }
            if (!winTile)
            {
                this.GetComponent<MeshRenderer>().material = uncovered;
            }
        }
        mineText.GetComponent<MeshRenderer>().enabled = true;

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
        if (covered && !winTile)
        {
            gameObject.GetComponent<MeshRenderer>().material = materialLightUp;
        }

        if(Input.GetMouseButtonUp(1))
        {
            RenderFlag();
        }
        
    }

    private void OnMouseExit()
    {
        if (covered && !winTile)
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
        }
    }

    public void RenderFlag()
    {
        // spawn flag on top of tile when right click
        isFlagged = !isFlagged;
        flag.GetComponent<SpriteRenderer>().enabled = isFlagged;

    }

    private void UncoverAdjacentTiles()
    {
        foreach(Tile_Script currentTile in adjacentTiles)
        {
            //uncover all adjacent nodes with 0 adjacent mines
            if (!currentTile.isMined && currentTile.covered && currentTile.adjacentMines == 0)
            {
                currentTile.UncoverTile();
            }
            //uncover all adjacent nodes with more than 1 adjacent mine, then stop uncovering
            else if (!currentTile.isMined && currentTile.covered && currentTile.adjacentMines > 0)
            {
                currentTile.UncoverTileExternal();
            }
        }
    }

    private void UncoverTileExternal()
    {
        mineText.text = adjacentMines.ToString();
        mineText.GetComponent<MeshRenderer>().enabled = true;
        covered = false;
        if (!winTile)
        {
            this.GetComponent<MeshRenderer>().material = uncovered;
        }
    }
    
}


