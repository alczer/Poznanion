﻿using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
public class Index
{
	public Index(int xx, int yy)
	{
		this.x = xx;
		this.y = yy;

	}

	public int x;
	public int y;
}

public class CastleCoords
{
	public Index index;
	public List<int> area;
}

public class CastleData
{
	public bool isFinished;
	public List<CastleCoords> fields= new List<CastleCoords>();
}

public class ReturnPoints
{
    public ReturnPoints()
    {
        

    }
    public ReturnPoints(int p, List<Index> meeplesPositionss)
    {
        this.points = p;
        this.meeplesPositions = meeplesPositionss;

    }


    public int points;
	public List<Index> meeplesPositions = new List<Index>();
}

public class AreaTuple
{
    public AreaTuple()
    {


    }
    public AreaTuple(int xx, int yy, Area areaa, bool init)
    {
        this.area = areaa;
        this.x = xx;
        this.y = yy;
        this.initialized = init;

    }


    public int x;
	public int y;
	public Area area;
	public bool initialized;
}
public class AreaTupleTwo
{
    public AreaTupleTwo()
    {


    }
    public AreaTupleTwo(int xx, int yy, List<int> areaa, bool init)
    {
        this.area = areaa;
        this.x = xx;
        this.y = yy;
        this.initialized = init;

    }


    public int x;
    public int y;
    public List<int> area;
    public bool initialized;
}


public class PointsCounter : MonoBehaviour {
	private const int POINTS_FOR_CASTLE_WHEN_FIELD = 2;
	private const int POINTS_FOR_CASTLE_TILE = 2;
	private const int POINTS_FOR_CASTLE_SHIELD = 1;
	private const int POINTS_FOR_ROAD_FIELD = 1;
	private const int POINTS_FOR_MONASTERY_FIELD = 1;
    GameManager GM;

    public AreaTuple areaNeighbour(ref GameObject[,] board, int x, int y, int edge)
	{
//        Debug.Log("Sprawdzam dla krawedzi"  + edge);
		AreaTuple neighbour = new AreaTuple();
		neighbour.initialized = false;
		int[] correspondingEdges = new int[13];
		correspondingEdges[1] = 9;
		correspondingEdges[2] = 8;
		correspondingEdges[3] = 7;
		correspondingEdges[4] = 12;
		correspondingEdges[5] = 11;
		correspondingEdges[6] = 10;
		correspondingEdges[7] = 3;
		correspondingEdges[8] = 2;
		correspondingEdges[9] = 1;
		correspondingEdges[10] = 6;
		correspondingEdges[11] = 5;
		correspondingEdges[12] = 4;
		neighbour.x = x;
		neighbour.y = y;
		if (edge == 1 || edge == 2 || edge == 3)
		{
			(neighbour.x)--;
		}
		else if (edge == 4 || edge == 5 || edge == 6)
		{
			(neighbour.y)++;
		}
		else if (edge == 7 || edge == 8 || edge == 9)
		{
			(neighbour.x)++;
		}
		else if (edge == 10 || edge == 11 || edge == 12)
		{
			(neighbour.y)--;
		}
		if (board[neighbour.x, neighbour.y] != null)
		{
			foreach (var area in board[neighbour.x, neighbour.y].GetComponent<Tile>().Areas)
			{
				if (area.edges.Contains(correspondingEdges[edge]))
				{
					neighbour.area = area;
					break;
				}
			}

            //neighbour.initialized = true;
//            Debug.Log("Obszar klokcka sąsiada to : " + String.Join(" ", neighbour.area.edges.Select(item => item.ToString()).ToArray()));
		}

        

        return neighbour;
	}

	public List<AreaTuple> areaNeighbours(ref GameObject[,] board, int x, int y, List<int> edges)
	{
		List<AreaTuple> neighbours = new List<AreaTuple>();
		foreach (var edge in edges)
		{
			AreaTuple neighbour = areaNeighbour(ref board, x, y, edge);
			neighbours.Add(neighbour);
		}
		foreach (var n in neighbours)
		{
//            Debug.Log("Dla "+x+" "+y+" Klocek sąsiad to : " + n.x + " " + n.y);
	//		Debug.Log("Obszar klokcka sąsiada to : " + String.Join(" ", n.area.edges.Select(item => item.ToString()).ToArray()));
		}
//		Debug.Log("---");
		//Debug.Log("wielkość listy:"+neighbours.Count);
		return neighbours;
	}

