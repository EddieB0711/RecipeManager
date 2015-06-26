using RecipeManager.Recipes;
using RecipeManager.RecipesTable.DataAccess;
using System;
using System.Windows.Input;

namespace RecipeManager.Main.Commands {

    internal class RecipeEditorCommand : ICommand {

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            var context = new RecipesContext();
            var editor = new FRecipesEditor();

            editor.Show();
        }
    }
}