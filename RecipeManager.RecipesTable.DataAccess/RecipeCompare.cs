namespace RecipeManager.RecipesTable.DataAccess
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    [Serializable]
    public partial class Recipe
    {
        public override bool Equals(object obj) => this.equals(obj as Recipe);
        public override int GetHashCode() => base.GetHashCode();

        private bool equals(Recipe other)
        {
            if (other == null)
                return false;
            if (!this.RecipeAuthor.Equals(other.RecipeAuthor))
                return false;
            if (!this.RecipeTitle.Equals(other.RecipeTitle))
                return false;
            if (!this.RecipeDate.Equals(other.RecipeDate))
                return false;
            if (!this.RecipeInstructions.Equals(other.RecipeInstructions))
                return false;

            return true;
        }
    }

    public static class RecipeExtension
    {
        public static Recipe DeepCopy(this Recipe other)
        {
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, other);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(memoryStream) as Recipe;
            }
        }
    }
}