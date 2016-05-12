using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class Index
{
	Index(int xx, int yy)
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
	public int points;
	public List<Index> meeplesPositions = new List<Index>();
}

public struct AreaTuple
{
	public int x;
	public int y;
	public Area area;
	public bool initialized;
}

public class PointsCounter : MonoBehaviour {
	private const int POINTS_FOR_CASTLE_WHEN_FIELD = 2;
	private const int POINTS_FOR_CASTLE_TILE = 2;
	private const int POINTS_FOR_CASTLE_SHIELD = 1;
	private const int POINTS_FOR_ROAD_FIELD = 1;
	private const int POINTS_FOR_MONASTERY_FIELD = 1;

	public AreaTuple areaNeighbour(ref GameObject[,] board, int x, int y, int edge)
	{
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

			neighbour.initialized = true;
		}
		return neighbour;
	}

	public List<AreaTuple> areaNeighbours(ref GameObject[,] board, int x, int y, List<int> edges)
	{
		List<AreaTuple> neighbours = new List<AreaTuple>();
		foreach (var edge in edges)
		{
			AreaTuple neighbour = areaNeighbour(ref board, x, y, edge);
			if (neighbour.initialized)
				neighbours.Add(neighbour);
		}
		foreach (var n in neighbours)
		{
			Debug.Log("Klocek sąsiad to : " + n.x + " " + n.y);
			Debug.Log("Obszar klokcka sąsiada to : " + String.Join(" ", n.area.edges.Select(item => item.ToString()).ToArray()));
		}
		Debug.Log("---");
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
		
	public void countPointsAfterMove(ref GameObject[,] board, int x, int y)
	{
		foreach (var area in board[x, y].GetComponent<Tile>().Areas)
		{
			CountAreaPoints (board, x, y, area);
		}

		for(int i = -1; i < 2; ++i)
			for(int j = -1; j < 2; ++j)
				if(board[x+i,y+j].GetComponent<Tile>().Areas.Exists(a => a.terrain = terrainTypes.monastery))
					countMonasteryPoints(new int[]{x+i,y+j},ref board);

	}
		
	public void CountAreaPoints(ref GameObject[,] board, int x, int y, Area area, ref GameObject[,] meeples)
	{
		List<AreaTuple> checkedAreas;
		ReturnPoints result = new ReturnPoints();
		switch(area.terrain)
		{
			case terrainTypes.castle:
			result = countCastle(board,x,y,new ReturnPoints(), List<AreaTuple>(), area);
				if (result.meeplesPositions.Count == 0)
				{
					//do nothing
				}
				else
				{
					if(result.points == (2 * POINTS_FOR_CASTLE_TILE)  /*&& Game.style == "classic"*/) result.points = POINTS_FOR_CASTLE_TILE;
					IncreasePointsOfWinner(RemoveMeeplesAndPickWinner(board,result.meeplesPositions,meeples), result.points);
				}
			break;	
		}
		// ...
	}

	public List<int> RemoveMeeplesAndPickWinner(ref GameObject[,] board, List<Index> meeplesPositions,ref GameObject[,] meeples)
	{
		const int MAX_PLAYER_SIZE = 5;
		int[] players = new int[5];

		foreach (Index index in meeplesPositions)
		{
			players [board [index.x, index.y].GetComponent<Tile> ().Areas.Find (a => a.player != null).player.color]++;
			Destroy(meeples[index.x, index.y]);
		}

		int max_count = players [0];
		List<int> maxList = new List<int> ();

		for (int i = 0; i < MAX_PLAYER_SIZE; ++i)
		{
			if (players [i] > max_count)
			{
				max_count = players [i];
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
	
	public ReturnPoints countCastle(ref GameObject[,] board, int x, int y, ReturnPoints accumulator, List<AreaTuple> checkedAreas, Area area)
	{
		AreaTuple currentTuple = new AreaTuple (x, y, area, true);
		if(checkedAreas.Contains(currentTuple))
			return accumulator;
		if(board[x,y].GetComponent<Tile>().Areas.Exists(a => a.player != null))
			accumulator.meeplesPositions.Add(Index(x,y));
		
		foreach(var neighbour in neighbours(board,x,y))
		{
			if(!neighbour)
				return ReturnPoints(0,empty);
			if(has_proporzec)
				accumulator.PointsCounter += POINTS_FOR_CASTLE_SHIELD;
			accumulator.points += POINTS_FOR_CASTLE_TILE;
		}
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
