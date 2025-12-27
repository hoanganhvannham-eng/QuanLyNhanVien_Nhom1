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
                                      nv.MaNV as N'Mã Nhân Viên', 
                                      nv.HoTen as N'Tên Nhân Viên', 
                                      ct.VaiTro as N'Vai Trò',
                                      da.NgayBatDau as N'Ngày Bắt Đầu Dự Án'
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
                string fileName = "BaoCaoDuAn_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";

                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "Excel Workbook|*.xlsx",
                    FileName = fileName
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("BaoCaoDuAn");

                                int colCount = dtGridViewBCDuAn.Columns.Count;

                                /* ================= TIÊU ĐỀ BÁO CÁO ================= */
                                ws.Cell(1, 1).Value = "BÁO CÁO DỰ ÁN";
                                ws.Range(1, 1, 1, colCount).Merge();
                                ws.Range(1, 1, 1, colCount).Style.Font.Bold = true;
                                ws.Range(1, 1, 1, colCount).Style.Font.FontSize = 18;
                                ws.Range(1, 1, 1, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                /* ================= NGÀY XUẤT ================= */
                                ws.Cell(2, 1).Value = "Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                                ws.Range(2, 1, 2, colCount).Merge();
                                ws.Range(2, 1, 2, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                ws.Range(2, 1, 2, colCount).Style.Font.Italic = true;

                                /* ================= HEADER ================= */
                                for (int i = 0; i < colCount; i++)
                                {
                                    ws.Cell(4, i + 1).Value = dtGridViewBCDuAn.Columns[i].HeaderText;
                                }

                                var headerRange = ws.Range(4, 1, 4, colCount);
                                headerRange.Style.Font.Bold = true;
                                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                /* ================= DỮ LIỆU ================= */
                                for (int i = 0; i < dtGridViewBCDuAn.Rows.Count; i++)
                                {
                                    for (int j = 0; j < colCount; j++)
                                    {
                                        var value = dtGridViewBCDuAn.Rows[i].Cells[j].Value;
                                        ws.Cell(i + 5, j + 1).Value = value != null ? value.ToString() : "";
                                    }
                                }

                                /* ================= BORDER + AUTOFIT ================= */
                                var dataRange = ws.Range(4, 1,
                                    dtGridViewBCDuAn.Rows.Count + 4,
                                    colCount);

                                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                ws.Columns().AdjustToContents();

                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xuất Excel: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
