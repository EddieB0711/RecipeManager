using Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RecipeManager.Main {
    /// <summary>
    /// Interaction logic for FormMain.xaml
    /// </summary>
    public partial class FormMain : RibbonWindow {
        public FormMain() {
            InitializeComponent();
        }

        private void miExit_MouseUp(object sender, MouseButtonEventArgs e) {
            this.Close();
        }
    }
}
