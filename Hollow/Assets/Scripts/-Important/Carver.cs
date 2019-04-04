using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carver
{
    private List<Coord> tmp = new List<Coord>();
    private int numberOfCarvings = 1;

    public void Start(int[,] map, int width, int height, RoomGenerator gen)
    {
        for (int i = 0; i < numberOfCarvings; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map[x, y] == 0)
                    {
                        if (map[x, y + 1] == 1 && map[x + 1, y - 1] == 1)
                        {
                            if (gen.IsInMapRange(x, y + 1) && gen.IsInMapRange(x + 1, y -1))
                            {
                                tmp.Add(new Coord(x, y + 1));
                                tmp.Add(new Coord(x + 1, y - 1));
                            }
                        }
                        if (map[x, y + 1] == 1 && map[x - 1, y - 1] == 1)
                        {
                            if (gen.IsInMapRange(x, y + 1) && gen.IsInMapRange(x - 1, y - 1))
                            {
                                tmp.Add(new Coord(x, y + 1));
                                tmp.Add(new Coord(x - 1, y - 1));
                            }
                        }

                        if (map[x, y - 1] == 1 && map[x - 1, y + 1] == 1)
                        {
                            if (gen.IsInMapRange(x, y - 1) && gen.IsInMapRange(x - 1, y + 1))
                            {
                                tmp.Add(new Coord(x, y - 1));
                                tmp.Add(new Coord(x - 1, y + 1));
                            }
                        }

                        if (map[x, y - 1] == 1 && map[x + 1, y + 1] == 1)
                        {
                            if (gen.IsInMapRange(x, y - 1) && gen.IsInMapRange(x + 1, y + 1))
                            {
                                tmp.Add(new Coord(x, y - 1));
                                tmp.Add(new Coord(x + 1, y + 1));
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < tmp.Count; i++)
        {
            map[tmp[i].tileX, tmp[i].tileY] = 0;
            if (map[tmp[i].tileX, tmp[i].tileY + 1] == 1 && map[tmp[i].tileX, tmp[i].tileY - 1] == 1)
            {
                map[tmp[i].tileX, tmp[i].tileY + 1] = 0;
                map[tmp[i].tileX, tmp[i].tileY - 1] = 0;
            }
        }
    }
}
