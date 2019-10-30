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
                if (random.NextDouble() > 0.66) currentGenerateNumbers[i] = 4; // 出现4的概率 1/3
                else currentGenerateNumbers[i] = 2; // 出现2的概率为 2/3
                currentCoord[i] = GetAvailableCoord();
                map[currentCoord[i].x, currentCoord[i].y] = currentGenerateNumbers[i];
            }
        }

        // 字段: 
        private int currentGenerateCount;
        private int[] currentGenerateNumbers = new int[2];
        private int[,] map = new int[4, 4];
        private Random random = new Random();
        private coord[] currentCoord = new coord[2];
        private int scores = 0;
        private int[] auxArray = new int[4];
        public bool gameIsOver = false;

        // 属性访问器: 
        public int[,] Map { get { return map; } }
        public int GenerateCount { get { return currentGenerateCount; } }
        public int[] GenerateNumbers { get { return currentGenerateNumbers; } }
        public coord[] Coords { get { return currentCoord; } }
        public int Scores { get { return scores; } }

        // 随机获取可用的坐标
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

        // 重来
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

        // 更新界面公共操作
        private void Update()
        {
            int nums = GetAvailableBlankNums();
            if (nums > 1) currentGenerateCount = random.Next(1, 2);
            else if (nums == 1) currentGenerateCount = 1;
            else if (!IsFaild()) currentGenerateCount = 0;
            else throw new GameOverException("游戏结束");
            for (int i = 0; i < currentGenerateCount; i++)
            {
                if (random.NextDouble() > 0.66) currentGenerateNumbers[i] = 4;
                else currentGenerateNumbers[i] = 2;
                currentCoord[i] = GetAvailableCoord();
                map[currentCoord[i].x, currentCoord[i].y] = currentGenerateNumbers[i];
            }
        }

        // 判断是否不能移动了
        private bool IsFaild()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int cell = map[i, j];
                    if (cell == 0) return false;
                    if (i - 1 >= 0 && cell == map[i - 1, j]) return false;
                    if (i + 1 < 4 && cell == map[i + 1, j]) return false;
                    if (j - 1 >= 0 && cell == map[i, j - 1]) return false;
                    if (j + 1 < 4 && cell == map[i, j + 1]) return false;
                }
            }
            gameIsOver = true;
            return true;
        }

        // 获取空位数量
        private int GetAvailableBlankNums()
        {
            int num = 0;
            foreach (var i in map) if (i == 0) num++;
            return num;
        }

        // 判断是否是空位
        private bool IsBlankCoord(coord cor)
        {
            if (map[cor.x, cor.y] == 0) return true;
            else return false;
        }

        // 移动元素公共操作
        private bool CommonMove()
        {
            bool move = false;
            int flag = 0;
            for (int i = 3; i > 0; i--)  //处理各个位置上的0
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
            if (auxArray[0] == 0) return move; // 如果第一位就是0，那么这一行/列都是0，直接结束即可
            for (int i = 0; i < 3; i++) // 合并
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
            if (auxArray[0] == auxArray[1]) // 四个都一样时的遗留问题
            {
                move = true;
                auxArray[0] *= 2;
                if (auxArray[0] != 0) scores += 2;
                auxArray[1] = 0;
            }
            return move;
        }
        public void PressUp() //上移
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
        public void PressDown() //下移
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
        public void PressLeft() //左移
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

        public void PressRight() //右移
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

    // 游戏结束专用错误类
    public class GameOverException : ApplicationException 
    {
        public GameOverException(string message) : base(message) { }
        public override string Message
        {
            get { return base.Message; }
        }

    }
}
