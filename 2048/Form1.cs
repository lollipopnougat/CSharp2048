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
