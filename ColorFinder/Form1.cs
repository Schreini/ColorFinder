using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorFinder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void TxtInput_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var vals = TxtInput.Text.Split(new[]{" "}, StringSplitOptions.RemoveEmptyEntries).ToList();
                vals.Add("255");
                vals.Add("255");
                int r = Convert.ToInt32(vals[0]);
                int g = Convert.ToInt32(vals[1]);
                int b = Convert.ToInt32(vals[2]);

                var allColors = Enum.GetValues(typeof (KnownColor))
                    .Cast<KnownColor>()
                    .Select(x => new ColorDiff() {Color = Color.FromKnownColor(x), Diff = 1000})
                    .ToList();

                foreach (var colorDiff in allColors)
                {
                      colorDiff.Diff = Math.Abs(colorDiff.Color.R - r)
                                     + Math.Abs(colorDiff.Color.G - g)
                                     + Math.Abs(colorDiff.Color.B - b);
                }

                allColors = allColors.OrderBy(x => x.Diff).ToList();
                TxtOutput.Clear();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("Diff\tR\tG\tB\tName"));
                foreach (var cd in allColors)
                {
                    sb.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}", cd.Diff, cd.Color.R, cd.Color.G, cd.Color.B, cd.Color.Name));
                }
                TxtOutput.Text = sb.ToString();
                TxtInput.BackColor = Color.LightGreen;
            }
            catch (Exception ex)
            {
                TxtInput.BackColor = Color.LightCoral;
                TxtOutput.Clear();
                TxtOutput.Text = ex.ToString();
            }
        }
    }

    public class ColorDiff
    {
        public Color Color;
        public int Diff;
    }
}
