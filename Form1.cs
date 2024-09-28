using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XO_Game.Properties;

namespace XO_Game
{

    enum enPlayer { P1, P2, None };

    public partial class Form1 : Form
    {

        enPlayer[,] PArr = new enPlayer[3, 3];

        PictureBox[,] PicArr;

        enPlayer Turn = enPlayer.None;

        string Player1Name = "Player 1";

        string Player2Name = "Player 2";

        int Rounds = 0;

        public Form1()
        {
            InitializeComponent();

            PicArr = new PictureBox[,] 
                { { pb1, pb2, pb3 },
                { pb4, pb5, pb6 },
                { pb7, pb8, pb9 } };

        }

        void reset_PArr()
        {

            for (int i = 0; i < PArr.GetLength(0); i++)
            {
                for (int j = 0; j < PArr.GetLength(1); j++)
                    PArr[i, j] = enPlayer.None;
            }

        }

        void reset_pictures()
        {

            foreach (PictureBox pictureBox in PicArr)
            {
                pictureBox.Image = Resources.question_mark_96;
                pictureBox.Enabled = true;
                pictureBox.BackColor = Color.Transparent;
            }

        }

        void switch_turn()
        {

            switch (Turn)
            {

                case enPlayer.P1:
                    Turn = enPlayer.P2;
                    lbl_turn.Text = Player2Name;
                    break;
                case enPlayer.P2:
                    Turn = enPlayer.P1;
                    lbl_turn.Text = Player1Name;
                    break;
                default:
                    Turn = enPlayer.P1;
                    lbl_turn.Text = Player1Name;
                    break;

            }

        }

        void reset_to_deffault()
        {

            reset_PArr();

            reset_pictures();

            Turn = enPlayer.P1;

            lbl_turn.Text = Player1Name;

            lbl_winner.Text = lbl_winner.Tag.ToString();

            Rounds = 0;

        }

        enPlayer check_row_winner()
        {

            enPlayer Winner = enPlayer.None;

            int Index = 0;

            for (int Row = 0; (Row < PArr.GetLength(0)) && (Winner == enPlayer.None); ++Row)
            {
                Index = Row;
                for (int Column = 0; Column < PArr.GetLength(1); ++Column)
                {
                    if (PArr[Row, Column] != PArr[Row, Row])
                    {
                        Winner = enPlayer.None;
                        break;
                    }
                    else
                        Winner = PArr[Row, Row];
                }
            }

            if (Winner != enPlayer.None)
            {
                for (int Column = 0; Column < PicArr.GetLength(1); ++Column)
                    PicArr[Index, Column].BackColor = Color.Lime;
            }

            return Winner;

        }

        enPlayer check_column_winner()
        {

            enPlayer Winner = enPlayer.None;

            int Index = 0;

            for (int Column = 0; (Column < PArr.GetLength(1)) && (Winner == enPlayer.None); ++Column)
            {
                Index = Column;
                for (int Row = 0; Row < PArr.GetLength(0); ++Row)
                {
                    if (PArr[Row, Column] != PArr[Column, Column])
                    {
                        Winner = enPlayer.None;
                        break;
                    }
                    else
                        Winner = PArr[Column, Column];
                }
            }

            if (Winner != enPlayer.None)
            {
                for (int Row = 0; Row < PicArr.GetLength(0); ++Row)
                    PicArr[Row, Index].BackColor = Color.Lime;
            }

            return Winner;

        }

        enPlayer check_diagonal_winner()
        {

            enPlayer Winner = enPlayer.None;

            if (PArr[0, 0] == PArr[1, 1] && PArr[1, 1] == PArr[2, 2] && PArr[1, 1] != enPlayer.None)
            {
                Winner = PArr[1, 1];
                PicArr[0, 0].BackColor = Color.Lime;
                PicArr[1, 1].BackColor = Color.Lime;
                PicArr[2, 2].BackColor = Color.Lime;
            }
            else if (PArr[0, 2] == PArr[1, 1] && PArr[1, 1] == PArr[2, 0] && PArr[1, 1] != enPlayer.None)
            {
                Winner = PArr[1, 1];
                PicArr[0, 2].BackColor = Color.Lime;
                PicArr[1, 1].BackColor = Color.Lime;
                PicArr[2, 0].BackColor = Color.Lime;
            }

            return Winner;
            
        }

