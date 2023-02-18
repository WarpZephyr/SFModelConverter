using System.Drawing;
using System.Windows.Forms;

namespace ACFAModelReplacer
{
    public class OverrideMenuStripSelectedColorTable : ProfessionalColorTable
    {
        // Selected Main Menu Item Outer Border
        public override Color MenuBorder
        {
            get { return Color.FromArgb(90, 90, 90); }
        }

        // Highlighted item in Main Menu Item
        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(82, 82, 82); }
        }

        // Highlighted Item borders
        public override Color MenuItemBorder
        {
            get { return Color.FromArgb(90, 90, 90); }
        }

        // Hovered over Main Menu Item Gradient Top
        public override Color MenuItemSelectedGradientBegin
        {
            get { return ColorTranslator.FromHtml("#4B4B4B"); }
        }

        // Hovered over Main Menu Item Gradient Bottom
        public override Color MenuItemSelectedGradientEnd
        {
            get { return ColorTranslator.FromHtml("#4B4B4B"); }
        }

        // Opened Main Menu Item Gradient Top
        public override Color MenuItemPressedGradientBegin
        {
            get { return ColorTranslator.FromHtml("#4B4B4B"); }
        }

        // Opened Main Menu Item Gradient Bottom
        public override Color MenuItemPressedGradientEnd
        {
            get { return ColorTranslator.FromHtml("#4B4B4B"); }
        }

        // Opened Main Menu Strip Item Inner Border
        public override Color ToolStripDropDownBackground
        {
            get { return ColorTranslator.FromHtml("#4B4B4B"); }
        }


        // Gradient top of unused image that gets turned into a line beside menustrip items
        public override Color ImageMarginGradientBegin
        {
            get { return ColorTranslator.FromHtml("#4B4B4B"); }
        }

        // Gradient bottom of unused image that gets turned into a line beside menustrip items
        public override Color ImageMarginGradientEnd
        {
            get { return ColorTranslator.FromHtml("#4B4B4B"); }
        }
    }
}
