namespace QuanLyNhanVien3
{
    partial class F_DuAnChung
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbTimDA = new System.Windows.Forms.ComboBox();
            this.dgvDA = new System.Windows.Forms.DataGridView();
            this.btnSuaDA = new System.Windows.Forms.Button();
            this.btnXoaDA = new System.Windows.Forms.Button();
            this.btnThemDA = new System.Windows.Forms.Button();
            this.tbGhiChuDA = new System.Windows.Forms.TextBox();
            this.tbMota = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.DatePickerNgayKetThuc = new System.Windows.Forms.DateTimePicker();
            this.DatePickerNgayBatDau = new System.Windows.Forms.DateTimePicker();
            this.tbTenDA = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbTenNhanVien = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnXuatPDF = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.btnSuaCTDA = new System.Windows.Forms.Button();
            this.btnXoaCTDA = new System.Windows.Forms.Button();
            this.btnThemCTDA = new System.Windows.Forms.Button();
            this.dgvChiTietDA = new System.Windows.Forms.DataGridView();
            this.cbMaDuAn = new System.Windows.Forms.ComboBox();
            this.cbMaNV = new System.Windows.Forms.ComboBox();
            this.tbGhiChuCTDA = new System.Windows.Forms.TextBox();
            this.tbVaiTro = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDA)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietDA)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1743, 991);
            this.panel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbTimDA);
            this.groupBox2.Controls.Add(this.dgvDA);
            this.groupBox2.Controls.Add(this.btnSuaDA);
            this.groupBox2.Controls.Add(this.btnXoaDA);
            this.groupBox2.Controls.Add(this.btnThemDA);
            this.groupBox2.Controls.Add(this.tbGhiChuDA);
            this.groupBox2.Controls.Add(this.tbMota);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.DatePickerNgayKetThuc);
            this.groupBox2.Controls.Add(this.DatePickerNgayBatDau);
            this.groupBox2.Controls.Add(this.tbTenDA);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 195);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1743, 397);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tìm Dự án";
            // 
            // cbTimDA
            // 
            this.cbTimDA.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTimDA.FormattingEnabled = true;
            this.cbTimDA.Location = new System.Drawing.Point(244, 37);
            this.cbTimDA.Name = "cbTimDA";
            this.cbTimDA.Size = new System.Drawing.Size(329, 27);
            this.cbTimDA.TabIndex = 88;
            this.cbTimDA.SelectedValueChanged += new System.EventHandler(this.cbTimDA_SelectedValueChanged);
            this.cbTimDA.TextChanged += new System.EventHandler(this.cbTimDA_TextChanged);
            // 
            // dgvDA
            // 
            this.dgvDA.AllowUserToAddRows = false;
            this.dgvDA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDA.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDA.Location = new System.Drawing.Point(631, 41);
            this.dgvDA.Name = "dgvDA";
            this.dgvDA.ReadOnly = true;
            this.dgvDA.RowHeadersWidth = 51;
            this.dgvDA.RowTemplate.Height = 24;
            this.dgvDA.Size = new System.Drawing.Size(1078, 319);
            this.dgvDA.TabIndex = 87;
            this.dgvDA.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDA_CellClick);
            // 
            // btnSuaDA
            // 
            this.btnSuaDA.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSuaDA.Location = new System.Drawing.Point(231, 327);
            this.btnSuaDA.Name = "btnSuaDA";
            this.btnSuaDA.Size = new System.Drawing.Size(125, 33);
            this.btnSuaDA.TabIndex = 54;
            this.btnSuaDA.Text = "Sửa";
            this.btnSuaDA.UseVisualStyleBackColor = true;
            this.btnSuaDA.Click += new System.EventHandler(this.btnSuaDA_Click);
            // 
            // btnXoaDA
            // 
            this.btnXoaDA.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoaDA.Location = new System.Drawing.Point(446, 327);
            this.btnXoaDA.Name = "btnXoaDA";
            this.btnXoaDA.Size = new System.Drawing.Size(125, 33);
            this.btnXoaDA.TabIndex = 53;
            this.btnXoaDA.Text = "Xóa";
            this.btnXoaDA.UseVisualStyleBackColor = true;
            this.btnXoaDA.Click += new System.EventHandler(this.btnXoaDA_Click);
            // 
            // btnThemDA
            // 
            this.btnThemDA.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemDA.Location = new System.Drawing.Point(18, 327);
            this.btnThemDA.Name = "btnThemDA";
            this.btnThemDA.Size = new System.Drawing.Size(125, 33);
            this.btnThemDA.TabIndex = 52;
            this.btnThemDA.Text = "Thêm";
            this.btnThemDA.UseVisualStyleBackColor = true;
            this.btnThemDA.Click += new System.EventHandler(this.btnThemDA_Click);
            // 
            // tbGhiChuDA
            // 
            this.tbGhiChuDA.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbGhiChuDA.Location = new System.Drawing.Point(244, 288);
            this.tbGhiChuDA.Name = "tbGhiChuDA";
            this.tbGhiChuDA.Size = new System.Drawing.Size(328, 27);
            this.tbGhiChuDA.TabIndex = 51;
            // 
            // tbMota
            // 
            this.tbMota.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMota.Location = new System.Drawing.Point(244, 238);
            this.tbMota.Name = "tbMota";
            this.tbMota.Size = new System.Drawing.Size(328, 27);
            this.tbMota.TabIndex = 50;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(14, 291);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 19);
            this.label7.TabIndex = 49;
            this.label7.Text = "Ghi Chú";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(14, 241);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 19);
            this.label6.TabIndex = 48;
            this.label6.Text = "Mô tả";
            // 
            // DatePickerNgayKetThuc
            // 
            this.DatePickerNgayKetThuc.CustomFormat = "dd/MM/yyyy";
            this.DatePickerNgayKetThuc.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DatePickerNgayKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DatePickerNgayKetThuc.Location = new System.Drawing.Point(244, 195);
            this.DatePickerNgayKetThuc.Name = "DatePickerNgayKetThuc";
            this.DatePickerNgayKetThuc.Size = new System.Drawing.Size(327, 27);
            this.DatePickerNgayKetThuc.TabIndex = 47;
            // 
            // DatePickerNgayBatDau
            // 
            this.DatePickerNgayBatDau.CustomFormat = "dd/MM/yyyy";
            this.DatePickerNgayBatDau.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DatePickerNgayBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DatePickerNgayBatDau.Location = new System.Drawing.Point(243, 143);
            this.DatePickerNgayBatDau.Name = "DatePickerNgayBatDau";
            this.DatePickerNgayBatDau.Size = new System.Drawing.Size(328, 27);
            this.DatePickerNgayBatDau.TabIndex = 46;
            // 
            // tbTenDA
            // 
            this.tbTenDA.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTenDA.Location = new System.Drawing.Point(244, 91);
            this.tbTenDA.Name = "tbTenDA";
            this.tbTenDA.Size = new System.Drawing.Size(328, 27);
            this.tbTenDA.TabIndex = 44;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(181, 19);
            this.label5.TabIndex = 43;
            this.label5.Text = "Ngày Dự Kiến / Kết Thúc";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(14, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 19);
            this.label4.TabIndex = 42;
            this.label4.Text = "Ngày Bắt Đầu";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 19);
            this.label3.TabIndex = 41;
            this.label3.Text = "Tên Dự Án";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 19);
            this.label2.TabIndex = 40;
            this.label2.Text = "Mã Dự Án";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1743, 100);
            this.panel2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(640, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(338, 53);
            this.label1.TabIndex = 1;
            this.label1.Text = "Quản Lí Dự Án";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbTenNhanVien);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Controls.Add(this.btnXuatPDF);
            this.groupBox1.Controls.Add(this.btnXuatExcel);
            this.groupBox1.Controls.Add(this.btnSuaCTDA);
            this.groupBox1.Controls.Add(this.btnXoaCTDA);
            this.groupBox1.Controls.Add(this.btnThemCTDA);
            this.groupBox1.Controls.Add(this.dgvChiTietDA);
            this.groupBox1.Controls.Add(this.cbMaDuAn);
            this.groupBox1.Controls.Add(this.cbMaNV);
            this.groupBox1.Controls.Add(this.tbGhiChuCTDA);
            this.groupBox1.Controls.Add(this.tbVaiTro);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 592);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1743, 399);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chi tiết Dự án";
            // 
            // tbTenNhanVien
            // 
            this.tbTenNhanVien.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTenNhanVien.Location = new System.Drawing.Point(242, 89);
            this.tbTenNhanVien.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbTenNhanVien.Name = "tbTenNhanVien";
            this.tbTenNhanVien.Size = new System.Drawing.Size(329, 27);
            this.tbTenNhanVien.TabIndex = 96;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(15, 92);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(108, 19);
            this.label12.TabIndex = 95;
            this.label12.Text = "Tên Nhân Viên";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(19, 336);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(125, 33);
            this.btnRefresh.TabIndex = 94;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnXuatPDF
            // 
            this.btnXuatPDF.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuatPDF.Location = new System.Drawing.Point(447, 337);
            this.btnXuatPDF.Name = "btnXuatPDF";
            this.btnXuatPDF.Size = new System.Drawing.Size(125, 33);
            this.btnXuatPDF.TabIndex = 93;
            this.btnXuatPDF.Text = "Xuất PDF";
            this.btnXuatPDF.UseVisualStyleBackColor = true;
            this.btnXuatPDF.Click += new System.EventHandler(this.btnXuatPDF_Click);
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuatExcel.Location = new System.Drawing.Point(232, 336);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(125, 33);
            this.btnXuatExcel.TabIndex = 92;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // btnSuaCTDA
            // 
            this.btnSuaCTDA.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSuaCTDA.Location = new System.Drawing.Point(232, 284);
            this.btnSuaCTDA.Name = "btnSuaCTDA";
            this.btnSuaCTDA.Size = new System.Drawing.Size(125, 33);
            this.btnSuaCTDA.TabIndex = 91;
            this.btnSuaCTDA.Text = "Sửa";
            this.btnSuaCTDA.UseVisualStyleBackColor = true;
            this.btnSuaCTDA.Click += new System.EventHandler(this.btnSuaCTDA_Click);
            // 
            // btnXoaCTDA
            // 
            this.btnXoaCTDA.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoaCTDA.Location = new System.Drawing.Point(447, 284);
            this.btnXoaCTDA.Name = "btnXoaCTDA";
            this.btnXoaCTDA.Size = new System.Drawing.Size(125, 33);
            this.btnXoaCTDA.TabIndex = 90;
            this.btnXoaCTDA.Text = "Xóa";
            this.btnXoaCTDA.UseVisualStyleBackColor = true;
            this.btnXoaCTDA.Click += new System.EventHandler(this.btnXoaCTDA_Click);
            // 
            // btnThemCTDA
            // 
            this.btnThemCTDA.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemCTDA.Location = new System.Drawing.Point(19, 283);
            this.btnThemCTDA.Name = "btnThemCTDA";
            this.btnThemCTDA.Size = new System.Drawing.Size(125, 33);
            this.btnThemCTDA.TabIndex = 89;
            this.btnThemCTDA.Text = "Thêm";
            this.btnThemCTDA.UseVisualStyleBackColor = true;
            this.btnThemCTDA.Click += new System.EventHandler(this.btnThemCTDA_Click);
            // 
            // dgvChiTietDA
            // 
            this.dgvChiTietDA.AllowUserToAddRows = false;
            this.dgvChiTietDA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvChiTietDA.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChiTietDA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChiTietDA.Location = new System.Drawing.Point(631, 46);
            this.dgvChiTietDA.Name = "dgvChiTietDA";
            this.dgvChiTietDA.ReadOnly = true;
            this.dgvChiTietDA.RowHeadersWidth = 51;
            this.dgvChiTietDA.RowTemplate.Height = 24;
            this.dgvChiTietDA.Size = new System.Drawing.Size(1078, 305);
            this.dgvChiTietDA.TabIndex = 88;
            this.dgvChiTietDA.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChiTietDA_CellClick);
            // 
            // cbMaDuAn
            // 
            this.cbMaDuAn.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMaDuAn.FormattingEnabled = true;
            this.cbMaDuAn.Location = new System.Drawing.Point(244, 135);
            this.cbMaDuAn.Name = "cbMaDuAn";
            this.cbMaDuAn.Size = new System.Drawing.Size(329, 27);
            this.cbMaDuAn.TabIndex = 72;
            // 
            // cbMaNV
            // 
            this.cbMaNV.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMaNV.FormattingEnabled = true;
            this.cbMaNV.Location = new System.Drawing.Point(243, 46);
            this.cbMaNV.Name = "cbMaNV";
            this.cbMaNV.Size = new System.Drawing.Size(329, 27);
            this.cbMaNV.TabIndex = 71;
            this.cbMaNV.SelectedValueChanged += new System.EventHandler(this.cbMaNV_SelectedValueChanged);
            // 
            // tbGhiChuCTDA
            // 
            this.tbGhiChuCTDA.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbGhiChuCTDA.Location = new System.Drawing.Point(244, 228);
            this.tbGhiChuCTDA.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbGhiChuCTDA.Name = "tbGhiChuCTDA";
            this.tbGhiChuCTDA.Size = new System.Drawing.Size(329, 27);
            this.tbGhiChuCTDA.TabIndex = 70;
            // 
            // tbVaiTro
            // 
            this.tbVaiTro.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbVaiTro.Location = new System.Drawing.Point(244, 180);
            this.tbVaiTro.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbVaiTro.Name = "tbVaiTro";
            this.tbVaiTro.Size = new System.Drawing.Size(329, 27);
            this.tbVaiTro.TabIndex = 69;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(15, 231);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 19);
            this.label8.TabIndex = 68;
            this.label8.Text = "Ghi Chú";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(15, 182);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 19);
            this.label9.TabIndex = 67;
            this.label9.Text = "Vai trò";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(15, 138);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 19);
            this.label10.TabIndex = 66;
            this.label10.Text = "Mã Dự Án";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(15, 49);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 19);
            this.label11.TabIndex = 65;
            this.label11.Text = "Mã Nhân Viên";
            // 
            // F_DuAnChung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1743, 991);
            this.Controls.Add(this.panel1);
            this.Name = "F_DuAnChung";
            this.Text = "F_DuAnChung";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_DuAnChung_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDA)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietDA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker DatePickerNgayKetThuc;
        private System.Windows.Forms.DateTimePicker DatePickerNgayBatDau;
        private System.Windows.Forms.TextBox tbTenDA;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbGhiChuDA;
        private System.Windows.Forms.TextBox tbMota;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSuaDA;
        private System.Windows.Forms.Button btnXoaDA;
        private System.Windows.Forms.Button btnThemDA;
        private System.Windows.Forms.DataGridView dgvDA;
        private System.Windows.Forms.ComboBox cbMaDuAn;
        private System.Windows.Forms.ComboBox cbMaNV;
        private System.Windows.Forms.TextBox tbGhiChuCTDA;
        private System.Windows.Forms.TextBox tbVaiTro;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnXuatPDF;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Button btnSuaCTDA;
        private System.Windows.Forms.Button btnXoaCTDA;
        private System.Windows.Forms.Button btnThemCTDA;
        private System.Windows.Forms.DataGridView dgvChiTietDA;
        private System.Windows.Forms.TextBox tbTenNhanVien;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbTimDA;
    }
}