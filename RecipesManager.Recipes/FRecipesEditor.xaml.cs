using Fluent;
using System.Windows.Input;

namespace RecipeManager.Recipes {

    /// <summary>
    /// Interaction logic for FRecipesEditor.xaml
    /// </summary>
    public partial class FRecipesEditor : RibbonWindow {

        public FRecipesEditor() {
            InitializeComponent();
        }

        public void miExit_MouseUp(object sender, MouseButtonEventArgs e) {
            this.Close();
        }
    }
}