namespace QuanLyNhanVien3
{
    partial class F_BaoCaoChamCong
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.cbBoxChucVu = new System.Windows.Forms.ComboBox();
            this.cbBoxMaPB = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.pnlControl = new System.Windows.Forms.Panel();
            this.grpTime = new System.Windows.Forms.GroupBox();
            this.dtpThoiGian = new System.Windows.Forms.DateTimePicker();
            this.grpReport = new System.Windows.Forms.GroupBox();
            this.btnSoNgayLamViec = new System.Windows.Forms.Button();
            this.btnDiTreVeSom = new System.Windows.Forms.Button();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.xuatpdf = new System.Windows.Forms.Button();
            this.txtTimkiem = new System.Windows.Forms.TextBox();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.btnXuatexcel = new System.Windows.Forms.Button();
            this.pnlResult = new System.Windows.Forms.Panel();
            this.dtGridViewBCChamCong = new System.Windows.Forms.DataGridView();
            this.lblResult = new System.Windows.Forms.Label();
            this.btnGioravao = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.pnlControl.SuspendLayout();
            this.grpTime.SuspendLayout();
            this.grpReport.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.pnlResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCChamCong)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(60)))), ((int)(((byte)(114)))));
            this.pnlHeader.Controls.Add(this.cbBoxChucVu);
            this.pnlHeader.Controls.Add(this.cbBoxMaPB);
            this.pnlHeader.Controls.Add(this.label11);
            this.pnlHeader.Controls.Add(this.label10);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1572, 80);
            this.pnlHeader.TabIndex = 2;
            // 
            // cbBoxChucVu
            // 
            this.cbBoxChucVu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbBoxChucVu.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBoxChucVu.FormattingEnabled = true;
            this.cbBoxChucVu.Location = new System.Drawing.Point(701, 46);
            this.cbBoxChucVu.Name = "cbBoxChucVu";
            this.cbBoxChucVu.Size = new System.Drawing.Size(307, 27);
            this.cbBoxChucVu.TabIndex = 197;
            this.cbBoxChucVu.SelectedIndexChanged += new System.EventHandler(this.cbBoxChucVu_SelectedIndexChanged);
            // 
            // cbBoxMaPB
            // 
            this.cbBoxMaPB.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbBoxMaPB.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBoxMaPB.FormattingEnabled = true;
            this.cbBoxMaPB.Location = new System.Drawing.Point(701, 8);
            this.cbBoxMaPB.Name = "cbBoxMaPB";
            this.cbBoxMaPB.Size = new System.Drawing.Size(307, 27);
            this.cbBoxMaPB.TabIndex = 196;
            this.cbBoxMaPB.SelectedIndexChanged += new System.EventHandler(this.cbBoxMaPB_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(565, 13);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 19);
            this.label11.TabIndex = 194;
            this.label11.Text = "Mã Phòng Ban";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(565, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 19);
            this.label10.TabIndex = 195;
            this.label10.Text = "Chức Vụ";
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(445, 38);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "BÁO CÁO CHẤM CÔNG";
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblSubTitle.Location = new System.Drawing.Point(22, 48);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(332, 23);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "Thống kê phân tích theo tháng";
            // 
            // pnlControl
            // 
            this.pnlControl.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlControl.Controls.Add(this.grpTime);
            this.pnlControl.Controls.Add(this.grpReport);
            this.pnlControl.Controls.Add(this.grpSearch);
            this.pnlControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControl.Location = new System.Drawing.Point(0, 80);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Padding = new System.Windows.Forms.Padding(15);
            this.pnlControl.Size = new System.Drawing.Size(1572, 140);
            this.pnlControl.TabIndex = 1;
            // 
            // grpTime
            // 
            this.grpTime.Controls.Add(this.dtpThoiGian);
            this.grpTime.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpTime.Location = new System.Drawing.Point(20, 20);
            this.grpTime.Name = "grpTime";
            this.grpTime.Size = new System.Drawing.Size(220, 80);
            this.grpTime.TabIndex = 0;
            this.grpTime.TabStop = false;
            this.grpTime.Text = "Thời gian";
            // 
            // dtpThoiGian
            // 
            this.dtpThoiGian.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpThoiGian.Location = new System.Drawing.Point(7, 35);
            this.dtpThoiGian.Name = "dtpThoiGian";
            this.dtpThoiGian.Size = new System.Drawing.Size(207, 30);
            this.dtpThoiGian.TabIndex = 0;
            this.dtpThoiGian.ValueChanged += new System.EventHandler(this.dtpThoiGian_ValueChanged);
            // 
            // grpReport
            // 
            this.grpReport.Controls.Add(this.btnGioravao);
            this.grpReport.Controls.Add(this.btnSoNgayLamViec);
            this.grpReport.Controls.Add(this.btnDiTreVeSom);
            this.grpReport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpReport.Location = new System.Drawing.Point(260, 20);
            this.grpReport.Name = "grpReport";
            this.grpReport.Size = new System.Drawing.Size(581, 80);
            this.grpReport.TabIndex = 1;
            this.grpReport.TabStop = false;
            this.grpReport.Text = "Loại báo cáo";
            // 
            // btnSoNgayLamViec
            // 
            this.btnSoNgayLamViec.Location = new System.Drawing.Point(15, 30);
            this.btnSoNgayLamViec.Name = "btnSoNgayLamViec";
            this.btnSoNgayLamViec.Size = new System.Drawing.Size(160, 35);
            this.btnSoNgayLamViec.TabIndex = 0;
            this.btnSoNgayLamViec.Text = "Số ngày làm việc";
            this.btnSoNgayLamViec.Click += new System.EventHandler(this.btnSoNgayLamViec_Click);
            // 
            // btnDiTreVeSom
            // 
            this.btnDiTreVeSom.Location = new System.Drawing.Point(194, 28);
            this.btnDiTreVeSom.Name = "btnDiTreVeSom";
            this.btnDiTreVeSom.Size = new System.Drawing.Size(160, 35);
            this.btnDiTreVeSom.TabIndex = 1;
            this.btnDiTreVeSom.Text = "Chi tiết";
            this.btnDiTreVeSom.Click += new System.EventHandler(this.btnDiTreVeSom_Click);
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.xuatpdf);
            this.grpSearch.Controls.Add(this.txtTimkiem);
            this.grpSearch.Controls.Add(this.btnTimKiem);
            this.grpSearch.Controls.Add(this.btnXuatexcel);
            this.grpSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpSearch.Location = new System.Drawing.Point(925, 18);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(582, 102);
            this.grpSearch.TabIndex = 2;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Tìm kiếm và xuất";
            // 
            // xuatpdf
            // 
            this.xuatpdf.Location = new System.Drawing.Point(416, 29);
            this.xuatpdf.Name = "xuatpdf";
            this.xuatpdf.Size = new System.Drawing.Size(100, 32);
            this.xuatpdf.TabIndex = 3;
            this.xuatpdf.Text = "Xuất pdf";
            this.xuatpdf.Click += new System.EventHandler(this.xuatpdf_Click);
            // 
            // txtTimkiem
            // 
            this.txtTimkiem.Location = new System.Drawing.Point(15, 32);
            this.txtTimkiem.Name = "txtTimkiem";
            this.txtTimkiem.Size = new System.Drawing.Size(200, 30);
            this.txtTimkiem.TabIndex = 0;
            this.txtTimkiem.TextChanged += new System.EventHandler(this.txtTimkiem_TextChanged);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Location = new System.Drawing.Point(225, 30);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(80, 32);
            this.btnTimKiem.TabIndex = 1;
            this.btnTimKiem.Text = "Tìm";
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // btnXuatexcel
            // 
            this.btnXuatexcel.Location = new System.Drawing.Point(310, 30);
            this.btnXuatexcel.Name = "btnXuatexcel";
            this.btnXuatexcel.Size = new System.Drawing.Size(100, 32);
            this.btnXuatexcel.TabIndex = 2;
            this.btnXuatexcel.Text = "Xuất Excel";
            this.btnXuatexcel.Click += new System.EventHandler(this.btnXuat_Click);
            // 
            // pnlResult
            // 
            this.pnlResult.BackColor = System.Drawing.Color.White;
            this.pnlResult.Controls.Add(this.dtGridViewBCChamCong);
            this.pnlResult.Controls.Add(this.lblResult);
            this.pnlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResult.Location = new System.Drawing.Point(0, 220);
            this.pnlResult.Name = "pnlResult";
            this.pnlResult.Padding = new System.Windows.Forms.Padding(15);
            this.pnlResult.Size = new System.Drawing.Size(1572, 450);
            this.pnlResult.TabIndex = 0;
            // 
            // dtGridViewBCChamCong
            // 
            this.dtGridViewBCChamCong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewBCChamCong.ColumnHeadersHeight = 29;
            this.dtGridViewBCChamCong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewBCChamCong.Location = new System.Drawing.Point(15, 45);
            this.dtGridViewBCChamCong.Name = "dtGridViewBCChamCong";
            this.dtGridViewBCChamCong.ReadOnly = true;
            this.dtGridViewBCChamCong.RowHeadersWidth = 51;
            this.dtGridViewBCChamCong.Size = new System.Drawing.Size(1542, 390);
            this.dtGridViewBCChamCong.TabIndex = 0;
            // 
            // lblResult
            // 
            this.lblResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblResult.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblResult.Location = new System.Drawing.Point(15, 15);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(1542, 30);
            this.lblResult.TabIndex = 1;
            this.lblResult.Text = "KẾT QUẢ BÁO CÁO";
            // 
            // btnGioravao
            // 
            this.btnGioravao.Location = new System.Drawing.Point(371, 30);
            this.btnGioravao.Name = "btnGioravao";
            this.btnGioravao.Size = new System.Drawing.Size(160, 35);
            this.btnGioravao.TabIndex = 2;
            this.btnGioravao.Text = "Thời gian ra vào";
            this.btnGioravao.Click += new System.EventHandler(this.btnGioravao_Click);
            // 
            // F_BaoCaoChamCong
            // 
            this.ClientSize = new System.Drawing.Size(1572, 670);
            this.Controls.Add(this.pnlResult);
            this.Controls.Add(this.pnlControl);
            this.Controls.Add(this.pnlHeader);
            this.Name = "F_BaoCaoChamCong";
            this.Text = "Báo cáo chấm công";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_BaoCaoChamCong_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlControl.ResumeLayout(false);
            this.grpTime.ResumeLayout(false);
            this.grpReport.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            this.pnlResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCChamCong)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;

        private System.Windows.Forms.Panel pnlControl;
        private System.Windows.Forms.GroupBox grpTime;
        private System.Windows.Forms.DateTimePicker dtpThoiGian;
        private System.Windows.Forms.GroupBox grpReport;
        private System.Windows.Forms.Button btnSoNgayLamViec;
        private System.Windows.Forms.Button btnDiTreVeSom;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.TextBox txtTimkiem;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnXuatexcel;

        private System.Windows.Forms.Panel pnlResult;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.DataGridView dtGridViewBCChamCong;
        private System.Windows.Forms.Button xuatpdf;
        private System.Windows.Forms.ComboBox cbBoxChucVu;
        private System.Windows.Forms.ComboBox cbBoxMaPB;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnGioravao;
    }
}
