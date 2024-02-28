using gradientEditor.Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace gradientEditor
{
    public partial class formTest : Form
    {
        public bool isAlertOpen = false;

        private readonly IColorHelper _colorHelper;
        public formTest(IColorHelper colorHelper)
        {
            InitializeComponent();
            _colorHelper = colorHelper;
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

            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.Name = "Color picker";
            buttonColumn.Text = "Pick";
            buttonColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(buttonColumn);

            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;

            dataGridView1.RowHeadersVisible = true;
            dataGridView1.RowPostPaint += dataGridView1_RowPostPaint;

            DataGridViewRow newRow = new DataGridViewRow();
            newRow.CreateCells(dataGridView1);
            dataGridView1.Rows.Add(newRow);

            txtResult.TextAlign = HorizontalAlignment.Left;
            txtResult.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Color picker"].Index && e.RowIndex >= 0)
            {
                ColorDialog MyDialog = new ColorDialog();
                MyDialog.AllowFullOpen = false;
                MyDialog.ShowHelp = true;
                string newText = "";
                if (MyDialog.ShowDialog() == DialogResult.OK)
                {

                    if (radioBtnHex.Checked)
                    {
                        newText = "#" + (MyDialog.Color.ToArgb() & 0x00FFFFFF).ToString("X6");
                    }
                    else
                    {
                        newText = _colorHelper.ConvertColorToRGBA(MyDialog.Color);
                    }
                }

                dataGridView1.Rows[e.RowIndex].Cells["Color"].Value = newText;
                dataGridView1.Rows[e.RowIndex].Cells["Color"].Style.BackColor = MyDialog.Color;

                var currentValue = dataGridView1.Rows[e.RowIndex].Cells["ColorStop"].Value;
                dataGridView1.Rows[e.RowIndex].Cells["ColorStop"].Value =
                    currentValue == null ? "50%" : currentValue;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            isAlertOpen = false;
            if (!ValidateColor(dataGridView1.Rows[e.RowIndex].Cells["Color"].Value.ToString()))
            {
                dataGridView1.Rows[e.RowIndex].Cells["Color"].Value = "";
                dataGridView1.Rows[e.RowIndex].Cells["ColorStop"].Value = "";

                ShowAlert();
                return;
            }
            isAlertOpen = false;
            if (e.RowIndex == dataGridView1.Rows.Count - 1)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridView1);
                dataGridView1.Rows.Add(newRow);
            }

            FormatResult(dataGridView1);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, brush, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void FormatResult(DataGridView dataGridView)
        {
            string gradientType = $"{cbType.SelectedItem}-gradient";
            string gradientDirection = $"{cbDirections.SelectedItem}";

            var gradientResult = _colorHelper.FormatGradientResult(gradientType, gradientDirection, dataGridView);

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

        private void radioBtnRgba_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnHex.Checked)
            {
                _colorHelper.ConvertFromRgbaToHex(dataGridView1);
            }
            else
            {
                _colorHelper.ConvertFromHexToRgba(dataGridView1);
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
                return _colorHelper.IsValidHexColor(color);
            }
            else
            {
                return _colorHelper.IsValidRGBAColor(color);
            }
        }

        private void ShowAlert()
        {
            if (!isAlertOpen)
            {
                using (frmAlert dialog = new frmAlert())
                {
                    DialogResult result = dialog.ShowDialog();
                    isAlertOpen = true;
                }
            }
        }

        private void readResult()
        {
            dataGridView1 = _colorHelper.ConvertResultToFormData(dataGridView1, txtResult.Text, this.cbType, this.cbDirections);
        }

        private void txtResult_TextChanged(object sender, EventArgs e)
        {
            readResult();
            previewUpdate();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.Value != null && ValidateColor(dataGridView1.CurrentCell.Value.ToString()) && radioBtnHex.Checked)
            {
                dataGridView1.CurrentCell.Style.BackColor = ColorTranslator.FromHtml(dataGridView1.CurrentCell.Value.ToString());
            }
            else if (dataGridView1.CurrentCell.Value != null && ValidateColor(dataGridView1.CurrentCell.Value.ToString()) && radioBtnRgba.Checked)
            {
                dataGridView1.CurrentCell.Style.BackColor = ColorTranslator.FromHtml(_colorHelper.ConvertColorFromRgbaToHex(dataGridView1.CurrentCell.Value.ToString()));
            }
        }
    }
}