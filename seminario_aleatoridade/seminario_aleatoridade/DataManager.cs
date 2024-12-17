using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace seminario_aleatoridade
{
    class DataManager
    {
        public static DataManager Instance;

        private List<int> numbersList = new List<int>();
        private List<Randomizer> randomizersList = new List<Randomizer>();
        private StringBuilder sortedNumbersBuilder = new StringBuilder();
        private string allSortedNumbers;
        private const int maxNumbersPerLine = 5;

        public bool aleatoridadeFiltrada = false;

        public int gameWidth { get; set; }
        public int gameHeight { get; set; }

        public DataManager()
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("DataManager já foi instanciado.");
            }

            Instance = this;
            allSortedNumbers = string.Empty; // Inicializa como vazio
        }

        public string GetAllSortedNumbers()
        {
            return allSortedNumbers;
        }

        public List<int> GetNumbers() { return numbersList; }

        public List<Randomizer> GetRandomizers() { return randomizersList; }

        public void Update(GameTime gameTime)
        {
            foreach (var randomizer in randomizersList)
            {
                randomizer.Update(gameTime);
            }
        }

        public void UpdateMaxTime(float newTime)
        {
            foreach (var randomizer in randomizersList)
            {
                randomizer.UpdateMaxTime(newTime);
            }
        }

        public void AddNumber(int number)
        {
            numbersList.Add(number);

            if (!UIManager.Instance.GraphicMode)
            {
                UpdateSortedNumbers();
            }
            else
            {
                sortedNumbersBuilder.Clear();
                allSortedNumbers = string.Empty;
            }
        }

        private void UpdateSortedNumbers()
        {
            sortedNumbersBuilder.Clear(); // Limpa o builder antes de começar
            int counter = 0;

            foreach (int number in numbersList)
            {
                sortedNumbersBuilder.Append(number).Append(";");

                counter++;
                if (counter >= maxNumbersPerLine)
                {
                    sortedNumbersBuilder.AppendLine();
                    counter = 0;
                }
            }

            // Atualiza a string final
            allSortedNumbers = sortedNumbersBuilder.ToString();
        }

        public void AddRandomizer(Randomizer randomizer)
        {
            if (!randomizersList.Contains(randomizer))
            {
                randomizersList.Add(randomizer);
            }
        }

        public void RemoveRandomizer(Randomizer randomizer)
        {
            randomizersList.Remove(randomizer);
        }
    }
}
