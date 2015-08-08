using System;
using System.Windows.Input;
using RecipeManager.Recipes;
using RecipeManager.Recipes.ViewModels;
using RecipeManager.RecipesTable.DataAccess;

namespace RecipeManager.Main.Commands
{
    internal class RecipeEditorCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var viewModel = new RecipesViewModel(new RecipesProvider().GetRecipes());
            var editor = new FRecipesEditor(viewModel);

            editor.Show();
        }
    }
}