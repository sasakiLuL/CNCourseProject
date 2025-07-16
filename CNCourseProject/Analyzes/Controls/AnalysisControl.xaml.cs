using System.Windows;
using System.Windows.Controls;

namespace ComputerNetworksCourseWork.Analyzes.Controls
{
    public partial class AnalysisControl : UserControl
    {
        public required Analysis Analysis { get; set; }

        public AnalysisControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        void OnNextIterationClickedHandler(object sender, EventArgs e)
        {
            if(!Analysis.Iterate())
            {
                MessageBox.Show("The error has been appeared. Check all of the fields");
            }
        }

        void OnResetFFAClickedHandler(object sender, EventArgs e)
        {
            if (!Analysis.ResetFFA())
            {
                MessageBox.Show("The error has been appeared. Check all of the fields");
            }
        }
        
        void OnFindOptimalPathResetClickedHandler(object sender, EventArgs e)
        {
            if (!Analysis.ResetOptimalPath())
            {
                MessageBox.Show("The error has been appeared. Check all of the fields");
            }
        }

        void OnFindOptimalPathClickedHandler(object sender, EventArgs e)
        {
            if (!Analysis.FindOptimalPath())
            {
                MessageBox.Show("The error has been appeared. Check all of the fields");
            }
        }
    }
}
