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
    public partial class F_BaoCaoTongHop: Form
    {
        connectData cn = new connectData();
        public F_BaoCaoTongHop()
        {
            InitializeComponent();
        }
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


        private void F_BaoCaoTongHop_Load(object sender, EventArgs e)
        {

        }

        private void btnTongHopChung_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                // Câu truy vấn đếm số lượng từ 3 bảng khác nhau và trả về 1 dòng kết quả
                string sql = @"
                    SELECT 
                        (SELECT COUNT(*) FROM tblNhanVien WHERE DeletedAt = 0) AS [Tổng Số Nhân Viên],
                        (SELECT COUNT(*) FROM tblPhongBan WHERE DeletedAt = 0) AS [Tổng Số Phòng Ban],
                        (SELECT COUNT(*) FROM tblDuAn WHERE DeletedAt = 0) AS [Tổng Số Dự Án]
                ";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCTongHop.DataSource = dt;

                    // Tự động chỉnh độ rộng cột cho đẹp
                    dtGridViewBCTongHop.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo tổng hợp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNhanVienNhieuDuAn_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                // Câu truy vấn Join bảng Nhân viên và ChiTietDuAn, Group by và lấy Top 1
                // Sử dụng TOP 1 WITH TIES để lấy tất cả những người cùng hạng nhất (nếu có nhiều người bằng nhau)
                string sql = @"
                    SELECT TOP 1 WITH TIES 
                        nv.MaNV AS [Mã Nhân Viên], 
                        nv.HoTen AS [Họ Tên], 
                        pb.TenPB AS [Phòng Ban],
                        COUNT(ct.MaDA) AS [Số Lượng Dự Án Tham Gia]
                    FROM tblNhanVien nv
                    JOIN tblChiTietDuAn ct ON nv.MaNV = ct.MaNV
                    JOIN tblPhongBan pb ON nv.MaPB = pb.MaPB
                    WHERE nv.DeletedAt = 0
                    GROUP BY nv.MaNV, nv.HoTen, pb.TenPB
                    ORDER BY COUNT(ct.MaDA) DESC;
                ";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCTongHop.DataSource = dt;

                    // Tự động chỉnh độ rộng cột
                    dtGridViewBCTongHop.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu dự án: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dtGridViewBCTongHop.Rows.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("BaoCao");

                                // Ghi header
                                for (int i = 0; i < dtGridViewBCTongHop.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dtGridViewBCTongHop   .Columns[i].HeaderText;
                                    // Tô đậm header
                                    ws.Cell(1, i + 1).Style.Font.Bold = true;
                                }

                                // Ghi dữ liệu
                                for (int i = 0; i < dtGridViewBCTongHop.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dtGridViewBCTongHop.Columns.Count; j++)
                                    {
                                        // Kiểm tra null trước khi to string
                                        var cellValue = dtGridViewBCTongHop.Rows[i].Cells[j].Value;
                                        ws.Cell(i + 2, j + 1).Value = cellValue != null ? cellValue.ToString() : "";
                                    }
                                }

                                // Format bảng đẹp
                                var range = ws.Range(1, 1, dtGridViewBCTongHop.Rows.Count + 1, dtGridViewBCTongHop.Columns.Count);
                                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
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
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
