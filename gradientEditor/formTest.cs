using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace gradientEditor
{
    public partial class formTest : Form
    {
        public formTest()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeComboBoxType();
            InitializeComboBoxDirection();
            InitializeRadioBtnColorFormat();
            this.webView21.Source = new System.Uri(Directory.GetCurrentDirectory() + "/html-resources/index.html", System.UriKind.Absolute);
        }

        private void InitializeRadioBtnColorFormat()
        {
           radioBtnHex.Checked = true;
        }

        private void InitializeComboBoxDirection()
        {
            cbDirections.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void InitializeComboBoxType()
        {
            cbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbType.Items.AddRange(new object[] { "linear", "radial" });
            cbType.SelectedIndex = 0;
        }

        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Add("Color", "Color");
            dataGridView1.Columns.Add("ColorStop", "Color stop");

            // Add a button column with a button in each cell
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.Name = "Color picker";
            buttonColumn.Text = "Pick";
            buttonColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(buttonColumn);

            // Allow adding new rows
            dataGridView1.AllowUserToAddRows = false;

            // Handle button click event
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;

            dataGridView1.RowHeadersVisible = true;
            // Subscribe to the RowPostPaint event
            dataGridView1.RowPostPaint += dataGridView1_RowPostPaint;


            //// Get the text to insert (you can use any method to get the text)
            //string newText = "Inserted Text";


            // Add a new empty row
            DataGridViewRow newRow = new DataGridViewRow();
            newRow.CreateCells(dataGridView1);
            dataGridView1.Rows.Add(newRow);

            //Add result dummy value 
            txtResult.TextAlign = HorizontalAlignment.Left;
            //txtResult.TextAlign = Vertical;
            txtResult.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the button column is clicked
            if (e.ColumnIndex == dataGridView1.Columns["Color picker"].Index && e.RowIndex >= 0)
            {
                ColorDialog MyDialog = new ColorDialog();
                // Keeps the user from selecting a custom color.
                MyDialog.AllowFullOpen = false;
                // Allows the user to get help. (The default is false.)
                MyDialog.ShowHelp = true;
                // Sets the initial color select to the current text color.
                // Update the text box color if the user clicks OK 
                string newText = "";
                if (MyDialog.ShowDialog() == DialogResult.OK)
                { 
                    newText = "#" + (MyDialog.Color.ToArgb() & 0x00FFFFFF).ToString("X6");
                }

                // Insert the text and change the color of the first column
                dataGridView1.Rows[e.RowIndex].Cells["Color"].Value = newText;
                dataGridView1.Rows[e.RowIndex].Cells["Color"].Style.BackColor = MyDialog.Color;

                //TODO: ask someone
                // Set a default value for color stop

                var currentValue = dataGridView1.Rows[e.RowIndex].Cells["ColorStop"].Value;
                dataGridView1.Rows[e.RowIndex].Cells["ColorStop"].Value =
                    currentValue == null ? "50%" : currentValue;

                //// Add a new empty row
                //DataGridViewRow newRow = new DataGridViewRow();
                //newRow.CreateCells(dataGridView1);
                //dataGridView1.Rows.Add(newRow);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == dataGridView1.Rows.Count -1)
            {
                // Add a new empty row
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridView1);
                dataGridView1.Rows.Add(newRow);
            }

            FormatResult(dataGridView1);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Assuming dataGridView1 is the name of your DataGridView control
            // Draw row numbers in the row headers
            using (SolidBrush brush = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, brush, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void FormatResult(DataGridView dataGridView)
        {
            string gradientResult = "";
            string gradientType = "linear-gradient";
            string gradientDirection = "to bottom";

            gradientResult = gradientType + "(" + gradientDirection + ",";

            foreach (DataGridViewRow row in dataGridView.Rows) {
                if (row.Cells["Color"].Value != null && row.Cells["ColorStop"].Value != null) {
                    var colorValue = row.Cells["Color"].Value.ToString();
                    var colorStop = row.Cells["ColorStop"].Value.ToString();

                    gradientResult += colorValue + " " + colorStop + ",";
                }
            }

            gradientResult = gradientResult.Substring(0, gradientResult.Length - 1);
            gradientResult += ")";

            txtResult.Text = gradientResult;

        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbType.SelectedItem.ToString() == "linear")
            {
                cbDirections.Items.Clear();
                cbDirections.Items.AddRange(new object[] { "to bottom", "to top",
                "to right", "to left", "to bottom right", "to bottom left", "to top right", "to top left" });
                cbDirections.SelectedIndex = 0;
            }
            else
            {
                cbDirections.Items.Clear();
                cbDirections.Items.AddRange(new object[] { "circle at center", "circle at center top","circle at top left",
                "circle at top right", "circle at center bottom", "circle at left", "circle at right",
                "circle at bottom right", "circle at bottom left"});
                cbDirections.SelectedIndex = 0;
            }
        }

        private void previewUpdate()
        {
            this.webView21.ExecuteScriptAsync($"document.getElementById('preview_space').style['background'] = '" + txtResult.Text + "';");
        }

        private void readResultValues()
        {
            Match gradientTypeMatch = Regex.Match(txtResult.Text, @"^.*?(?=-)");
            Match gradientDirectionMatch = Regex.Match(txtResult.Text, @"(?<=\()(.*?)(?=,)");
            MatchCollection colorsWithStopsMatch= Regex.Matches(txtResult.Text, @"[^,\s][^\,]*[^,\s]*"); //starts from the 2nd element

            this.cbType.SelectedItem = gradientTypeMatch?.Value;
            this.cbDirections.SelectedItem = gradientDirectionMatch?.Value;

            try
            {
                int index = 0;
                foreach (Match match in colorsWithStopsMatch)
                {
                    if (index > 0) // don't take the first element 
                    {
                        string[] colorInfo = match.Value.Split(' ');
                        // Insert the text and change the color of the first column
                        dataGridView1.Rows[index - 1].Cells["Color"].Value = colorInfo[0];
                        // dataGridView1.Rows[match.Index - 1].Cells["Color"].Style.BackColor = colorMatch.Value;
                        dataGridView1.Rows[index - 1].Cells["ColorStop"].Value = colorInfo[1].Replace(")",""); //tha last value contains bracket (TODO: idea, fix it with regex)
                    }
                    index++;
                }
            }
            catch (RegexMatchTimeoutException) { }
        }

        private void txtResult_TextChanged(object sender, EventArgs e)
        {
            readResultValues();
            //Change the backround of preview on every result change
            previewUpdate();
        }
    }
}
