using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThapHN
{
    public partial class DemoThapHN : Form
    {
        TimeSpan time;
        int moveCount;//Số lần di chuyển
        PictureBox[] disks;//mang nhung cai dia
        int[] Vitri;//vi tri dia
        Stack<PictureBox> disksA, disksB, disksC, firstClickDisks, secondClickDisks;
        const int firstY = 402;
        public DemoThapHN()
        {
            InitializeComponent();
            disks = new PictureBox[] { pic1, pic2, pic3, pic4, pic5, pic6, pic7, pic8 };
            CotA.Tag = disksA = new Stack<PictureBox>();
            CotB.Tag = disksB = new Stack<PictureBox>();
            CotC.Tag = disksC = new Stack<PictureBox>();
        }
        private void LuatChoi_Click(object sender, EventArgs e)
        {
            MessageBox.Show("- Mỗi lượt chỉ di chuyển 1 đĩa.\n- Đĩa nằm trên phải nhỏ hơn đĩa nằm dưới.", "Luật chơi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*private void btGiai_Click(object sender, EventArgs e)
        {
            Choi_Click(disks, e);
            ChiuThua.Enabled = false;
            Choi.Enabled = false;
            Choi.Text = "Bắt đầu";
            DemThoiGian.Stop();
           
        }*/
        private void pic3_Click(object sender, EventArgs e)
        {
        }
        private void ProcessMovingDisk(PictureBox ClickRod)
        {
            if (secondClickDisks.Count == 0)
                MoveDisk(new Point(ClickRod.Location.X, firstY));
            else
            {
                PictureBox firstTopDisk = firstClickDisks.Peek();
                PictureBox secondTopDisk = secondClickDisks.Peek();
                if (int.Parse(firstTopDisk.Tag.ToString()) < int.Parse(secondTopDisk.Tag.ToString()))
                    MoveDisk(new Point(secondTopDisk.Location.X, secondTopDisk.Location.Y - 20));
                else
                    secondClickDisks = null;
            }
        }

        private void pic3Rod_Click(object sender, EventArgs e)
        {
            if (SoDia.Enabled) return;
            PictureBox ClickRod = (PictureBox)sender;
            Stack<PictureBox> disksOfClickRod = (Stack<PictureBox>)ClickRod.Tag;
            if (firstClickDisks == null)
            {
                if (disksOfClickRod.Count == 0) return;
                firstClickDisks = disksOfClickRod;
                ClickRod.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (secondClickDisks == null)
            {
                if (disksOfClickRod == firstClickDisks)
                {
                    firstClickDisks = null;
                    ClickRod.BorderStyle = BorderStyle.None;
                    return;
                }
                secondClickDisks = disksOfClickRod;
                ProcessMovingDisk(ClickRod);
            }
        }

        private void MoveDisk(Point point)
        {
            PictureBox firstTopDisk = firstClickDisks.Pop();
            firstTopDisk.Location = point;
            secondClickDisks.Push(firstTopDisk);
            ++moveCount;
            SoLanDiChuyen.Text = string.Format("Lượt đi: {0} lần", moveCount);
            firstClickDisks = secondClickDisks = null;
            CotA.BorderStyle = CotB.BorderStyle = CotC.BorderStyle = BorderStyle.None;

            if (disksC.Count == SoDia.Value)
            {
                ChiuThua.PerformClick();
                MessageBox.Show("Chúc mừng bạn đã hoàn thành trò chơi", "Good Job!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void DemThoiGian_Tick(object sender, EventArgs e)
        {
            time = time.Add(new TimeSpan(0, 0, 1));
            ThoiGian.Text = string.Format("Thời gian: {0:00}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);
        }

        private void ChiuThua_Click(object sender, EventArgs e)
        {
            DemThoiGian.Stop();
            SoDia.Enabled = true;
            ChiuThua.Enabled = false;
            Choi.Text = "Bắt đầu";
        }

        private void Choi_Click(object sender, EventArgs e)
        {
            //reset
            DemThoiGian.Stop();
            foreach (PictureBox disk in disks)
                disk.Visible = false;
            time = new TimeSpan(0);
            moveCount = 0;
            ThoiGian.Text = "Thời gian: 00:00:00";
            SoLanDiChuyen.Text = "Lượt đi: 0 lần";
            disksA.Clear();
            disksB.Clear();
            disksC.Clear();
            CotA.BorderStyle = CotB.BorderStyle = CotC.BorderStyle = BorderStyle.None;
            firstClickDisks = secondClickDisks = null;
            //khởi tạo
            SoDia.Enabled = false;
            ChiuThua.Enabled = true;
            Choi.Text = "Chơi lại";
            switch ((int)SoDia.Value)
            {
                case 2:
                    lblBest.Text = "Tốt nhất: 3 lần";
                    break;
                case 3: lblBest.Text = "Tốt nhất: 7 lần";
                    break;
                case 4:
                    lblBest.Text = "Tốt nhất: 15 lần";
                    break;
                case 5:
                    lblBest.Text = "Tốt nhất: 31 lần";
                    break;
                case 6:
                    lblBest.Text = "Tốt nhất: 63 lần";
                    break;
                case 7:
                    lblBest.Text = "Tốt nhất: 127 lần";
                    break;
                case 8:
                    lblBest.Text = "Tốt nhất: 255 lần";
                    break;
            }
            int x = CotA.Location.X, y = firstY;
            for (int i = (int)SoDia.Value - 1; i >= 0; i--, y -= 20)
            {
                disks[i].Location = new Point(x, y);
                disks[i].Visible = true;
                disksA.Push(disks[i]);
            }
            DemThoiGian.Start();
        }
        void HieuUng(int CotNguon, int CotDich)
        {
            SoLanDiChuyen.Text = "Lượt đi: " + moveCount.ToString();
            int speed = 10;//toc do di chuyen dia
            PictureBox picDia = disks[1];
            //tim dia can chuyen 
            for (int i = 1; i <= (int)SoDia.Value; i++)
            {
                if (Vitri[i] == CotNguon)
                {
                    picDia = disks[i];
                    Vitri[i] = CotDich;
                    break;
                }
            }
            //Tinh so dia trong cot dich
            int CountDich = -1;
            for (int i = 1; i <= (int)SoDia.Value; i++)
            {
                if (Vitri[i] == CotDich)
                    CountDich++;
            }
            //Nang dia len tren
            while (picDia.Top > 100)
            {
                picDia.Top -= speed;
                Application.DoEvents();
            }
            //Nang dia sang phai sang trai
            int ViTriMoi = CotDich == 1 ? 190 : CotDich == 2 ? 395 : 600;
            if (picDia.Left + picDia.Width / 2 < ViTriMoi)
            {
                picDia.Left += speed;
                Application.DoEvents();
            }
            else if (picDia.Left + picDia.Width / 2 > ViTriMoi)
            {
                while (picDia.Left + picDia.Width / 2 > ViTriMoi)
                {
                    picDia.Left -= speed;
                    Application.DoEvents();
                }
            }
            //Dat dia xuong
            while (picDia.Top < 402 - CountDich * 20)
            {
                picDia.Top += speed;
                Application.DoEvents();
            }
        }
        //Chuyen dia
        public void ChuyenDia(int SoDia,int CotNguon,int CotTG,int CotDich)
        {
            if (SoDia > 0) ;
            ChuyenDia(SoDia - 1, CotNguon, CotDich, CotTG);
            moveCount++;
            HieuUng(CotNguon, CotDich);
            ChuyenDia(SoDia - 1, CotTG, CotNguon, CotDich);
        }
    }
}
