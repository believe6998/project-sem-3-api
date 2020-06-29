namespace project_sem_3_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_model_ticket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "IdOrder", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "IdObject", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "PassengerName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "PassengerName");
            DropColumn("dbo.Tickets", "IdObject");
            DropColumn("dbo.Tickets", "IdOrder");
        }
    }
}
