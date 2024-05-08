using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TimeWarpAdventures.Models
{
    public class Terrain
    {
        public Texture2D terrainTexture;
        private int[,] heightMap;
        private int width, height;
        private int tileSize = 32;

        private const int MaxHeight = 10;
        private const int MinHeight = -5;

        public Terrain(int width, int height, GraphicsDevice graphicsDevice)
        {
            this.width = width;
            this.height = height;
            heightMap = GenerateHeightMap();
            terrainTexture = GenerateTerrainTexture(graphicsDevice);
        }

        private int[,] GenerateHeightMap()
        {
            int[,] heightMap = new int[width, height];
            Random random = new Random();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double noise = random.NextDouble();
                    heightMap[x, y] = (int)Math.Round((noise + 1) / 2 * (MaxHeight - MinHeight) + MinHeight);
                }
            }

            return heightMap;
        }

        private Texture2D GenerateTerrainTexture(GraphicsDevice graphicsDevice)
        {
            Color[] colorData = new Color[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int height = heightMap[x, y];
                    Color color = height >= 0 ? Color.SandyBrown : Color.DarkGreen;
                    colorData[x + y * width] = color;
                }
            }

            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            texture.SetData(colorData);

            return texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int height = heightMap[x, y];
                    Vector2 position = new Vector2(x * tileSize, y * tileSize - height * tileSize);
                    spriteBatch.Draw(terrainTexture, position, null, Color.White, 0f, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
                }
            }
        }
    }

}
