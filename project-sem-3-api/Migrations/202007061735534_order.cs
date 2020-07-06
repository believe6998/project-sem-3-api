namespace project_sem_3_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tickets", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Tickets", new[] { "Order_Id" });
            DropColumn("dbo.Tickets", "Order_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "Order_Id", c => c.Long());
            CreateIndex("dbo.Tickets", "Order_Id");
            AddForeignKey("dbo.Tickets", "Order_Id", "dbo.Orders", "Id");
        }
    }
}
