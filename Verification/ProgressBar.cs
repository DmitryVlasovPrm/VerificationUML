using System.Windows.Forms;

namespace Verification
{
	public partial class ProgressBar : Form
	{
		public ProgressBar()
		{
			InitializeComponent();
			progressBar1.Minimum = 0;
			progressBar1.Maximum = 100;
		}

		public void SetStep(int step)
		{
			progressBar1.Step = step;
		}

		public void SetValue(int value)
		{
			progressBar1.Value = value;
		}

		public void PerformProgress()
		{
			progressBar1.PerformStep();
		}
	}
}
