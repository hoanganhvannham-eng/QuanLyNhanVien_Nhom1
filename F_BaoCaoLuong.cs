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
                if (ctl is TextBox) ((TextBox)ctl).Clear();
                else if (ctl is ComboBox) ((ComboBox)ctl).SelectedIndex = -1;
                // Không reset DateTimePicker về Now để người dùng tiện thao tác tiếp
                else if (ctl.HasChildren) ClearAllInputs(ctl);
            }
        }

        private void btnLuongHangThang_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                // Lấy tháng và năm từ DateTimePicker trên giao diện
                // Giả sử tên control là dtpThoiGian
                int thang = dateTimePicker1.Value.Month;
                int nam = dateTimePicker1.Value.Year;

                string sql = @"SELECT l.MaLuong AS N'Mã Lương', 
                                      nv.MaNV AS N'Mã Nhân Viên', 
                                      nv.HoTen AS N'Họ Tên', 
                                      l.Thang AS N'Tháng', 
                                      l.Nam AS N'Năm', 
                                      l.LuongCoBan AS N'Lương Cơ Bản', 
                                      l.SoNgayCong AS N'Ngày Công',
                                      l.PhuCap AS N'Phụ Cấp',
                                      l.KhauTru AS N'Khấu Trừ',
                                      l.TongLuong AS N'Tổng Lương'
                               FROM tblLuong l
                               JOIN tblNhanVien nv ON l.MaNV = nv.MaNV
                               WHERE l.DeletedAt = 0 
                               AND l.Thang = @Thang 
                               AND l.Nam = @Nam
                               ORDER BY l.TongLuong DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Giả sử tên GridView là dtGridViewBCLuong
                    dtGridViewBCLuong.DataSource = dt;
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bảng lương: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNVTongLuongCaoNhat_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                int thang = dateTimePicker1.Value.Month;
                int nam = dateTimePicker1.Value.Year;

                // Lấy TOP 1 nhân viên lương cao nhất (WITH TIES để lấy nhiều người nếu bằng lương nhau)
                string sql = @"SELECT TOP 1 WITH TIES 
                                      nv.HoTen AS N'Họ Tên', 
                                      pb.TenPB AS N'Phòng Ban',
                                      cv.TenCV AS N'Chức Vụ',
                                      l.TongLuong AS N'Tổng Lương Cao Nhất'
                               FROM tblLuong l
                               JOIN tblNhanVien nv ON l.MaNV = nv.MaNV
                               JOIN tblPhongBan pb ON nv.MaPB = pb.MaPB
                               JOIN tblChucVu cv ON nv.MaCV = cv.MaCV
                               WHERE l.DeletedAt = 0 
                               AND l.Thang = @Thang 
                               AND l.Nam = @Nam
                               ORDER BY l.TongLuong DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCLuong.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnTongLuongPhongBan_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                int thang = dateTimePicker1.Value.Month;
                int nam = dateTimePicker1.Value.Year;

                // Group by theo tên phòng ban và tính tổng lương
                string sql = @"SELECT pb.TenPB AS N'Tên Phòng Ban', 
                                      COUNT(l.MaNV) AS N'Số Nhân Viên Đã Nhận Lương',
                                      SUM(l.TongLuong) AS N'Tổng Quỹ Lương'
                               FROM tblLuong l
                               JOIN tblNhanVien nv ON l.MaNV = nv.MaNV
                               JOIN tblPhongBan pb ON nv.MaPB = pb.MaPB
                               WHERE l.DeletedAt = 0 
                               AND l.Thang = @Thang 
                               AND l.Nam = @Nam
                               GROUP BY pb.TenPB";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCLuong.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
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
                                var ws = wb.Worksheets.Add("BaoCaoLuong");

                                // Ghi header
                                for (int i = 0; i < dtGridViewBCLuong.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dtGridViewBCLuong.Columns[i].HeaderText;
                                    // Tô đậm header cho đẹp
                                    ws.Cell(1, i + 1).Style.Font.Bold = true;
                                    ws.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                }

                                // Ghi dữ liệu
                                for (int i = 0; i < dtGridViewBCLuong.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dtGridViewBCLuong.Columns.Count; j++)
                                    {
                                        // Kiểm tra null để tránh lỗi
                                        var cellValue = dtGridViewBCLuong.Rows[i].Cells[j].Value;
                                        ws.Cell(i + 2, j + 1).Value = cellValue != null ? cellValue.ToString() : "";
                                    }
                                }

                                // Kẻ khung viền
                                var range = ws.Range(1, 1, dtGridViewBCLuong.Rows.Count + 1, dtGridViewBCLuong.Columns.Count);
                                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                // Tự động co giãn cột
                                ws.Columns().AdjustToContents();

                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu trên bảng để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    

        
    }

}
