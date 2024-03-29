﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public PlayerControl player;
    public bool aboutToDie;
    public bool aboutToWin;
    public Tile_Script activeTile;

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
            //GameObject.Find("Tile").GetComponent<Tile_Script>().TileFall();
            activeTile.TileFall();
        }
    }
    public void OnTileClick(Tile_Script t)
    {
        Debug.Log("CLICK @ " + t.coord);

        // take the coordinate and decide if the player should move
        if (IsValidMove(player.coord, t.coord) && (!aboutToDie) && (!aboutToWin))
        {
            // the move is valid, so make it happen
            player.MoveToTile(t);
            activeTile = t; // refrencing the tile that is the location of the player

            if (t.isMined)
            {
                aboutToDie = true; // detects if the tile the player is moving to is a bomb
                Invoke("LoadGameEnd", 4);
            } else if(t.winTile)
            {
                aboutToWin = true;
                Invoke("LoadGameWin", 1);
            }
            else {
                t.UncoverTile();
            }
        }

    }

    bool IsValidMove(Vector2Int playerCoord, Vector2Int tileCoord)
    {
        float calcRad = (playerCoord - tileCoord).magnitude;

        return (calcRad <= player.moveDistance); // if the wanted movement is withing or equal to the max distance allowed to travel, then return true to move
    }

    void LoadGameEnd()
    {
        SceneManager.LoadScene("EndScene");
    }
    
    void LoadGameWin()
    {
        SceneManager.LoadScene("WinScene");
    }
}
