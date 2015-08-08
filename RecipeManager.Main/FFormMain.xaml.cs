using System.Windows.Input;
using Fluent;

namespace RecipeManager.Main
{
    /// <summary>
    /// Interaction logic for FormMain.xaml
    /// </summary>
    public partial class FormMain : RibbonWindow
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void miExit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}