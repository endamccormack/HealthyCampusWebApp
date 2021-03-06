namespace HealthyCampusWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllModelsCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        RecipeId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Method = c.String(),
                        PrepTime = c.Double(nullable: false),
                        CookTime = c.Double(nullable: false),
                        DifficultyLevel = c.Int(nullable: false),
                        ImageURL = c.String(),
                    })
                .PrimaryKey(t => t.RecipeId);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        IngredientId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AmountInGrams = c.Int(nullable: false),
                        IngredientType = c.String(),
                        Recipe_RecipeId = c.Int(),
                    })
                .PrimaryKey(t => t.IngredientId)
                .ForeignKey("dbo.Recipes", t => t.Recipe_RecipeId)
                .Index(t => t.Recipe_RecipeId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagName = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.TagName);
            
            CreateTable(
                "dbo.TagRecipes",
                c => new
                    {
                        Tag_TagName = c.String(nullable: false, maxLength: 40),
                        Recipe_RecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagName, t.Recipe_RecipeId })
                .ForeignKey("dbo.Tags", t => t.Tag_TagName, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.Recipe_RecipeId, cascadeDelete: true)
                .Index(t => t.Tag_TagName)
                .Index(t => t.Recipe_RecipeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagRecipes", "Recipe_RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.TagRecipes", "Tag_TagName", "dbo.Tags");
            DropForeignKey("dbo.Ingredients", "Recipe_RecipeId", "dbo.Recipes");
            DropIndex("dbo.TagRecipes", new[] { "Recipe_RecipeId" });
            DropIndex("dbo.TagRecipes", new[] { "Tag_TagName" });
            DropIndex("dbo.Ingredients", new[] { "Recipe_RecipeId" });
            DropTable("dbo.TagRecipes");
            DropTable("dbo.Tags");
            DropTable("dbo.Ingredients");
            DropTable("dbo.Recipes");
        }
    }
}