	public void countMonasteryPoints(int[] coord, ref GameObject[,] board, ref GameObject[,] meeples)
	{
		if(board[coord[0], coord[1]].GetComponent<Tile>().Areas.All(a => a.player == null))
			return;
		for(int i = -1; i < 2; ++i)
			for(int j = -1; j < 2; ++j)
				if (board[coord[0]+i,coord[1]+j] == null)
					return;

		board[coord[0], coord[1]].GetComponent<Tile>().Areas.Find (a => a.player != null).player.points += 9;
		board [coord[0], coord[1]].GetComponent<Tile>().Areas.Find (a => a.player != null).player = null;
		Destroy(meeples[coord [0], coord [1]]);


	}
	public void countPointsAfterMove(ref GameObject[,] board, int x, int y, ref GameObject[,] meeples)
	{
        Debug.Log("Jestem w counterze");
        foreach (var area in board[x, y].GetComponent<Tile>().Areas)
		{
            Debug.Log("Liczę dla roznych obszarów");
            CountAreaPoints(ref board, x, y, area, ref meeples);
		}
        Debug.Log("Liczę dla monastery");
        for (int i = -1; i < 2; ++i)
			for(int j = -1; j < 2; ++j)
				if(board[x + i, y + j]!=null && board[x+i,y+j].GetComponent<Tile>().Areas.Exists(a => a.terrain == terrainTypes.monastery))
					countMonasteryPoints(new int[]{x+i,y+j},ref board, ref meeples);

	}
		
	public void CountAreaPoints(ref GameObject[,] board, int x, int y, Area area, ref GameObject[,] meeples)
	{
        
		List<AreaTupleTwo> checkedAreas = new List<AreaTupleTwo>();
		ReturnPoints result = new ReturnPoints();
		switch(area.terrain)
		{
			case terrainTypes.castle:
			result = countCastle(ref board,x,y,result, ref checkedAreas, area);
                Debug.Log("Punkty z zamków: "+result.points);
				if (result.meeplesPositions.Count == 0)
				{
					//do nothing
				}
				else
				{
					if(result.points == (2 * POINTS_FOR_CASTLE_TILE)) result.points = POINTS_FOR_CASTLE_TILE;
                    foreach (var player in RemoveMeeplesAndPickWinner(ref board, result.meeplesPositions, ref meeples))
                    {
                    //    GM.AddScore((PlayerColor)player, result.points);
                    }                
				}
			break;	
		}
		
	}

