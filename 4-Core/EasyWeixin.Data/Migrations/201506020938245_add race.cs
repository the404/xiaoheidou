namespace EasyWeixin.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrace : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MainRaces",
                c => new
                    {
                        MainRaceId = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        WeixinUserId = c.Int(),
                        Score = c.Double(nullable: false),
                        SumScore = c.Double(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MainRaceId)
                .ForeignKey("dbo.WeixinUsers", t => t.WeixinUserId)
                .Index(t => t.WeixinUserId);
            
            CreateTable(
                "dbo.SubRaces",
                c => new
                    {
                        SubRaceId = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        WeixinUserId = c.Int(),
                        MainRaceId = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SubRaceId)
                .ForeignKey("dbo.WeixinUsers", t => t.WeixinUserId)
                .ForeignKey("dbo.MainRaces", t => t.MainRaceId, cascadeDelete: true)
                .Index(t => t.WeixinUserId)
                .Index(t => t.MainRaceId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.SubRaces", new[] { "MainRaceId" });
            DropIndex("dbo.SubRaces", new[] { "WeixinUserId" });
            DropIndex("dbo.MainRaces", new[] { "WeixinUserId" });
            DropForeignKey("dbo.SubRaces", "MainRaceId", "dbo.MainRaces");
            DropForeignKey("dbo.SubRaces", "WeixinUserId", "dbo.WeixinUsers");
            DropForeignKey("dbo.MainRaces", "WeixinUserId", "dbo.WeixinUsers");
            DropTable("dbo.SubRaces");
            DropTable("dbo.MainRaces");
        }
    }
}
