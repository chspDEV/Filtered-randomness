using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace seminario_aleatoridade
{
    class TextureManager
    {
        public static TextureManager Instance;
        public Texture2D Pixel;

        public TextureManager(GraphicsDevice graphicsDevice)
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("TextureManager já foi instanciado.");
            }

            Instance = this;

            // Criar textura de 1x1 pixel
            Pixel = new Texture2D(graphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
        }
    }
}
