using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Script : MonoBehaviour
{
    public Tile_Script tilePrefab;
    public int numTiles = 12;
    public float distBetweenTiles = 2.0f;
    public int tilesPerRow = 9;
    public static List<Tile_Script> tilesAll;
    public static List<Tile_Script> tilesMined;
    public static List<Tile_Script> tilesUnmined;
    public float percentMines = 0.10f;
    public GameControl gameController; // link GameControl to grid
    public Material winTileMaterial;
    public Tile_Script winTile;

    // Start is called before the first frame update
    void Start()
    {
        CreateTiles();
        
    }

    void CreateTiles()
    {
        float xOffset = 0.0f;
        float yOffset = 0.0f;

        tilesAll = new List<Tile_Script>();
        tilesMined = new List<Tile_Script>();
        tilesUnmined = new List<Tile_Script>();

        for (int tilesCreated = 0; tilesCreated < numTiles; tilesCreated += 1)
        {
            
            if (tilesCreated % tilesPerRow == 0 && tilesCreated > 0)
            {
                yOffset += distBetweenTiles;
                xOffset = 0;
            }

            Vector3 tileScreenPosition = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z); // creates the location of the tile
            Tile_Script newTile = Instantiate(tilePrefab, tileScreenPosition, tilePrefab.transform.rotation); // creates the tile object

            newTile.ID = tilesCreated;
            newTile.tilesPerRow = tilesPerRow;
            newTile.coord = new Vector2Int(tilesCreated % tilesPerRow, (int) (tilesCreated / tilesPerRow));  // assign tile coordinates in integer grid
            newTile.tileClickedEvent += gameController.OnTileClick; // add GameControl as listener to Tile click event

            tilesAll.Add(newTile); // adds tile to list of tiles

            xOffset += distBetweenTiles;
        }

        AssignMines();
        CreateWinTile();
    }

    void CreateWinTile()
    {
        System.Random rnd = new System.Random();
        winTile = tilesUnmined[rnd.Next(tilesUnmined.Count - 20, tilesUnmined.Count)];
        winTile.GetComponent<MeshRenderer>().material = winTileMaterial;
        winTile.winTile = true;
    }


    void AssignMines()
    {
        tilesUnmined.AddRange(tilesAll);

        System.Random rnd = new System.Random();
        for(int minesAssigned = 0; minesAssigned < numTiles * percentMines; minesAssigned += 1)
        {
            Tile_Script currentTile = tilesUnmined[rnd.Next(0, tilesUnmined.Count)];
            tilesMined.Add(currentTile);
            tilesUnmined.Remove(currentTile);

            Tile_Script currentTileComponent = currentTile.GetComponent(typeof(Tile_Script)) as Tile_Script;
            currentTileComponent.isMined = true;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
