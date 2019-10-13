using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public PlayerControl player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTileClick(Tile_Script t)
    {
        Debug.Log("CLICK @ " + t.coord);

        // take the coordinate and decide if the player should move
        if (IsValidMove(player.coord, t.coord))
        {
            // the move is valid, so make it happen
            player.MoveToTile(t);
        }
    }

    bool IsValidMove(Vector2Int playerCoord, Vector2Int tileCoord)
    {
        float calcRad = (playerCoord - tileCoord).magnitude;

        Debug.Log((playerCoord - tileCoord).magnitude);

        return (calcRad <= player.moveDistance); // if the wanted movement is withing or equal to the max distance allowed to travel, then return true to move
    }
}
