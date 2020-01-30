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
            duaLabelArray = new Label[,] // 显示用文本标签
            {
                { label1, label2, label3, label4 },
                { label5, label6, label7, label8 },
                { label9, label10, label11, label12 },
                { label13, label14, label15, label16 }
            };
            dualTLPArray = new TableLayoutPanel[,] // 背景色显示用方块
            {
                {tableLayoutPanel2, tableLayoutPanel3, tableLayoutPanel4, tableLayoutPanel5 },
                {tableLayoutPanel6, tableLayoutPanel7, tableLayoutPanel8, tableLayoutPanel9 },
                {tableLayoutPanel10, tableLayoutPanel11, tableLayoutPanel12, tableLayoutPanel13 },
                {tableLayoutPanel14, tableLayoutPanel15, tableLayoutPanel16, tableLayoutPanel17 }
            };
            MapSync(game.Map);
        }

        private Game game = new Game(); // 2048游戏对象
        private Label[,] duaLabelArray; 
        private TableLayoutPanel[,] dualTLPArray;

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right) return false;
            else return base.ProcessDialogKey(keyData);
        }

        // 方向按键点击事件处理函数
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

        }

        // 界面同步函数
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
        
        // 颜色字典
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

        // 重来按钮点击事件函数
        private void button1_Click(object sender, EventArgs e)
        {
            game.Restart();
            MapSync(game.Map);
        }
    }

    // 自定义的坐标结构体
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
