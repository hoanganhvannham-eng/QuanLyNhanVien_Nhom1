using ClosedXML.Excel;
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

namespace QuanLyNhanVien3
{
    public partial class F_BaoCaoNhanVien: Form
    {
        public F_BaoCaoNhanVien()
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

        private void btnThongKeNhanVien_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                string sqlLoadDataNhanVien = @"SELECT nv.MaNV, nv.HoTen, pb.TenPB, cv.TenCV, nv.Email
                                                    FROM tblNhanVien nv
                                                    JOIN tblPhongBan pb ON nv.MaPB = pb.MaPB
                                                    JOIN tblChucVu cv ON nv.MaCV = cv.MaCV
                                                    WHERE nv.DeletedAt = 0
                                                    ORDER BY pb.TenPB, cv.TenCV;"; 

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataNhanVien, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCNhanVien.DataSource = dt;
                }
                cn.disconnect();
                ClearAllInputs(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsoluongtheogioitinh_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                string sqlLoadDataNhanVien = @"SELECT GioiTinh, COUNT(*) AS SoLuong
                                                FROM tblNhanVien
                                                WHERE DeletedAt = 0
                                                GROUP BY GioiTinh;
                                                ";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataNhanVien, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCNhanVien.DataSource = dt;
                }
                cn.disconnect();
                ClearAllInputs(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatEXL_Click(object sender, EventArgs e)
        {
            if (dtGridViewBCNhanVien.Rows.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("NhanVien");

                                // Ghi header
                                for (int i = 0; i < dtGridViewBCNhanVien.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dtGridViewBCNhanVien.Columns[i].HeaderText;
                                }

                                // Ghi dữ liệu
                                for (int i = 0; i < dtGridViewBCNhanVien.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dtGridViewBCNhanVien.Columns.Count; j++)
                                    {
                                        ws.Cell(i + 2, j + 1).Value = dtGridViewBCNhanVien.Rows[i].Cells[j].Value?.ToString();
                                    }
                                }

                                // Thêm border cho toàn bảng
                                var range = ws.Range(1, 1, dtGridViewBCNhanVien.Rows.Count + 1, dtGridViewBCNhanVien.Columns.Count);
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

        private void btnTimKiemTheoTen_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txttimkiemtheoten.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên nhân viên để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();   // MỞ KẾT NỐI

                string sql = @"SELECT nv.MaNV, nv.HoTen, pb.TenPB, cv.TenCV, nv.Email
                       FROM tblNhanVien nv
                       JOIN tblPhongBan pb ON nv.MaPB = pb.MaPB
                       JOIN tblChucVu cv ON nv.MaCV = cv.MaCV
                       WHERE nv.DeletedAt = 0 
                             AND nv.HoTen LIKE @TenTimKiem
                       ORDER BY pb.TenPB, cv.TenCV;";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    // chỉ thêm 1 lần % thôi
                    cmd.Parameters.AddWithValue("@TenTimKiem", "%" + txttimkiemtheoten.Text + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCNhanVien.DataSource = dt;
                }

                cn.disconnect(); // ĐÓNG KẾT NỐI
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }
    }
}
