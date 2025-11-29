namespace QuanLyNhanVien3
{
    partial class F_HopDong
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dtGridViewHD = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.tbLoaiHD = new System.Windows.Forms.TextBox();
            this.checkHienMK = new System.Windows.Forms.CheckBox();
            this.tbMKKhoiPhuc = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnKhoiPhucHDCu = new System.Windows.Forms.Button();
            this.btnXemHDCu = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbGhiChu = new System.Windows.Forms.TextBox();
            this.tbLuongCoBan = new System.Windows.Forms.TextBox();
            this.cbMaNV = new System.Windows.Forms.ComboBox();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnThem = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbMaHD = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DatePickerNgayKetThuc = new System.Windows.Forms.DateTimePicker();
            this.DatePickerNgayBatDau = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnThoat = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewHD)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtGridViewHD
            // 
            this.dtGridViewHD.AllowUserToAddRows = false;
            this.dtGridViewHD.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewHD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dtGridViewHD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridViewHD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewHD.Location = new System.Drawing.Point(0, 0);
            this.dtGridViewHD.Name = "dtGridViewHD";
            this.dtGridViewHD.ReadOnly = true;
            this.dtGridViewHD.RowHeadersWidth = 51;
            this.dtGridViewHD.RowTemplate.Height = 24;
            this.dtGridViewHD.Size = new System.Drawing.Size(1202, 423);
            this.dtGridViewHD.TabIndex = 86;
            this.dtGridViewHD.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtGridViewHD_CellClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnThoat);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1202, 74);
            this.panel1.TabIndex = 92;
            // 
            // panel4
            // 
            this.panel4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.panel4.Location = new System.Drawing.Point(6, 76);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(466, 319);
            this.panel4.TabIndex = 95;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(513, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 46);
            this.label1.TabIndex = 92;
            this.label1.Text = "Hợp Đồng";
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(3, 72);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(494, 305);
            this.panel3.TabIndex = 94;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dtGridViewHD);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 424);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1202, 423);
            this.panel2.TabIndex = 93;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.tbLoaiHD);
            this.panel5.Controls.Add(this.checkHienMK);
            this.panel5.Controls.Add(this.tbMKKhoiPhuc);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.btnKhoiPhucHDCu);
            this.panel5.Controls.Add(this.btnXemHDCu);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.tbGhiChu);
            this.panel5.Controls.Add(this.tbLuongCoBan);
            this.panel5.Controls.Add(this.cbMaNV);
            this.panel5.Controls.Add(this.btnXuatExcel);
            this.panel5.Controls.Add(this.btnRefresh);
            this.panel5.Controls.Add(this.btnTimKiem);
            this.panel5.Controls.Add(this.btnSua);
            this.panel5.Controls.Add(this.btnXoa);
            this.panel5.Controls.Add(this.btnThem);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.tbMaHD);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.DatePickerNgayKetThuc);
            this.panel5.Controls.Add(this.DatePickerNgayBatDau);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 74);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1202, 361);
            this.panel5.TabIndex = 94;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(652, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 19);
            this.label9.TabIndex = 163;
            this.label9.Text = "Loại Hợp Đồng";
            // 
            // tbLoaiHD
            // 
            this.tbLoaiHD.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tbLoaiHD.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLoaiHD.Location = new System.Drawing.Point(787, 28);
            this.tbLoaiHD.Name = "tbLoaiHD";
            this.tbLoaiHD.Size = new System.Drawing.Size(344, 27);
            this.tbLoaiHD.TabIndex = 162;
            // 
            // checkHienMK
            // 
            this.checkHienMK.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.checkHienMK.AutoSize = true;
            this.checkHienMK.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkHienMK.Location = new System.Drawing.Point(957, 299);
            this.checkHienMK.Name = "checkHienMK";
            this.checkHienMK.Size = new System.Drawing.Size(150, 23);
            this.checkHienMK.TabIndex = 161;
            this.checkHienMK.Text = "Hiển thị mật khẩu";
            this.checkHienMK.UseVisualStyleBackColor = true;
            this.checkHienMK.CheckedChanged += new System.EventHandler(this.checkHienMK_CheckedChanged_1);
            // 
            // tbMKKhoiPhuc
            // 
            this.tbMKKhoiPhuc.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tbMKKhoiPhuc.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMKKhoiPhuc.Location = new System.Drawing.Point(957, 270);
            this.tbMKKhoiPhuc.Name = "tbMKKhoiPhuc";
            this.tbMKKhoiPhuc.Size = new System.Drawing.Size(174, 27);
            this.tbMKKhoiPhuc.TabIndex = 160;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(953, 248);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 19);
            this.label8.TabIndex = 159;
            this.label8.Text = "MK Khôi Phục";
            // 
            // btnKhoiPhucHDCu
            // 
            this.btnKhoiPhucHDCu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnKhoiPhucHDCu.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKhoiPhucHDCu.Location = new System.Drawing.Point(656, 297);
            this.btnKhoiPhucHDCu.Name = "btnKhoiPhucHDCu";
            this.btnKhoiPhucHDCu.Size = new System.Drawing.Size(275, 35);
            this.btnKhoiPhucHDCu.TabIndex = 158;
            this.btnKhoiPhucHDCu.Text = "Khôi Phục Hợp Đồng Cũ";
            this.btnKhoiPhucHDCu.UseVisualStyleBackColor = true;
            this.btnKhoiPhucHDCu.Click += new System.EventHandler(this.btnKhoiPhucHDCu_Click_1);
            // 
            // btnXemHDCu
            // 
            this.btnXemHDCu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnXemHDCu.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXemHDCu.Location = new System.Drawing.Point(656, 244);
            this.btnXemHDCu.Name = "btnXemHDCu";
            this.btnXemHDCu.Size = new System.Drawing.Size(275, 35);
            this.btnXemHDCu.TabIndex = 157;
            this.btnXemHDCu.Text = "Hiển Thị Hợp Đồng Cũ";
            this.btnXemHDCu.UseVisualStyleBackColor = true;
            this.btnXemHDCu.Click += new System.EventHandler(this.btnXemHDCu_Click_1);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(652, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 19);
            this.label7.TabIndex = 156;
            this.label7.Text = "Ghi Chú";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(652, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 19);
            this.label6.TabIndex = 155;
            this.label6.Text = "Lương Cơ Bản";
            // 
            // tbGhiChu
            // 
            this.tbGhiChu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tbGhiChu.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbGhiChu.Location = new System.Drawing.Point(787, 146);
            this.tbGhiChu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbGhiChu.Name = "tbGhiChu";
            this.tbGhiChu.Size = new System.Drawing.Size(344, 27);
            this.tbGhiChu.TabIndex = 154;
            // 
            // tbLuongCoBan
            // 
            this.tbLuongCoBan.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tbLuongCoBan.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLuongCoBan.Location = new System.Drawing.Point(787, 90);
            this.tbLuongCoBan.Name = "tbLuongCoBan";
            this.tbLuongCoBan.Size = new System.Drawing.Size(344, 27);
            this.tbLuongCoBan.TabIndex = 153;
            // 
            // cbMaNV
            // 
            this.cbMaNV.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbMaNV.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMaNV.FormattingEnabled = true;
            this.cbMaNV.Location = new System.Drawing.Point(217, 89);
            this.cbMaNV.Name = "cbMaNV";
            this.cbMaNV.Size = new System.Drawing.Size(283, 27);
            this.cbMaNV.TabIndex = 142;
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnXuatExcel.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuatExcel.Location = new System.Drawing.Point(375, 248);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(125, 33);
            this.btnXuatExcel.TabIndex = 139;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click_1);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRefresh.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(235, 249);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(125, 33);
            this.btnRefresh.TabIndex = 138;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click_1);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnTimKiem.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTimKiem.Location = new System.Drawing.Point(375, 297);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(125, 33);
            this.btnTimKiem.TabIndex = 137;
            this.btnTimKiem.Text = "Tìm Kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click_1);
            // 
            // btnSua
            // 
            this.btnSua.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSua.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSua.Location = new System.Drawing.Point(95, 297);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(125, 33);
            this.btnSua.TabIndex = 136;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click_1);
            // 
            // btnXoa
            // 
            this.btnXoa.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnXoa.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoa.Location = new System.Drawing.Point(235, 297);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(125, 33);
            this.btnXoa.TabIndex = 135;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click_1);
            // 
            // btnThem
            // 
            this.btnThem.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnThem.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThem.Location = new System.Drawing.Point(95, 249);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(125, 33);
            this.btnThem.TabIndex = 134;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click_2);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(91, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 19);
            this.label3.TabIndex = 124;
            this.label3.Text = "Mã Nhân Viên";
            // 
            // tbMaHD
            // 
            this.tbMaHD.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tbMaHD.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMaHD.Location = new System.Drawing.Point(219, 33);
            this.tbMaHD.Name = "tbMaHD";
            this.tbMaHD.Size = new System.Drawing.Size(281, 27);
            this.tbMaHD.TabIndex = 123;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(91, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 19);
            this.label2.TabIndex = 122;
            this.label2.Text = "Mã Hợp Đồng";
            // 
            // DatePickerNgayKetThuc
            // 
            this.DatePickerNgayKetThuc.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DatePickerNgayKetThuc.CustomFormat = "dd/MM/yyyy";
            this.DatePickerNgayKetThuc.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DatePickerNgayKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DatePickerNgayKetThuc.Location = new System.Drawing.Point(219, 195);
            this.DatePickerNgayKetThuc.Name = "DatePickerNgayKetThuc";
            this.DatePickerNgayKetThuc.Size = new System.Drawing.Size(281, 27);
            this.DatePickerNgayKetThuc.TabIndex = 121;
            // 
            // DatePickerNgayBatDau
            // 
            this.DatePickerNgayBatDau.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DatePickerNgayBatDau.CustomFormat = "dd/MM/yyyy";
            this.DatePickerNgayBatDau.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DatePickerNgayBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DatePickerNgayBatDau.Location = new System.Drawing.Point(218, 143);
            this.DatePickerNgayBatDau.Name = "DatePickerNgayBatDau";
            this.DatePickerNgayBatDau.Size = new System.Drawing.Size(282, 27);
            this.DatePickerNgayBatDau.TabIndex = 120;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(91, 201);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 19);
            this.label5.TabIndex = 119;
            this.label5.Text = "Ngày Kết Thúc";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(91, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 19);
            this.label4.TabIndex = 118;
            this.label4.Text = "Ngày Bắt Đầu";
            // 
            // btnThoat
            // 
            this.btnThoat.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnThoat.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThoat.Location = new System.Drawing.Point(1002, 18);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(163, 50);
            this.btnThoat.TabIndex = 169;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // F_HopDong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1202, 847);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "F_HopDong";
            this.Text = "HopDong";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_HopDong_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewHD)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dtGridViewHD;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox cbMaNV;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbMaHD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DatePickerNgayKetThuc;
        private System.Windows.Forms.DateTimePicker DatePickerNgayBatDau;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbLoaiHD;
        private System.Windows.Forms.CheckBox checkHienMK;
        private System.Windows.Forms.TextBox tbMKKhoiPhuc;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnKhoiPhucHDCu;
        private System.Windows.Forms.Button btnXemHDCu;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbGhiChu;
        private System.Windows.Forms.TextBox tbLuongCoBan;
        private System.Windows.Forms.Button btnThoat;
    }
}