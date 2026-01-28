namespace QuanLyNhanVien3
{
    partial class F_ChamCongChiTiet
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.lblGioVao = new System.Windows.Forms.Label();
            this.tbGioVao = new System.Windows.Forms.TextBox();
            this.lblGioVe = new System.Windows.Forms.Label();
            this.tbGioVe = new System.Windows.Forms.TextBox();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.tbGhiChu = new System.Windows.Forms.TextBox();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.lblMaPB = new System.Windows.Forms.Label();
            this.cbBoxMaPB = new System.Windows.Forms.ComboBox();
            this.lblChucVu = new System.Windows.Forms.Label();
            this.cbBoxChucVu = new System.Windows.Forms.ComboBox();
            this.lblMaChamCong = new System.Windows.Forms.Label();
            this.tbMaChamCong = new System.Windows.Forms.TextBox();
            this.lblMaNV = new System.Windows.Forms.Label();
            this.tbmanhanvien = new System.Windows.Forms.TextBox();
            this.lblTenNV = new System.Windows.Forms.Label();
            this.tbtennhanvien = new System.Windows.Forms.TextBox();
            this.lblNgay = new System.Windows.Forms.Label();
            this.dateTimeNgayChamCong = new System.Windows.Forms.DateTimePicker();
            this.panelAction = new System.Windows.Forms.Panel();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.btnrestar = new System.Windows.Forms.Button();
            this.xuatpdf = new System.Windows.Forms.Button();
            this.xuatexcel = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();
            this.panelGrid = new System.Windows.Forms.Panel();
            this.dtGridViewChamCong = new System.Windows.Forms.DataGridView();
            this.buttonhienthidaxoa = new System.Windows.Forms.Button();
            this.buttonkhoiphuc = new System.Windows.Forms.Button();
            this.txtMKKhoiPhuc = new System.Windows.Forms.TextBox();
            this.checkshowpassword = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelAction.SuspendLayout();
            this.panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewChamCong)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(18, 10, 18, 10);
            this.panelHeader.Size = new System.Drawing.Size(1632, 78);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(341, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "CHẤM CÔNG NHÂN VIÊN";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panelRight);
            this.panelTop.Controls.Add(this.panelLeft);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 78);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(12);
            this.panelTop.Size = new System.Drawing.Size(1632, 285);
            this.panelTop.TabIndex = 1;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.txtMKKhoiPhuc);
            this.panelRight.Controls.Add(this.checkshowpassword);
            this.panelRight.Controls.Add(this.label5);
            this.panelRight.Controls.Add(this.buttonkhoiphuc);
            this.panelRight.Controls.Add(this.buttonhienthidaxoa);
            this.panelRight.Controls.Add(this.lblGioVao);
            this.panelRight.Controls.Add(this.tbGioVao);
            this.panelRight.Controls.Add(this.lblGioVe);
            this.panelRight.Controls.Add(this.tbGioVe);
            this.panelRight.Controls.Add(this.lblGhiChu);
            this.panelRight.Controls.Add(this.tbGhiChu);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(743, 12);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(877, 261);
            this.panelRight.TabIndex = 1;
            // 
            // lblGioVao
            // 
            this.lblGioVao.AutoSize = true;
            this.lblGioVao.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGioVao.Location = new System.Drawing.Point(20, 30);
            this.lblGioVao.Name = "lblGioVao";
            this.lblGioVao.Size = new System.Drawing.Size(71, 23);
            this.lblGioVao.TabIndex = 0;
            this.lblGioVao.Text = "Giờ vào";
            // 
            // tbGioVao
            // 
            this.tbGioVao.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tbGioVao.Location = new System.Drawing.Point(120, 26);
            this.tbGioVao.Name = "tbGioVao";
            this.tbGioVao.Size = new System.Drawing.Size(450, 30);
            this.tbGioVao.TabIndex = 1;
            // 
            // lblGioVe
            // 
            this.lblGioVe.AutoSize = true;
            this.lblGioVe.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGioVe.Location = new System.Drawing.Point(20, 70);
            this.lblGioVe.Name = "lblGioVe";
            this.lblGioVe.Size = new System.Drawing.Size(61, 23);
            this.lblGioVe.TabIndex = 2;
            this.lblGioVe.Text = "Giờ về";
            // 
            // tbGioVe
            // 
            this.tbGioVe.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tbGioVe.Location = new System.Drawing.Point(120, 66);
            this.tbGioVe.Name = "tbGioVe";
            this.tbGioVe.Size = new System.Drawing.Size(450, 30);
            this.tbGioVe.TabIndex = 3;
            // 
            // lblGhiChu
            // 
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGhiChu.Location = new System.Drawing.Point(20, 110);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Size = new System.Drawing.Size(70, 23);
            this.lblGhiChu.TabIndex = 4;
            this.lblGhiChu.Text = "Ghi chú";
            // 
            // tbGhiChu
            // 
            this.tbGhiChu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tbGhiChu.Location = new System.Drawing.Point(120, 106);
            this.tbGhiChu.Multiline = true;
            this.tbGhiChu.Name = "tbGhiChu";
            this.tbGhiChu.Size = new System.Drawing.Size(450, 100);
            this.tbGhiChu.TabIndex = 5;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.lblMaPB);
            this.panelLeft.Controls.Add(this.cbBoxMaPB);
            this.panelLeft.Controls.Add(this.lblChucVu);
            this.panelLeft.Controls.Add(this.cbBoxChucVu);
            this.panelLeft.Controls.Add(this.lblMaChamCong);
            this.panelLeft.Controls.Add(this.tbMaChamCong);
            this.panelLeft.Controls.Add(this.lblMaNV);
            this.panelLeft.Controls.Add(this.tbmanhanvien);
            this.panelLeft.Controls.Add(this.lblTenNV);
            this.panelLeft.Controls.Add(this.tbtennhanvien);
            this.panelLeft.Controls.Add(this.lblNgay);
            this.panelLeft.Controls.Add(this.dateTimeNgayChamCong);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(12, 12);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(731, 261);
            this.panelLeft.TabIndex = 0;
            // 
            // lblMaPB
            // 
            this.lblMaPB.AutoSize = true;
            this.lblMaPB.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMaPB.Location = new System.Drawing.Point(10, 18);
            this.lblMaPB.Name = "lblMaPB";
            this.lblMaPB.Size = new System.Drawing.Size(127, 23);
            this.lblMaPB.TabIndex = 0;
            this.lblMaPB.Text = "Mã phòng ban";
            // 
            // cbBoxMaPB
            // 
            this.cbBoxMaPB.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.cbBoxMaPB.Location = new System.Drawing.Point(160, 15);
            this.cbBoxMaPB.Name = "cbBoxMaPB";
            this.cbBoxMaPB.Size = new System.Drawing.Size(480, 31);
            this.cbBoxMaPB.TabIndex = 1;
            this.cbBoxMaPB.SelectedIndexChanged += new System.EventHandler(this.cbBoxMaPB_SelectedIndexChanged);
            // 
            // lblChucVu
            // 
            this.lblChucVu.AutoSize = true;
            this.lblChucVu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblChucVu.Location = new System.Drawing.Point(10, 56);
            this.lblChucVu.Name = "lblChucVu";
            this.lblChucVu.Size = new System.Drawing.Size(74, 23);
            this.lblChucVu.TabIndex = 2;
            this.lblChucVu.Text = "Chức vụ";
            // 
            // cbBoxChucVu
            // 
            this.cbBoxChucVu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.cbBoxChucVu.Location = new System.Drawing.Point(160, 53);
            this.cbBoxChucVu.Name = "cbBoxChucVu";
            this.cbBoxChucVu.Size = new System.Drawing.Size(480, 31);
            this.cbBoxChucVu.TabIndex = 3;
            this.cbBoxChucVu.SelectedIndexChanged += new System.EventHandler(this.cbBoxChucVu_SelectedIndexChanged);
            // 
            // lblMaChamCong
            // 
            this.lblMaChamCong.AutoSize = true;
            this.lblMaChamCong.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMaChamCong.Location = new System.Drawing.Point(10, 94);
            this.lblMaChamCong.Name = "lblMaChamCong";
            this.lblMaChamCong.Size = new System.Drawing.Size(127, 23);
            this.lblMaChamCong.TabIndex = 4;
            this.lblMaChamCong.Text = "Mã chấm công";
            // 
            // tbMaChamCong
            // 
            this.tbMaChamCong.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.tbMaChamCong.Location = new System.Drawing.Point(160, 91);
            this.tbMaChamCong.Name = "tbMaChamCong";
            this.tbMaChamCong.Size = new System.Drawing.Size(480, 30);
            this.tbMaChamCong.TabIndex = 5;
            this.tbMaChamCong.TextChanged += new System.EventHandler(this.tbMaChamCong_TextChanged);
            // 
            // lblMaNV
            // 
            this.lblMaNV.AutoSize = true;
            this.lblMaNV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMaNV.Location = new System.Drawing.Point(10, 132);
            this.lblMaNV.Name = "lblMaNV";
            this.lblMaNV.Size = new System.Drawing.Size(64, 23);
            this.lblMaNV.TabIndex = 6;
            this.lblMaNV.Text = "Mã NV";
            // 
            // tbmanhanvien
            // 
            this.tbmanhanvien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.tbmanhanvien.Location = new System.Drawing.Point(160, 129);
            this.tbmanhanvien.Name = "tbmanhanvien";
            this.tbmanhanvien.Size = new System.Drawing.Size(480, 30);
            this.tbmanhanvien.TabIndex = 7;
            this.tbmanhanvien.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // lblTenNV
            // 
            this.lblTenNV.AutoSize = true;
            this.lblTenNV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTenNV.Location = new System.Drawing.Point(10, 170);
            this.lblTenNV.Name = "lblTenNV";
            this.lblTenNV.Size = new System.Drawing.Size(119, 23);
            this.lblTenNV.TabIndex = 8;
            this.lblTenNV.Text = "Tên nhân viên";
            this.lblTenNV.Click += new System.EventHandler(this.label8_Click);
            // 
            // tbtennhanvien
            // 
            this.tbtennhanvien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.tbtennhanvien.Location = new System.Drawing.Point(160, 167);
            this.tbtennhanvien.Name = "tbtennhanvien";
            this.tbtennhanvien.Size = new System.Drawing.Size(480, 30);
            this.tbtennhanvien.TabIndex = 9;
            this.tbtennhanvien.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lblNgay
            // 
            this.lblNgay.AutoSize = true;
            this.lblNgay.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNgay.Location = new System.Drawing.Point(10, 208);
            this.lblNgay.Name = "lblNgay";
            this.lblNgay.Size = new System.Drawing.Size(52, 23);
            this.lblNgay.TabIndex = 10;
            this.lblNgay.Text = "Ngày";
            // 
            // dateTimeNgayChamCong
            // 
            this.dateTimeNgayChamCong.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dateTimeNgayChamCong.Location = new System.Drawing.Point(160, 205);
            this.dateTimeNgayChamCong.Name = "dateTimeNgayChamCong";
            this.dateTimeNgayChamCong.Size = new System.Drawing.Size(480, 30);
            this.dateTimeNgayChamCong.TabIndex = 11;
            this.dateTimeNgayChamCong.ValueChanged += new System.EventHandler(this.dateTimeNgayChamCong_ValueChanged);
            // 
            // panelAction
            // 
            this.panelAction.Controls.Add(this.btnThem);
            this.panelAction.Controls.Add(this.btnSua);
            this.panelAction.Controls.Add(this.btnXoa);
            this.panelAction.Controls.Add(this.btnTimKiem);
            this.panelAction.Controls.Add(this.btnrestar);
            this.panelAction.Controls.Add(this.xuatpdf);
            this.panelAction.Controls.Add(this.xuatexcel);
            this.panelAction.Controls.Add(this.btnThoat);
            this.panelAction.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAction.Location = new System.Drawing.Point(0, 363);
            this.panelAction.Name = "panelAction";
            this.panelAction.Padding = new System.Windows.Forms.Padding(12);
            this.panelAction.Size = new System.Drawing.Size(1632, 60);
            this.panelAction.TabIndex = 2;
            // 
            // btnThem
            // 
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThem.Location = new System.Drawing.Point(12, 12);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(120, 36);
            this.btnThem.TabIndex = 0;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnSua
            // 
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSua.Location = new System.Drawing.Point(142, 12);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(120, 36);
            this.btnSua.TabIndex = 1;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXoa.Location = new System.Drawing.Point(272, 12);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(120, 36);
            this.btnXoa.TabIndex = 2;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTimKiem.Location = new System.Drawing.Point(402, 12);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(120, 36);
            this.btnTimKiem.TabIndex = 3;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // btnrestar
            // 
            this.btnrestar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnrestar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnrestar.Location = new System.Drawing.Point(532, 12);
            this.btnrestar.Name = "btnrestar";
            this.btnrestar.Size = new System.Drawing.Size(120, 36);
            this.btnrestar.TabIndex = 4;
            this.btnrestar.Text = "Làm mới";
            this.btnrestar.UseVisualStyleBackColor = true;
            // 
            // xuatpdf
            // 
            this.xuatpdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xuatpdf.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.xuatpdf.Location = new System.Drawing.Point(662, 12);
            this.xuatpdf.Name = "xuatpdf";
            this.xuatpdf.Size = new System.Drawing.Size(120, 36);
            this.xuatpdf.TabIndex = 5;
            this.xuatpdf.Text = "Xuất PDF";
            this.xuatpdf.UseVisualStyleBackColor = true;
            this.xuatpdf.Click += new System.EventHandler(this.xuatpdf_Click);
            // 
            // xuatexcel
            // 
            this.xuatexcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xuatexcel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.xuatexcel.Location = new System.Drawing.Point(792, 12);
            this.xuatexcel.Name = "xuatexcel";
            this.xuatexcel.Size = new System.Drawing.Size(120, 36);
            this.xuatexcel.TabIndex = 6;
            this.xuatexcel.Text = "Xuất Excel";
            this.xuatexcel.UseVisualStyleBackColor = true;
            this.xuatexcel.Click += new System.EventHandler(this.xuatexcel_Click);
            // 
            // btnThoat
            // 
            this.btnThoat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThoat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThoat.Location = new System.Drawing.Point(922, 12);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(120, 36);
            this.btnThoat.TabIndex = 7;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            // 
            // panelGrid
            // 
            this.panelGrid.Controls.Add(this.dtGridViewChamCong);
            this.panelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrid.Location = new System.Drawing.Point(0, 423);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(1632, 297);
            this.panelGrid.TabIndex = 3;
            // 
            // dtGridViewChamCong
            // 
            this.dtGridViewChamCong.AllowUserToAddRows = false;
            this.dtGridViewChamCong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewChamCong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridViewChamCong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewChamCong.Location = new System.Drawing.Point(0, 0);
            this.dtGridViewChamCong.Name = "dtGridViewChamCong";
            this.dtGridViewChamCong.ReadOnly = true;
            this.dtGridViewChamCong.RowHeadersWidth = 51;
            this.dtGridViewChamCong.RowTemplate.Height = 24;
            this.dtGridViewChamCong.Size = new System.Drawing.Size(1632, 297);
            this.dtGridViewChamCong.TabIndex = 0;
            this.dtGridViewChamCong.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtGridViewChamCong_CellClick);
            // 
            // buttonhienthidaxoa
            // 
            this.buttonhienthidaxoa.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonhienthidaxoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonhienthidaxoa.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.buttonhienthidaxoa.Location = new System.Drawing.Point(606, 170);
            this.buttonhienthidaxoa.Name = "buttonhienthidaxoa";
            this.buttonhienthidaxoa.Size = new System.Drawing.Size(120, 36);
            this.buttonhienthidaxoa.TabIndex = 6;
            this.buttonhienthidaxoa.Text = "Đã xóa";
            this.buttonhienthidaxoa.UseVisualStyleBackColor = true;
            this.buttonhienthidaxoa.Click += new System.EventHandler(this.buttonhienthidaxoa_Click);
            // 
            // buttonkhoiphuc
            // 
            this.buttonkhoiphuc.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonkhoiphuc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonkhoiphuc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.buttonkhoiphuc.Location = new System.Drawing.Point(744, 170);
            this.buttonkhoiphuc.Name = "buttonkhoiphuc";
            this.buttonkhoiphuc.Size = new System.Drawing.Size(120, 36);
            this.buttonkhoiphuc.TabIndex = 7;
            this.buttonkhoiphuc.Text = "Khôi phục";
            this.buttonkhoiphuc.UseVisualStyleBackColor = true;
            this.buttonkhoiphuc.Click += new System.EventHandler(this.buttonkhoiphuc_Click);
            // 
            // txtMKKhoiPhuc
            // 
            this.txtMKKhoiPhuc.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtMKKhoiPhuc.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMKKhoiPhuc.Location = new System.Drawing.Point(634, 91);
            this.txtMKKhoiPhuc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMKKhoiPhuc.Name = "txtMKKhoiPhuc";
            this.txtMKKhoiPhuc.Size = new System.Drawing.Size(210, 30);
            this.txtMKKhoiPhuc.TabIndex = 41;
            // 
            // checkshowpassword
            // 
            this.checkshowpassword.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.checkshowpassword.AutoSize = true;
            this.checkshowpassword.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkshowpassword.Location = new System.Drawing.Point(650, 132);
            this.checkshowpassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkshowpassword.Name = "checkshowpassword";
            this.checkshowpassword.Size = new System.Drawing.Size(184, 26);
            this.checkshowpassword.TabIndex = 40;
            this.checkshowpassword.Text = "Hiển Thị Mật Khẩu";
            this.checkshowpassword.UseVisualStyleBackColor = true;
            this.checkshowpassword.CheckedChanged += new System.EventHandler(this.checkshowpassword_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(670, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 22);
            this.label5.TabIndex = 39;
            this.label5.Text = "MK Khôi Phục";
            // 
            // F_ChamCongChiTiet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1632, 720);
            this.Controls.Add(this.panelGrid);
            this.Controls.Add(this.panelAction);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelHeader);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "F_ChamCongChiTiet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chấm công chi tiết";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_ChamCongChiTiet_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            this.panelAction.ResumeLayout(false);
            this.panelGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewChamCong)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;

        private System.Windows.Forms.Label lblMaPB;
        private System.Windows.Forms.Label lblChucVu;
        private System.Windows.Forms.Label lblMaChamCong;
        private System.Windows.Forms.Label lblMaNV;
        private System.Windows.Forms.Label lblTenNV;
        private System.Windows.Forms.Label lblNgay;

        private System.Windows.Forms.ComboBox cbBoxMaPB;
        private System.Windows.Forms.ComboBox cbBoxChucVu;
        private System.Windows.Forms.TextBox tbMaChamCong;
        private System.Windows.Forms.TextBox tbmanhanvien;
        private System.Windows.Forms.TextBox tbtennhanvien;
        private System.Windows.Forms.DateTimePicker dateTimeNgayChamCong;

        private System.Windows.Forms.Label lblGioVao;
        private System.Windows.Forms.Label lblGioVe;
        private System.Windows.Forms.Label lblGhiChu;
        private System.Windows.Forms.TextBox tbGioVao;
        private System.Windows.Forms.TextBox tbGioVe;
        private System.Windows.Forms.TextBox tbGhiChu;

        private System.Windows.Forms.Panel panelAction;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnrestar;
        private System.Windows.Forms.Button xuatpdf;
        private System.Windows.Forms.Button xuatexcel;
        private System.Windows.Forms.Button btnThoat;

        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.DataGridView dtGridViewChamCong;
        private System.Windows.Forms.Button buttonkhoiphuc;
        private System.Windows.Forms.Button buttonhienthidaxoa;
        private System.Windows.Forms.TextBox txtMKKhoiPhuc;
        private System.Windows.Forms.CheckBox checkshowpassword;
        private System.Windows.Forms.Label label5;
    }
}
