using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace gradientEditor
{
    public partial class frmGradientEditor : Form
    {
        public frmGradientEditor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = false;
            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.
        }

        //private void ColorPicker_Click(object sender, EventArgs e)
        //{
        //    ColorDialog MyDialog = new ColorDialog();
        //    Keeps the user from selecting a custom color.
        //    MyDialog.AllowFullOpen = false;
        //    Allows the user to get help. (The default is false.)
        //    MyDialog.ShowHelp = true;
        //    Sets the initial color select to the current text color.
        //    MyDialog.Color = txtBox.ForeColor;

        //    Update the text box color if the user clicks OK
        //    if (MyDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        txtBox.Text = "#" + (MyDialog.Color.ToArgb() & 0x00FFFFFF).ToString("X6");
        //    }
        //}

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = false;
            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.
            // Update the text box color if the user clicks OK 
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                //dataGridView1.Rows[e.RowIndex].Cells[0].Value = "#" + (MyDialog.Color.ToArgb() & 0x00FFFFFF).ToString("X6");
                DataGridViewRow selectedRow = dataGridView1.Rows[dataGridView1.Rows.Count - 1];
                selectedRow.Cells[0].Value = "#" + (MyDialog.Color.ToArgb() & 0x00FFFFFF).ToString("X6");

                DataGridViewRow newRow = new DataGridViewRow();

                // Add cells to the new row
                newRow.CreateCells(dataGridView1);
                dataGridView1.Rows.Add(newRow);
            }
        }
    }
}
