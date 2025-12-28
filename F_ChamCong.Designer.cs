namespace QuanLyNhanVien3
{
    partial class F_ChamCong
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_ChamCong));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnThoat = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDungQuetCam = new System.Windows.Forms.Button();
            this.btnChonAnh = new System.Windows.Forms.Button();
            this.btnChamCong = new System.Windows.Forms.Button();
            this.pictureBoxChamCong = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dtGridViewChamCong = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChamCong)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewChamCong)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnThoat);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnDungQuetCam);
            this.panel1.Controls.Add(this.btnChonAnh);
            this.panel1.Controls.Add(this.btnChamCong);
            this.panel1.Controls.Add(this.pictureBoxChamCong);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1406, 666);
            this.panel1.TabIndex = 0;
            // 
            // btnThoat
            // 
            this.btnThoat.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnThoat.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThoat.Location = new System.Drawing.Point(1215, 112);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(163, 50);
            this.btnThoat.TabIndex = 168;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Cyan;
            this.label7.Location = new System.Drawing.Point(609, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(213, 46);
            this.label7.TabIndex = 167;
            this.label7.Text = "Chấm Công";
            // 
            // btnDungQuetCam
            // 
            this.btnDungQuetCam.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDungQuetCam.Location = new System.Drawing.Point(100, 334);
            this.btnDungQuetCam.Name = "btnDungQuetCam";
            this.btnDungQuetCam.Size = new System.Drawing.Size(230, 42);
            this.btnDungQuetCam.TabIndex = 166;
            this.btnDungQuetCam.Text = "Dừng Quét";
            this.btnDungQuetCam.UseVisualStyleBackColor = true;
            this.btnDungQuetCam.Click += new System.EventHandler(this.btnDungQuetCam_Click);
            // 
            // btnChonAnh
            // 
            this.btnChonAnh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChonAnh.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChonAnh.Location = new System.Drawing.Point(1107, 377);
            this.btnChonAnh.Name = "btnChonAnh";
            this.btnChonAnh.Size = new System.Drawing.Size(230, 42);
            this.btnChonAnh.TabIndex = 165;
            this.btnChonAnh.Text = "Chon tu thu vien";
            this.btnChonAnh.UseVisualStyleBackColor = true;
            this.btnChonAnh.Visible = false;
            // 
            // btnChamCong
            // 
            this.btnChamCong.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChamCong.Location = new System.Drawing.Point(100, 274);
            this.btnChamCong.Name = "btnChamCong";
            this.btnChamCong.Size = new System.Drawing.Size(230, 42);
            this.btnChamCong.TabIndex = 162;
            this.btnChamCong.Text = "Cham Cong";
            this.btnChamCong.UseVisualStyleBackColor = true;
            this.btnChamCong.Click += new System.EventHandler(this.btnChamCong_Click);
            // 
            // pictureBoxChamCong
            // 
            this.pictureBoxChamCong.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxChamCong.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pictureBoxChamCong.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxChamCong.ErrorImage")));
            this.pictureBoxChamCong.Location = new System.Drawing.Point(397, 112);
            this.pictureBoxChamCong.Name = "pictureBoxChamCong";
            this.pictureBoxChamCong.Size = new System.Drawing.Size(687, 506);
            this.pictureBoxChamCong.TabIndex = 1;
            this.pictureBoxChamCong.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dtGridViewChamCong);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 666);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1406, 174);
            this.panel3.TabIndex = 2;
            // 
            // dtGridViewChamCong
            // 
            this.dtGridViewChamCong.AllowUserToAddRows = false;
            this.dtGridViewChamCong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewChamCong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridViewChamCong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewChamCong.Location = new System.Drawing.Point(0, 0);
            this.dtGridViewChamCong.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtGridViewChamCong.Name = "dtGridViewChamCong";
            this.dtGridViewChamCong.ReadOnly = true;
            this.dtGridViewChamCong.RowHeadersWidth = 51;
            this.dtGridViewChamCong.RowTemplate.Height = 24;
            this.dtGridViewChamCong.Size = new System.Drawing.Size(1406, 174);
            this.dtGridViewChamCong.TabIndex = 121;
            // 
            // timer1
            // 
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // F_ChamCong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1406, 840);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "F_ChamCong";
            this.Text = "ChamCong";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_ChamCong_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChamCong)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewChamCong)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBoxChamCong;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnChamCong;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridView dtGridViewChamCong;
        private System.Windows.Forms.Button btnChonAnh;
        private System.Windows.Forms.Button btnDungQuetCam;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnThoat;
    }
}