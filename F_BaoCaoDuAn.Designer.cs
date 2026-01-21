namespace QuanLyNhanVien3
{
    partial class F_BaoCaoDuAn
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.pnlControl = new System.Windows.Forms.Panel();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.grpReport = new System.Windows.Forms.GroupBox();
            this.btnDSNhanVienTheoDuAn = new System.Windows.Forms.Button();
            this.btnSoLuongNhanVien = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.pnlResult = new System.Windows.Forms.Panel();
            this.dtGridViewBCDuAn = new System.Windows.Forms.DataGridView();
            this.lblResult = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlControl.SuspendLayout();
            this.grpSearch.SuspendLayout();
            this.grpReport.SuspendLayout();
            this.pnlResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCDuAn)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(60)))), ((int)(((byte)(114)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1225, 80);
            this.pnlHeader.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(435, 38);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "BÁO CÁO DỰ ÁN";
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblSubTitle.Location = new System.Drawing.Point(22, 48);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(391, 23);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "Thống kê phân tích dự án đang triển khai";
            // 
            // pnlControl
            // 
            this.pnlControl.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlControl.Controls.Add(this.grpSearch);
            this.pnlControl.Controls.Add(this.grpReport);
            this.pnlControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControl.Location = new System.Drawing.Point(0, 80);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Padding = new System.Windows.Forms.Padding(15);
            this.pnlControl.Size = new System.Drawing.Size(1225, 140);
            this.pnlControl.TabIndex = 1;
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.label2);
            this.grpSearch.Controls.Add(this.txtTimKiem);
            this.grpSearch.Controls.Add(this.btnTimKiem);
            this.grpSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpSearch.Location = new System.Drawing.Point(20, 20);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(435, 80);
            this.grpSearch.TabIndex = 0;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Tìm kiếm dự án";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label2.Location = new System.Drawing.Point(15, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Tên dự án:";
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Location = new System.Drawing.Point(111, 32);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(200, 30);
            this.txtTimKiem.TabIndex = 1;
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Location = new System.Drawing.Point(330, 30);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(80, 32);
            this.btnTimKiem.TabIndex = 2;
            this.btnTimKiem.Text = "Tìm";
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // grpReport
            // 
            this.grpReport.Controls.Add(this.btnDSNhanVienTheoDuAn);
            this.grpReport.Controls.Add(this.btnSoLuongNhanVien);
            this.grpReport.Controls.Add(this.btnXuatExcel);
            this.grpReport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpReport.Location = new System.Drawing.Point(487, 20);
            this.grpReport.Name = "grpReport";
            this.grpReport.Size = new System.Drawing.Size(643, 80);
            this.grpReport.TabIndex = 1;
            this.grpReport.TabStop = false;
            this.grpReport.Text = "Báo cáo";
            // 
            // btnDSNhanVienTheoDuAn
            // 
            this.btnDSNhanVienTheoDuAn.Location = new System.Drawing.Point(15, 30);
            this.btnDSNhanVienTheoDuAn.Name = "btnDSNhanVienTheoDuAn";
            this.btnDSNhanVienTheoDuAn.Size = new System.Drawing.Size(233, 35);
            this.btnDSNhanVienTheoDuAn.TabIndex = 0;
            this.btnDSNhanVienTheoDuAn.Text = "Danh sách NV theo dự án";
            this.btnDSNhanVienTheoDuAn.Click += new System.EventHandler(this.btnDSNhanVienTheoDuAn_Click);
            // 
            // btnSoLuongNhanVien
            // 
            this.btnSoLuongNhanVien.Location = new System.Drawing.Point(254, 30);
            this.btnSoLuongNhanVien.Name = "btnSoLuongNhanVien";
            this.btnSoLuongNhanVien.Size = new System.Drawing.Size(204, 35);
            this.btnSoLuongNhanVien.TabIndex = 1;
            this.btnSoLuongNhanVien.Text = "Số lượng nhân viên";
            this.btnSoLuongNhanVien.Click += new System.EventHandler(this.btnSoLuongNhanVien_Click);
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Location = new System.Drawing.Point(474, 30);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(108, 35);
            this.btnXuatExcel.TabIndex = 2;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // pnlResult
            // 
            this.pnlResult.BackColor = System.Drawing.Color.White;
            this.pnlResult.Controls.Add(this.dtGridViewBCDuAn);
            this.pnlResult.Controls.Add(this.lblResult);
            this.pnlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResult.Location = new System.Drawing.Point(0, 220);
            this.pnlResult.Name = "pnlResult";
            this.pnlResult.Padding = new System.Windows.Forms.Padding(15);
            this.pnlResult.Size = new System.Drawing.Size(1225, 393);
            this.pnlResult.TabIndex = 0;
            // 
            // dtGridViewBCDuAn
            // 
            this.dtGridViewBCDuAn.AllowUserToAddRows = false;
            this.dtGridViewBCDuAn.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewBCDuAn.ColumnHeadersHeight = 29;
            this.dtGridViewBCDuAn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewBCDuAn.Location = new System.Drawing.Point(15, 45);
            this.dtGridViewBCDuAn.Name = "dtGridViewBCDuAn";
            this.dtGridViewBCDuAn.ReadOnly = true;
            this.dtGridViewBCDuAn.RowHeadersWidth = 51;
            this.dtGridViewBCDuAn.Size = new System.Drawing.Size(1195, 333);
            this.dtGridViewBCDuAn.TabIndex = 0;
            // 
            // lblResult
            // 
            this.lblResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblResult.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblResult.Location = new System.Drawing.Point(15, 15);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(1195, 30);
            this.lblResult.TabIndex = 1;
            this.lblResult.Text = "KẾT QUẢ BÁO CÁO";
            // 
            // F_BaoCaoDuAn
            // 
            this.ClientSize = new System.Drawing.Size(1225, 613);
            this.Controls.Add(this.pnlResult);
            this.Controls.Add(this.pnlControl);
            this.Controls.Add(this.pnlHeader);
            this.Name = "F_BaoCaoDuAn";
            this.Text = "Báo cáo dự án";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_BaoCaoDuAn_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlControl.ResumeLayout(false);
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            this.grpReport.ResumeLayout(false);
            this.pnlResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCDuAn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;

        private System.Windows.Forms.Panel pnlControl;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnTimKiem;

        private System.Windows.Forms.GroupBox grpReport;
        private System.Windows.Forms.Button btnDSNhanVienTheoDuAn;
        private System.Windows.Forms.Button btnSoLuongNhanVien;
        private System.Windows.Forms.Button btnXuatExcel;

        private System.Windows.Forms.Panel pnlResult;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.DataGridView dtGridViewBCDuAn;
    }
}