        enPlayer check_winner()
        {

            enPlayer Winner = enPlayer.None;

            Winner = check_row_winner();

            if (Winner != enPlayer.None)
                return Winner;

            Winner = check_column_winner();

            if (Winner != enPlayer.None)
                return Winner;

            return check_diagonal_winner();

        }

        void set_winner(enPlayer Winner)
        {

            if (Winner == enPlayer.None)
                return;

            foreach(PictureBox pictureBox in PicArr)
                pictureBox.Enabled = false;

            if (Winner == enPlayer.P1)
                lbl_winner.Text = Player1Name;
            else
                lbl_winner.Text = Player2Name;

            lbl_turn.Text = "Game Over";

            MessageBox.Show("Winner: " + lbl_winner.Text,
                "Game Over",
                MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk);

        }

        void set_draw()
        {
            Turn = enPlayer.None;
            lbl_turn.Text = "Game Over";
            lbl_winner.Text = "Draw";
            MessageBox.Show("Draw!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        void set_turn(PictureBox pictureBox, int row_index, int column_index)
        {

            ++Rounds;

            if (Turn == enPlayer.P1)
                pictureBox.Image = Resources.X;
            else
                pictureBox.Image = Resources.O;

            PArr[row_index, column_index] = Turn;

            pictureBox.Enabled = false;

            enPlayer Winner = check_winner();

            if (Winner != enPlayer.None)
                set_winner(Winner);
            else if (Rounds == 9)
                set_draw();
            else
                switch_turn();

        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            Color color = Color.White;

            Pen pen = new Pen(color);

            pen.Width = 7;
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            e.Graphics.DrawLine(pen, 200, 100, 200, 350);
            e.Graphics.DrawLine(pen, 300, 100, 300, 350);
            e.Graphics.DrawLine(pen, 100, 180, 400, 180);
            e.Graphics.DrawLine(pen, 100, 260, 400, 260);

        }

        private void sataButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            reset_to_deffault();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender,
                ((PictureBox)sender).Tag.ToString()[0] - '0',
                ((PictureBox)sender).Tag.ToString()[1] - '0');
        }

        private void pb2_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender,
                ((PictureBox)sender).Tag.ToString()[0] - '0',
                ((PictureBox)sender).Tag.ToString()[1] - '0');
        }

        private void pb3_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender,
                ((PictureBox)sender).Tag.ToString()[0] - '0',
                ((PictureBox)sender).Tag.ToString()[1] - '0');
        }

        private void pb4_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender,
                ((PictureBox)sender).Tag.ToString()[0] - '0',
                ((PictureBox)sender).Tag.ToString()[1] - '0');
        }

        private void pb5_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender,
                ((PictureBox)sender).Tag.ToString()[0] - '0',
                ((PictureBox)sender).Tag.ToString()[1] - '0');
        }

        private void pb6_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender,
                ((PictureBox)sender).Tag.ToString()[0] - '0',
                ((PictureBox)sender).Tag.ToString()[1] - '0');
        }

        private void pb7_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender,
                ((PictureBox)sender).Tag.ToString()[0] - '0',
                ((PictureBox)sender).Tag.ToString()[1] - '0');
        }

        private void pb8_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender,
                ((PictureBox)sender).Tag.ToString()[0] - '0',
                ((PictureBox)sender).Tag.ToString()[1] - '0');
        }

        private void pb9_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender,
                ((PictureBox)sender).Tag.ToString()[0] - '0',
                ((PictureBox)sender).Tag.ToString()[1] - '0');
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            reset_to_deffault();
        }

    }
}
