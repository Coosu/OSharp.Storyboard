namespace Milkitic.OsbTool
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tbSource = new System.Windows.Forms.TextBox();
            this.tbHandled = new System.Windows.Forms.TextBox();
            this.btnAdj = new System.Windows.Forms.Button();
            this.tbTiming = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabC1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_Spath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_Tpath = new System.Windows.Forms.TextBox();
            this.btn_ExeComp = new System.Windows.Forms.Button();
            this.btnSOfg = new System.Windows.Forms.Button();
            this.btnTOfg = new System.Windows.Forms.Button();
            this.tabC1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbSource
            // 
            this.tbSource.Location = new System.Drawing.Point(12, 31);
            this.tbSource.Multiline = true;
            this.tbSource.Name = "tbSource";
            this.tbSource.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSource.Size = new System.Drawing.Size(260, 237);
            this.tbSource.TabIndex = 0;
            // 
            // tbHandled
            // 
            this.tbHandled.Location = new System.Drawing.Point(278, 31);
            this.tbHandled.Multiline = true;
            this.tbHandled.Name = "tbHandled";
            this.tbHandled.ReadOnly = true;
            this.tbHandled.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbHandled.Size = new System.Drawing.Size(260, 237);
            this.tbHandled.TabIndex = 1;
            // 
            // btnAdj
            // 
            this.btnAdj.Location = new System.Drawing.Point(159, 67);
            this.btnAdj.Name = "btnAdj";
            this.btnAdj.Size = new System.Drawing.Size(75, 23);
            this.btnAdj.TabIndex = 2;
            this.btnAdj.Text = "Adjust";
            this.btnAdj.UseVisualStyleBackColor = true;
            this.btnAdj.Click += new System.EventHandler(this.btnAdj_Click);
            // 
            // tbTiming
            // 
            this.tbTiming.Location = new System.Drawing.Point(53, 13);
            this.tbTiming.Name = "tbTiming";
            this.tbTiming.Size = new System.Drawing.Size(100, 21);
            this.tbTiming.TabIndex = 3;
            this.tbTiming.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "timing";
            // 
            // tabC1
            // 
            this.tabC1.Controls.Add(this.tabPage1);
            this.tabC1.Controls.Add(this.tabPage2);
            this.tabC1.Location = new System.Drawing.Point(12, 274);
            this.tabC1.Name = "tabC1";
            this.tabC1.SelectedIndex = 0;
            this.tabC1.Size = new System.Drawing.Size(526, 123);
            this.tabC1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbY);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.tbX);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.tbTiming);
            this.tabPage1.Controls.Add(this.btnAdj);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(518, 97);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Adjustment";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbY
            // 
            this.tbY.Location = new System.Drawing.Point(53, 67);
            this.tbY.Name = "tbY";
            this.tbY.Size = new System.Drawing.Size(100, 21);
            this.tbY.TabIndex = 7;
            this.tbY.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "y";
            // 
            // tbX
            // 
            this.tbX.Location = new System.Drawing.Point(53, 40);
            this.tbX.Name = "tbX";
            this.tbX.Size = new System.Drawing.Size(100, 21);
            this.tbX.TabIndex = 5;
            this.tbX.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "x";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnTOfg);
            this.tabPage2.Controls.Add(this.btnSOfg);
            this.tabPage2.Controls.Add(this.btn_ExeComp);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.tb_Tpath);
            this.tabPage2.Controls.Add(this.tb_Spath);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(518, 97);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Compresser";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Source code:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(276, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "Adjusted code:";
            // 
            // tb_Spath
            // 
            this.tb_Spath.Location = new System.Drawing.Point(95, 14);
            this.tb_Spath.Name = "tb_Spath";
            this.tb_Spath.Size = new System.Drawing.Size(380, 21);
            this.tb_Spath.TabIndex = 0;
            this.tb_Spath.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "Source path: ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "Target path: ";
            // 
            // tb_Tpath
            // 
            this.tb_Tpath.Location = new System.Drawing.Point(95, 41);
            this.tb_Tpath.Name = "tb_Tpath";
            this.tb_Tpath.Size = new System.Drawing.Size(380, 21);
            this.tb_Tpath.TabIndex = 0;
            this.tb_Tpath.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btn_ExeComp
            // 
            this.btn_ExeComp.Location = new System.Drawing.Point(95, 68);
            this.btn_ExeComp.Name = "btn_ExeComp";
            this.btn_ExeComp.Size = new System.Drawing.Size(75, 23);
            this.btn_ExeComp.TabIndex = 2;
            this.btn_ExeComp.Text = "Execute";
            this.btn_ExeComp.UseVisualStyleBackColor = true;
            this.btn_ExeComp.Click += new System.EventHandler(this.btn_ExeComp_Click);
            // 
            // btnSOfg
            // 
            this.btnSOfg.Location = new System.Drawing.Point(481, 12);
            this.btnSOfg.Name = "btnSOfg";
            this.btnSOfg.Size = new System.Drawing.Size(31, 23);
            this.btnSOfg.TabIndex = 3;
            this.btnSOfg.Text = "...";
            this.btnSOfg.UseVisualStyleBackColor = true;
            this.btnSOfg.Click += new System.EventHandler(this.btnSOfg_Click);
            // 
            // btnTOfg
            // 
            this.btnTOfg.Location = new System.Drawing.Point(481, 39);
            this.btnTOfg.Name = "btnTOfg";
            this.btnTOfg.Size = new System.Drawing.Size(31, 23);
            this.btnTOfg.TabIndex = 3;
            this.btnTOfg.Text = "...";
            this.btnTOfg.UseVisualStyleBackColor = true;
            this.btnTOfg.Click += new System.EventHandler(this.btnTOfg_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 409);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tabC1);
            this.Controls.Add(this.tbHandled);
            this.Controls.Add(this.tbSource);
            this.Name = "FrmMain";
            this.Text = "Storyboard Tool";
            this.tabC1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSource;
        private System.Windows.Forms.TextBox tbHandled;
        private System.Windows.Forms.Button btnAdj;
        private System.Windows.Forms.TextBox tbTiming;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabC1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox tbY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_Spath;
        private System.Windows.Forms.Button btnSOfg;
        private System.Windows.Forms.Button btn_ExeComp;
        private System.Windows.Forms.TextBox tb_Tpath;
        private System.Windows.Forms.Button btnTOfg;
    }
}

