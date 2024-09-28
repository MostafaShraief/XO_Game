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

        void reset_pictures()
        {

            foreach (PictureBox pictureBox in PicArr)
            {
                pictureBox.Image = Resources.question_mark_96;
                pictureBox.Enabled = true;
                pictureBox.Tag = "?";
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

            reset_pictures();

            Turn = enPlayer.P1;

            lbl_turn.Text = Player1Name;

            lbl_winner.Text = lbl_winner.Tag.ToString();

            Rounds = 0;

        }

        bool check_values(PictureBox P1, PictureBox P2, PictureBox P3)
        {
            return (P1.Tag.ToString() != "?" &&
                    P1.Tag.ToString() == P2.Tag.ToString() &&
                    P2.Tag.ToString() == P3.Tag.ToString());
        }

        void set_color(PictureBox P1, PictureBox P2, PictureBox P3)
        {
            P1.BackColor = Color.Lime;
            P2.BackColor = Color.Lime;
            P3.BackColor = Color.Lime;
        }

        enPlayer get_winner(PictureBox P)
        {

            if (P.Tag.ToString() == "?")
                return enPlayer.None;
            else if (P.Tag.ToString() == "P1")
                return enPlayer.P1;
            else
                return enPlayer.P2;

        }

        enPlayer check_row_winner()
        {
            // Top Row
            if (check_values(pb1, pb2, pb3))
            {
                set_color(pb1, pb2, pb3);
                return get_winner(pb1);
            }
            // Middle Row
            if (check_values(pb4, pb5, pb6))
            {
                set_color(pb4, pb5, pb6);
                return get_winner(pb4);
            }
            // Bottom Row
            if (check_values(pb7, pb8, pb9))
            {
                set_color(pb7, pb8, pb9);
                return get_winner(pb7);
            }

            return enPlayer.None;

        }

        enPlayer check_column_winner()
        {
            // Top Column
            if (check_values(pb1, pb4, pb7))
            {
                set_color(pb1, pb4, pb7);
                return get_winner(pb1);
            }
            // Middle Column
            if (check_values(pb2, pb5, pb8))
            {
                set_color(pb2, pb5, pb8);
                return get_winner(pb2);
            }
            // Bottom Column
            if (check_values(pb3, pb6, pb9))
            {
                set_color(pb3, pb6, pb9);
                return get_winner(pb3);
            }

            return enPlayer.None;

        }

        enPlayer check_diagonal_winner()
        {
            // top left to bottom right diagonal
            if (check_values(pb1, pb5, pb9))
            {
                set_color(pb1, pb5, pb9);
                return get_winner(pb1);
            }
            // top right to bottom left diagonal
            if (check_values(pb3, pb5, pb7))
            {
                set_color(pb3, pb5, pb7);
                return get_winner(pb3);
            }

            return enPlayer.None;

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

            foreach (PictureBox pictureBox in PicArr)
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

        void set_turn(PictureBox pictureBox)
        {

            ++Rounds;

            if (Turn == enPlayer.P1)
            {
                pictureBox.Image = Resources.X;
                pictureBox.Tag = "P1";
            }
            else
            {
                pictureBox.Image = Resources.O;
                pictureBox.Tag = "P2";
            }

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

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            reset_to_deffault();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender);
        }

        private void pb2_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender);
        }

        private void pb3_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender);
        }

        private void pb4_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender);
        }

        private void pb5_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender);
        }

        private void pb6_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender);
        }

        private void pb7_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender);
        }

        private void pb8_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender);
        }

        private void pb9_Click(object sender, EventArgs e)
        {
            set_turn((PictureBox)sender);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            reset_to_deffault();
        }

    }
}