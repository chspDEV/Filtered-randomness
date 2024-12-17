using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace seminario_aleatoridade
{
    class Randomizer
    {
        private float maxTimeToRandomize;
        private float currentTime;
        private readonly Random random;
        private readonly DataManager dataManager;

        public int CurrentNumber { get; private set; }


        private bool isFiltered;
        private int filterTarget; 
        private float filterWeight;
        private int variation = 15;

        public Randomizer(float maxTime, float weight, int target, int _variation)
        {
            maxTimeToRandomize = maxTime;
            random = new Random(Guid.NewGuid().GetHashCode());
            dataManager = DataManager.Instance;


            isFiltered = false;
            filterTarget = target; //valor padrão para o qual os números tenderão
            filterWeight = weight; //Peso 
            variation = _variation;

            dataManager.target = filterTarget;
            dataManager.peso = filterWeight;
            dataManager.variacao = variation;
        }

        public void Update(GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentTime >= maxTimeToRandomize)
            {
                CurrentNumber = isFiltered ? GenerateFilteredRandom() : random.Next(1, 101);
                dataManager.AddNumber(CurrentNumber);
                currentTime = 0f;
            }

            HandleInput();
        }

        public void UpdateMaxTime(float newTime)
        {
            maxTimeToRandomize = Math.Max(0.0025f, newTime);
        }

        public void SetFilteredRandom(bool enabled)
        {
            isFiltered = enabled;
        }

        public void ConfigureFilteredRandom(int target, float weight)
        {
            filterTarget = Clamp(target, 1, 100);
            filterWeight = Clamp(weight, 0.0f, 1.0f);
        }


        private int GenerateFilteredRandom()
        {
            //determina se favorece o número-alvo com base no peso
            bool favorTarget = random.NextDouble() < filterWeight;

            if (favorTarget)
            {
                //Gera proximo ao valor-alvo
                int deviation = random.Next(-variation, variation + 1); 
                return Clamp(filterTarget + deviation, 1, 100);
            }
            else
            {
                return random.Next(1, 101);
            }
        }

        //clamp para inteiros
        private int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        // clamp para float
        private float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.F)) 
            {
                isFiltered = true;
                DataManager.Instance.aleatoridadeFiltrada = isFiltered;
            }

            if (keyboardState.IsKeyDown(Keys.G))
            {
                isFiltered = false;
                DataManager.Instance.aleatoridadeFiltrada = isFiltered;
            }
        }
    }
}
