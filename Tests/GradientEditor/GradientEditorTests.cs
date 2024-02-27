using gradientEditor.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tests.GradientEditor
{
    [TestClass]
    public class GradientEditorTests
    {
        [TestMethod]
        public void ConvertColorToRGBA_RedColor_ReturnsRgbaColorRed()
        {
            //arrange
            IColorHelper colorHelper = new ColorHelper();
            var expectedColor = "rgba(255, 0, 0, 1.00)";

            //act
            var rgbaResult = colorHelper.ConvertColorToRGBA(Color.Red);

            //assert
            Assert.AreEqual(expectedColor, rgbaResult);
        }

        [TestMethod]
        public void ConvertColorToHex_RedColor_ReturnsHexColorRed()
        {
            //arrange
            IColorHelper colorHelper = new ColorHelper();
            var expectedColor = "Red";

            //act
            var hexResult = colorHelper.ConvertColorToHex(Color.Red);

            //assert
            Assert.AreEqual(expectedColor, hexResult);
        }

        [TestMethod]
        public void ConvertHexToColor_HexRedColor_ReturnsColorRed()
        {
            //arrange
            IColorHelper colorHelper = new ColorHelper();
            var hexColor = "#FF0000";
            var expectedColor = Color.FromArgb(0, 255, 0, 0);

            //act
            var colorResult = colorHelper.ConvertHexToColor(hexColor);

            //assert
            Assert.AreEqual(expectedColor, colorResult);
        }

        [TestMethod]
        public void ConvertRgbaToColor_RgbaRedColor_ReturnsColorRed()
        {
            //arrange
            IColorHelper colorHelper = new ColorHelper();
            var rgbaColor = "rgba(255, 0, 0, 1.00)";
            var expectedColor = Color.FromArgb(255, 255, 0, 0);

            //act
            var colorResult = colorHelper.ConvertRgbaToColor(rgbaColor);

            //assert
            Assert.AreEqual(expectedColor, colorResult);
        }

        [TestMethod]
        public void ConvertColorFromRgbaToHex_RgbaRedColor_ReturnsHexRed()
        {
            //arrange
            IColorHelper colorHelper = new ColorHelper();
            var rgbaColor = "rgba(255, 0, 0, 1.00)";
            var expectedColor = "#FF0000";

            //act
            var colorResult = colorHelper.ConvertColorFromRgbaToHex(rgbaColor);

            //assert
            Assert.AreEqual(expectedColor, colorResult);
        }

        [TestMethod]
        public void ConvertFromHexToRgba_HexTwoColors_ReturnsTwoRgbaColors()
        {
            //arrange
            IColorHelper colorHelper = new ColorHelper();
            var hexColor1 = "#FF0000";
            var hexColor2 = "#0000FF";
            var expectedRgbaColor1 = "rgba(255, 0, 0, 1.00)";
            var expectedRgbaColor2 = "rgba(0, 0, 255, 1.00)";
            var dataGridView = new DataGridView();
            var colorColumn = new DataGridViewTextBoxColumn();
            colorColumn.HeaderText = "Color";
            colorColumn.Name = "Color";
            dataGridView.Columns.Add(colorColumn);

            DataGridViewRow row1 = new DataGridViewRow();
            var colorCell = new DataGridViewTextBoxCell();
            colorCell.Value = hexColor1;
            row1.Cells.Add(colorCell);
            dataGridView.Rows.Add(row1);

            DataGridViewRow row2 = new DataGridViewRow();
            var colorCell2 = new DataGridViewTextBoxCell();
            colorCell2.Value = hexColor2;
            row2.Cells.Add(colorCell2);
            dataGridView.Rows.Add(row2);

            //act
            var colorResultView = colorHelper.ConvertFromHexToRgba(dataGridView);

            //assert
            Assert.AreEqual(expectedRgbaColor1, colorResultView.Rows[0].Cells["Color"].Value.ToString());
            Assert.AreEqual(expectedRgbaColor2, colorResultView.Rows[1].Cells["Color"].Value.ToString());
        }

        [TestMethod]
        public void ConvertFromRgbaToHex_RgbaTwoColors_ReturnsTwoHexColors()
        {
            //arrange
            IColorHelper colorHelper = new ColorHelper();
            var hexColor1 = "rgba(255, 0, 0, 1.00)";
            var hexColor2 = "rgba(0, 0, 255, 1.00)";
            var expectedRgbaColor1 = "#FF0000";
            var expectedRgbaColor2 = "#0000FF";
            var dataGridView = new DataGridView();
            var colorColumn = new DataGridViewTextBoxColumn();
            colorColumn.HeaderText = "Color";
            colorColumn.Name = "Color";
            dataGridView.Columns.Add(colorColumn);

            DataGridViewRow row1 = new DataGridViewRow();
            var colorCell = new DataGridViewTextBoxCell();
            colorCell.Value = hexColor1;
            row1.Cells.Add(colorCell);
            dataGridView.Rows.Add(row1);

            DataGridViewRow row2 = new DataGridViewRow();
            var colorCell2 = new DataGridViewTextBoxCell();
            colorCell2.Value = hexColor2;
            row2.Cells.Add(colorCell2);
            dataGridView.Rows.Add(row2);

            //act
            var colorResultView = colorHelper.ConvertFromRgbaToHex(dataGridView);

            //assert
            Assert.AreEqual(expectedRgbaColor1, colorResultView.Rows[0].Cells["Color"].Value.ToString());
            Assert.AreEqual(expectedRgbaColor2, colorResultView.Rows[1].Cells["Color"].Value.ToString());
        }

        [TestMethod]
        public void IsValidHexColor_HexColor_ReturnsTrue()
        {
            //arrange
            IColorHelper colorHelper = new ColorHelper();
            var hexColor = "#FF0000";

            //act
            var colorResult = colorHelper.IsValidHexColor(hexColor);

            //assert
            Assert.AreEqual(true, colorResult);
        }

        [TestMethod]
        public void IsValidHexColor_NotHexColor_ReturnsFalse()
        {
            //arrange
            IColorHelper colorHelper = new ColorHelper();
            var hexColor = "rgba(255, 0, 0, 1.00)";

            //act
            var colorResult = colorHelper.IsValidHexColor(hexColor);

            //assert
            Assert.AreEqual(false, colorResult);
        }

        [TestMethod]
        public void IsValidRGBAColor_RgbaColor_ReturnsTrue()
        {
            //arrange
            IColorHelper colorHelper = new ColorHelper();
            var rgbaColor = "rgba(255, 0, 0, 1.00)";

            //act
            var colorResult = colorHelper.IsValidRGBAColor(rgbaColor);

            //assert
            Assert.AreEqual(true, colorResult);
        }

        [TestMethod]
        public void IsValidRGBAColor_RgbaColor_ReturnsFalse()
        {
            //arrange
            IColorHelper colorHelper = new ColorHelper();
            var rgbaColor = "#FF0000";

            //act
            var colorResult = colorHelper.IsValidRGBAColor(rgbaColor);

            //assert
            Assert.AreEqual(false, colorResult);
        }
    }
}
