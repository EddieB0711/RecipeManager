namespace RecipeManager.RecipesTable.DataAccess
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("test.recipes")]
    public partial class Recipe
    {
        [Key]
        public int RecipeIndex { get; set; }

        [StringLength(30)]
        public string RecipeTitle { get; set; }

        [StringLength(30)]
        public string RecipeAuthor { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RecipeDate { get; set; }

        [StringLength(250)]
        public string RecipeInstructions { get; set; }
    }
}