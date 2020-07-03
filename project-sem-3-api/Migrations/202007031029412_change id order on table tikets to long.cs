namespace project_sem_3_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeidorderontabletiketstolong : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tickets", "IdOrder", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "IdOrder", c => c.Int(nullable: false));
        }
    }
}
