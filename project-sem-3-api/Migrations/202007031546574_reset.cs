namespace project_sem_3_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reset : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Orders");
            AddColumn("dbo.Orders", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "UpdatedAt", c => c.DateTime());
            AddColumn("dbo.Orders", "DeletedAt", c => c.DateTime());
            AddColumn("dbo.Orders", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "IdObjectPassenger", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Tickets", "IdOrder", c => c.Long(nullable: false));
            AlterColumn("dbo.TrainTrainCars", "Date", c => c.String());
            AddPrimaryKey("dbo.Orders", "Id");
            DropColumn("dbo.Tickets", "IdObject");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "IdObject", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.Orders");
            AlterColumn("dbo.TrainTrainCars", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tickets", "IdOrder", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Tickets", "IdObjectPassenger");
            DropColumn("dbo.Orders", "Status");
            DropColumn("dbo.Orders", "DeletedAt");
            DropColumn("dbo.Orders", "UpdatedAt");
            DropColumn("dbo.Orders", "CreatedAt");
            AddPrimaryKey("dbo.Orders", "Id");
        }
    }
}
