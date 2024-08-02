using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set;}

    [SerializeField] public LayerMask tileMask;
    [SerializeField] private List<List<CombatTile>> tileMap = new List<List<CombatTile>>();

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null){
            Instance = this;
        } 
        else {
            Destroy(this.gameObject);
        }
    }

    public void generateTileMap(){
        int height = this.gameObject.transform.childCount;
        int width = this.gameObject.transform.GetChild(0).childCount;
        for(int y = 0; y < height; y++){
            List<CombatTile> row = new List<CombatTile>();
            for(int x = 0; x < width; x++){
                CombatTile tile = this.gameObject.transform.GetChild(y).GetChild(x).gameObject.GetComponent<CombatTile>();
                if(tile.Passable){
                    tile.x = x;
                    tile.y = y;
                    row.Add(tile);
                } 
                else {
                    tile.x = -1;
                    tile.y = -1;
                    row.Add(null);
                }
            }
            tileMap.Add(row);
        }
    }

    public void ClearTileMap(){
        foreach(List<CombatTile> row in tileMap){
            foreach(CombatTile tile in row){
                tile.x = -1;
                tile.y = -1;
            }
            row.Clear();
        }
        tileMap.Clear();
    }

    public int getDistance(CombatTile tile1, CombatTile tile2){
        if(!tile1.Passable || !tile2.Passable) return -1;
        return Math.Abs(tile1.x - tile2.x) + Math.Abs(tile1.y - tile2.y);
    }

    private CombatTile getTile(int x, int y){
        return this.tileMap[x][y];
    }
}
