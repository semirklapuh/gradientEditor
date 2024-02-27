using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gradientEditor.Helpers
{
    public interface IColorHelper
    {
        string ConvertColorToRGBA(Color color);

        string ConvertColorToHex(Color color);

        Color ConvertHexToColor(string hex);

        Color ConvertRgbaToColor(string rgba);

        DataGridView ConvertFromHexToRgba(DataGridView dataGridView);

        DataGridView ConvertFromRgbaToHex(DataGridView dataGridView);

        string ConvertColorFromRgbaToHex(string rgbaColor);

        bool IsValidHexColor(string hexColor);

        bool IsValidRGBAColor(string rgbaColor);
    }
}
