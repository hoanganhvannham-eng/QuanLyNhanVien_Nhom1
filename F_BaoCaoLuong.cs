using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace QuanLyNhanVien3
{
    public partial class F_BaoCaoLuong: Form
    {
        public F_BaoCaoLuong()
        {
            InitializeComponent();
        }

        connectData cn = new connectData();
        private void ClearAllInputs(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).Clear();
                else if (ctl is ComboBox)
                    ((ComboBox)ctl).SelectedIndex = -1;
                else if (ctl is DateTimePicker)
                    ((DateTimePicker)ctl).Value = DateTime.Now;
                else if (ctl.HasChildren)
                    ClearAllInputs(ctl);
            }
        }

        private void btnLuongHangThang_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                string sqlLoadDataLuong = @"SELECT l.MaLuong, nv.HoTen, l.Thang, l.Nam, l.LuongCoBan, l.PhuCap, l.KhauTru, l.TongLuong
                                            FROM tblLuong l
                                            JOIN tblNhanVien nv ON l.MaNV = nv.MaNV
                                            WHERE l.DeletedAt = 0
                                            ORDER BY l.Nam, l.Thang;";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataLuong, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCLuong.DataSource = dt;
                }
                cn.disconnect();
                ClearAllInputs(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu bảng lương: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
