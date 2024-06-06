using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeWarpAdventures.Classes;
using Microsoft.Xna.Framework;
using static TimeWarpAdventures.Game1;
using SharpDX.Direct3D9;
using Type = TimeWarpAdventures.Game1.Type;

namespace TimeWarpAdventures.Models;

public class MapCell
{
    public Rectangle Rect;
    public Type Type;

    public MapCell(int x, int y, int width, int height)
    {
        Rect = new Rectangle(x, y, width, height);
    }
}

public class Map
{
    public int WidthCell;
    public int HeightCell;
    private readonly MapCell[,] map;
    private Level level;
    private List<Point> pointMonsters = new List<Point>();

    public MapCell[,] Dungeon { get { return map; } }

    public Map(Level level)
    {
        this.level = level;
        WidthCell = 100;
        HeightCell = 100;
        var cellsWidth = World.Width/ WidthCell;
        var cellsHeight = World.WindowHeight / HeightCell;
        map = new MapCell[cellsWidth + 1, cellsHeight + 1];
        var groundY = (int)Ground.Top / HeightCell;
        for (int x = 0; x <= cellsWidth; x++)
        {
            for (int y = 0; y <= cellsHeight; y++)
            {
                map[x, y] = new MapCell(x * WidthCell, y * HeightCell, WidthCell, HeightCell);
                if (y > groundY)
                    map[x, y].Type = Type.Ground;
                else
                    map[x, y].Type = Type.Empty;
            }
        }
            
        foreach (var box in level.Boxes)
        {
            var x = (int)(box.Position.X / WidthCell);
            var y = (int)(box.Position.Y / HeightCell) + 1;
            map[x, y].Type = Type.Box;
        }
    }

    public int GetGroundY()
    {
        for(var y = 0; y < map.GetLength(1); y++)
            if(map[0, y].Type == Type.Ground)
                return y;
        return (World.WindowHeight / HeightCell) + 1;
    }

    public Point GetMapCell(Essence essence)
    {
        var indexWidth = (int)(essence.Position.X + World.PositionX) / WidthCell;
        var indexHight = (int)(essence.Position.Y + essence.Height) / HeightCell;

        return new Point(indexWidth, indexHight);
    }

    public Point GetMapCell(Thing thing)
    {
        var indexWidth = (int)thing.Position.X / WidthCell;
        var indexHight = (int)(thing.Position.Y + thing.Height) / HeightCell;

        return new Point(indexWidth, indexHight);
    }

    public void UpdateMonster()
    {
        var monsters = World.Monsters;
        foreach (var point in pointMonsters)
            map[point.X, point.Y].Type = Type.Empty;
        pointMonsters.Clear();

        foreach (var monster in monsters)
        {
            var point = GetMapCell(monster);
            map[point.X, point.Y].Type = Type.Monster;
            pointMonsters.Add(point);
        }
    }

    public List<Point> GetMapCellsPlayers()
    {
        var list = new List<Point>();
        foreach (var player in World.Players)
            list.Add(GetMapCell(player));
        return list;
    }

    public List<Point> GetMapCellNowPlayer()
    {
        var list = new List<Point>
        {
            GetMapCell(World.NowPlayer)
        };
        return list;
    }

    public bool InBounds(Point point)
    {
        return point is { X: >= 0, Y: >= 0 } && Dungeon.GetLength(0) > point.X
           && Dungeon.GetLength(1) > point.Y;
    }

    public void Update()
    {
        //UpdateMonster();
    }

    internal int GetDistFromSurface(Point point)
    {
        if(point.X < 0 || point.X >= map.GetLength(0)) return 0;
        var cnt = 0;
        MapCell newCell;
        for(int y = point.Y; y < map.GetLength(1) && y >= 0; y++)
        {
            cnt++;
            newCell = map[point.X, y];
            if (newCell.Type == Type.Ground || newCell.Type == Type.Box)
                return cnt;
            
        }
        return cnt;
    }
}
