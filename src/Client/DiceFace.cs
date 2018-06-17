using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using Color = System.Drawing.Color;


namespace TheQ.DiceRoller.Client
{
    public partial class DiceFace : UserControl
    {
        public DiceFace()
        {
            InitializeComponent();

            this.Number.Parent = this.Face;
        }


        public void SetNumber(DiceType type, int number)
        {
            this.Number.BackColor = Color.Transparent;
            this.Face.LoadAsync(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), $"Media\\D{(int) type}.png"));
            var font = this.Number.Font;
            this.Number.Font = new Font(font.FontFamily, this.GetHeight(type), FontStyle.Bold, GraphicsUnit.Pixel);
            this.Number.Text = number.ToString();
        }

        private int GetHeight(DiceType type)
        {
            switch (type)
            {
                case DiceType.D4:
                    this.Number.ForeColor = Color.FromArgb(160, 16, 160);
                    return (int) (this.Height / 2.3);
                case DiceType.D6:
                    this.Number.ForeColor = Color.FromArgb(16, 96, 160);
                    return this.Height / 2;
                case DiceType.D8:
                    this.Number.ForeColor = Color.FromArgb(64, 64, 64);
                    return (int) (this.Height / 2.3);
                case DiceType.D10:
                    this.Number.ForeColor = Color.FromArgb(160, 64, 16);
                    return (int) (this.Height / 3.5);
                case DiceType.D12:
                    this.Number.ForeColor = Color.FromArgb(16, 160, 64);
                    return (int) (this.Height / 2.5);
                case DiceType.D20:
                    this.Number.ForeColor = Color.FromArgb(64, 16, 160);
                    return this.Height / 5;
                case DiceType.D100:
                    this.Number.ForeColor = Color.FromArgb(160, 16, 16);
                    return this.Height / 5;
            }

            return this.Height / 4;
       }
    }
}