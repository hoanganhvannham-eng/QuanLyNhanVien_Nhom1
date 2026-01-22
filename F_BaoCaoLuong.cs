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
    public partial class F_BaoCaoLuong : Form
    {
        public F_BaoCaoLuong()
        {
            InitializeComponent();
        }

        connectData cn = new connectData();

        private void btnLuongHangThang_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                int thang = dateTimePicker1.Value.Month;
                int nam = dateTimePicker1.Value.Year;

                // Sửa lại query theo đúng cấu trúc CSDL
                string sql = @"SELECT l.MaLuong_ChienCD232928 AS N'Mã Lương', 
                                      nv.MaNV_TuanhCD233018 AS N'Mã Nhân Viên', 
                                      nv.HoTen_TuanhCD233018 AS N'Họ Tên', 
                                      l.Thang_ChienCD232928 AS N'Tháng', 
                                      l.Nam_ChienCD232928 AS N'Năm', 
                                      l.LuongCoBan_ChienCD232928 AS N'Lương Cơ Bản', 
                                      l.SoNgayCongChuan_ChienCD232928 AS N'Ngày Công Chuẩn',
                                      l.PhuCap_ChienCD232928 AS N'Phụ Cấp',
                                      l.KhauTru_ChienCD232928 AS N'Khấu Trừ',
                                      (l.LuongCoBan_ChienCD232928 + l.PhuCap_ChienCD232928 - l.KhauTru_ChienCD232928) AS N'Tổng Lương'
                               FROM tblLuong_ChienCD232928 l
                               JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                               JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                               WHERE l.DeletedAt_ChienCD232928 = 0 
                               AND l.Thang_ChienCD232928 = @Thang 
                               AND l.Nam_ChienCD232928 = @Nam
                               ORDER BY N'Tổng Lương' DESC";

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

                string sql = @"SELECT TOP 1 WITH TIES 
                                      nv.HoTen_TuanhCD233018 AS N'Họ Tên', 
                                      nv.MaNV_TuanhCD233018 AS N'Mã Nhân Viên',
                                      (l.LuongCoBan_ChienCD232928 + l.PhuCap_ChienCD232928 - l.KhauTru_ChienCD232928) AS N'Tổng Lương Cao Nhất'
                               FROM tblLuong_ChienCD232928 l
                               JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                               JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                               WHERE l.DeletedAt_ChienCD232928 = 0 
                               AND l.Thang_ChienCD232928 = @Thang 
                               AND l.Nam_ChienCD232928 = @Nam
                               ORDER BY N'Tổng Lương Cao Nhất' DESC";

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

                // Vì bảng nhân viên không có cột phòng ban, ta sẽ bỏ phần này hoặc tìm cách khác
                // Tạm thời chỉ hiển thị tổng lương toàn công ty
                string sql = @"SELECT 
                                      'Toàn Công Ty' AS N'Phòng Ban', 
                                      COUNT(DISTINCT nv.Id_TuanhCD233018) AS N'Số Nhân Viên Đã Nhận Lương',
                                      SUM(l.LuongCoBan_ChienCD232928 + l.PhuCap_ChienCD232928 - l.KhauTru_ChienCD232928) AS N'Tổng Quỹ Lương'
                               FROM tblLuong_ChienCD232928 l
                               JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                               JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                               WHERE l.DeletedAt_ChienCD232928 = 0 
                               AND l.Thang_ChienCD232928 = @Thang 
                               AND l.Nam_ChienCD232928 = @Nam";

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
                string fileName = $"BaoCaoLuong_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx";

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
                                var ws = wb.Worksheets.Add("BaoCaoLuong");

                                int colCount = dtGridViewBCLuong.Columns.Count;

                                /* ================= TIÊU ĐỀ BÁO CÁO ================= */
                                ws.Cell(1, 1).Value = "BÁO CÁO LƯƠNG NHÂN VIÊN";
                                ws.Range(1, 1, 1, colCount).Merge();
                                ws.Range(1, 1, 1, colCount).Style.Font.Bold = true;
                                ws.Range(1, 1, 1, colCount).Style.Font.FontSize = 18;
                                ws.Range(1, 1, 1, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                /* ================= NGÀY XUẤT ================= */
                                ws.Cell(2, 1).Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                                ws.Range(2, 1, 2, colCount).Merge();
                                ws.Range(2, 1, 2, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(2, 1, 2, colCount).Style.Font.Italic = true;

                                /* ================= HEADER ================= */
                                for (int i = 0; i < colCount; i++)
                                {
                                    ws.Cell(4, i + 1).Value = dtGridViewBCLuong.Columns[i].HeaderText;
                                    ws.Cell(4, i + 1).Style.Font.Bold = true;
                                    ws.Cell(4, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ws.Cell(4, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                }

                                /* ================= DỮ LIỆU ================= */
                                for (int i = 0; i < dtGridViewBCLuong.Rows.Count; i++)
                                {
                                    for (int j = 0; j < colCount; j++)
                                    {
                                        var value = dtGridViewBCLuong.Rows[i].Cells[j].Value;
                                        ws.Cell(i + 5, j + 1).Value = value != null ? value.ToString() : "";
                                    }
                                }

                                /* ================= BORDER ================= */
                                var range = ws.Range(
                                    4, 1,
                                    dtGridViewBCLuong.Rows.Count + 4,
                                    colCount
                                );

                                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                /* ================= AUTO SIZE ================= */
                                ws.Columns().AdjustToContents();

                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearAllInputs(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                if (ctl is TextBox) ((TextBox)ctl).Clear();
                else if (ctl is ComboBox) ((ComboBox)ctl).SelectedIndex = -1;
                else if (ctl.HasChildren) ClearAllInputs(ctl);
            }
        }
    }
}