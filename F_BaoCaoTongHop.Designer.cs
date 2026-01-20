namespace QuanLyNhanVien3
{
    partial class F_BaoCaoTongHop
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
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.lblLoaiBaoCao = new System.Windows.Forms.Label();
            this.cboLoaiBaoCao = new System.Windows.Forms.ComboBox();
            this.lblPhongBan = new System.Windows.Forms.Label();
            this.cboPhongBan = new System.Windows.Forms.ComboBox();
            this.lblTuNgay = new System.Windows.Forms.Label();
            this.dtpTuNgay = new System.Windows.Forms.DateTimePicker();
            this.lblDenNgay = new System.Windows.Forms.Label();
            this.dtpDenNgay = new System.Windows.Forms.DateTimePicker();
            this.btnXemBaoCao = new System.Windows.Forms.Button();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.cardNV = new System.Windows.Forms.Panel();
            this.lblNVValue = new System.Windows.Forms.Label();
            this.lblNVText = new System.Windows.Forms.Label();
            this.cardPB = new System.Windows.Forms.Panel();
            this.lblPBValue = new System.Windows.Forms.Label();
            this.lblPBText = new System.Windows.Forms.Label();
            this.cardDA = new System.Windows.Forms.Panel();
            this.lblDAValue = new System.Windows.Forms.Label();
            this.lblDAText = new System.Windows.Forms.Label();
            this.gbContent = new System.Windows.Forms.GroupBox();
            this.dtGridViewBCTongHop = new System.Windows.Forms.DataGridView();
            this.lblGridTitle = new System.Windows.Forms.Label();
            this.panelAction = new System.Windows.Forms.Panel();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.btnXuatPDF = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.cardNV.SuspendLayout();
            this.cardPB.SuspendLayout();
            this.cardDA.SuspendLayout();
            this.gbContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCTongHop)).BeginInit();
            this.panelAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1098, 55);
            this.panelHeader.TabIndex = 4;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(321, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Báo cáo chi tiết nhân sự";
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.lblLoaiBaoCao);
            this.gbFilter.Controls.Add(this.cboLoaiBaoCao);
            this.gbFilter.Controls.Add(this.lblPhongBan);
            this.gbFilter.Controls.Add(this.cboPhongBan);
            this.gbFilter.Controls.Add(this.lblTuNgay);
            this.gbFilter.Controls.Add(this.dtpTuNgay);
            this.gbFilter.Controls.Add(this.lblDenNgay);
            this.gbFilter.Controls.Add(this.dtpDenNgay);
            this.gbFilter.Controls.Add(this.btnXemBaoCao);
            this.gbFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFilter.Location = new System.Drawing.Point(0, 55);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(1098, 110);
            this.gbFilter.TabIndex = 3;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Điều kiện báo cáo";
            // 
            // lblLoaiBaoCao
            // 
            this.lblLoaiBaoCao.Location = new System.Drawing.Point(20, 30);
            this.lblLoaiBaoCao.Name = "lblLoaiBaoCao";
            this.lblLoaiBaoCao.Size = new System.Drawing.Size(100, 23);
            this.lblLoaiBaoCao.TabIndex = 0;
            this.lblLoaiBaoCao.Text = "Loại báo cáo";
            // 
            // cboLoaiBaoCao
            // 
            this.cboLoaiBaoCao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiBaoCao.Location = new System.Drawing.Point(20, 55);
            this.cboLoaiBaoCao.Name = "cboLoaiBaoCao";
            this.cboLoaiBaoCao.Size = new System.Drawing.Size(250, 24);
            this.cboLoaiBaoCao.TabIndex = 1;
            // 
            // lblPhongBan
            // 
            this.lblPhongBan.Location = new System.Drawing.Point(290, 30);
            this.lblPhongBan.Name = "lblPhongBan";
            this.lblPhongBan.Size = new System.Drawing.Size(100, 23);
            this.lblPhongBan.TabIndex = 2;
            this.lblPhongBan.Text = "Phòng ban";
            // 
            // cboPhongBan
            // 
            this.cboPhongBan.Location = new System.Drawing.Point(290, 55);
            this.cboPhongBan.Name = "cboPhongBan";
            this.cboPhongBan.Size = new System.Drawing.Size(200, 24);
            this.cboPhongBan.TabIndex = 3;
            // 
            // lblTuNgay
            // 
            this.lblTuNgay.Location = new System.Drawing.Point(510, 30);
            this.lblTuNgay.Name = "lblTuNgay";
            this.lblTuNgay.Size = new System.Drawing.Size(100, 23);
            this.lblTuNgay.TabIndex = 4;
            this.lblTuNgay.Text = "Từ ngày";
            // 
            // dtpTuNgay
            // 
            this.dtpTuNgay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTuNgay.Location = new System.Drawing.Point(510, 55);
            this.dtpTuNgay.Name = "dtpTuNgay";
            this.dtpTuNgay.Size = new System.Drawing.Size(200, 22);
            this.dtpTuNgay.TabIndex = 5;
            // 
            // lblDenNgay
            // 
            this.lblDenNgay.Location = new System.Drawing.Point(713, 27);
            this.lblDenNgay.Name = "lblDenNgay";
            this.lblDenNgay.Size = new System.Drawing.Size(100, 23);
            this.lblDenNgay.TabIndex = 6;
            this.lblDenNgay.Text = "Đến ngày";
            // 
            // dtpDenNgay
            // 
            this.dtpDenNgay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDenNgay.Location = new System.Drawing.Point(716, 53);
            this.dtpDenNgay.Name = "dtpDenNgay";
            this.dtpDenNgay.Size = new System.Drawing.Size(200, 22);
            this.dtpDenNgay.TabIndex = 7;
            // 
            // btnXemBaoCao
            // 
            this.btnXemBaoCao.Location = new System.Drawing.Point(932, 46);
            this.btnXemBaoCao.Name = "btnXemBaoCao";
            this.btnXemBaoCao.Size = new System.Drawing.Size(160, 40);
            this.btnXemBaoCao.TabIndex = 8;
            this.btnXemBaoCao.Text = "XEM BÁO CÁO";
            this.btnXemBaoCao.Click += new System.EventHandler(this.btnXemBaoCao_Click);
            // 
            // panelSummary
            // 
            this.panelSummary.Controls.Add(this.cardNV);
            this.panelSummary.Controls.Add(this.cardPB);
            this.panelSummary.Controls.Add(this.cardDA);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummary.Location = new System.Drawing.Point(0, 165);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(1098, 90);
            this.panelSummary.TabIndex = 2;
            // 
            // cardNV
            // 
            this.cardNV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.cardNV.Controls.Add(this.lblNVValue);
            this.cardNV.Controls.Add(this.lblNVText);
            this.cardNV.Location = new System.Drawing.Point(20, 15);
            this.cardNV.Name = "cardNV";
            this.cardNV.Size = new System.Drawing.Size(200, 60);
            this.cardNV.TabIndex = 0;
            // 
            // lblNVValue
            // 
            this.lblNVValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNVValue.Location = new System.Drawing.Point(15, 5);
            this.lblNVValue.Name = "lblNVValue";
            this.lblNVValue.Size = new System.Drawing.Size(100, 23);
            this.lblNVValue.TabIndex = 0;
            this.lblNVValue.Text = "0";
            this.lblNVValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNVText
            // 
            this.lblNVText.Location = new System.Drawing.Point(18, 35);
            this.lblNVText.Name = "lblNVText";
            this.lblNVText.Size = new System.Drawing.Size(100, 23);
            this.lblNVText.TabIndex = 1;
            this.lblNVText.Text = "Nhân viên";
            this.lblNVText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardPB
            // 
            this.cardPB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.cardPB.Controls.Add(this.lblPBValue);
            this.cardPB.Controls.Add(this.lblPBText);
            this.cardPB.Location = new System.Drawing.Point(260, 15);
            this.cardPB.Name = "cardPB";
            this.cardPB.Size = new System.Drawing.Size(200, 60);
            this.cardPB.TabIndex = 1;
            // 
            // lblPBValue
            // 
            this.lblPBValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPBValue.Location = new System.Drawing.Point(15, 5);
            this.lblPBValue.Name = "lblPBValue";
            this.lblPBValue.Size = new System.Drawing.Size(100, 23);
            this.lblPBValue.TabIndex = 0;
            this.lblPBValue.Text = "0";
            this.lblPBValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPBText
            // 
            this.lblPBText.Location = new System.Drawing.Point(18, 35);
            this.lblPBText.Name = "lblPBText";
            this.lblPBText.Size = new System.Drawing.Size(100, 23);
            this.lblPBText.TabIndex = 1;
            this.lblPBText.Text = "Phòng ban";
            this.lblPBText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardDA
            // 
            this.cardDA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.cardDA.Controls.Add(this.lblDAValue);
            this.cardDA.Controls.Add(this.lblDAText);
            this.cardDA.Location = new System.Drawing.Point(500, 15);
            this.cardDA.Name = "cardDA";
            this.cardDA.Size = new System.Drawing.Size(200, 60);
            this.cardDA.TabIndex = 2;
            // 
            // lblDAValue
            // 
            this.lblDAValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDAValue.Location = new System.Drawing.Point(15, 5);
            this.lblDAValue.Name = "lblDAValue";
            this.lblDAValue.Size = new System.Drawing.Size(100, 23);
            this.lblDAValue.TabIndex = 0;
            this.lblDAValue.Text = "0";
            this.lblDAValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDAText
            // 
            this.lblDAText.Location = new System.Drawing.Point(18, 35);
            this.lblDAText.Name = "lblDAText";
            this.lblDAText.Size = new System.Drawing.Size(100, 23);
            this.lblDAText.TabIndex = 1;
            this.lblDAText.Text = "Dự án";
            this.lblDAText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbContent
            // 
            this.gbContent.Controls.Add(this.dtGridViewBCTongHop);
            this.gbContent.Controls.Add(this.lblGridTitle);
            this.gbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbContent.Location = new System.Drawing.Point(0, 255);
            this.gbContent.Name = "gbContent";
            this.gbContent.Size = new System.Drawing.Size(1098, 406);
            this.gbContent.TabIndex = 0;
            this.gbContent.TabStop = false;
            this.gbContent.Text = "Nội dung báo cáo";
            // 
            // dtGridViewBCTongHop
            // 
            this.dtGridViewBCTongHop.AllowUserToAddRows = false;
            this.dtGridViewBCTongHop.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewBCTongHop.ColumnHeadersHeight = 29;
            this.dtGridViewBCTongHop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewBCTongHop.Location = new System.Drawing.Point(3, 48);
            this.dtGridViewBCTongHop.Name = "dtGridViewBCTongHop";
            this.dtGridViewBCTongHop.ReadOnly = true;
            this.dtGridViewBCTongHop.RowHeadersVisible = false;
            this.dtGridViewBCTongHop.RowHeadersWidth = 51;
            this.dtGridViewBCTongHop.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtGridViewBCTongHop.Size = new System.Drawing.Size(1092, 355);
            this.dtGridViewBCTongHop.TabIndex = 0;
            // 
            // lblGridTitle
            // 
            this.lblGridTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGridTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblGridTitle.Location = new System.Drawing.Point(3, 18);
            this.lblGridTitle.Name = "lblGridTitle";
            this.lblGridTitle.Size = new System.Drawing.Size(1092, 30);
            this.lblGridTitle.TabIndex = 1;
            this.lblGridTitle.Text = "Danh sách chi tiết";
            // 
            // panelAction
            // 
            this.panelAction.Controls.Add(this.btnXuatExcel);
            this.panelAction.Controls.Add(this.btnXuatPDF);
            this.panelAction.Controls.Add(this.btnIn);
            this.panelAction.Controls.Add(this.btnLamMoi);
            this.panelAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAction.Location = new System.Drawing.Point(0, 661);
            this.panelAction.Name = "panelAction";
            this.panelAction.Size = new System.Drawing.Size(1098, 63);
            this.panelAction.TabIndex = 1;
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Location = new System.Drawing.Point(20, 15);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(75, 23);
            this.btnXuatExcel.TabIndex = 0;
            this.btnXuatExcel.Text = "Excel";
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // btnXuatPDF
            // 
            this.btnXuatPDF.Location = new System.Drawing.Point(120, 15);
            this.btnXuatPDF.Name = "btnXuatPDF";
            this.btnXuatPDF.Size = new System.Drawing.Size(75, 23);
            this.btnXuatPDF.TabIndex = 1;
            this.btnXuatPDF.Text = "PDF";
            // 
            // btnIn
            // 
            this.btnIn.Location = new System.Drawing.Point(220, 15);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(75, 23);
            this.btnIn.TabIndex = 2;
            this.btnIn.Text = "In";
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(300, 15);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(75, 23);
            this.btnLamMoi.TabIndex = 3;
            this.btnLamMoi.Text = "Làm mới";
            // 
            // F_BaoCaoTongHop
            // 
            this.ClientSize = new System.Drawing.Size(1098, 724);
            this.Controls.Add(this.gbContent);
            this.Controls.Add(this.panelAction);
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.panelHeader);
            this.Name = "F_BaoCaoTongHop";
            this.Text = "Báo cáo chi tiết";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_BaoCaoTongHop_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.gbFilter.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.cardNV.ResumeLayout(false);
            this.cardPB.ResumeLayout(false);
            this.cardDA.ResumeLayout(false);
            this.gbContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCTongHop)).EndInit();
            this.panelAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Label lblLoaiBaoCao;
        private System.Windows.Forms.ComboBox cboLoaiBaoCao;
        private System.Windows.Forms.Label lblPhongBan;
        private System.Windows.Forms.ComboBox cboPhongBan;
        private System.Windows.Forms.Label lblTuNgay;
        private System.Windows.Forms.DateTimePicker dtpTuNgay;
        private System.Windows.Forms.Label lblDenNgay;
        private System.Windows.Forms.DateTimePicker dtpDenNgay;
        private System.Windows.Forms.Button btnXemBaoCao;

        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Panel cardNV;
        private System.Windows.Forms.Panel cardPB;
        private System.Windows.Forms.Panel cardDA;
        private System.Windows.Forms.Label lblNVValue;
        private System.Windows.Forms.Label lblPBValue;
        private System.Windows.Forms.Label lblDAValue;
        private System.Windows.Forms.Label lblNVText;
        private System.Windows.Forms.Label lblPBText;
        private System.Windows.Forms.Label lblDAText;

        private System.Windows.Forms.GroupBox gbContent;
        private System.Windows.Forms.Label lblGridTitle;
        private System.Windows.Forms.DataGridView dtGridViewBCTongHop;

        private System.Windows.Forms.Panel panelAction;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Button btnXuatPDF;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Button btnLamMoi;
    }
}
