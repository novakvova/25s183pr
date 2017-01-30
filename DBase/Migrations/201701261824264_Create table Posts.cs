namespace DBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatetablePosts : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserRoles", newName: "RoleUsers");
            DropPrimaryKey("dbo.RoleUsers");
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserPosts",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Post_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Post_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.Post_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Post_Id);
            
            AddPrimaryKey("dbo.RoleUsers", new[] { "Role_Id", "User_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPosts", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.UserPosts", "User_Id", "dbo.Users");
            DropIndex("dbo.UserPosts", new[] { "Post_Id" });
            DropIndex("dbo.UserPosts", new[] { "User_Id" });
            DropPrimaryKey("dbo.RoleUsers");
            DropTable("dbo.UserPosts");
            DropTable("dbo.Posts");
            AddPrimaryKey("dbo.RoleUsers", new[] { "User_Id", "Role_Id" });
            RenameTable(name: "dbo.RoleUsers", newName: "UserRoles");
        }
    }
}
