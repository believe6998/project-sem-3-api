namespace project_sem_3_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_date_to_string_train_train_cả : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TrainTrainCars", "Date", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TrainTrainCars", "Date", c => c.DateTime(storeType: "date"));
        }
    }
}
