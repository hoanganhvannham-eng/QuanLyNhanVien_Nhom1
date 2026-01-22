namespace QuanLyNhanVien3
{
    partial class F_BaoCaoNhanVien
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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxmanhanvientimkiem = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txttimkiemtheoten = new System.Windows.Forms.TextBox();
            this.gbReportType = new System.Windows.Forms.GroupBox();
            this.btnThongKeNhanVien = new System.Windows.Forms.Button();
            this.btnsoluongtheogioitinh = new System.Windows.Forms.Button();
            this.btnXuatEXL = new System.Windows.Forms.Button();
            this.gbContent = new System.Windows.Forms.GroupBox();
            this.dtGridViewBCNhanVien = new System.Windows.Forms.DataGridView();
            this.lblGridTitle = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.gbSearch.SuspendLayout();
            this.gbReportType.SuspendLayout();
            this.gbContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCNhanVien)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.lblSubTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1385, 78);
            this.panelHeader.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(294, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "BÁO CÁO NHÂN VIÊN";
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSubTitle.Location = new System.Drawing.Point(20, 45);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(232, 23);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "Thống kê tìm kiếm nhân viên";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.gbSearch);
            this.panelTop.Controls.Add(this.gbReportType);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 78);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(12);
            this.panelTop.Size = new System.Drawing.Size(1385, 210);
            this.panelTop.TabIndex = 1;
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.label2);
            this.gbSearch.Controls.Add(this.textBoxmanhanvientimkiem);
            this.gbSearch.Controls.Add(this.label3);
            this.gbSearch.Controls.Add(this.txttimkiemtheoten);
            this.gbSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbSearch.Location = new System.Drawing.Point(12, 102);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(1361, 96);
            this.gbSearch.TabIndex = 0;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "Tìm kiếm";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label2.Location = new System.Drawing.Point(20, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Mã nhân viên";
            // 
            // textBoxmanhanvientimkiem
            // 
            this.textBoxmanhanvientimkiem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxmanhanvientimkiem.Location = new System.Drawing.Point(150, 32);
            this.textBoxmanhanvientimkiem.Name = "textBoxmanhanvientimkiem";
            this.textBoxmanhanvientimkiem.Size = new System.Drawing.Size(240, 30);
            this.textBoxmanhanvientimkiem.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label3.Location = new System.Drawing.Point(420, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Tên nhân viên";
            // 
            // txttimkiemtheoten
            // 
            this.txttimkiemtheoten.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txttimkiemtheoten.Location = new System.Drawing.Point(550, 32);
            this.txttimkiemtheoten.Name = "txttimkiemtheoten";
            this.txttimkiemtheoten.Size = new System.Drawing.Size(260, 30);
            this.txttimkiemtheoten.TabIndex = 3;
            // 
            // gbReportType
            // 
            this.gbReportType.Controls.Add(this.btnThongKeNhanVien);
            this.gbReportType.Controls.Add(this.btnsoluongtheogioitinh);
            this.gbReportType.Controls.Add(this.btnXuatEXL);
            this.gbReportType.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbReportType.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbReportType.Location = new System.Drawing.Point(12, 12);
            this.gbReportType.Name = "gbReportType";
            this.gbReportType.Size = new System.Drawing.Size(1361, 90);
            this.gbReportType.TabIndex = 1;
            this.gbReportType.TabStop = false;
            this.gbReportType.Text = "Chọn loại báo cáo";
            // 
            // btnThongKeNhanVien
            // 
            this.btnThongKeNhanVien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThongKeNhanVien.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnThongKeNhanVien.Location = new System.Drawing.Point(18, 32);
            this.btnThongKeNhanVien.Name = "btnThongKeNhanVien";
            this.btnThongKeNhanVien.Size = new System.Drawing.Size(300, 44);
            this.btnThongKeNhanVien.TabIndex = 0;
            this.btnThongKeNhanVien.Text = "Thống kê nhân viên";
            this.btnThongKeNhanVien.Click += new System.EventHandler(this.btnThongKeNhanVien_Click);
            // 
            // btnsoluongtheogioitinh
            // 
            this.btnsoluongtheogioitinh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsoluongtheogioitinh.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnsoluongtheogioitinh.Location = new System.Drawing.Point(340, 32);
            this.btnsoluongtheogioitinh.Name = "btnsoluongtheogioitinh";
            this.btnsoluongtheogioitinh.Size = new System.Drawing.Size(340, 44);
            this.btnsoluongtheogioitinh.TabIndex = 1;
            this.btnsoluongtheogioitinh.Text = "Thống kê theo giới tính";
            this.btnsoluongtheogioitinh.Click += new System.EventHandler(this.btnsoluongtheogioitinh_Click);
            // 
            // btnXuatEXL
            // 
            this.btnXuatEXL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXuatEXL.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnXuatEXL.Location = new System.Drawing.Point(700, 32);
            this.btnXuatEXL.Name = "btnXuatEXL";
            this.btnXuatEXL.Size = new System.Drawing.Size(220, 44);
            this.btnXuatEXL.TabIndex = 2;
            this.btnXuatEXL.Text = "Xuất Excel";
            this.btnXuatEXL.Click += new System.EventHandler(this.btnXuatEXL_Click);
            // 
            // gbContent
            // 
            this.gbContent.Controls.Add(this.dtGridViewBCNhanVien);
            this.gbContent.Controls.Add(this.lblGridTitle);
            this.gbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbContent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbContent.Location = new System.Drawing.Point(0, 288);
            this.gbContent.Name = "gbContent";
            this.gbContent.Padding = new System.Windows.Forms.Padding(10);
            this.gbContent.Size = new System.Drawing.Size(1385, 462);
            this.gbContent.TabIndex = 0;
            this.gbContent.TabStop = false;
            this.gbContent.Text = "Bảng số liệu";
            // 
            // dtGridViewBCNhanVien
            // 
            this.dtGridViewBCNhanVien.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewBCNhanVien.ColumnHeadersHeight = 29;
            this.dtGridViewBCNhanVien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewBCNhanVien.Location = new System.Drawing.Point(10, 63);
            this.dtGridViewBCNhanVien.Name = "dtGridViewBCNhanVien";
            this.dtGridViewBCNhanVien.ReadOnly = true;
            this.dtGridViewBCNhanVien.RowHeadersVisible = false;
            this.dtGridViewBCNhanVien.RowHeadersWidth = 51;
            this.dtGridViewBCNhanVien.Size = new System.Drawing.Size(1365, 389);
            this.dtGridViewBCNhanVien.TabIndex = 0;
            this.dtGridViewBCNhanVien.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtGridViewBCNhanVien_CellClick);
            // 
            // lblGridTitle
            // 
            this.lblGridTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGridTitle.Location = new System.Drawing.Point(10, 33);
            this.lblGridTitle.Name = "lblGridTitle";
            this.lblGridTitle.Size = new System.Drawing.Size(1365, 30);
            this.lblGridTitle.TabIndex = 1;
            this.lblGridTitle.Text = "Chi tiết danh sách nhân viên";
            // 
            // F_BaoCaoNhanVien
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1385, 750);
            this.Controls.Add(this.gbContent);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelHeader);
            this.Name = "F_BaoCaoNhanVien";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Báo cáo nhân viên";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.gbReportType.ResumeLayout(false);
            this.gbContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCNhanVien)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.GroupBox gbReportType;
        private System.Windows.Forms.Button btnThongKeNhanVien;
        private System.Windows.Forms.Button btnsoluongtheogioitinh;
        private System.Windows.Forms.Button btnXuatEXL;

        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxmanhanvientimkiem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txttimkiemtheoten;

        private System.Windows.Forms.GroupBox gbContent;
        private System.Windows.Forms.Label lblGridTitle;
        private System.Windows.Forms.DataGridView dtGridViewBCNhanVien;
    }
}
