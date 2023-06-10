using System.Drawing;
using System.Windows.Forms;

namespace CustomForms
{
    /// <summary>
    /// A class that inherits ProfessionalColorTable to override colors based on whats selected.
    /// </summary>
    public class SelectableColorTable : ProfessionalColorTable
    {
        public Color SelectedMenuBorder { get; set; }
        public Color SelectedMenuItem { get; set; }
        public Color SelectedMenuItemBorder { get; set; }
        public Color SelectedGradientTopMenu { get; set; }
        public Color SelectedGradientTopMenuItem { get; set; }
        public Color SelectedGradientBottomMenu { get; set; }
        public Color SelectedGradientBottomMenuItem { get; set; }
        public Color BackgroundDropDown { get; set; }
        public Color GradientTopImageMargin { get; set; }
        public Color GradientBottomImageMargin { get; set; }

        internal static Color DefaultDark = Color.FromArgb(40, 40, 40);
        internal static Color DefaultDarkSelected = Color.FromArgb(50, 50, 50);
        internal static Color DefaultDarkGradientFlatSelected = ColorTranslator.FromHtml("#282828");
        internal static Color DefaultDarkBackground = ColorTranslator.FromHtml("#282828");

        public SelectableColorTable()
        {
            SelectedMenuBorder = DefaultDark;
            SelectedMenuItem = DefaultDarkSelected;
            SelectedMenuItemBorder = DefaultDark;
            SelectedGradientTopMenu = DefaultDarkGradientFlatSelected;
            SelectedGradientTopMenuItem = DefaultDarkGradientFlatSelected;
            SelectedGradientBottomMenu = DefaultDarkGradientFlatSelected;
            SelectedGradientBottomMenuItem = DefaultDarkGradientFlatSelected;
            BackgroundDropDown = DefaultDarkBackground;
            GradientTopImageMargin = DefaultDarkGradientFlatSelected;
            GradientBottomImageMargin = DefaultDarkGradientFlatSelected;
        }

        /// <summary>
        /// Sets colors back to the default dark colors.
        /// </summary>
        public void SetDefaultDark()
        {
            SelectedMenuBorder = DefaultDark;
            SelectedMenuItem = DefaultDarkSelected;
            SelectedMenuItemBorder = DefaultDark;
            SelectedGradientTopMenu = DefaultDarkGradientFlatSelected;
            SelectedGradientTopMenuItem = DefaultDarkGradientFlatSelected;
            SelectedGradientBottomMenu = DefaultDarkGradientFlatSelected;
            SelectedGradientBottomMenuItem = DefaultDarkGradientFlatSelected;
            BackgroundDropDown = DefaultDarkBackground;
            GradientTopImageMargin = DefaultDarkGradientFlatSelected;
            GradientBottomImageMargin = DefaultDarkGradientFlatSelected;
        }

        public override Color MenuBorder
        {
            get { return SelectedMenuBorder; }
        }

        public override Color MenuItemSelected
        {
            get { return SelectedMenuItem; }
        }

        public override Color MenuItemBorder
        {
            get { return SelectedMenuItemBorder; }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return SelectedGradientTopMenuItem; }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return SelectedGradientBottomMenuItem; }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return SelectedGradientTopMenu; }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return SelectedGradientBottomMenu; }
        }

        public override Color ToolStripDropDownBackground
        {
            get { return BackgroundDropDown; }
        }

        public override Color ImageMarginGradientBegin
        {
            get { return GradientTopImageMargin; }
        }

        public override Color ImageMarginGradientEnd
        {
            get { return GradientBottomImageMargin; }
        }
    }
}
