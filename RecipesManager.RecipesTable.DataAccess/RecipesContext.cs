using RecipeManager.ConnectionBuilding;
using RecipeManager.Generic.DataAccess;

namespace RecipeManager.RecipesTable.DataAccess {

    public class RecipesContext {
        public Query<RecipesRecord> Recipes { get; private set; }

        public RecipesContext() {
            this.Recipes = new Query<RecipesRecord>(ConnectionBuilder.GetConnectionString("recipes"), "recipes");
        }
    }
}