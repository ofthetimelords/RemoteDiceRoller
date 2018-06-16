namespace TheQ.RemoteDiceRoller
{
    partial class FormMain
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
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.Connect = new System.Windows.Forms.Button();
            this.TempLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.SysLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.D4Amount = new System.Windows.Forms.NumericUpDown();
            this.D6Amount = new System.Windows.Forms.NumericUpDown();
            this.D8Amount = new System.Windows.Forms.NumericUpDown();
            this.D10Amount = new System.Windows.Forms.NumericUpDown();
            this.D12Amount = new System.Windows.Forms.NumericUpDown();
            this.D20Amount = new System.Windows.Forms.NumericUpDown();
            this.D100Amount = new System.Windows.Forms.NumericUpDown();
            this.RollD4 = new System.Windows.Forms.Button();
            this.RollD6 = new System.Windows.Forms.Button();
            this.RollD8 = new System.Windows.Forms.Button();
            this.RollD10 = new System.Windows.Forms.Button();
            this.RollD12 = new System.Windows.Forms.Button();
            this.RollD20 = new System.Windows.Forms.Button();
            this.RollD100 = new System.Windows.Forms.Button();
            this.Dice = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.D4Amount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D6Amount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D8Amount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D10Amount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D12Amount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D20Amount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.D100Amount)).BeginInit();
            this.Dice.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(12, 12);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(75, 23);
            this.Connect.TabIndex = 0;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // TempLabel
            // 
            this.TempLabel.AutoSize = true;
            this.TempLabel.Location = new System.Drawing.Point(3, 0);
            this.TempLabel.Name = "TempLabel";
            this.TempLabel.Size = new System.Drawing.Size(35, 13);
            this.TempLabel.TabIndex = 1;
            this.TempLabel.Text = "label1";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.TempLabel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 131);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(628, 81);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // SysLog
            // 
            this.SysLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.SysLog.Location = new System.Drawing.Point(646, 12);
            this.SysLog.Multiline = true;
            this.SysLog.Name = "SysLog";
            this.SysLog.ReadOnly = true;
            this.SysLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SysLog.Size = new System.Drawing.Size(154, 200);
            this.SysLog.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "d4";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "d6";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(165, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "d8";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(241, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "d10";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(323, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "d12";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(405, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "d20";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(487, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "d100";
            // 
            // D4Amount
            // 
            this.D4Amount.Location = new System.Drawing.Point(16, 47);
            this.D4Amount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.D4Amount.Name = "D4Amount";
            this.D4Amount.Size = new System.Drawing.Size(44, 20);
            this.D4Amount.TabIndex = 6;
            this.D4Amount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // D6Amount
            // 
            this.D6Amount.Location = new System.Drawing.Point(92, 47);
            this.D6Amount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.D6Amount.Name = "D6Amount";
            this.D6Amount.Size = new System.Drawing.Size(44, 20);
            this.D6Amount.TabIndex = 6;
            this.D6Amount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // D8Amount
            // 
            this.D8Amount.Location = new System.Drawing.Point(168, 47);
            this.D8Amount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.D8Amount.Name = "D8Amount";
            this.D8Amount.Size = new System.Drawing.Size(44, 20);
            this.D8Amount.TabIndex = 6;
            this.D8Amount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // D10Amount
            // 
            this.D10Amount.Location = new System.Drawing.Point(244, 47);
            this.D10Amount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.D10Amount.Name = "D10Amount";
            this.D10Amount.Size = new System.Drawing.Size(44, 20);
            this.D10Amount.TabIndex = 6;
            this.D10Amount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // D12Amount
            // 
            this.D12Amount.Location = new System.Drawing.Point(326, 47);
            this.D12Amount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.D12Amount.Name = "D12Amount";
            this.D12Amount.Size = new System.Drawing.Size(44, 20);
            this.D12Amount.TabIndex = 6;
            this.D12Amount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // D20Amount
            // 
            this.D20Amount.Location = new System.Drawing.Point(408, 47);
            this.D20Amount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.D20Amount.Name = "D20Amount";
            this.D20Amount.Size = new System.Drawing.Size(44, 20);
            this.D20Amount.TabIndex = 6;
            this.D20Amount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // D100Amount
            // 
            this.D100Amount.Location = new System.Drawing.Point(490, 47);
            this.D100Amount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.D100Amount.Name = "D100Amount";
            this.D100Amount.Size = new System.Drawing.Size(44, 20);
            this.D100Amount.TabIndex = 6;
            this.D100Amount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // RollD4
            // 
            this.RollD4.Location = new System.Drawing.Point(16, 74);
            this.RollD4.Name = "RollD4";
            this.RollD4.Size = new System.Drawing.Size(44, 23);
            this.RollD4.TabIndex = 7;
            this.RollD4.Text = "Roll!";
            this.RollD4.UseVisualStyleBackColor = true;
            this.RollD4.Click += new System.EventHandler(this.RollD4_Click);
            // 
            // RollD6
            // 
            this.RollD6.Location = new System.Drawing.Point(92, 74);
            this.RollD6.Name = "RollD6";
            this.RollD6.Size = new System.Drawing.Size(44, 23);
            this.RollD6.TabIndex = 7;
            this.RollD6.Text = "Roll!";
            this.RollD6.UseVisualStyleBackColor = true;
            this.RollD6.Click += new System.EventHandler(this.RollD6_Click);
            // 
            // RollD8
            // 
            this.RollD8.Location = new System.Drawing.Point(168, 74);
            this.RollD8.Name = "RollD8";
            this.RollD8.Size = new System.Drawing.Size(44, 23);
            this.RollD8.TabIndex = 7;
            this.RollD8.Text = "Roll!";
            this.RollD8.UseVisualStyleBackColor = true;
            this.RollD8.Click += new System.EventHandler(this.RollD8_Click);
            // 
            // RollD10
            // 
            this.RollD10.Location = new System.Drawing.Point(244, 74);
            this.RollD10.Name = "RollD10";
            this.RollD10.Size = new System.Drawing.Size(44, 23);
            this.RollD10.TabIndex = 7;
            this.RollD10.Text = "Roll!";
            this.RollD10.UseVisualStyleBackColor = true;
            this.RollD10.Click += new System.EventHandler(this.RollD10_Click);
            // 
            // RollD12
            // 
            this.RollD12.Location = new System.Drawing.Point(326, 74);
            this.RollD12.Name = "RollD12";
            this.RollD12.Size = new System.Drawing.Size(44, 23);
            this.RollD12.TabIndex = 7;
            this.RollD12.Text = "Roll!";
            this.RollD12.UseVisualStyleBackColor = true;
            this.RollD12.Click += new System.EventHandler(this.RollD12_Click);
            // 
            // RollD20
            // 
            this.RollD20.Location = new System.Drawing.Point(408, 74);
            this.RollD20.Name = "RollD20";
            this.RollD20.Size = new System.Drawing.Size(44, 23);
            this.RollD20.TabIndex = 7;
            this.RollD20.Text = "Roll!";
            this.RollD20.UseVisualStyleBackColor = true;
            this.RollD20.Click += new System.EventHandler(this.RollD20_Click);
            // 
            // RollD100
            // 
            this.RollD100.Location = new System.Drawing.Point(490, 74);
            this.RollD100.Name = "RollD100";
            this.RollD100.Size = new System.Drawing.Size(44, 23);
            this.RollD100.TabIndex = 7;
            this.RollD100.Text = "Roll!";
            this.RollD100.UseVisualStyleBackColor = true;
            this.RollD100.Click += new System.EventHandler(this.RollD100_Click);
            // 
            // Dice
            // 
            this.Dice.Controls.Add(this.RollD100);
            this.Dice.Controls.Add(this.label1);
            this.Dice.Controls.Add(this.RollD20);
            this.Dice.Controls.Add(this.label2);
            this.Dice.Controls.Add(this.RollD12);
            this.Dice.Controls.Add(this.label3);
            this.Dice.Controls.Add(this.RollD10);
            this.Dice.Controls.Add(this.label4);
            this.Dice.Controls.Add(this.RollD8);
            this.Dice.Controls.Add(this.label5);
            this.Dice.Controls.Add(this.RollD6);
            this.Dice.Controls.Add(this.label6);
            this.Dice.Controls.Add(this.RollD4);
            this.Dice.Controls.Add(this.label7);
            this.Dice.Controls.Add(this.D100Amount);
            this.Dice.Controls.Add(this.D4Amount);
            this.Dice.Controls.Add(this.D20Amount);
            this.Dice.Controls.Add(this.D6Amount);
            this.Dice.Controls.Add(this.D12Amount);
            this.Dice.Controls.Add(this.D8Amount);
            this.Dice.Controls.Add(this.D10Amount);
            this.Dice.Enabled = false;
            this.Dice.Location = new System.Drawing.Point(93, 12);
            this.Dice.Name = "Dice";
            this.Dice.Size = new System.Drawing.Size(547, 113);
            this.Dice.TabIndex = 8;
            this.Dice.TabStop = false;
            this.Dice.Text = "Dice";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 224);
            this.Controls.Add(this.Dice);
            this.Controls.Add(this.SysLog);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.Connect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.D4Amount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D6Amount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D8Amount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D10Amount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D12Amount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D20Amount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.D100Amount)).EndInit();
            this.Dice.ResumeLayout(false);
            this.Dice.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.Label TempLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox SysLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown D4Amount;
        private System.Windows.Forms.NumericUpDown D6Amount;
        private System.Windows.Forms.NumericUpDown D8Amount;
        private System.Windows.Forms.NumericUpDown D10Amount;
        private System.Windows.Forms.NumericUpDown D12Amount;
        private System.Windows.Forms.NumericUpDown D20Amount;
        private System.Windows.Forms.NumericUpDown D100Amount;
        private System.Windows.Forms.Button RollD4;
        private System.Windows.Forms.Button RollD6;
        private System.Windows.Forms.Button RollD8;
        private System.Windows.Forms.Button RollD10;
        private System.Windows.Forms.Button RollD12;
        private System.Windows.Forms.Button RollD20;
        private System.Windows.Forms.Button RollD100;
        private System.Windows.Forms.GroupBox Dice;
    }
}

