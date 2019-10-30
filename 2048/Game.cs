using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2048
{
    class Game
    {
        public Game()
        {
            currentGenerateCount = random.Next(1, 2);
            for (int i = 0; i < currentGenerateCount; i++)
            {
                if (random.NextDouble() > 0.66)
                {
                    currentGenerateNumbers[i] = 4;
                }
                else currentGenerateNumbers[i] = 2;
                currentCoord[i] = GetAvailableCoord();
                map[currentCoord[i].x, currentCoord[i].y] = currentGenerateNumbers[i];
            }
            

        }
        private int currentGenerateCount;
        private int[] currentGenerateNumbers = new int[2];
        private int[,] map = new int[4, 4];
        private Random random = new Random();
        private coord[] currentCoord = new coord[2];
        private int scores = 0;
        private int[] auxArray = new int[4];
        public int[,] Map { get { return map; } }
        public int GenerateCount { get { return currentGenerateCount; } }
        public int[] GenerateNumbers { get { return currentGenerateNumbers; } }
        public coord[] Coords { get { return currentCoord; } }
        public bool gameIsOver = false;
        public int Scores { get { return scores; } }

        private coord GetAvailableCoord()
        {
            if (IsFaild())
            {
                gameIsOver = true;
                throw new GameOverException("游戏结束");
            }
            coord tmp = new coord();
            do
            {
                tmp.x = random.Next(0, 4);
                tmp.y = random.Next(0, 4);
            } while (!IsBlankCoord(tmp));
            return tmp;
        }

        public void Restart()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    map[i, j] = 0;
                    scores = 0;
                }
            }
            Update();
        }

        private void Update()
        {
            int nums = GetAvailableBlankNums();
            if (nums > 1) currentGenerateCount = random.Next(1, 2);
            else if (nums == 1) currentGenerateCount = 1;
            else if (!IsFaild()) currentGenerateCount = 0;
            else throw new GameOverException("游戏结束");
            for (int i = 0; i < currentGenerateCount; i++)
            {
                if (random.NextDouble() > 0.66)
                {
                    currentGenerateNumbers[i] = 4;
                }
                else currentGenerateNumbers[i] = 2;
                currentCoord[i] = GetAvailableCoord();
                map[currentCoord[i].x, currentCoord[i].y] = currentGenerateNumbers[i];
            }
        }

        private bool IsFaild()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int cell = map[i, j];
                    if (cell == 0)
                    {
                        return false;
                    }
                    if (i - 1 >= 0 && cell == map[i - 1, j])
                    {
                        return false;
                    }
                    if (i + 1 < 4 && cell == map[i + 1, j])
                    {
                        return false;
                    }
                    if (j - 1 >= 0 && cell == map[i, j - 1])
                    {
                        return false;
                    }
                    if (j + 1 < 4 && cell == map[i, j + 1])
                    {
                        return false;
                    }
                }
            }
            gameIsOver = true;
            return true;
        }

        private int GetAvailableBlankNums()
        {
            int num = 0;
            foreach (var i in map) if (i == 0) num++;
            return num;
        }
        private bool IsBlankCoord(coord cor)
        {
            if (map[cor.x, cor.y] == 0) return true;
            else return false;
        }
        private bool CommonMove()
        {
            bool move = false;
            int flag = 0;
            for (int i = 3; i > 0; i--)
            {
                if (auxArray[i] != 0 && auxArray[i - 1] == 0)
                {
                    move = true;
                    auxArray[i - 1] = auxArray[i];
                    auxArray[i] = 0;
                    for (int j = i; flag > 0; j++)
                    {
                        auxArray[j] = auxArray[j + 1];
                        auxArray[j + 1] = 0;
                        flag--;
                    }
                }
                else if (auxArray[i] != 0 && auxArray[i - 1] != 0) flag++;
            }
            if (auxArray[0] == 0) return move;
            for (int i = 0; i < 3; i++)
            {
                if (auxArray[i] == auxArray[i + 1])
                {
                    move = true;
                    auxArray[i] *= 2;
                    if (auxArray[i] != 0) scores += 2;
                    for (int j = i + 1; j < 3; j++)
                    {
                        auxArray[j] = auxArray[j + 1];
                        auxArray[j + 1] = 0;
                    }
                }
            }
            if (auxArray[0] == auxArray[1])
            {
                move = true;
                auxArray[0] *= 2;
                if (auxArray[0] != 0) scores += 2;
                auxArray[1] = 0;
            }
            return move;
        }
        public void PressUp()
        {
            bool flag = false;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++) auxArray[j] = map[j, i];
                if (CommonMove()) flag = true;
                for (int j = 0; j < 4; j++) map[j, i] = auxArray[j];
            }
            if (flag) Update();
            else if (IsFaild()) throw new GameOverException("游戏结束");
        }
        public void PressDown()
        {
            bool flag = false;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++) auxArray[3 - j] = map[j, i];
                if (CommonMove()) flag = true;
                for (int j = 0; j < 4; j++) map[j, i] = auxArray[3 - j];
            }
            if (flag) Update();
            else if (IsFaild()) throw new GameOverException("游戏结束");
        }
        public void PressLeft()
        {
            bool flag = false;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++) auxArray[j] = map[i, j];
                if (CommonMove()) flag = true;
                for (int j = 0; j < 4; j++) map[i, j] = auxArray[j];
            }
            if (flag) Update();
            else if (IsFaild()) throw new GameOverException("游戏结束");
        }

        public void PressRight()
        {
            bool flag = false;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++) auxArray[3 - j] = map[i, j];
                if (CommonMove()) flag = true;
                for (int j = 0; j < 4; j++) map[i, j] = auxArray[3 - j];
            }
            if (flag) Update();
            else if (IsFaild()) throw new GameOverException("游戏结束");
        }
    }

    public class GameOverException : ApplicationException
    {
        public GameOverException(string message) : base(message) { }
        public override string Message
        {
            get { return base.Message; }
        }

    }
}
