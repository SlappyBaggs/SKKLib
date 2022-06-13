using System;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace SKKLib.Printing
{
    public static class SKKPrintController
    {
        private static PrintDocument printDocument_ = null;
        private static PrintDialog printDialog_ = new PrintDialog();
        private static PageSetupDialog pageSetupDialog_ = new PageSetupDialog();
        private static PrintPreviewDialog printPreviewDialog_ = new PrintPreviewDialog();

        private static bool initialized_ = false;

        private static void Initialize()
        {
            ToolStripButton but1 = new ToolStripButton();
            but1.Image = ((ToolStrip)(printPreviewDialog_.Controls[1])).ImageList.Images[0];
            but1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            but1.Click += printPreview_PrintClick;
            ((ToolStrip)(printPreviewDialog_.Controls[1])).Items.RemoveAt(0);
            ((ToolStrip)(printPreviewDialog_.Controls[1])).Items.Insert(0, but1);

           // ToolStripButton but2 = new ToolStripButton();
            //but2.Image = ((ToolStrip)(printPreviewDialog_.Controls[1])).ImageList.Images[0];
            //but2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //but2.Click += printPreview_PageSetupClick;
            //((ToolStrip)(printPreviewDialog_.Controls[1])).Items.Insert(0, but2);

            initialized_ = true;
        }

        //private static void printPreview_PageSetupClick(object sender, EventArgs e)
        //{
            //ShowPageSetupDialog(printPreviewDialog_.Document);
        //}

        private static void printPreview_PrintClick(object sender, EventArgs e)
        {
            try
            {
                if (ShowPrintDialog() == DialogResult.OK) printDocument_.Print();
            }
            catch (Exception ex)
            {
                Controls.Forms.MessageBox.ShowMessage(ex.Message);
            }
        }

        public static PrintDocument PrintDocument
        {
            get => printDocument_;
            set
            {
                if (!initialized_) Initialize();
                if (printDocument_ == value) return;
                printDocument_ = value;
                printPreviewDialog_.Document = PrintDocument;
                printDialog_.Document = PrintDocument;
                pageSetupDialog_.Document = PrintDocument;
            }
        }

        public static DialogResult ShowPrintDialog(PrintDocument pd = null)
        {
            if (!initialized_) Initialize();
            if (pd != null) PrintDocument = pd;
            return printDialog_.ShowDialog();
        }

        public static DialogResult ShowPrintPreviewDialog(PrintDocument pd = null, System.Drawing.Size? winSize = null)
        {
            if (!initialized_) Initialize();
            if (pd != null) PrintDocument = pd;
            printPreviewDialog_.Shown += (s, e) => PrintPreviewShown(new System.Drawing.Size(620, 720));
            return printPreviewDialog_.ShowDialog();
        }

        private static void PrintPreviewShown(System.Drawing.Size winSize) => ((Form)printPreviewDialog_).Size = winSize;

        public static DialogResult ShowPageSetupDialog(PrintDocument pd = null)
        {
            if (!initialized_) Initialize();
            if (pd != null) PrintDocument = pd;
            return pageSetupDialog_.ShowDialog();
        }
    }
}
