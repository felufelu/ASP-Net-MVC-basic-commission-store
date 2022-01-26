namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.orderdetails", "Description", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.orderdetails", "Description");
        }
    }
}
