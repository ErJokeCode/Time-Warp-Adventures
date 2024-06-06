using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TimeWarpAdventures.Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Xml.Serialization;

namespace TimeWarpAdventures.Models;
public class ItemLevel
{
    public string Key { get; set; }
    public Level Value { get; set; }

    public ItemLevel() { }

    public ItemLevel(string key, Level value)
    {
        Key = key;
        Value = value;
    }
}

public class ItemLink
{
    public string Level { get; set; }
    public List<string> Links { get; set; }
    public List<int> Positions { get; set; }

    public ItemLink() { }

    public ItemLink(string level, List<string> links, List<int> positions)
    {
        Level = level;
        Links = links;
        Positions = positions;
    }

    public IEnumerable<(string, int)> GetDict()
    {
        for (int i = 0; i < Links.Count; i++)
            yield return (Links[i], Positions[i]);
    }

    public int GetPosition(string key)
    {
        foreach(var link in GetDict())
            if(link.Item1 == key)
                return link.Item2;
        return -1;
    }
}


public class Level : Thing
{
    public string NameBackground {  get; set; }
    public string NameTextureGround { get; set; }
    public List<Monster> Monsters { get; set; } = new List<Monster>();
    public List<Box> Boxes { get; set; } = new List<Box>();
    public List<Lamp> Lamps { get; set; } = new List<Lamp>();
    public List<Thing> Things { get; set; } = new List<Thing>();

    private Map map;

    public Level() { }

    public Level(Texture2D picture, int positionX, string nameBack, string nameGround) 
        : base(picture, new Vector2(positionX, Ground.Top - picture.Height))
    {
        NameBackground = nameBack;
        NameTextureGround = nameGround;
        map = new Map(this);
        SetCanUse(true);
    }

    public Level(Texture2D picture, int positionX, string nameBack, string nameGround, 
        List<Monster> monsters, List<Box> boxes, List<Lamp> lamps, List<Thing> things) 
        : base(picture, new Vector2(positionX, Ground.Top - picture.Height))
    {
        NameBackground = nameBack;
        NameTextureGround = nameGround;
        Monsters = monsters;
        Boxes = boxes;
        Lamps = lamps;
        Things = things;
        map = new Map(this);
        SetCanUse(true);
    }

    public IEnumerable<Lamp> GetAvailableLamps() => Lamps.Where(lamp => lamp.IsClicked == true);

    public Map GetMap() => map = map != null ? map : new Map(this);

    public new void Update()
    {
        if (World.NowPlayer.IntersectionWithThing(this))
            SetUsed(true);
        else
            SetUsed(false);
    }
}

