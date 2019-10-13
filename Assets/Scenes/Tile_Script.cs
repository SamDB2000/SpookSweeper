using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Script : MonoBehaviour
{
    public bool isMined = false;
    public Material materialIdle;
    public Material materialLightUp;
    public TextMesh displayText;


    public Vector2Int coord; // the coordinates of the tile

    public delegate void TileClicked(Tile_Script t); // passes the event to the listeners of the delgate

    public event TileClicked tileClickedEvent; // creates the event

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp()
    {
        tileClickedEvent(this); // sends the object as the event
    }

}