	public List<int> RemoveMeeplesAndPickWinner(ref GameObject[,] board, List<Index> meeplesPositions,ref GameObject[,] meeples)
	{
		const int MAX_PLAYER_SIZE = 5;
		int[] players = new int[5];

		foreach (Index index in meeplesPositions)
		{
			players [(int)board [index.x, index.y].GetComponent<Tile> ().Areas.Find (a => a.player != null).player.color]++;
			Destroy(meeples[index.x, index.y]);
		}

		int max_count = players[0];
		List<int> maxList = new List<int> ();

		for (int i = 0; i < MAX_PLAYER_SIZE; ++i)
		{
			if (players[i] > max_count)
			{
				max_count = players[i];
				maxList = new List<int> ();
				maxList.Add (i);
			}
			else if(players[i] == max_count)
			{
				maxList.Add (i);
			}

		}
		return maxList;
	}
    /*
public ReturnPoints countCastle(ref GameObject[,] board, int x, int y,  ReturnPoints accumulator, ref List<AreaTuple> checkedAreas, Area area)
    {
        Debug.Log("Akumulator: " + accumulator.points);
        AreaTuple currentTuple = new AreaTuple(x, y, area, true);
        if (checkedAreas.Contains(currentTuple))
        {
            return accumulator;
        }            
        checkedAreas.Add(currentTuple);
        if(board[x,y].GetComponent<Tile>().Areas.Exists(a => a.player != null))
            accumulator.meeplesPositions.Add(new Index(x,y));
        if(board[x, y].GetComponent<Tile>().Plus)
            accumulator.points += POINTS_FOR_CASTLE_SHIELD;
        accumulator.points += POINTS_FOR_CASTLE_TILE;
        Debug.Log("Przed petla");

        List<AreaTuple> an = areaNeighbours(ref board, x, y, area.edges);

        foreach (var neighbour in an)
        {
            Debug.Log("SASIAD"+neighbour.x + " " +neighbour.y);
            if (board[neighbour.x, neighbour.y] == null)
            {
                return new ReturnPoints(0, new List<Index>());
            }               
            accumulator = countCastle(ref board, neighbour.x, neighbour.y, accumulator, ref checkedAreas, area);
            if(accumulator.points == 0) return accumulator;
        }
        Debug.Log("ZA petla");
        return accumulator;
    }
    */
    public ReturnPoints countCastle(ref GameObject[,] board, int x, int y, ReturnPoints accumulator, ref List<AreaTupleTwo> checkedAreas, Area area)
    {
        AreaTupleTwo currentTuple = new AreaTupleTwo(x, y, area.edges, true);
//if (checkedAreas.Contains(currentTuple))//tu error - nie rozpoznaje
        if (checkedAreas.Any(opt => opt.x ==currentTuple.x && opt.y == currentTuple.y && Enumerable.SequenceEqual(opt.area.OrderBy(t => t), currentTuple.area.OrderBy(t => t))))
        {

            Debug.Log("Odwiedzony:" + x + " " + y + " Akumulator: " + accumulator.points + " więc exit.");
            return accumulator;
        }
        checkedAreas.Add(currentTuple);
        Debug.Log("Dodajemy nowy:" + x + " " + y + " Otrzymany akumulator: " + accumulator.points);
        Debug.Log("Wszystkie odwiedzone:");
        //wypisz checkedAreas
        foreach(var jararar in checkedAreas)
        {
            Debug.Log("x: " + jararar.x + " y: " + jararar.y + " Area: edges: " + String.Join(" ", jararar.area.Select(item => item.ToString()).ToArray()));
        }
        if (board[x, y].GetComponent<Tile>().Areas.Exists(a => a.player != null))
            accumulator.meeplesPositions.Add(new Index(x, y));
        if (board[x, y].GetComponent<Tile>().Plus)
            accumulator.points += POINTS_FOR_CASTLE_SHIELD;
        accumulator.points += POINTS_FOR_CASTLE_TILE;

        List<AreaTuple> an = areaNeighbours(ref board, x, y, area.edges);
        /*
        foreach ()
        {

        }
        */
        Debug.Log("Sprawdzamy sąsiadów!");
        foreach (var neighbour in an)
        {
            Debug.Log("Sprawdzamy sąsiada: " + neighbour.x + " " + neighbour.y);
            if (board[neighbour.x, neighbour.y] == null)
            {
                Debug.Log("Ten: " + neighbour.x + " " + neighbour.y + " to null, wiec exit");
                return new ReturnPoints(0, new List<Index>());
            }
            accumulator = countCastle(ref board, neighbour.x, neighbour.y, accumulator, ref checkedAreas, neighbour.area);
            if (accumulator.points == 0)
            {
                Debug.Log("Sąsiad: " + neighbour.x + " " + neighbour.y + " zwrócił nulla, więc my: " + x + " " + y + "  też!");
                return accumulator;
            }
            Debug.Log("Po sprawdzeniu sąsiada: " + neighbour.x + " " + neighbour.y + " akumulator to: " + accumulator.points);
        }
        Debug.Log("Sąsiedzi sprawdzeni, zwracamy akumulator: " + accumulator.points + "!");
        return accumulator;
    }

    /*
	public ReturnPoints countFarm(ref GameObject[,] board, int x, int y, ReturnPoints accumulator, visited, zamkiVisited)
	{
		AreaTuple currentTuple = new AreaTuple (x, y, area, true);
		if(checkedAreas.Contains(currentTuple))
			return accumulator;
		if(board[x,y].GetComponent<Tile>().Areas.Exists(a => a.player != null))
			accumulator.meeplesPositions.Add(Index(x,y));
		foreach(zamek in currentTile.zamki)
			AreaTuple currentZamek = new AreaTuple(x,y,ZamekData,true);
			if(zamkiVisited.Contains(currentZamek) 
			{
				//do nothing
			}
			else
			{
				ZamekData res = CheckIfCastleFinished(); 
				if(res.finished)
				{
					zamkiVisited.Add(res.fields);
					accumulator.points += POINTS_FOR_CASTLE_WHEN_FIELD;
				}
			}
		foreach(neighbour in neighbours(board,x,y))
		{
			if(!neighbour)
				return ReturnPoints(0,empty);
		}
	}
	*/
}
