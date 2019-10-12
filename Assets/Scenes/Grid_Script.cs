using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Script : MonoBehaviour
{
    public Tile_Script tilePrefab;
    public int numTiles = 12;
    public float distBetweenTiles = 2.0f;
    public int tilesPerRow = 9;
    static List<Tile_Script> tilesAll;
    static List<Tile_Script> tilesMined;
    static List<Tile_Script> tilesUnmined;
    public float percentMines = 0.10f;

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

            var tilePosition = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
            var newTile = Instantiate(tilePrefab, tilePosition, transform.rotation);
            tilesAll.Add(newTile);

            xOffset += distBetweenTiles;
        }

        AssignMines();
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
