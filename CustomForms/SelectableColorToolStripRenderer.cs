using System.Drawing;
using System.Windows.Forms;

namespace CustomForms
{
    /// <summary>
    /// A class that inherits ToolStripProfessionalRenderer to override colors based on whats selected, is currently incomplete.
    /// </summary>
    public class SelectableColorToolStripRenderer : ToolStripProfessionalRenderer
    {
        public Color ArrowColor { get; set; }

        public new SelectableColorTable ColorTable { get; set; }

        public SelectableColorToolStripRenderer() : base(new SelectableColorTable())
        {
            ArrowColor = Color.White;
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            if (e.Item is ToolStripMenuItem toolStripMenuItem) e.ArrowColor = ArrowColor;
            base.OnRenderArrow(e);
        }

        // Fix border bug on toolstrips
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            
        }
    }
}
