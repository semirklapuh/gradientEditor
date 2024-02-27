﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gradientEditor.Helpers
{
    public class ColorHelper : IColorHelper
    {
        public string ConvertColorToRGBA(Color color)
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

        public string ConvertColorToHex(Color color)
        {
            // Use ColorTranslator.ToHtml to convert Color to hex color code
            string hexColor = ColorTranslator.ToHtml(color);

            return hexColor;
        }

        public Color ConvertHexToColor(string hex)
        {
            // Remove any leading '#' character
            hex = hex.TrimStart('#');

            // Convert the hex string to an integer
            int intValue = Convert.ToInt32(hex, 16);

            // Create a Color object from the integer value
            Color color = Color.FromArgb(intValue);

            return color;
        }

        public Color ConvertRgbaToColor(string rgba)
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

        public DataGridView ConvertFromHexToRgba(DataGridView dataGridView)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells["Color"].Value != null)
                {
                    var colorValue = ConvertHexToColor(row.Cells["Color"].Value.ToString());
                    row.Cells["Color"].Value = ConvertColorToRGBA(colorValue);
                }
            }

            return dataGridView;
        }

        public DataGridView ConvertFromRgbaToHex(DataGridView dataGridView)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells["Color"].Value != null)
                {
                    var colorValue = ConvertRgbaToColor(row.Cells["Color"].Value.ToString());
                    row.Cells["Color"].Value = ConvertColorToHex(colorValue);
                }
            }

            return dataGridView;
        }

        public string ConvertColorFromRgbaToHex(string rgbaColor)
        {
            var colorValue = ConvertRgbaToColor(rgbaColor);
            var hexValue = ConvertColorToHex(colorValue);
            return hexValue;
        }

       public bool IsValidHexColor(string hexColor)
        {
            // Define a regular expression pattern for a valid hex color code
            string hexPattern = @"^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$";

            // Use Regex.IsMatch to check if the input matches the pattern
            return Regex.IsMatch(hexColor, hexPattern);
        }

        public bool IsValidRGBAColor(string rgbaColor)
        {
            // Define a regular expression pattern for a valid RGBA color code
            string rgbaPattern = @"^rgba\(\s*(\d{1,3}\s*,\s*){2}\d{1,3}\s*,\s*(\d*(\.\d+)?)\)$";

            // Use Regex.IsMatch to check if the input matches the pattern
            return Regex.IsMatch(rgbaColor, rgbaPattern);
        }
    }
}
