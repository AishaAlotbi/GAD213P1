using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

public class PathFinder 
{
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> openList = new List<OverlayTile>(); // List of Tiles to check
        List<OverlayTile> closedList = new List<OverlayTile>(); // List of checked Tiles

        openList.Add(start);

        while(openList.Count > 0)
        {
            // Find OverlayTile with lowest cost

            OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First(); // Gives overlay tile with the lowest f in the open list

            openList.Remove(currentOverlayTile); // Found tile in path, remove it
            closedList.Add(currentOverlayTile); // And add it to the closed list

            if (currentOverlayTile == end)
            {
                // finalize path

                return GetFinishedList(start, end);

            }

            var neighbourTiles = GetNeighbourTile(currentOverlayTile);

            foreach(var neighbour in neighbourTiles)
            {
                if(neighbour.isBlocked || closedList.Contains(neighbour))
                {
                    continue;
                }

                neighbour.G = GetManhattenDistance(start, neighbour);
                neighbour.H = GetManhattenDistance(end, neighbour);

                neighbour.previous = currentOverlayTile;

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }

            }
        }

        return new List<OverlayTile>();
    }

    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();

        OverlayTile currentTile = end;

        while(currentTile != start) // loop through past nodes and return node
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }

        finishedList.Reverse();
        return finishedList;
;
    }

    private int GetManhattenDistance(OverlayTile start, OverlayTile neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }

    private List<OverlayTile> GetNeighbourTile(OverlayTile currentOverlayTile)
    {
        var map = MapManager.Instance.map;

        List<OverlayTile> neighbours = new List<OverlayTile>();

        // Check for neighbour tiles

        // Top Neighbour
        Vector2Int locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y + 1
            );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]); // If tile exists add to neighbour
        }

        // Bottom Neighbour
            locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y - 1
            );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]); // If tile exists add to neighbour
        }


        // Right Neighbour
            locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x + 1,
            currentOverlayTile.gridLocation.y
            );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]); // If tile exists add to neighbour
        }


        // Left Neighbour
            locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x - 1,
            currentOverlayTile.gridLocation.y 
            );

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]); // If tile exists add to neighbour
        }

        return neighbours;
    }
}
