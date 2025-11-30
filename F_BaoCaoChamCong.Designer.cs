namespace QuanLyNhanVien3
{
    partial class F_BaoCaoChamCong
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtpThoiGian = new System.Windows.Forms.DateTimePicker();
            this.btnDiTreVeSom = new System.Windows.Forms.Button();
            this.btnSoNgayLamViec = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dtGridViewBCChamCong = new System.Windows.Forms.DataGridView();
            this.btnXuat = new System.Windows.Forms.Button();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.txtTimkiem = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCChamCong)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1617, 152);
            this.panel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(585, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(469, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "Báo Cáo Chấm Công";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtTimkiem);
            this.panel2.Controls.Add(this.btnTimKiem);
            this.panel2.Controls.Add(this.btnXuat);
            this.panel2.Controls.Add(this.dtpThoiGian);
            this.panel2.Controls.Add(this.btnDiTreVeSom);
            this.panel2.Controls.Add(this.btnSoNgayLamViec);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 152);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1617, 274);
            this.panel2.TabIndex = 4;
            // 
            // dtpThoiGian
            // 
            this.dtpThoiGian.Location = new System.Drawing.Point(460, 20);
            this.dtpThoiGian.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpThoiGian.Name = "dtpThoiGian";
            this.dtpThoiGian.Size = new System.Drawing.Size(224, 26);
            this.dtpThoiGian.TabIndex = 2;
            // 
            // btnDiTreVeSom
            // 
            this.btnDiTreVeSom.Location = new System.Drawing.Point(460, 144);
            this.btnDiTreVeSom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDiTreVeSom.Name = "btnDiTreVeSom";
            this.btnDiTreVeSom.Size = new System.Drawing.Size(404, 45);
            this.btnDiTreVeSom.TabIndex = 1;
            this.btnDiTreVeSom.Text = "Nhân viên đi trễ hoặc về sớm (nếu có quy định giờ)";
            this.btnDiTreVeSom.UseVisualStyleBackColor = true;
            this.btnDiTreVeSom.Click += new System.EventHandler(this.btnDiTreVeSom_Click);
            // 
            // btnSoNgayLamViec
            // 
            this.btnSoNgayLamViec.Location = new System.Drawing.Point(460, 68);
            this.btnSoNgayLamViec.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSoNgayLamViec.Name = "btnSoNgayLamViec";
            this.btnSoNgayLamViec.Size = new System.Drawing.Size(404, 45);
            this.btnSoNgayLamViec.TabIndex = 0;
            this.btnSoNgayLamViec.Text = "Số ngày làm việc mỗi nhân viên trong tháng";
            this.btnSoNgayLamViec.UseVisualStyleBackColor = true;
            this.btnSoNgayLamViec.Click += new System.EventHandler(this.btnSoNgayLamViec_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dtGridViewBCChamCong);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 426);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1617, 519);
            this.panel3.TabIndex = 5;
            // 
            // dtGridViewBCChamCong
            // 
            this.dtGridViewBCChamCong.AllowUserToAddRows = false;
            this.dtGridViewBCChamCong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewBCChamCong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridViewBCChamCong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewBCChamCong.Location = new System.Drawing.Point(0, 0);
            this.dtGridViewBCChamCong.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtGridViewBCChamCong.Name = "dtGridViewBCChamCong";
            this.dtGridViewBCChamCong.ReadOnly = true;
            this.dtGridViewBCChamCong.RowHeadersWidth = 51;
            this.dtGridViewBCChamCong.RowTemplate.Height = 24;
            this.dtGridViewBCChamCong.Size = new System.Drawing.Size(1617, 519);
            this.dtGridViewBCChamCong.TabIndex = 122;
            // 
            // btnXuat
            // 
            this.btnXuat.Location = new System.Drawing.Point(1080, 166);
            this.btnXuat.Name = "btnXuat";
            this.btnXuat.Size = new System.Drawing.Size(110, 40);
            this.btnXuat.TabIndex = 3;
            this.btnXuat.Text = "Xuất Excel";
            this.btnXuat.UseVisualStyleBackColor = true;
            this.btnXuat.Click += new System.EventHandler(this.btnXuat_Click);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Location = new System.Drawing.Point(1080, 73);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(110, 40);
            this.btnTimKiem.TabIndex = 4;
            this.btnTimKiem.Text = "Tìm Kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // txtTimkiem
            // 
            this.txtTimkiem.Location = new System.Drawing.Point(1223, 84);
            this.txtTimkiem.Name = "txtTimkiem";
            this.txtTimkiem.Size = new System.Drawing.Size(169, 26);
            this.txtTimkiem.TabIndex = 5;
            // 
            // F_BaoCaoChamCong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1617, 945);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "F_BaoCaoChamCong";
            this.Text = "F_BaoCaoChamCong";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCChamCong)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtpThoiGian;
        private System.Windows.Forms.Button btnDiTreVeSom;
        private System.Windows.Forms.Button btnSoNgayLamViec;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dtGridViewBCChamCong;
        private System.Windows.Forms.TextBox txtTimkiem;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnXuat;
    }
}