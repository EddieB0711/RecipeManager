namespace RecipeManager.Recipes.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using RecipesTable.DataAccess;
    using Utilities;

    public class RecipesViewModel : INotifyPropertyChanged
    {
        private ReadOnlyCollection<Recipe> _originalRecipes;
        private IEnumerable<Recipe> _currentRecipes;
        private Recipe _selectedRecipe;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyCollection<Recipe> OriginalRecipes => _originalRecipes;

        public IEnumerable<Recipe> Recipes
        {
            get { return _currentRecipes; }
            set
            {
                if (value != null)
                {
                    _currentRecipes = value;

                    if (!_currentRecipes.Contains(_selectedRecipe))
                    {
                        _selectedRecipe = _currentRecipes.FirstOrDefault();
                        this.NotifyInfoChanged();
                    }

                    this.OnPropertyChanged("Recipes");
                }
            }
        }

        public Recipe SelectedItem
        {
            get { return _selectedRecipe; }
            set
            {
                _selectedRecipe = value;
                this.NotifyInfoChanged();
                this.OnPropertyChanged("SelectedItem");
            }
        }

        public string TxtAuthor
        {
            get { return _selectedRecipe.RecipeAuthor; }
            set
            {
                _selectedRecipe.RecipeAuthor = value;
                this.OnPropertyChanged("TxtAuthor");
            }
        }

        public string TxtTitle
        {
            get { return _selectedRecipe.RecipeTitle; }
            set
            {
                _selectedRecipe.RecipeTitle = value;
                this.OnPropertyChanged("TxtTitle");
            }
        }

        public DateTime? TxtDate
        {
            get { return _selectedRecipe.RecipeDate; }
            set
            {
                _selectedRecipe.RecipeDate = value;
                this.OnPropertyChanged("TxtDate");
            }
        }

        public string TxtInstructions
        {
            get { return _selectedRecipe.RecipeInstructions; }
            set
            {
                _selectedRecipe.RecipeInstructions = value;
                this.OnPropertyChanged("TxtInstructions");
            }
        }

        public RecipesViewModel(IEnumerable<Recipe> recipes)
        {
            Assertions.CheckNull(recipes, "recipes");

            _originalRecipes = new ReadOnlyCollection<Recipe>(recipes.Select(x => x.DeepCopy()).ToList());
            _currentRecipes = recipes.ToList();
            _selectedRecipe = _currentRecipes.FirstOrDefault() ?? new Recipe();
        }

        private void NotifyInfoChanged()
        {
            this.OnPropertyChanged("TxtAuthor");
            this.OnPropertyChanged("TxtTitle");
            this.OnPropertyChanged("TxtDate");
            this.OnPropertyChanged("TxtInstructions");
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}