namespace RecipeManager.RecipesTable.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class RecipesProvider
    {
        public IEnumerable<Recipe> GetRecipes()
        {
            using (var context = new RecipesContext())
            {
                return context.recipes.ToList();
            }
        }

        public IEnumerable<Recipe> GetRecipes(Expression<Func<Recipe, bool>> predicate)
        {
            using (var context = new RecipesContext())
            {
                return context.recipes.Where(predicate).AsEnumerable();
            }
        }
    }
}