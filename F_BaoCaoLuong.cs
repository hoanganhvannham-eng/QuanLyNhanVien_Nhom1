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

        private void btnNVTongLuongCaoNhat_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                string sqlLoadDataLuong = @"SELECT TOP 1 nv.HoTen, l.Thang, l.Nam, l.TongLuong
                                            FROM tblLuong l
                                            JOIN tblNhanVien nv ON l.MaNV = nv.MaNV
                                            WHERE l.DeletedAt = 0
                                            ORDER BY l.TongLuong DESC;";

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

        private void btnTongLuongPhongBan_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                string sqlLoadDataLuong = @"SELECT pb.TenPB, SUM(l.TongLuong) AS TongChiPhi
                                            FROM tblLuong l
                                            JOIN tblNhanVien nv ON l.MaNV = nv.MaNV
                                            JOIN tblPhongBan pb ON nv.MaPB = pb.MaPB
                                            WHERE l.DeletedAt = 0 AND nv.DeletedAt = 0
                                            GROUP BY pb.TenPB;";

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

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dtGridViewBCLuong.Rows.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("Luong");

                                // Ghi header
                                for (int i = 0; i < dtGridViewBCLuong.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dtGridViewBCLuong.Columns[i].HeaderText;
                                }

                                // Ghi dữ liệu
                                for (int i = 0; i < dtGridViewBCLuong.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dtGridViewBCLuong.Columns.Count; j++)
                                    {
                                        ws.Cell(i + 2, j + 1).Value = dtGridViewBCLuong.Rows[i].Cells[j].Value?.ToString();
                                    }
                                }

                                // Thêm border cho toàn bảng
                                var range = ws.Range(1, 1, dtGridViewBCLuong.Rows.Count + 1, dtGridViewBCLuong.Columns.Count);
                                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                // Tự động co giãn cột
                                ws.Columns().AdjustToContents();

                                // Lưu file
                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
