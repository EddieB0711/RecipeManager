namespace RecipeManager.RecipesTable.DataAccess
{
    using System.Data.Entity;

    public partial class RecipesContext : DbContext
    {
        public RecipesContext()
            : base("name=RecipesContext")
        {
        }

        public virtual DbSet<Recipe> recipes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .Property(e => e.RecipeTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Recipe>()
                .Property(e => e.RecipeAuthor)
                .IsUnicode(false);

            modelBuilder.Entity<Recipe>()
                .Property(e => e.RecipeInstructions)
                .IsUnicode(false);
        }
    }
}