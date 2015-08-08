namespace RecipeManager.Recipes
{
    using System.Windows.Input;
    using Fluent;
    using ViewModels;
    using Utilities;

    /// <summary>
    /// Interaction logic for FRecipesEditor.xaml
    /// </summary>
    public partial class FRecipesEditor : RibbonWindow
    {
        private readonly RecipesViewModel _viewModel;

        public FRecipesEditor(RecipesViewModel viewModel)
        {
            Assertions.CheckNull(viewModel, "viewModel");

            this._viewModel = viewModel;
            InitializeComponent();

            this.listRecipes.DataContext = viewModel;
            this.txtAuthor.DataContext = viewModel;
            this.txtDate.DataContext = viewModel;
            this.txtTitle.DataContext = viewModel;
            this.txtInstructions.DataContext = viewModel;
        }

        public void miExit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}