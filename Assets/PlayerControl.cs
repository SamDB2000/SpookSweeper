using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //create public coordinate of the player
    public Vector2Int coord;
    public int moveDistance; // the max radius of the movement of the player
    public float moveRate; // the speed the player moves to tiles
    private Vector3 finalCoord;
    public bool IsMoving => (transform.position != finalCoord);
    public bool isDed; // is the player dead? idk, check this boolean out!
    public float shrinkRate; // the rate the player shrinks when dead
    public float speedRot; // speed of roation for DEATH

    // Start is called before the first frame update
    void Start()
    {
        finalCoord = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currPos = transform.position; // finding the current possition of the player
        Vector3 posDiff = finalCoord - currPos;
        Vector3 movement = posDiff.normalized * moveRate; // changing the players location to move at a constant rate towards the new tile
        if(posDiff.magnitude < moveRate)  // testing if the movement over shoots the new location
        {
            transform.position = finalCoord; // jump to end
        } else
        {
            transform.Translate(movement); // move
        }

        if (isDed)
        {
            transform.Rotate(Vector3.forward, 55 * Time.deltaTime * speedRot);
            transform.localScale *= shrinkRate;
            
        }

    }

    public void MoveToTile(Tile_Script t)
    {
        coord = t.coord;
        finalCoord = t.transform.position;
    }

    public void KilLBadPlayer()
    {
        isDed = true;
    }
}
