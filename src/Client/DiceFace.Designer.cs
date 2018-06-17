namespace TheQ.DiceRoller.Client
{
    partial class DiceFace
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Face = new System.Windows.Forms.PictureBox();
            this.Number = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Face)).BeginInit();
            this.SuspendLayout();
            // 
            // Face
            // 
            this.Face.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Face.BackColor = System.Drawing.Color.Transparent;
            this.Face.Location = new System.Drawing.Point(0, 0);
            this.Face.Name = "Face";
            this.Face.Size = new System.Drawing.Size(150, 150);
            this.Face.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Face.TabIndex = 0;
            this.Face.TabStop = false;
            // 
            // Number
            // 
            this.Number.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Number.BackColor = System.Drawing.Color.Transparent;
            this.Number.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.Number.Location = new System.Drawing.Point(5, 0);
            this.Number.Name = "Number";
            this.Number.Size = new System.Drawing.Size(141, 150);
            this.Number.TabIndex = 1;
            this.Number.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DiceFace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.Number);
            this.Controls.Add(this.Face);
            this.Name = "DiceFace";
            ((System.ComponentModel.ISupportInitialize)(this.Face)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Face;
        private System.Windows.Forms.Label Number;
    }
}
