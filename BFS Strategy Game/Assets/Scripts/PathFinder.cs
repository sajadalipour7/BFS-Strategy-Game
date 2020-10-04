﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    [SerializeField] Waypoint startWaypoint, endWaypoint;
    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
    Queue<Waypoint> queue = new Queue<Waypoint>();
    bool isRunning = true;
    Waypoint searchCenter; // the current searchCenter
    List<Waypoint> path = new List<Waypoint>();
    
    Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    public List<Waypoint> GetPath()
    {
        LoadBlocks();
        ColorStartAndEnd();
        BreadthFirstSearch();
        CreatePath();
        return path;
    }
   

    private void CreatePath()
    {
        path.Add(endWaypoint);
        Waypoint previous = endWaypoint.exploredFrom;
        while (previous != startWaypoint)
        {
            // add intermediate waypoints
            path.Add(previous);
            previous = previous.exploredFrom;
        }
        path.Add(startWaypoint);
        path.Reverse();
    }

    private void BreadthFirstSearch()
    {
        queue.Enqueue(startWaypoint);
        while (queue.Count > 0 && isRunning)
        {
            searchCenter=queue.Dequeue();
            //print("Searching from: "+searchCenter); 
            HaltIfEndFound();
            ExploreNeighbours();
            searchCenter.isExplored = true;
        }
        // todo work-out path
        //print("Finished pathfinding?");
    }

    private void HaltIfEndFound()
    {
        if (searchCenter == endWaypoint)
        {
            //print("Searching from end node, therefore stopping"); // todo remove
            isRunning = false;
        }
    }

    private void ExploreNeighbours()
    {
        if (!isRunning)
        {
            return;
        }

        foreach(Vector2Int direction in directions)
        {
            Vector2Int neighbourCoordinates = searchCenter.GetGridPos() + direction;
            if(grid.ContainsKey(neighbourCoordinates))
            {
                QueueNewNeighbours(neighbourCoordinates);
            }
        }
    }

    private void QueueNewNeighbours(Vector2Int neighbourCoordinates)
    {
        Waypoint neighbour = grid[neighbourCoordinates];
        if (neighbour.isExplored || queue.Contains(neighbour))
        {
            //do nothing
        }
        else
        {
         
            queue.Enqueue(neighbour);
            neighbour.exploredFrom = searchCenter;
            //print("Queueing " + neighbour);
        }
    }

    void ColorStartAndEnd()
    {
        startWaypoint.SetTopColor(Color.green);
        endWaypoint.SetTopColor(Color.red);
    }

    private void LoadBlocks()
    {
        var waypoints = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypoints)
        {
            var gridPos = waypoint.GetGridPos();
            if (grid.ContainsKey(gridPos))
            {
                Debug.LogWarning("Overlapping block" + waypoint);
            }
            else
            {
                grid.Add(gridPos, waypoint);
            }
            
        }
    }

    
  
}
