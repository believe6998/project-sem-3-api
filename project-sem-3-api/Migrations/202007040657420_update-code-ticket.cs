namespace project_sem_3_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecodeticket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "Code", c => c.String());
            DropColumn("dbo.Orders", "Code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Code", c => c.String());
            DropColumn("dbo.Tickets", "Code");
        }
    }
}
