using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public List<int> tmpX = new List<int>();
    public List<int> tmpY = new List<int>();

    Camera mainCamera;
    RoomSpawner mySpawner;

    //[HideInInspector]
    public List<RandomeBackground> waterSpawn;

    public Vector3 mapOffset;
    public bool openDown = false;
    public bool openUp = false;
    public bool leftCorner = false;
    public bool rightCorner = false;

    public bool spawnWater;
    public bool addExtraOneTiles = true;

    [Space(20)]
    public bool startArea = false;

    private float offsetX = 19;
    private float offsetY = 14.285f;

    [Space(20)]
    [Range(1, 100)]
    public int proomThreshHoldSize = 20;
    [Range(1, 100)]
    public int pwallThreshHoldSize = 0;

    public GameObject openArea;
    public GameObject wall;
    public List<Transform> spawnLocations;

    private List<Transform> ceilSpawnTest;
    private List<Vector3> ceilSpawn;


    [Header("The best value so far has been 51")]
    [Range(0, 100)]
    public int randomFillPercent;
    public int passageRadius = 1;

    [Header("The value that i want to work for these is 80 by 180")]
    public int height = 160;
    public int width = 120;

    public string seed;
    public bool useRandomSeed;

    [HideInInspector]
    public int[,] map;

    public int numberOfSmoothingSweeps = 5;

    public void Start()
    {
        ceilSpawn = new List<Vector3>();
        waterSpawn = new List<RandomeBackground>();
        mySpawner = GetComponent<RoomSpawner>();
        GenerateMap();
    }

    void GenerateMap ()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < numberOfSmoothingSweeps; i++)
        {
            SmoothMap();
        }

        Openings();
        
        ProcessMap();

        CheckForError();

        GenerateRoom();
    }

    void Openings()
    {
        OpenSides();

        if (openDown)
        {
            OpeningDown();
        }
    }

    void OpenSides()
    {
        if (!leftCorner)
        {
            Left();
        }
        if (!rightCorner)
        {
            Right();
        }
    }

    void Left()
    {
        int tmp = 5;
        int tmpRandomNumber = UnityEngine.Random.Range(height / 2, height / 2 - tmp);

        for (int i = 0; i < tmp; i++)
        {
            map[0, i + (height / 2 )- (tmp / 2)] = 0;
        }
    }

    void Right()
    {
        int tmp = 5;
        int tmpRandomNumber = UnityEngine.Random.Range(height / 2, height / 2 - tmp);

        for (int i = 0; i < tmp; i++)
        {
            map[width - 1, i + (height / 2) - (tmp / 2)] = 0;
        }
    }

    void OpeningDown()
    {
        int tmp = 5;
        int tmpRandomNumber = UnityEngine.Random.Range(width / 8, (width / 2 + width / 4) - tmp);
        for (int i = 0; i < tmp; i++)
        {
            map[i + tmpRandomNumber, 0] = 0;
        }
    }

    void ProcessMap()
    {
        int tmp1 = 0;
        int tmp2 = 0;

        List<List<Coord>> wallRegions = GetRegions(1);

        int wallThreshHoldSize = pwallThreshHoldSize;
        foreach (List<Coord> wallRegion in wallRegions)
        {
            tmp1++;
            if (wallRegion.Count < wallThreshHoldSize)
            {
                foreach (Coord tile in wallRegion)
                {
                    map[tile.tileX, tile.tileY] = 0;
                }
            }
        }

        List<List<Coord>> roomRegions = GetRegions(0);
        int roomThreshHoldSize = proomThreshHoldSize;
        List<Room> survingingRooms = new List<Room>();

        foreach (List<Coord> roomRegion in roomRegions)
        {
            tmp2++;
            if (roomRegion.Count < roomThreshHoldSize)
            {
                foreach (Coord tile in roomRegion)
                {
                    map[tile.tileX, tile.tileY] = 1;
                }
            }
            else
            {
                survingingRooms.Add(new Room(roomRegion, map));
            }
        }

        survingingRooms.Sort();
        survingingRooms[0].isMainRoom = true;
        survingingRooms[0].isAccessibleFromMainRoom = true;
        
        ConnectClosestRooms(survingingRooms);
    }

    void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibiltyFromMainRoom = false)
    {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessibiltyFromMainRoom)
        {
            foreach (Room room in allRooms)
            {
                if (room.isAccessibleFromMainRoom)
                    roomListB.Add(room);
                else
                    roomListA.Add(room);
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;

        foreach (Room roomA in roomListA)
        {
            if (!forceAccessibiltyFromMainRoom)
            {
                possibleConnectionFound = false;
                if (roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }
            foreach (Room roomB in roomListB)
            {
                if (roomA.edgeTiles == roomB.edgeTiles || roomA.IsConnected(roomB))
                {
                    continue;
                }

                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                    {
                        Coord tileA = roomA.edgeTiles[tileIndexA];
                        Coord tileB = roomB.edgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                        if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                        {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }

            if (possibleConnectionFound && !forceAccessibiltyFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }

        if (possibleConnectionFound && forceAccessibiltyFromMainRoom)
        {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms, true);
        }

        if (!forceAccessibiltyFromMainRoom)
        {
            ConnectClosestRooms(allRooms, true);
        }
    }

    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
    {
        Room.ConnectRooms(roomA, roomB);

        List<Coord> line = GetLine(tileA, tileB);
        foreach (Coord c in line)
        {
            DrawCircle(c, passageRadius);
        }
    }

    void DrawCircle(Coord c, int r)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if (x*x + y*y <= r*r)
                {
                    int drawX = c.tileX + x;
                    int drawY = c.tileY + y;

                    if (IsInMapRange(drawX, drawY))
                    {
                        map[drawX, drawY] = 0;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();

        int x = from.tileX;
        int y = from.tileY;

        int dX = to.tileX - from.tileX;
        int dY = to.tileY - from.tileY;

        bool inverted = false;

        int step = Math.Sign(dX);
        int gradiantStep = Math.Sign(dY);

        int longest = Mathf.Abs(dX);
        int shortest = Mathf.Abs(dY);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dY);
            shortest = Mathf.Abs(dX);

            step = Math.Sign(dY);
            gradiantStep = Math.Sign(dX);
        }

        int gradientAccumilation = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));

            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }

            gradientAccumilation += shortest;
            if (gradientAccumilation >= longest)
            {
                if (inverted)
                {
                    x += gradiantStep;
                }
                else
                {
                    x += gradiantStep;
                }
                gradientAccumilation -= longest;
            }
        }
        return line;
    }

    Vector3 CoordToWorldPoint(Coord tile)
    {
        return new Vector3((-(width / 2f) + tile.tileX) / 2.1f, (-(height / 2f) + tile.tileY) / 2.1f, -1);
    }

    List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }

        return regions;
    }

    List<Coord> GetRegionTiles (int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width, height];
        int tilesType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
                    {
                        if (mapFlags[x, y] == 0 && map[x, y] == tilesType)
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }
        return tiles;
    }

    bool IsInMapRange(int x, int y)
    {
        //This sort of decides the thickness of the edges

        int tmp = 2;
        return x >= tmp && x < width -tmp && y >= tmp && y < height -tmp;
    }

    void RandomFillMap ()
    {
        if (useRandomSeed)
        {
            seed = Mathf.RoundToInt(UnityEngine.Random.Range(0, 10000)).ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //This if statement sort of decides how thick the walls will be NOT AT ALL
                //I might wanna remove this if statement to remove the outer wall!
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap ()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                {
                    //This is a wall tile!!!
                    map[x, y] = 1;
                }
                else if (neighbourWallTiles < 4)
                {
                    //This is open area!!!
                    map[x, y] = 0;
                }
            }
        }
    }

    int GetSurroundingWallCount (int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neightbourX = gridX - 1; neightbourX <= gridX + 1; neightbourX++)
        {
            for (int neightbourY = gridY - 1; neightbourY <= gridY + 1; neightbourY++)
            {
                if (IsInMapRange(neightbourX, neightbourY))
                {
                    if (neightbourX != gridX || neightbourY != gridY)
                    {
                        wallCount += map[neightbourX, neightbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    int GetMoreSouroundingWallCount (int gridX, int gridY)
    {
        int wallCount = 0;

        for (int neighbourX = gridX - 2; neighbourX <= gridX + 2; neighbourX++)
        {
            for (int neighbourY = gridY - 2; neighbourY <= gridY + 2; neighbourY++)
            {
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    TileDirections GetClosestNeighbours (int x, int y, int wallOrNot = 0)
    {
        TileDirections newTileDirection = new TileDirections();

        if (x - 1 >= 0 && x + 1 < width && y - 1 >= 0 && y + 1 < height)
        {
            if (map[x, y + 1] == wallOrNot)
                newTileDirection.up = true;
            if (map[x + 1, y] == wallOrNot)
                newTileDirection.right = true;
            if (map[x, y - 1] == wallOrNot)
                newTileDirection.down = true;
            if (map[x - 1, y] == wallOrNot)
                newTileDirection.left = true;

            newTileDirection.wallNeighbours += map[x + 1, y];
            newTileDirection.wallNeighbours += map[x - 1, y];
            newTileDirection.wallNeighbours += map[x, y + 1];
            newTileDirection.wallNeighbours += map[x, y - 1];

        }
        else
        {
            if (y - 1 < 0)
            {
                newTileDirection.edge = true;
                newTileDirection.downEdge = true;
                newTileDirection.wallNeighbours += 3;
            }
            if (y + 1 > height - 1)
            {
                newTileDirection.edge = true;
                newTileDirection.upEdge = true;
                newTileDirection.wallNeighbours += 3;
            }
            if (x - 1 < 0)
            {
                newTileDirection.edge = true;
                newTileDirection.rightEdge = true;
                newTileDirection.wallNeighbours += 3;
            }
            if (x + 1 > width - 1)
            {
                newTileDirection.edge = true;
                newTileDirection.leftEdge = true;
                newTileDirection.wallNeighbours += 3;
            }
        }

        return newTileDirection;
    }

    private void GenerateRoom ()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 pos = (new Vector3(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f, 0)) / 2.1f;
                    if (map[x, y] == 1)
                    {
                        SpawnWall(pos, x, y);
                    }
                    else if (map[x, y] == 0 /*|| map[x, y] == 2*/)
                    {
                        GameObject background = Instantiate(openArea, pos + mapOffset, transform.rotation, transform);
                        int neighbourTiles = GetSurroundingWallCount(x, y);

                        ///This adds random one spot walls
                        if (addExtraOneTiles)
                        {
                            int neighbours = GetMoreSouroundingWallCount(x, y);
                            int tmpRandom = UnityEngine.Random.Range(0, 1000);
                            if (tmpRandom <= 6 && neighbours < 1)
                            {
                                SpawnWall(pos, x, y);
                                Destroy(background);
                            }
                        }
                        ///

                        if (neighbourTiles == 8)
                        {
                            SpawnWall(pos, x, y);
                            Destroy(background);
                        }

                        TileDirections closestNeighbour = GetClosestNeighbours(x, y, 1);
                        RandomeBackground backgroundScript = background.GetComponent<RandomeBackground>();

                        if (closestNeighbour.down && !closestNeighbour.right && !closestNeighbour.left && !closestNeighbour.up)
                        {
                            backgroundScript.isAboveGround = true;
                            spawnLocations.Add(backgroundScript.gameObject.transform);
                        }

                        backgroundScript.RandomSprite();

                        NeighbourLogic(closestNeighbour, null, backgroundScript, x, y);
                    }
                }
            }

            WaterSpawner WS = GetComponentInChildren<WaterSpawner>();
            WS.roomGen = this;

            int tmp = UnityEngine.Random.Range(0, tmpX.Count);

            int momentaryX = tmpX[tmp];
            int momentaryY = tmpY[tmp];

            if (spawnWater)
            {
                WS.spawnLocations = waterSpawn;
                WS.SetList(momentaryX, momentaryY);
            }
        }

        //This is all happens after the room has fully spawned in!
        GetComponent<CompositeCollider2D>().GenerateGeometry();

        mySpawner.StartSpawning(spawnLocations);

        if (startArea)
            mySpawner.SpawnPlayer(spawnLocations);

    }

    public void SpawnWall(Vector3 pos, int x, int y)
    {
        GameObject tile = Instantiate(wall, pos + mapOffset, wall.transform.rotation, transform);
        Wall tileWallComponent = tile.GetComponent<Wall>();

        int neighbourTiles = GetSurroundingWallCount(x, y);
        TileDirections closestNeighbour = GetClosestNeighbours(x, y);

        NeighbourLogic(closestNeighbour, tileWallComponent, null, 0, 0);

        if (neighbourTiles == 8)
        {
            tileWallComponent.innerWall = true;
            tileWallComponent.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {

        }
    }

    public void NeighbourLogic(TileDirections closestNeighbour, Wall currentWall, RandomeBackground backgroundScript, int x, int y)
    {
        if (currentWall != null)
        {
            //If its a edge tile this happens
            if (closestNeighbour.edge)
            {
                if (closestNeighbour.wallNeighbours == 4)
                {
                    currentWall.innerWall = true;
                    return;
                }
                if (closestNeighbour.downEdge)
                {
                    currentWall.Rotate(1, true);
                }
                if (closestNeighbour.rightEdge)
                {
                    currentWall.Rotate(2, true);
                }
                if (closestNeighbour.upEdge)
                {
                    currentWall.Rotate(3, true);
                }
                if (closestNeighbour.leftEdge)
                {
                    currentWall.Rotate(4, true);
                }
                return;
            }

            //This happens if you have three neighbours
            if (closestNeighbour.wallNeighbours == 3)
            {
                if (closestNeighbour.up)
                {
                    currentWall.Rotate(1, true);
                }
                if (closestNeighbour.right)
                {
                    currentWall.Rotate(2, true);
                }
                if (closestNeighbour.down)
                {
                    currentWall.Rotate(3, true);
                }
                if (closestNeighbour.left)
                {
                    currentWall.Rotate(4, true);
                }
            }

            //This is the logic for if you have 4 neighbours
            if (closestNeighbour.wallNeighbours == 4)
            {
                currentWall.innerWall = true;
                return;
            }

            //This happens if you have no neighbours
            if (closestNeighbour.wallNeighbours == 0)
            {
                currentWall.SoloWall();
            }

            //This happens if you only have one neighbour
            if (closestNeighbour.wallNeighbours == 1)
            {
                //Pointing up
                if (closestNeighbour.left && closestNeighbour.up && closestNeighbour.right)
                {
                    currentWall.EndWall(1);
                }

                //Pointing right
                if (closestNeighbour.up && closestNeighbour.right && closestNeighbour.down)
                {
                    currentWall.EndWall(2);
                }

                //Pointing down
                if (closestNeighbour.right && closestNeighbour.down && closestNeighbour.left)
                {
                    currentWall.EndWall(3);
                }

                //Pointing left
                if (closestNeighbour.down && closestNeighbour.left && closestNeighbour.up)
                {
                    currentWall.EndWall(4);
                }
            }


            //This happens if you have 2 neighbours
            if (closestNeighbour.wallNeighbours == 2)
            {
                if (closestNeighbour.left && closestNeighbour.up)
                {
                    //Top left corner
                    currentWall.CornerLogic(1);
                }
                if (closestNeighbour.up && closestNeighbour.right)
                {
                    //Top right corner
                    currentWall.CornerLogic(2);
                }
                if (closestNeighbour.right && closestNeighbour.down)
                {
                    //Bottom right corner
                    currentWall.CornerLogic(3);
                }
                if (closestNeighbour.down && closestNeighbour.left)
                {
                    //Bottom left corner
                    currentWall.CornerLogic(4);
                }
                if (closestNeighbour.up && closestNeighbour.down)
                {
                    //up and down is walls
                    currentWall.CornerLogic(5);
                }
                if (closestNeighbour.right && closestNeighbour.left)
                {
                    //left and right is walls
                    currentWall.CornerLogic(6);
                }
            }
        }
        else
        {
            if (closestNeighbour.up)
            {
                backgroundScript.RoofSpikes();
            }
            if (closestNeighbour.down)
            {
                backgroundScript.RandomPlant();
            }

            if (closestNeighbour.up && !closestNeighbour.left && !closestNeighbour.right)
            {
                backgroundScript.canSpawnWater = true;
                waterSpawn.Add(backgroundScript);

                tmpX.Add(x);
                tmpY.Add(y);

                ceilSpawn.Add(new Vector3((-width / 2f + x + 0.5f), (-height / 2f + y), 0) * GameManager.globalScale);
            }
        }
    }

    private void CheckForError ()
    {
        
    }

    public List<Vector3> CeilList ()
    {
        return ceilSpawn;
    }
}

public struct Coord
{
    public int tileX;
    public int tileY;

    public Coord(int x, int y)
    {
        tileX = x;
        tileY = y;
    }
}