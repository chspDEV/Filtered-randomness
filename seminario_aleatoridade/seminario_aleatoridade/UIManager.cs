using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace seminario_aleatoridade
{
    class UIManager
    {
        public static UIManager Instance;

        public string bigDisplay;
        public List<string> texts = new List<string>();

        private int r;
        private List<SpriteFont> fonts;

        public bool GraphicMode = false;
        

        public UIManager(List<SpriteFont> fonts)
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("DebugManager já foi instanciado.");
            }

            Instance = this;
            this.fonts = fonts;
        }

        public void SetBigDisplay(string str) { bigDisplay = str;}

        public void SetText(string text)
        {
            if (texts.Contains(text)) { return; }

            texts.Add(text);
        }

        public void DrawNumberGraph(SpriteBatch spriteBatch, List<int> numbers, SpriteFont font, Vector2 startPosition, int barWidth, int maxHeight)
        {
            int[] frequencies = new int[100]; // De 0 a 100
            foreach (int number in numbers)
            {
                frequencies[number - 1]++;
            }

            // Frequência máxima para normalizar a altura
            int maxFrequency = 150;

            float currentX = startPosition.X;

            for (int i = 0; i < frequencies.Length; i++)
            {
                int frequency = frequencies[i];

                int barHeight = (int)((frequency / (float)maxFrequency) * maxHeight);

    
                string numberText = (i + 1).ToString();
                Vector2 textSize = font.MeasureString("0");
                float digitStartY = startPosition.Y + 5; 

                for (int j = 0; j < numberText.Length; j++)
                {
                    spriteBatch.DrawString(
                        font,
                        numberText[j].ToString(), 
                        new Vector2(
                            currentX + barWidth / 2 - textSize.X / 2, //centralizando
                            digitStartY
                        ),
                        Color.White
                    );

                    digitStartY += textSize.Y - 5;
                }

                Rectangle barRectangle = new Rectangle(
                    (int)currentX,
                    (int)(startPosition.Y - barHeight), // A barra cresce de baixo para cima
                    barWidth,
                    barHeight
                );

                // Cor da barra
                Color barColor = frequency > 0
                    ? Color.Lerp(Color.PaleVioletRed, Color.Green, (frequency / (float)maxFrequency))
                    : Color.Gray;

                spriteBatch.Draw(TextureManager.Instance.Pixel, barRectangle, barColor);

                currentX += barWidth + 10; 
            }
        }


        public void Update(GameTime gt)
        {
            
            
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice gd)
        {
            spriteBatch.Begin();
            if (!GraphicMode)
            {
                float centerX = DataManager.Instance.gameWidth / 2f;
                float centerY = DataManager.Instance.gameHeight / 2f;

                for (var i = 0; i < DataManager.Instance.GetRandomizers().Count; i++)
                {
                    if (DataManager.Instance.GetRandomizers()[i] != null)
                    {
                        var buffer = centerX - (DataManager.Instance.GetRandomizers().Count * 75) + i * 150;
                        spriteBatch.DrawString(
                            fonts[0],
                            " [" + DataManager.Instance.GetRandomizers()[i].CurrentNumber.ToString() + "] ",
                            new Vector2(buffer, centerY + r),
                            Color.White
                        );
                    }
                }


                spriteBatch.DrawString(
                    fonts[1],
                    DataManager.Instance.GetAllSortedNumbers(),
                    new Vector2(20, 20), 
                    Color.White
                );
            }
            else
            {
                //desenhando debug da aleatoridade filtrada
                spriteBatch.DrawString(fonts[0], 
                                        "Aleatoridade Filtrada: " + DataManager.Instance.aleatoridadeFiltrada.ToString(), 
                                        new Vector2(10, 10), 
                                        Color.White);

                // Centralizar o gráfico
                float graphWidth = 65 * (5 + 5); // Máx colunas * (largura barra + espaçamento)
                float graphStartX = (DataManager.Instance.gameWidth - graphWidth) / 2f; // Centraliza horizontalmente
                float graphStartY = DataManager.Instance.gameHeight - 200; // Ajusta posição vertical

                DrawNumberGraph(
                                    spriteBatch,
                                    DataManager.Instance.GetNumbers(),
                                    fonts[1],
                                    new Vector2(50, DataManager.Instance.gameHeight - 150), // Ajuste da posição inicial
                                    5,  // Largura da barra
                                    300  // Altura máxima da barra
                                );
            }

            spriteBatch.End();
        }

             
        }
    }

