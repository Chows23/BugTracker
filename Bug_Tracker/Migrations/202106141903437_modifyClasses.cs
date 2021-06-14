namespace Bug_Tracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyClasses : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.TicketAttachments", "FilePath", c => c.String(nullable: false));
            AlterColumn("dbo.TicketAttachments", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.TicketAttachments", "FileUrl", c => c.String(nullable: false));
            AlterColumn("dbo.Tickets", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Tickets", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.TicketComments", "Comment", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TicketComments", "Comment", c => c.String());
            AlterColumn("dbo.Tickets", "Description", c => c.String());
            AlterColumn("dbo.Tickets", "Title", c => c.String());
            AlterColumn("dbo.TicketAttachments", "FileUrl", c => c.String());
            AlterColumn("dbo.TicketAttachments", "Description", c => c.String());
            AlterColumn("dbo.TicketAttachments", "FilePath", c => c.String());
            AlterColumn("dbo.Projects", "Name", c => c.String());
        }
    }
}
