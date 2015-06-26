using RecipeManager.Base.DataAccess;
using System;

namespace RecipeManager.RecipesTable.DataAccess {

    public class RecipesRecord {
        [PrimaryKey]
        public long? RecipeIndex { get; set; }
        public string RecipeTitle { get; set; }
        public string RecipeAuthor { get; set; }
        public DateTime? RecipeDate { get; set; }
        public string RecipeInstructions { get; set; }

        public override bool Equals(object obj) {
            return this.equals((RecipesRecord)obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        private bool equals(RecipesRecord other) {
            if (this.RecipeIndex != other.RecipeIndex) {
                return false;
            } if (this.RecipeTitle != other.RecipeTitle) {
                return false;
            } if (this.RecipeAuthor != other.RecipeAuthor) {
                return false;
            } if (this.RecipeDate != other.RecipeDate) {
                return false;
            } if (this.RecipeInstructions != other.RecipeInstructions) {
                return false;
            }

            return true;
        }
    }
}