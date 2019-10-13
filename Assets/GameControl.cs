using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public PlayerControl player;
    public bool aboutToDie;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (aboutToDie && !player.IsMoving)
        {
            // invoke death
            player.KilLBadPlayer();
            //Tile_Script.TileFall();
        }
    }
    public void OnTileClick(Tile_Script t)
    {
        Debug.Log("CLICK @ " + t.coord);

        // take the coordinate and decide if the player should move
        if (IsValidMove(player.coord, t.coord) && (!player.isDed))
        {
            // the move is valid, so make it happen
            player.MoveToTile(t);

            if (t.isMined)
            {
                aboutToDie = true; // detects if the tile the player is moving to is a bomb
            }
        }

    }

    bool IsValidMove(Vector2Int playerCoord, Vector2Int tileCoord)
    {
        float calcRad = (playerCoord - tileCoord).magnitude;

        return (calcRad <= player.moveDistance); // if the wanted movement is withing or equal to the max distance allowed to travel, then return true to move
    }

    
}
