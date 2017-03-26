using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MemoryApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int poziom = 20;
        private int liczbaObrazkow = 10;

        private int ileZgadnietych = 0;
        private int nrPicBoxOdkryty1 = -1;
        private int nrPicBoxOdkryty2 = -1;

        private FileInfo[] animowane;
        private FileInfo[] zabawne;


        private void opcjeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo
                    (@"C:\Users\Jacek\Documents\GitHub\MemoryGame\Animowane");

            animowane = dirInfo.GetFiles("*.jpg");


            dirInfo = new System.IO.DirectoryInfo
                    (@"C:\Users\Jacek\Documents\GitHub\MemoryGame\Zabawne");


            zabawne = dirInfo.GetFiles("*.jpg");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < poziom; i++)
             {
                PictureBox picBox = ((PictureBox)flpKontener.Controls[i]);

                            picBox.Tag = picBox.Image = null;
                            picBox.BackColor = Color.Transparent;
                            picBox.Enabled = true;
                            ileZgadnietych = 0;
                            nrPicBoxOdkryty1 = nrPicBoxOdkryty2 = -1;
             }

            if (łatwyToolStripMenuItem.Checked) poziom = 8;
            else
            if (średniToolStripMenuItem.Checked) poziom = 14;
            else poziom = 20;
            int[] losNrObrazkow = losuj(poziom / 2, liczbaObrazkow);
            int[] losNrPicBox = losuj(poziom, poziom);
            int k = 0;
            for (int i = 0; i < poziom / 2; i++)
            {
                ((PictureBox)flpKontener.Controls[losNrPicBox[k]]).Tag =
                losNrObrazkow[i];
                ((PictureBox)flpKontener.Controls[losNrPicBox[k]]).BackColor =
                Color.AliceBlue;
                ((PictureBox)flpKontener.Controls[losNrPicBox[k + 1]]).Tag =
                losNrObrazkow[i];
                ((PictureBox)flpKontener.Controls[losNrPicBox[k + 1]]).BackColor =
                Color.AliceBlue;
                k += 2;
            }
        }
        private int[] losuj(int ileElem,int zakres)
        {
            var result = Enumerable.Range(0, zakres)
                 .OrderBy(n => Guid.NewGuid())
                 .Take(ileElem)
                 .OrderBy(n => Guid.NewGuid());

            return result.ToArray();
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox20_MouseEnter(object sender, EventArgs e)
        {
            PictureBox picBox = (PictureBox)sender;
            if (picBox.Tag != null)
            {
                picBox.BackColor = Color.DarkGray;
            }
        }

        private void pictureBox20_MouseLeave(object sender, EventArgs e)
        {
            PictureBox picBox = (PictureBox)sender;
            if (picBox.Tag != null)
            {
                picBox.BackColor = Color.AliceBlue;
            }
        }
        
        private void pictureBox20_Click(object sender, EventArgs e)
        {
            try
            {

                PictureBox picBoxObecny = ((PictureBox)sender);
                int nrPicBoxObecny =
                flpKontener.Controls.GetChildIndex(picBoxObecny);
                if (picBoxObecny.Tag != null)
                {
                    if (nrPicBoxObecny != nrPicBoxOdkryty1 &&
                    nrPicBoxObecny != nrPicBoxOdkryty2)
                    {
                        int nrObrazka = (int)picBoxObecny.Tag;
                        if (animowaneToolStripMenuItem.Checked)
                            picBoxObecny.Image =
                            Image.FromFile(animowane[nrObrazka].FullName);
                        else
                            picBoxObecny.Image =
                            Image.FromFile(zabawne[nrObrazka].FullName);
                        if (nrPicBoxOdkryty1 == -1)
                            nrPicBoxOdkryty1 = nrPicBoxObecny;
                        else
                        if (nrPicBoxOdkryty2 == -1)
                        {
                            nrPicBoxOdkryty2 = nrPicBoxObecny;
                            if (ileZgadnietych == (poziom - 2))
                            {
                                DialogResult result = MessageBox.Show
                                    (this, "Koniec gry.\nJeszcze raz to samo ?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                    btnStart_Click(sender, e);
                            }
                        }
                        else
                        {
                            PictureBox picBox1 =
                            (PictureBox)flpKontener.Controls[nrPicBoxOdkryty1];
                            PictureBox picBox2 =
                            (PictureBox)flpKontener.Controls[nrPicBoxOdkryty2];
                            if (!((int)picBox1.Tag == (int)picBox2.Tag))
                            {
                                picBox1.Image = null;
                                picBox2.Image = null;
                            }
                            else
                            {
                                picBox1.Enabled = false;
                                picBox2.Enabled = false;
                                ileZgadnietych += 2;
                            }
                            nrPicBoxOdkryty1 = nrPicBoxObecny;
                            nrPicBoxOdkryty2 = -1;
                        }
                    }
                }
            }
            catch (Exception)
            {
                DialogResult wyjatek = MessageBox.Show
                                    (this, "Ten obrazek jest juz odkryty ?", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                throw;
            }     
        }

        private void łatwyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (łatwyToolStripMenuItem.Checked) return;
            łatwyToolStripMenuItem.Checked = true;
            średniToolStripMenuItem.Checked = trudnyToolStripMenuItem.Checked = false;
        }

        private void średniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (średniToolStripMenuItem.Checked) return;
            średniToolStripMenuItem.Checked = true;
            łatwyToolStripMenuItem.Checked = trudnyToolStripMenuItem.Checked = false;
        }

        private void trudnyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (trudnyToolStripMenuItem.Checked) return;
            trudnyToolStripMenuItem.Checked = true;
            łatwyToolStripMenuItem.Checked = średniToolStripMenuItem.Checked = false;
        }

        private void zabawneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (zabawneToolStripMenuItem.Checked) return;
            zabawneToolStripMenuItem.Checked = true;
            animowaneToolStripMenuItem.Checked = false;

        }

        private void animowaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (animowaneToolStripMenuItem.Checked) return;
            animowaneToolStripMenuItem.Checked = true;
            zabawneToolStripMenuItem.Checked = false;
        }
    }
}
