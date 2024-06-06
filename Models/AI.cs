using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeWarpAdventures.Classes;
using Microsoft.Xna.Framework;
using static TimeWarpAdventures.Game1;
using SharpDX.Direct3D9;
using SharpDX.Direct3D11;
using System.IO;

namespace TimeWarpAdventures.Models;

public class AI
{
    public List<Direction> GetDirectionsTo(Map map, Essence essence, Essence findEssense)
    {
        var start = map.GetMapCell(essence);
        var end = map.GetMapCell(findEssense);
        var path = FindPath(map, start, end);    

        return CreateDirect(path);
    }

    public List<Direction> GetDirectionsTo(Map map, Essence essence, Thing findThing, List<Monster> monsters)
    {
        var start = map.GetMapCell(essence);
        var end = map.GetMapCell(findThing);
        var path = FindPath(map, start, end);

        return CreateDirect(path);
    }

    private List<Direction> CreateDirect(SinglyLinkedList<Point> path)
    {
        var dirs = new List<Direction>();

        var nowPos = default(Point);
        var previosPos = default(Point);
        foreach (var point in path.Reverse())
        {
            previosPos = nowPos;
            nowPos = point;
            if (previosPos != default(Point))
            {
                if (nowPos.X - previosPos.X > 0)
                    dirs.Add(Direction.Right);
                else if (nowPos.X - previosPos.X < 0)
                    dirs.Add(Direction.Left);

                if (nowPos.Y - previosPos.Y < 0)
                    dirs.Add(Direction.Up);
            }
            if (dirs.Count > 0) break;
        }

        return dirs;
    }

    private SinglyLinkedList<Point> FindPath(Map map, Point start, Point end)
    {
        var queue = new Queue<SinglyLinkedList<Point>>();
        var visitedHS = new HashSet<Point>();

        queue.Enqueue(new SinglyLinkedList<Point>(start));

        while (queue.Count != 0)
        {
            var pathPoint = queue.Dequeue();
            var point = pathPoint.Value;

            if (!map.InBounds(point)) continue;
            if (map.Dungeon[point.X, point.Y].Type == Game1.Type.Ground) continue;
            if (visitedHS.Contains(point)) continue;

            visitedHS.Add(point);
            if (end.Equals(point))
                return pathPoint;

            AddPointsAround(map, queue, pathPoint);
        }
        return new SinglyLinkedList<Point>(start);
    }

    private void AddPointsAround(Map map, Queue<SinglyLinkedList<Point>> queue, SinglyLinkedList<Point> pathPoint)
    {
        foreach (var pointAround in GetPointsAround(map, pathPoint.Value))
        {
            var newPathPoint = new SinglyLinkedList<Point>(pointAround, pathPoint);
            queue.Enqueue(newPathPoint);
        }
    }

    private IEnumerable<Point> GetPointsAround(Map map, Point point)
    {
        for (var dx = -1; dx <= 1; dx++)
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx != 0 && dy != 0) continue;
                yield return new Point { X = point.X + dx, Y = point.Y + dy };
            }
    }
}