using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhanVien3
{
    public partial class F_FormMain: Form
    {
        public F_FormMain()
        {
            InitializeComponent();
        }
        private void OpenChildForm(Form childForm)
        {
            // Kiểm tra xem form này đã mở chưa
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.GetType() == childForm.GetType())
                {
                    frm.Activate(); // Nếu đã mở thì chỉ cần kích hoạt form đó
                    return;
                }
            }

            // Nếu chưa mở -> mở mới
            childForm.MdiParent = this; 
            childForm.Show();
        }

        private void ThongTinDuAnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new F_DuAn());
            //DuAn DA = new DuAn();
            //DA.MdiParent = this;
            //DA.Show();
        }

        private void chiTietDuAnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new F_ChiTietDuAn());
            //F_ChiTietDuAn CTDA = new F_ChiTietDuAn();
            //CTDA.MdiParent = this;
            //CTDA.Show();
        }

        private void phongBanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new F_PhongBan());
            //PhongBan PB = new PhongBan();
            //PB.MdiParent = this;
            //PB.Show();
        }

        private void chucVuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new F_ChucVu());
            //ChucVu CV = new ChucVu();
            //CV.MdiParent = this;
            //CV.Show();
        }

        private void nhanVienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new F_NhanVien());
            //NhanVien NV = new NhanVien();
            //NV.MdiParent = this;
            //NV.Show();
        }

        private void luongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new F_Luong());
            //Luong L = new Luong();
            //L.MdiParent = this;
            //L.Show();
        }

        private void chamConngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new F_ChamCong());
            //ChamCong CC = new ChamCong();
            //CC.MdiParent = this;
            //CC.Show();
        }

        private void HopDongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new F_HopDong());
            //HopDong HD = new HopDong();
            //HD.MdiParent = this;
            //HD.Show();
        }

        private void TaiKoanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new F_TaiKhoan());
            //TaiKhoan TK = new TaiKhoan();
            //TK.MdiParent = this;
            //TK.Show();
        }

        private void F_FormMain_Load(object sender, EventArgs e)
        {

        }

        private void thoátToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có chắc chắn muốn thoat khong?", "tieu de thoat",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void ThongKe_Click(object sender, EventArgs e)
        {
            OpenChildForm(new F_ThongKeNhanVien());
        }
    }
}
