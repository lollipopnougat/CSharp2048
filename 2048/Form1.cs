using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _2048
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            duaLabelArray = new Label[,]
            {
                { label1, label2, label3, label4 },
                { label5, label6, label7, label8 },
                { label9, label10, label11, label12 },
                { label13, label14, label15, label16 }
            };
            dualTLPArray = new TableLayoutPanel[,]
            {
                {tableLayoutPanel2, tableLayoutPanel3, tableLayoutPanel4, tableLayoutPanel5 },
                {tableLayoutPanel6, tableLayoutPanel7, tableLayoutPanel8, tableLayoutPanel9 },
                {tableLayoutPanel10, tableLayoutPanel11, tableLayoutPanel12, tableLayoutPanel13 },
                {tableLayoutPanel14, tableLayoutPanel15, tableLayoutPanel16, tableLayoutPanel17 }
            };
            MapSync(game.Map);
            //for(int i = 0; i < game.GenerateCount; i++)
            //{
            //    duaLabelArray[game.Coords[i].x, game.Coords[i].y].Text = game.GenerateNumbers[i].ToString();
            //    dualTLPArray[game.Coords[i].x, game.Coords[i].y].BackColor = game.SetNumber(game.Coords[i], game.GenerateNumbers[i]);
            //}
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            //switch (keyData)
            //{
            //    case Keys.Tab: label1.Text = "1"; break;
            //    case Keys.Left: label1.Text = "2"; break;
            //    case Keys.Right: label1.Text = "3"; break;
            //}
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right) return false;
            else return base.ProcessDialogKey(keyData);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (game.gameIsOver)
            {
                MessageBox.Show("游戏已经结束", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Up: game.PressUp(); MapSync(game.Map); break;
                    case Keys.Down: game.PressDown(); MapSync(game.Map); break;
                    case Keys.Left: game.PressLeft(); MapSync(game.Map); break;
                    case Keys.Right: game.PressRight(); MapSync(game.Map); break;
                    default: break;
                }
            }
            catch (GameOverException er)
            {
                MessageBox.Show(er.Message, "啊哟", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception err) 
            {
                MessageBox.Show(err.ToString(), "啊哟", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //MessageBox.Show(game.num.ToString());
        }
        private void MapSync(int[,] orimap)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (orimap[i, j] == 0) duaLabelArray[i, j].Text = " ";
                    else duaLabelArray[i, j].Text = orimap[i, j].ToString();
                    dualTLPArray[i, j].BackColor = colorMap[orimap[i, j]];
                }
            }
            label18.Text = "分数: " + game.Scores.ToString();
        }
        private Game game = new Game();
        private Label[,] duaLabelArray;
        private TableLayoutPanel[,] dualTLPArray;
        Dictionary<int, Color> colorMap = new Dictionary<int, Color>()
        {
            { 0, Color.AliceBlue },
            { 2, Color.DodgerBlue },
            { 4, Color.HotPink },
            { 8, Color.LimeGreen },
            { 16, Color.Gold },
            { 32, Color.DarkOrange },
            { 64, Color.Tomato },
            { 128, Color.MediumSlateBlue },
            { 256, Color.DeepSkyBlue },
            { 512, Color.MediumSeaGreen },
            { 1024, Color.DarkGoldenrod },
            { 2048, Color.Firebrick },
            { 4096, Color.RoyalBlue }
        };

        private void button1_Click(object sender, EventArgs e)
        {
            game.Restart();
            MapSync(game.Map);
        }
    }


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
                //SetNumber(currentCoord[i], currentGenerateNumbers[i]);
            }
            //num = random.Next(0, 1);

            //duaLabelArray = LA;
            //dualTLPArray = TLPA;

        }
        //public int num;
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

        //private Label[,] duaLabelArray;
        //private TableLayoutPanel[,] dualTLPArray;
        //Dictionary<int, Color> colorMap = new Dictionary<int, Color>() 
        //{
        //    { 2, Color.DodgerBlue },
        //    { 4, Color.HotPink },
        //    { 8, Color.LimeGreen },
        //    { 16, Color.Gold },
        //    { 32, Color.DarkOrange },
        //    { 64, Color.Tomato },
        //    { 128, Color.MediumSlateBlue },
        //    { 256, Color.DeepSkyBlue },
        //    { 512, Color.MediumSeaGreen },
        //    { 1024, Color.DarkGoldenrod },
        //    { 2048, Color.Firebrick },
        //    { 4096, Color.RoyalBlue }
        //};
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
        //public Color SetNumber(coord crd, int num)
        //{
        //    map[crd.x, crd.y] = num;
        //    return colorMap[num];
        //}

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
                    for(int j = i; flag > 0; j++)
                    {
                        auxArray[j] = auxArray[j + 1];
                        auxArray[j + 1] = 0;
                        flag--;
                    }
                }
                else if (auxArray[i] != 0 && auxArray[i - 1] != 0) flag++;
            }
            if (auxArray[0] == 0) return move;
            for(int i = 0; i < 3; i++)
            {
                if (auxArray[i] == auxArray[i + 1])
                {
                    move = true;
                    auxArray[i] *= 2;
                    if(auxArray[i] != 0) scores += 2;
                    for(int j = i + 1; j < 3; j++)
                    {
                        auxArray[j] = auxArray[j + 1];
                        auxArray[j + 1] = 0;
                    }
                }
            }
            if(auxArray[0] == auxArray[1])
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
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int k = 0; k < 3; k++)
            //    {
            //        if (map[3, i] == map[2, i] && map[2, i] == map[1, i])
            //        {
            //            map[1, i] *= 2;
            //            map[3, i] = 0;
            //        }
            //        if (map[2, i] == map[1, i] && map[1, i] == map[0, i])
            //        {
            //            map[0, i] *= 2;
            //            map[2, i] = 0;
            //        }
            //        for (int j = 2; j >= 0; j--)
            //        {
            //            if (map[j, i] == map[j + 1, i])
            //            {
            //                map[j, i] *= 2;
            //                map[j + 1, i] = 0;
            //                if (map[j, i] != 0) scores += 2;
            //                flag = true;
            //            }
            //            if (map[j, i] == 0 && map[j + 1, i] != 0)
            //            {
            //                map[j, i] = map[j + 1, i];
            //                map[j + 1, i] = 0;
            //                flag = true;
            //            }
            //        }
            //        for (int j = 0; j < 3; j++)
            //        {
            //            if (map[j, i] == 0)
            //            {
            //                map[j, i] = map[j + 1, i];
            //                map[j + 1, i] = 0;
            //                if (map[j + 1, i] != 0) flag = true;
            //            }
            //        }
            //    }
            //}
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++) auxArray[j] = map[j, i];
                if(CommonMove()) flag = true;
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
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int k = 0; k < 3; k++)
            //    {
            //        if (map[3, i] == map[2, i] && map[2, i] == map[1, i])
            //        {
            //            map[3, i] *= 2;
            //            map[1, i] = 0;
            //        }
            //        if (map[2, i] == map[1, i] && map[1, i] == map[0, i])
            //        {
            //            map[2, i] *= 2;
            //            map[0, i] = 0;
            //        }
            //        for (int j = 0; j < 3; j++)
            //        {
            //            if (map[j, i] == map[j + 1, i])
            //            {
            //                map[j + 1, i] *= 2;
            //                map[j, i] = 0;
            //                if (map[j + 1, i] != 0) scores += 2;
            //                flag = true;
            //            }
            //            if (map[j + 1, i] == 0 && map[j, i] != 0)
            //            {
            //                map[j + 1, i] = map[j, i];
            //                map[j, i] = 0;
            //                flag = true;
            //            }
            //        }
            //        for (int j = 0; j < 2; j++)
            //        {
            //            if (map[j + 1, i] == 0)
            //            {
            //                map[j + 1, i] = map[j, i];
            //                map[j, i] = 0;
            //                if (map[j, i] != 0) flag = true;
            //            }
            //        }
            //    }
            //}
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
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int k = 0; k < 3; k++)
            //    {
            //        if (map[i, 3] == map[i, 2] && map[i, 2] == map[i, 1])
            //        {
            //            map[i, 1] *= 2;
            //            map[i, 3] = 0;
            //        }
            //        if (map[i, 2] == map[i, 1] && map[i, 1] == map[i, 0])
            //        {
            //            map[i, 0] *= 2;
            //            map[i, 2] = 0;
            //        }
            //        for (int j = 3; j > 0; j--)
            //        {
            //            if (map[i, j] == map[i, j - 1])
            //            {
            //                map[i, j - 1] *= 2;
            //                map[i, j] = 0;
            //                if (map[i, j - 1] != 0) scores += 2;
            //                flag = true;
            //            }
            //            if (map[i, j - 1] == 0 && map[i, j] != 0)
            //            {
            //                map[i, j - 1] = map[i, j];
            //                map[i, j] = 0;
            //                flag = true;
            //            }
            //        }
            //        for (int j = 0; j < 3; j++)
            //        {
            //            if (map[i, j] == 0)
            //            {
            //                map[i, j] = map[i, j + 1];
            //                map[i, j + 1] = 0;
            //                if (map[i, j + 1] != 0) flag = true;
            //            }
            //        }
            //    }
            //}
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
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int k = 0; k < 3; k++)
            //    {
            //        if (map[i, 3] == map[i, 2] && map[i, 2] == map[i, 1])
            //        {
            //            map[i, 3] *= 2;
            //            map[i, 1] = 0;
            //        }
            //        if (map[i, 2] == map[i, 1] && map[i, 1] == map[i, 0])
            //        {
            //            map[i, 2] *= 2;
            //            map[i, 0] = 0;
            //        }
            //        for (int j = 0; j < 3; j++)
            //        {
            //            if (map[i, j] == map[i, j + 1])
            //            {
            //                map[i, j + 1] *= 2;
            //                map[i, j] = 0;
            //                if (map[i, j + 1] != 0) scores += 2;
            //                flag = true;
            //            }
            //            if (map[i, j + 1] == 0 && map[i, j] != 0)
            //            {
            //                map[i, j + 1] = map[i, j];
            //                map[i, j] = 0;
            //                flag = true;
            //            }
            //        }
            //        for (int j = 3; j > 0; j--)
            //        {
            //            if (map[i, j] == 0)
            //            {
            //                map[i, j] = map[i, j - 1];
            //                map[i, j - 1] = 0;
            //                if (map[i, j - 1] != 0) flag = true;
            //            }
            //        }
            //    }
            //}
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


    struct coord
    {
        public coord(int ix = 0, int iy = 0)
        {
            x = ix;
            y = iy;
        }
        public int x;
        public int y;
    }
}
