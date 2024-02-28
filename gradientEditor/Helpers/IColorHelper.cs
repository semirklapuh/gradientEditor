using System.Drawing;
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

        string FormatGradientResult(string gradientType, string gradientDirection, DataGridView dataGridView);

        DataGridView ConvertResultToFormData(DataGridView dataGridView, string resultString, ComboBox selectedType, ComboBox selectedDirection);
    }
}
