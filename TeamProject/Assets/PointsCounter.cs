using UnityEngine;
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
	private const int POINTS_FOR_FINISHED_CASTLE_WHEN_FIELD = 3;
	private const int POINTS_FOR_CASTLE_TILE = 2;
	private const int POINTS_FOR_CASTLE_SHIELD = 2;
	private const int POINTS_FOR_ROAD_FIELD = 1;
	private const int POINTS_FOR_MONASTERY_FIELD = 1;
    GameManager GM;
    void Awake()
    {
        GM = GameManager.Instance;
    }
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
            //Debug.Log("Obszar klokcka sąsiada to : " + String.Join(" ", neighbour.area.edges.Select(item => item.ToString()).ToArray()));
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
        //foreach (var n in neighbours)
        //{
        //    Debug.Log("Dla "+x+" "+y+" Klocek sąsiad to : " + n.x + " " + n.y);
        //    Debug.Log("Obszar klokcka sąsiada to : " + String.Join(" ", n.area.edges.Select(item => item.ToString()).ToArray()));
        //}
        //Debug.Log("---");
		//Debug.Log("wielkość listy:"+neighbours.Count);
		return neighbours;
	}

    public bool checkClosedArea(int x, int y, ref GameObject[,] board, ref List<AreaTupleTwo> checkedAreas, ref List<AreaTupleTwo> checkedGivenAreas, Area area)
    {
        AreaTupleTwo currentTuple = new AreaTupleTwo(x, y, area.edges, true);
        if (checkedGivenAreas.Any(opt => opt.x == currentTuple.x && opt.y == currentTuple.y && Enumerable.SequenceEqual(opt.area.OrderBy(t => t), currentTuple.area.OrderBy(t => t))))
        {
            return true;
        }
        checkedAreas.Add(currentTuple);
        checkedGivenAreas.Add(currentTuple);
        List<AreaTuple> an = areaNeighbours(ref board, x, y, area.edges);
        foreach (var neighbour in an)
        {
            if (board[neighbour.x, neighbour.y] == null)
            {
                return false;
            }

            bool returnValue = checkClosedArea(neighbour.x, neighbour.y, ref board, ref checkedAreas, ref checkedGivenAreas, neighbour.area);
            if (returnValue == false)
            {
                return false;
            }
        }
        return true;

    }


    public void countMonasteryPoints(int[] coord, ref GameObject[,] board, ref GameObject[,] meeples)
	{
		if(board[coord[0], coord[1]].GetComponent<Tile>().Areas.All(a => a.player == null))
			return;
		for(int i = -1; i < 2; ++i)
			for(int j = -1; j < 2; ++j)
				if (board[coord[0]+i,coord[1]+j] == null)
					return;

        GM.AddScore(board[coord[0], coord[1]].GetComponent<Tile>().Areas.Find(a => a.player != null).player.color, 9);
        // board[coord[0], coord[1]].GetComponent<Tile>().Areas.Find (a => a.player != null).player.points += 9;

        GM.ReturnMeeple(board[coord[0], coord[1]].GetComponent<Tile>().Areas.Find(a => a.player != null).player.color);
        board [coord[0], coord[1]].GetComponent<Tile>().Areas.Find (a => a.player != null).player = null;
        Destroy(meeples[coord [0], coord [1]]);
	}

	public void countPointsAfterMove(ref GameObject[,] board, int x, int y, ref GameObject[,] meeples)
	{
       // Debug.Log("Jestem w counterze");
        foreach (var area in board[x, y].GetComponent<Tile>().Areas)
		{
         //   Debug.Log("Liczę dla roznych obszarów");
            CountAreaPoints(ref board, x, y, area, ref meeples);
		}
    //    Debug.Log("Liczę dla monastery");
        for (int i = -1; i < 2; ++i)
            for (int j = -1; j < 2; ++j)
                if (board[x + i, y + j] != null && board[x + i, y + j].GetComponent<Tile>().Areas.Exists(a => a.terrain == terrainTypes.monastery))
                    countMonasteryPoints(new int[] { x + i, y + j }, ref board, ref meeples);
    }
		
	public void CountAreaPoints(ref GameObject[,] board, int x, int y, Area area, ref GameObject[,] meeples)
	{
        
		List<AreaTupleTwo> checkedAreas = new List<AreaTupleTwo>();
		ReturnPoints result = new ReturnPoints();
		switch(area.terrain)
		{
			case terrainTypes.castle:
			result = countCastle(ref board,x,y,result, ref checkedAreas, area);
              //  Debug.Log("Punkty z zamków: "+result.points);
				if (result.meeplesPositions.Count == 0)
				{
					//do nothing
				}
				else
				{
					if(result.points == (2 * POINTS_FOR_CASTLE_TILE)) result.points = POINTS_FOR_CASTLE_TILE;
                    foreach (var player in RemoveMeeplesAndPickWinner(ref board, result.meeplesPositions, ref meeples))
                    {

               //         Debug.Log("Points castle: " + result.points);
                        GM.AddScore((PlayerColor)player, result.points);
                      
                    }                
				}
			break;
            case terrainTypes.road:
                result = countRoads(ref board, x, y, result, ref checkedAreas, area);
                if (result.meeplesPositions.Count == 0)
                {
                    //do nothing
                }
                else
                {
                    foreach (var player in RemoveMeeplesAndPickWinner(ref board, result.meeplesPositions, ref meeples))
                    {
                     //   Debug.Log("Points road: " + result.points);
                        GM.AddScore((PlayerColor)player, result.points);

                    }
                }
            break;
            case terrainTypes.grass:
                List<AreaTupleTwo> checkedCastles = new List<AreaTupleTwo>();               
                    Debug.Log("LICZE DLA TRAWY!");
                    result = countGrass(ref board, x, y, result, ref checkedAreas, ref checkedCastles, area);
                    Debug.Log("Znaleziono meepli: " + result.meeplesPositions.Count);
                    Debug.Log("Punkty za zamknięte zamki (o ile znajdzie się meeple): " + result.points);
                    if (result.meeplesPositions.Count == 0)
                    {
                        //do nothing
                    }
                    else
                    {
                        foreach (var player in RemoveMeeplesAndPickWinner(ref board, result.meeplesPositions, ref meeples))
                        {
                            Debug.Log("Przyznane punkty za farmę: " + result.points);
                            GM.AddScore((PlayerColor)player, result.points);

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
            
            // return meeple to player
            GM.ReturnMeeple(board [index.x, index.y].GetComponent<Tile> ().Areas.Find (a => a.player != null).player.color);
            board[index.x, index.y].GetComponent<Tile>().Areas.Find(aa => aa.player != null).player = null;
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

    public ReturnPoints countRoads(ref GameObject[,] board, int x, int y, ReturnPoints accumulator, ref List<AreaTupleTwo> checkedAreas, Area area)
    {
       // Debug.Log("--------------------------------------------------------------------------- liczymy dla drogi");

        AreaTupleTwo currentTuple = new AreaTupleTwo(x, y, area.edges, true);
        if (checkedAreas.Any(opt => opt.x == currentTuple.x && opt.y == currentTuple.y && Enumerable.SequenceEqual(opt.area.OrderBy(t => t), currentTuple.area.OrderBy(t => t))))
        {
       //     Debug.Log("Odwiedzony:" + x + " " + y + " Akumulator: " + accumulator.points + " więc exit.");
            return accumulator;
        }
        checkedAreas.Add(currentTuple);
      //  Debug.Log("Dodajemy nowy:" + x + " " + y + " Otrzymany akumulator: " + accumulator.points);
       // Debug.Log("Wszystkie odwiedzone:");
        //wypisz checkedAreas
        foreach (var jararar in checkedAreas)
        {
        //    Debug.Log("x: " + jararar.x + " y: " + jararar.y + " Area: edges: " + String.Join(" ", jararar.area.Select(item => item.ToString()).ToArray()));
        }
        if (board[x, y].GetComponent<Tile>().Areas.Exists(a => a.player != null && a.terrain==terrainTypes.road && Enumerable.SequenceEqual(a.edges.OrderBy(t => t), currentTuple.area.OrderBy(t => t))))
            accumulator.meeplesPositions.Add(new Index(x, y));
        accumulator.points += POINTS_FOR_ROAD_FIELD;

        List<AreaTuple> an = areaNeighbours(ref board, x, y, area.edges);
      //  Debug.Log("Sprawdzamy sąsiadów!");
        foreach (var neighbour in an)
        {
         //   Debug.Log("Sprawdzamy sąsiada: " + neighbour.x + " " + neighbour.y);
            if (board[neighbour.x, neighbour.y] == null)
            {
           //     Debug.Log("Ten: " + neighbour.x + " " + neighbour.y + " to null, wiec exit");
                return new ReturnPoints(0, new List<Index>());
            }
            accumulator = countRoads(ref board, neighbour.x, neighbour.y, accumulator, ref checkedAreas, neighbour.area);
            if (accumulator.points == 0)
            {
       //         Debug.Log("Sąsiad: " + neighbour.x + " " + neighbour.y + " zwrócił nulla, więc my: " + x + " " + y + "  też!");
                return accumulator;
            }
    //        Debug.Log("Po sprawdzeniu sąsiada: " + neighbour.x + " " + neighbour.y + " akumulator to: " + accumulator.points);
        }
   //     Debug.Log("Sąsiedzi sprawdzeni, zwracamy akumulator: " + accumulator.points + "!");
        return accumulator;
    }

    public ReturnPoints countGrass(ref GameObject[,] board, int x, int y, ReturnPoints accumulator, ref List<AreaTupleTwo> checkedAreas,ref List<AreaTupleTwo> checkedCastles, Area area)
    {


        AreaTupleTwo currentTuple = new AreaTupleTwo(x, y, area.edges, true);
        if (checkedAreas.Any(opt => opt.x == currentTuple.x && opt.y == currentTuple.y && Enumerable.SequenceEqual(opt.area.OrderBy(t => t), currentTuple.area.OrderBy(t => t))))
        {          
            return accumulator;
        }
        checkedAreas.Add(currentTuple);
        if (board[x, y].GetComponent<Tile>().Areas.Exists(a => a.player != null && a.terrain == terrainTypes.grass && Enumerable.SequenceEqual(a.edges.OrderBy(t => t), currentTuple.area.OrderBy(t => t))))
        {
            accumulator.meeplesPositions.Add(new Index(x, y));
            
            Debug.Log("Znaleziono meepla :"+x + " " + y);
        }
            


        foreach (var castle in board[x, y].GetComponent<Tile>().Areas.Where(ar => ar.terrain == terrainTypes.castle)) //sprawdzamy każdy zamek na tilu
        {
            List<AreaTupleTwo> checkedAreasThisCastle = new List<AreaTupleTwo>();
            if (checkedCastles.Any(opt => opt.x == x && opt.y == y && Enumerable.SequenceEqual(opt.area.OrderBy(t => t), castle.edges.OrderBy(t => t))))
            {
                continue;
            }
            else if (checkClosedArea(x,y,ref board,ref checkedCastles,ref checkedAreasThisCastle,castle))
            {
                accumulator.points += POINTS_FOR_FINISHED_CASTLE_WHEN_FIELD;
            }
        }
        List<AreaTuple> an = areaNeighbours(ref board, x, y, area.edges);
        foreach (var neighbour in an)
        {
            if (board[neighbour.x, neighbour.y] != null)
            {
                accumulator = countGrass(ref board, neighbour.x, neighbour.y, accumulator, ref checkedAreas, ref checkedCastles, neighbour.area);
            }
            //{
            //    return new ReturnPoints(0, new List<Index>());
            //}
            
            //if (accumulator.points == 0)
            //{
            //    return accumulator;
            //}
        }
        return accumulator;
    }

    public ReturnPoints countCastle(ref GameObject[,] board, int x, int y, ReturnPoints accumulator, ref List<AreaTupleTwo> checkedAreas, Area area)
    {
        AreaTupleTwo currentTuple = new AreaTupleTwo(x, y, area.edges, true);
        if (checkedAreas.Any(opt => opt.x ==currentTuple.x && opt.y == currentTuple.y && Enumerable.SequenceEqual(opt.area.OrderBy(t => t), currentTuple.area.OrderBy(t => t))))
        {

            //Debug.Log("Odwiedzony:" + x + " " + y + " Akumulator: " + accumulator.points + " więc exit.");
            return accumulator;
        }
        checkedAreas.Add(currentTuple);
       // Debug.Log("Dodajemy nowy:" + x + " " + y + " Otrzymany akumulator: " + accumulator.points);
       // Debug.Log("Wszystkie odwiedzone:");
        //wypisz checkedAreas
        foreach(var jararar in checkedAreas)
        {
        //    Debug.Log("x: " + jararar.x + " y: " + jararar.y + " Area: edges: " + String.Join(" ", jararar.area.Select(item => item.ToString()).ToArray()));
        }
        if (board[x, y].GetComponent<Tile>().Areas.Exists(a => a.player != null && a.terrain==terrainTypes.castle && Enumerable.SequenceEqual(a.edges.OrderBy(t => t), currentTuple.area.OrderBy(t => t))))
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
       // Debug.Log("Sprawdzamy sąsiadów!");
        foreach (var neighbour in an)
        {
        //    Debug.Log("Sprawdzamy sąsiada: " + neighbour.x + " " + neighbour.y);
            if (board[neighbour.x, neighbour.y] == null)
            {
           //     Debug.Log("Ten: " + neighbour.x + " " + neighbour.y + " to null, wiec exit");
                return new ReturnPoints(0, new List<Index>());
            }
            accumulator = countCastle(ref board, neighbour.x, neighbour.y, accumulator, ref checkedAreas, neighbour.area);
            if (accumulator.points == 0)
            {
            //    Debug.Log("Sąsiad: " + neighbour.x + " " + neighbour.y + " zwrócił nulla, więc my: " + x + " " + y + "  też!");
                return accumulator;
            }
          //  Debug.Log("Po sprawdzeniu sąsiada: " + neighbour.x + " " + neighbour.y + " akumulator to: " + accumulator.points);
        }
      //  Debug.Log("Sąsiedzi sprawdzeni, zwracamy akumulator: " + accumulator.points + "!");
        return accumulator;
    }

    /*
	public ReturnPoints countFarm(ref GameObject[,] board, int x, int y, ReturnPoints accumulator, visited, zamkiVisited)
	{
		AreaTupleTwo currentTuple = new AreaTupleTwo (x, y, area, true);
		if(checkedAreas.Contains(currentTuple))//poprawic sprawdzanie
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
