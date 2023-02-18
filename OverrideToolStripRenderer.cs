using System.Drawing;
using System.Windows.Forms;

namespace ACFAModelReplacer
{
    internal class OverrideToolStripRenderer : ToolStripProfessionalRenderer
    {
        public OverrideToolStripRenderer() : base(new OverrideMenuStripSelectedColorTable()) {}
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            var tsMenuItem = e.Item as ToolStripMenuItem;
            if (tsMenuItem != null)
                e.ArrowColor = Color.White;
            base.OnRenderArrow(e);
        }
        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            
            base.OnRenderItemCheck(e);
        }
    }
}
