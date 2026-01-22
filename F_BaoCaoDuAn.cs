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
	public partial class F_BaoCaoDuAn : Form
	{
		connectData cn = new connectData();

		public F_BaoCaoDuAn()
		{
			InitializeComponent();
		}

		// ============== FORM LOAD ==============
		private void F_BaoCaoDuAn_Load(object sender, EventArgs e)
		{
			try
			{
				LoadComboBoxDuAn();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khởi tạo form: " + ex.Message, "Lỗi",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// ============== LOAD COMBOBOX DỰ ÁN ==============
		private void LoadComboBoxDuAn()
		{
			try
			{
				cn.connect();

				string sql = @"SELECT MaDA_KienCD233824, TenDA_KienCD233824 
                               FROM tblDuAn_KienCD233824
                               WHERE DeletedAt_KienCD233824 = 0
                               ORDER BY TenDA_KienCD233824";

				SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn);
				DataTable dt = new DataTable();
				da.Fill(dt);

				// Thêm dòng mặc định
				DataRow emptyRow = dt.NewRow();
				emptyRow["MaDA_KienCD233824"] = DBNull.Value;
				emptyRow["TenDA_KienCD233824"] = "-- Chọn dự án --";
				dt.Rows.InsertAt(emptyRow, 0);

				comboBoxduan.DataSource = dt;
				comboBoxduan.DisplayMember = "TenDA_KienCD233824";
				comboBoxduan.ValueMember = "MaDA_KienCD233824";
				comboBoxduan.SelectedIndex = 0;

				cn.disconnect();
			}
			catch (Exception ex)
			{
				cn.disconnect();
				MessageBox.Show("Lỗi load danh sách dự án: " + ex.Message, "Lỗi",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// ============== NÚT LOAD NHÂN VIÊN THEO DỰ ÁN ==============
		private void btnDSNhanVienTheoDuAn_Click(object sender, EventArgs e)
		{
			try
			{
				// Kiểm tra ComboBox
				if (comboBoxduan.SelectedValue == null ||
					comboBoxduan.SelectedValue == DBNull.Value ||
					comboBoxduan.SelectedIndex <= 0)
				{
					MessageBox.Show("Vui lòng chọn dự án!", "Thông báo",
						MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				string maDuAn = comboBoxduan.SelectedValue.ToString();
				LoadNhanVienTheoDuAn(maDuAn);

				// Kiểm tra kết quả
				if (dtGridViewBCDuAn.Rows.Count == 0)
				{
					MessageBox.Show("Không có nhân viên nào trong dự án này!",
						"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					MessageBox.Show($"Đã load {dtGridViewBCDuAn.Rows.Count} nhân viên!",
						"Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// ============== HÀM LOAD NHÂN VIÊN THEO DỰ ÁN ==============
		private void LoadNhanVienTheoDuAn(string maDuAn)
		{
			try
			{
				cn.connect();

				string sql = @"
                    SELECT 
                        nv.MaNV_TuanhCD233018 AS N'Mã Nhân Viên',
                        nv.HoTen_TuanhCD233018 AS N'Tên Nhân Viên',
                        ct.VaiTro_KienCD233824 AS N'Vai Trò'
                    FROM tblChiTietDuAn_KienCD233824 ct
                    INNER JOIN tblNhanVien_TuanhCD233018 nv 
                        ON ct.MaNV_TuanhCD233018 = nv.MaNV_TuanhCD233018
                    WHERE ct.MaDA_KienCD233824 = @MaDuAn
                      AND ct.DeletedAt_KienCD233824 = 0
                    ORDER BY nv.HoTen_TuanhCD233018";

				using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
				{
					cmd.Parameters.AddWithValue("@MaDuAn", maDuAn);

					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						da.Fill(dt);
						dtGridViewBCDuAn.DataSource = dt;
					}
				}

				cn.disconnect();
			}
			catch (Exception ex)
			{
				if (cn.conn != null && cn.conn.State == ConnectionState.Open)
				{
					cn.disconnect();
				}
				throw new Exception("Lỗi load dữ liệu: " + ex.Message);
			}
		}

		// ============== NÚT THỐNG KÊ SỐ LƯỢNG NHÂN VIÊN ==============
		private void btnSoLuongNhanVien_Click(object sender, EventArgs e)
		{
			try
			{
				cn.connect();

				string sql = @"
                    SELECT 
                        da.MaDA_KienCD233824 AS N'Mã Dự Án', 
                        da.TenDA_KienCD233824 AS N'Tên Dự Án', 
                        COUNT(ct.MaNV_TuanhCD233018) AS N'Số Lượng Nhân Viên'
                    FROM tblDuAn_KienCD233824 da
                    LEFT JOIN tblChiTietDuAn_KienCD233824 ct 
                        ON da.MaDA_KienCD233824 = ct.MaDA_KienCD233824 
                        AND ct.DeletedAt_KienCD233824 = 0
                    WHERE da.DeletedAt_KienCD233824 = 0
                    GROUP BY da.MaDA_KienCD233824, da.TenDA_KienCD233824
                    ORDER BY COUNT(ct.MaNV_TuanhCD233018) DESC";

				using (SqlDataAdapter adapter = new SqlDataAdapter(sql, cn.conn))
				{
					DataTable dt = new DataTable();
					adapter.Fill(dt);
					dtGridViewBCDuAn.DataSource = dt;
				}

				cn.disconnect();

				MessageBox.Show("Đã load thống kê thành công!", "Thông báo",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				cn.disconnect();
				MessageBox.Show("Lỗi tải thống kê: " + ex.Message, "Lỗi",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// ============== NÚT TÌM KIẾM ==============
		private void btnTimKiem_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
				{
					MessageBox.Show("Vui lòng nhập tên dự án để tìm kiếm!", "Thông báo",
						MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				cn.connect();

				string sql = @"
                    SELECT 
                        MaDA_KienCD233824 AS N'Mã Dự Án', 
                        TenDA_KienCD233824 AS N'Tên Dự Án', 
                        MoTa_KienCD233824 AS N'Mô Tả', 
                        NgayBatDau_KienCD233824 AS N'Ngày Bắt Đầu', 
                        NgayKetThuc_KienCD233824 AS N'Ngày Kết Thúc'
                    FROM tblDuAn_KienCD233824
                    WHERE DeletedAt_KienCD233824 = 0 
                      AND TenDA_KienCD233824 LIKE @TenTimKiem
                    ORDER BY TenDA_KienCD233824";

				using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
				{
					cmd.Parameters.AddWithValue("@TenTimKiem", "%" + txtTimKiem.Text.Trim() + "%");

					using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						adapter.Fill(dt);
						dtGridViewBCDuAn.DataSource = dt;

						if (dt.Rows.Count == 0)
						{
							MessageBox.Show("Không tìm thấy dự án nào!", "Thông báo",
								MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
				}

				cn.disconnect();
			}
			catch (Exception ex)
			{
				cn.disconnect();
				MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// ============== NÚT XUẤT EXCEL ==============
		private void btnXuatExcel_Click(object sender, EventArgs e)
		{
			if (dtGridViewBCDuAn.Rows.Count == 0)
			{
				MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			string fileName = "BaoCaoDuAn_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xlsx";

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

							// TIÊU ĐỀ
							ws.Cell(1, 1).Value = "BÁO CÁO DỰ ÁN";
							ws.Range(1, 1, 1, colCount).Merge();
							ws.Range(1, 1, 1, colCount).Style.Font.Bold = true;
							ws.Range(1, 1, 1, colCount).Style.Font.FontSize = 18;
							ws.Range(1, 1, 1, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

							// NGÀY XUẤT
							ws.Cell(2, 1).Value = "Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
							ws.Range(2, 1, 2, colCount).Merge();
							ws.Range(2, 1, 2, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
							ws.Range(2, 1, 2, colCount).Style.Font.Italic = true;

							// HEADER
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

							// DỮ LIỆU
							for (int i = 0; i < dtGridViewBCDuAn.Rows.Count; i++)
							{
								for (int j = 0; j < colCount; j++)
								{
									var value = dtGridViewBCDuAn.Rows[i].Cells[j].Value;
									ws.Cell(i + 5, j + 1).Value = value != null ? value.ToString() : "";
								}
							}

							// BORDER
							var dataRange = ws.Range(4, 1, dtGridViewBCDuAn.Rows.Count + 4, colCount);
							dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
							dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

							ws.Columns().AdjustToContents();
							wb.SaveAs(sfd.FileName);
						}

						MessageBox.Show("Xuất Excel thành công!\nFile: " + sfd.FileName,
							"Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					catch (Exception ex)
					{
						MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi",
							MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		// ============== COMBOBOX CHANGED (NẾU CẦN TỰ ĐỘNG LOAD) ==============
		private void comboBoxduan_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Nếu muốn tự động load khi chọn combobox, bỏ comment code dưới
			/*
            try
            {
                if (comboBoxduan.SelectedValue != null && 
                    comboBoxduan.SelectedValue != DBNull.Value &&
                    comboBoxduan.SelectedIndex > 0)
                {
                    string maDuAn = comboBoxduan.SelectedValue.ToString();
                    LoadNhanVienTheoDuAn(maDuAn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
		}

		// ============== HÀM XÓA DỮ LIỆU INPUT ==============
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
	}
}