using Syncfusion.Lic.util.encoders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public bool isAlertOpen = false;
        public formTest()
        {
            InitializeComponent();
            InitializeComboBoxType();
            InitializeComboBoxDirection();
            InitializeDataGridView();
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

                    if (radioBtnHex.Checked)
                    {
                        newText = "#" + (MyDialog.Color.ToArgb() & 0x00FFFFFF).ToString("X6");
                    }
                    else
                    {
                        newText = ConvertColorToRGBA(MyDialog.Color);
                    }
                }

                // Insert the text and change the color of the first column
                dataGridView1.Rows[e.RowIndex].Cells["Color"].Value = newText;
                dataGridView1.Rows[e.RowIndex].Cells["Color"].Style.BackColor = MyDialog.Color;

                //TODO: ask someone
                // Set a default value for color stop
                var currentValue = dataGridView1.Rows[e.RowIndex].Cells["ColorStop"].Value;
                dataGridView1.Rows[e.RowIndex].Cells["ColorStop"].Value =
                    currentValue == null ? "50%" : currentValue;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            isAlertOpen = false;
            if (dataGridView1.Rows[e.RowIndex].Cells["Color"].Value == null || !ValidateColor(dataGridView1.Rows[e.RowIndex].Cells["Color"].Value.ToString()))
            {
                dataGridView1.Rows[e.RowIndex].Cells["Color"].Value = "";
                dataGridView1.Rows[e.RowIndex].Cells["ColorStop"].Value = "";
                dataGridView1.Rows[e.RowIndex].Cells["Color"].Style.BackColor = Color.White;

                ShowAlert();
                return;
            }
            //isAlertOpen = false;
            if (e.RowIndex == dataGridView1.Rows.Count - 1)
            {
                // Add a new empty row
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridView1);
                dataGridView1.Rows.Add(newRow);
            }

            //dataGridView1.Rows[e.RowIndex].Cells["Color"].Style.BackColor
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
            string gradientType = $"{cbType.SelectedItem}-gradient";
            string gradientDirection = $"{cbDirections.SelectedItem}";
            List<string> colorCounter = new List<string>();

            gradientResult = gradientType + "(" + gradientDirection + ",";

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells["Color"].Value != null && row.Cells["ColorStop"].Value != null)
                {
                    var colorValue = row.Cells["Color"].Value.ToString();
                    var colorStop = row.Cells["ColorStop"].Value.ToString();
                    colorCounter.Add(colorValue);
                    gradientResult += colorValue + " " + colorStop + ",";
                }
            }

            gradientResult = gradientResult.Substring(0, gradientResult.Length - 1);
            gradientResult += ")";

            if (colorCounter.Count() >= 2)
            {
                txtResult.Text = gradientResult;
            }
            else
            {
                txtResult.Text = "";
            }
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
            FormatResult(dataGridView1);
        }

        private void previewUpdate()
        {
            this.webView21.ExecuteScriptAsync($"document.getElementById('preview_space').style['background'] = '" + txtResult.Text + "';");
        }

        private void cbDirections_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormatResult(dataGridView1);
        }

        private string ConvertColorToRGBA(Color color)
        {
            int red = color.R;
            int green = color.G;
            int blue = color.B;
            int alpha = color.A;

            // Normalize alpha to a float value between 0.0 and 1.0
            //float alphaNormalized = alpha / 255.0f;
            float alphaNormalized = 1;

            // Format the RGBA string
            string rgbaFormat = $"rgba({red}, {green}, {blue}, {alphaNormalized:F2})";

            return rgbaFormat;
        }

        private string ConvertColorToHex(Color color)
        {
            // Use ColorTranslator.ToHtml to convert Color to hex color code
            string hexColor = ColorTranslator.ToHtml(color);

            return hexColor;
        }

        private Color ConvertHexToColor(string hex)
        {
            // Remove any leading '#' character
            hex = hex.TrimStart('#');

            // Convert the hex string to an integer
            int intValue = Convert.ToInt32(hex, 16);

            // Create a Color object from the integer value
            Color color = Color.FromArgb(intValue);

            return color;
        }

        private Color ConvertRgbaToColor(string rgba)
        {
            // Remove "rgba(" and ")" and split the components
            string[] components = rgba.Replace("rgba(", "").Replace(")", "").Split(',');

            // Parse the components and convert to integers
            int alpha = (int)(Convert.ToDouble(components[3]) * 255); // Normalize alpha to 0-255 range
            int red = Convert.ToInt32(components[0].Trim());
            int green = Convert.ToInt32(components[1].Trim());
            int blue = Convert.ToInt32(components[2].Trim());

            // Create a Color object from the components
            Color color = Color.FromArgb(alpha, red, green, blue);

            return color;
        }

        private void ConvertFromHexToRgba(DataGridView dataGridView)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells["Color"].Value != null)
                {
                    var colorValue = ConvertHexToColor(row.Cells["Color"].Value.ToString());
                    row.Cells["Color"].Value = ConvertColorToRGBA(colorValue);
                }
            }
        }

        private void ConvertFromRgbaToHex(DataGridView dataGridView)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells["Color"].Value != null)
                {
                    var colorValue = ConvertRgbaToColor(row.Cells["Color"].Value.ToString());
                    row.Cells["Color"].Value = ConvertColorToHex(colorValue);
                }
            }
        }

        private void radioBtnRgba_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnHex.Checked)
            {
                ConvertFromRgbaToHex(dataGridView1);
            }
            else
            {
                ConvertFromHexToRgba(dataGridView1);
            }
        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            FormatResult(dataGridView1);
        }

        private bool ValidateColor(string color)
        {
            if (radioBtnHex.Checked)
            {
                return IsValidHexColor(color);
            }
            else
            {
                return IsValidRGBAColor(color);
            }
        }

        bool IsValidHexColor(string hexColor)
        {
            // Define a regular expression pattern for a valid hex color code
            string hexPattern = @"^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$";

            // Use Regex.IsMatch to check if the input matches the pattern
            return Regex.IsMatch(hexColor, hexPattern);
        }

        bool IsValidRGBAColor(string rgbaColor)
        {
            // Define a regular expression pattern for a valid RGBA color code
            string rgbaPattern = @"^rgba\(\s*(\d{1,3}\s*,\s*){2}\d{1,3}\s*,\s*(\d*(\.\d+)?)\)$";

            // Use Regex.IsMatch to check if the input matches the pattern
            return Regex.IsMatch(rgbaColor, rgbaPattern);
        }


        private void ShowAlert()
        {
            // Check if the dialog has not been shown yet
            if (!isAlertOpen)
            {
                // Show the dialog
                using (frmAlert dialog = new frmAlert())
                {
                    // Optionally handle the dialog result or any other logic
                    DialogResult result = dialog.ShowDialog();
                    // ...

                    // Set the flag to indicate that the dialog has been shown
                    isAlertOpen = true;
                }
                private void readResult()
                {
                    // Define regex patterns
                    string gradientTypePattern = @"^(linear|radial)";
                    string directionPattern = @"(?<=\().+?(?=,)";
                    string colorsPattern = @"(?:#(?:[0-9a-fA-F]{3}){1,2}|rgba\([^)]+\))\s+\d+%";

                    Match typeMatch = Regex.Match(txtResult.Text, gradientTypePattern);
                    Match directionMatch = Regex.Match(txtResult.Text, directionPattern);
                    MatchCollection colors = Regex.Matches(txtResult.Text, colorsPattern);

                    this.cbType.SelectedItem = typeMatch?.Value;
                    this.cbDirections.SelectedItem = directionMatch?.Value;

                    bool isHex = txtResult.Text.Contains("#");

                    if (isHex)
                    {
                        //this.radioBtnHex.Checked = true;
                    }
                    else
                    {
                        //this.radioBtnRgba.Checked = true;
                    }

                    var index = 0;
                    foreach (Match color in colors)
                    {

                        string colorValue = color.Value;
                        string hexColorPattern = @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})";
                        string rgbaColorPattern = @"rgba\((\d+,\s*\d+,\s*\d+,\s*\d+\.\d+)\)";

                        Match colorMatch;
                        if (isHex)
                        {
                            colorMatch = Regex.Match(colorValue, hexColorPattern);
                        }
                        else
                        {
                            colorMatch = Regex.Match(rgbaColorPattern, hexColorPattern);
                        }

                        if (colorMatch.Success)
                        {
                            dataGridView1.Rows[index].Cells["Color"].Value = colorMatch?.Value;
                        }

                        // Extract the percentage
                        string percentagePattern = @" (\S+)$"; // Percentage pattern
                        Match percentageMatch = Regex.Match(colorValue, percentagePattern.Replace(" ", ""));

                        if (percentageMatch.Success)
                        {
                            dataGridView1.Rows[index].Cells["ColorStop"].Value = percentageMatch.Value;
                        }
                        index++;

                    }

                }

                private void txtResult_TextChanged(object sender, EventArgs e)
                {
                    readResult();
                    //Change the backround of preview on every result text change
                    previewUpdate();
                }
            }
        }
    }
}