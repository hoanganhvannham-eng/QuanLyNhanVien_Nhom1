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
    public partial class F_BaoCaoDuAn: Form
    {
        connectData cn = new connectData();
        // Hàm xóa dữ liệu input (nếu cần dùng lại)
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

        public F_BaoCaoDuAn()
        {
            InitializeComponent();
        }

        private void btnDSNhanVienTheoDuAn_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                // Join 3 bảng để lấy thông tin: Tên Dự Án - Tên Nhân Viên - Vai Trò
                string sql = @"SELECT da.TenDA as N'Tên Dự Án', 
                                      nv.MaNV as N'Mã NV', 
                                      nv.HoTen as N'Tên Nhân Viên', 
                                      ct.VaiTro as N'Vai Trò',
                                      da.NgayBatDau as N'Ngày Bắt Đầu DA'
                               FROM tblDuAn da
                               JOIN tblChiTietDuAn ct ON da.MaDA = ct.MaDA
                               JOIN tblNhanVien nv ON ct.MaNV = nv.MaNV
                               WHERE da.DeletedAt = 0 AND ct.DeletedAt = 0
                               ORDER BY da.TenDA, nv.HoTen;";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCDuAn.DataSource = dt; // Lưu ý: Đổi tên GridView cho phù hợp bên Design
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSoLuongNhanVien_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                string sql = @"SELECT da.MaDA as N'Mã Dự Án', 
                                      da.TenDA as N'Tên Dự Án', 
                                      COUNT(ct.MaNV) as N'Số Lượng Nhân Viên'
                               FROM tblDuAn da
                               LEFT JOIN tblChiTietDuAn ct ON da.MaDA = ct.MaDA AND ct.DeletedAt = 0
                               WHERE da.DeletedAt = 0
                               GROUP BY da.MaDA, da.TenDA
                               ORDER BY COUNT(ct.MaNV) DESC;";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCDuAn.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTimKiem.Text)) // Lưu ý: Đổi tên TextBox thành txtTenDuAn
                {
                    MessageBox.Show("Vui lòng nhập tên dự án để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();
                // Tìm kiếm gần đúng theo tên dự án
                string sql = @"SELECT MaDA as N'Mã Dự Án', 
                                      TenDA as N'Tên Dự Án', 
                                      MoTa as N'Mô Tả', 
                                      NgayBatDau as N'Ngày Bắt Đầu', 
                                      NgayKetThuc as N'Ngày Kết Thúc'
                               FROM tblDuAn
                               WHERE DeletedAt = 0 
                               AND TenDA LIKE @TenTimKiem
                               ORDER BY TenDA;";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@TenTimKiem", "%" + txtTimKiem.Text + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCDuAn.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dtGridViewBCDuAn.Rows.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("BaoCaoDuAn");

                                // Ghi header
                                for (int i = 0; i < dtGridViewBCDuAn.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dtGridViewBCDuAn.Columns[i].HeaderText;
                                }

                                // Ghi dữ liệu
                                for (int i = 0; i < dtGridViewBCDuAn.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dtGridViewBCDuAn.Columns.Count; j++)
                                    {
                                        // Kiểm tra null để tránh lỗi khi convert string
                                        var cellValue = dtGridViewBCDuAn.Rows[i].Cells[j].Value;
                                        ws.Cell(i + 2, j + 1).Value = cellValue != null ? cellValue.ToString() : "";
                                    }
                                }

                                // Format bảng đẹp (Border + AutoFit)
                                var range = ws.Range(1, 1, dtGridViewBCDuAn.Rows.Count + 1, dtGridViewBCDuAn.Columns.Count);
                                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                                ws.Columns().AdjustToContents();

                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
