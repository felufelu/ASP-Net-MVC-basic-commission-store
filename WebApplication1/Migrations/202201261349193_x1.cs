namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.orderdetails", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.orderdetails", "Description", c => c.Int(nullable: false));
        }
    }
}
