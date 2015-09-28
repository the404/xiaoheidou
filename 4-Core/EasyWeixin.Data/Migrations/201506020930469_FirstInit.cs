namespace EasyWeixin.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 250),
                        Email = c.String(maxLength: 250),
                        DateCreated = c.DateTime(),
                        QQ = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 50),
                        UserTypeID = c.Int(),
                        LoginType = c.Int(),
                        UserPhoto = c.String(maxLength: 100),
                        CompanyName = c.String(),
                        UserCode = c.String(),
                        Alt = c.String(),
                        WeixinToken = c.String(),
                        openid = c.String(),
                        AppId = c.String(),
                        AppSecret = c.String(),
                        WeixinUser = c.String(),
                        Md5Password = c.String(),
                        Header = c.String(),
                        InDate = c.DateTime(nullable: false),
                        ID = c.Guid(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Buttons",
                c => new
                    {
                        ButtonID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        name = c.String(maxLength: 16),
                        type = c.String(),
                        key = c.String(maxLength: 128),
                        AddTime = c.DateTime(),
                        IsOrder = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ButtonID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ResponseMessages",
                c => new
                    {
                        ResponseMessageID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        UserId = c.Int(),
                        ResponseType = c.Int(),
                        ButtonType = c.Int(),
                        Content = c.String(),
                        ResponseMusicID = c.Int(),
                        ResponseImageID = c.Int(),
                        ResponseImageTextID = c.Int(),
                        ResponseVideoID = c.Int(),
                        Link = c.String(),
                        AddTime = c.DateTime(),
                        ResponseKeyRuleID = c.Int(),
                        ButtonID = c.Int(),
                        SubButtonID = c.Int(),
                    })
                .PrimaryKey(t => t.ResponseMessageID)
                .ForeignKey("dbo.Buttons", t => t.ButtonID)
                .ForeignKey("dbo.SubButtons", t => t.SubButtonID)
                .ForeignKey("dbo.ResponseMusics", t => t.ResponseMusicID)
                .ForeignKey("dbo.ResponseImages", t => t.ResponseImageID)
                .ForeignKey("dbo.ResponseImageTexts", t => t.ResponseImageTextID)
                .ForeignKey("dbo.ResponseVideos", t => t.ResponseVideoID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.ResponseKeyRules", t => t.ResponseKeyRuleID)
                .Index(t => t.ButtonID)
                .Index(t => t.SubButtonID)
                .Index(t => t.ResponseMusicID)
                .Index(t => t.ResponseImageID)
                .Index(t => t.ResponseImageTextID)
                .Index(t => t.ResponseVideoID)
                .Index(t => t.UserId)
                .Index(t => t.ResponseKeyRuleID);
            
            CreateTable(
                "dbo.SubButtons",
                c => new
                    {
                        SubButtonID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        name = c.String(maxLength: 16),
                        type = c.String(),
                        key = c.String(maxLength: 128),
                        AddTime = c.DateTime(),
                        IsOrder = c.Int(nullable: false),
                        ButtonID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubButtonID)
                .ForeignKey("dbo.Buttons", t => t.ButtonID, cascadeDelete: true)
                .Index(t => t.ButtonID);
            
            CreateTable(
                "dbo.ResponseMusics",
                c => new
                    {
                        ResponseMusicID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        MusicUrl = c.String(maxLength: 400),
                        HQMusicUrl = c.String(maxLength: 400),
                        MusicName = c.String(maxLength: 200),
                        Description = c.String(),
                        AddTime = c.DateTime(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResponseMusicID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ResponseImages",
                c => new
                    {
                        ResponseImageID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        ImageUrl = c.String(nullable: false, maxLength: 400),
                        ImageName = c.String(maxLength: 200),
                        Description = c.String(),
                        AddTime = c.DateTime(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResponseImageID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ResponseImageTexts",
                c => new
                    {
                        ResponseImageTextID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        ImageTextName = c.String(maxLength: 200),
                        PicUrl = c.String(),
                        Url = c.String(),
                        AddTime = c.DateTime(),
                        UserId = c.Int(nullable: false),
                        ImageTextType = c.Int(nullable: false),
                        ImageTextDesc = c.String(),
                    })
                .PrimaryKey(t => t.ResponseImageTextID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Guesses",
                c => new
                    {
                        GuessID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        GuessTitle = c.String(),
                        GuessName = c.String(),
                        GuessDesc = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        GetURL = c.String(),
                        GetShortURL = c.String(),
                        QuickResponse = c.String(),
                        GuessStyle = c.String(),
                        UserId = c.Int(),
                        ResponseImageTextID = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GuessID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.ResponseImageTexts", t => t.ResponseImageTextID, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ResponseImageTextID);
            
            CreateTable(
                "dbo.GuessUsers",
                c => new
                    {
                        GuessUserID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        GuessUserName = c.String(),
                        Identification = c.String(),
                        GuessUserEmail = c.String(),
                        GuessUserPhone = c.String(),
                        GuessUserWexinID = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        GuessTimes = c.Int(nullable: false),
                        GuessProcess = c.String(),
                        Answer = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        GuessID = c.Int(),
                        IP = c.String(),
                    })
                .PrimaryKey(t => t.GuessUserID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Guesses", t => t.GuessID)
                .Index(t => t.UserId)
                .Index(t => t.GuessID);
            
            CreateTable(
                "dbo.ResponseVideos",
                c => new
                    {
                        ResponseVideoID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        VideoUrl = c.String(maxLength: 400),
                        VideoName = c.String(maxLength: 200),
                        Description = c.String(),
                        AddTime = c.DateTime(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResponseVideoID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ResponseKeyRules",
                c => new
                    {
                        ResponseKeyRuleID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        RuleName = c.String(nullable: false, maxLength: 200),
                        UserId = c.Int(),
                        AddTime = c.DateTime(),
                        IsOrder = c.Int(),
                    })
                .PrimaryKey(t => t.ResponseKeyRuleID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ResponseKeys",
                c => new
                    {
                        ResponseKeyID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 50),
                        UserId = c.Int(),
                        IsFullMatch = c.Int(),
                        AddTime = c.DateTime(),
                        IsOrder = c.Int(),
                        ResponseKeyRuleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResponseKeyID)
                .ForeignKey("dbo.ResponseKeyRules", t => t.ResponseKeyRuleID, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.ResponseKeyRuleID)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.WeixinUserInUsers",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        WeixinUserId = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.WeixinUsers", t => t.WeixinUserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.WeixinUserId);
            
            CreateTable(
                "dbo.WeixinUsers",
                c => new
                    {
                        WeixinUserId = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        AddDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Subscribe = c.Int(nullable: false),
                        Openid = c.String(),
                        Sex = c.Int(nullable: false),
                        City = c.String(),
                        Province = c.String(),
                        Country = c.String(),
                        SubscribeTime = c.Int(nullable: false),
                        Remark = c.String(),
                        Nickname = c.String(),
                        Headimgurl = c.String(),
                        Language = c.String(),
                        Unionid = c.String(),
                        Privilege = c.String(),
                    })
                .PrimaryKey(t => t.WeixinUserId);
            
            CreateTable(
                "dbo.WeixinUserInActivities",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        WeixinUserId = c.Int(nullable: false),
                        ActType = c.Int(nullable: false),
                        ActId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        SumCount = c.Int(nullable: false),
                        TodayAdd = c.Int(nullable: false),
                        Today = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.WeixinUsers", t => t.WeixinUserId, cascadeDelete: true)
                .Index(t => t.WeixinUserId);
            
            CreateTable(
                "dbo.QrCodes",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        SceneId = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        AwardNum = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        QrCodeUrl = c.String(),
                        AddTime = c.DateTime(nullable: false),
                        IsWeixinSend = c.Boolean(nullable: false),
                        OpenId = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_Membership",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ID = c.Guid(nullable: false, identity: true),
                        CreateDate = c.DateTime(),
                        ConfirmationToken = c.String(maxLength: 128),
                        IsConfirmed = c.Boolean(),
                        LastPasswordFailureDate = c.DateTime(),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        Password = c.String(nullable: false, maxLength: 128),
                        PasswordChangedDate = c.DateTime(),
                        PasswordSalt = c.String(nullable: false, maxLength: 128),
                        PasswordVerificationToken = c.String(maxLength: 128),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                        PasswordResetToken = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 256),
                        RoleDesc = c.String(maxLength: 300),
                        RoleImageName = c.String(maxLength: 300),
                        RoleControllerName = c.String(maxLength: 300),
                        AddTime = c.DateTime(),
                        IsOrder = c.Int(),
                        IsMenu = c.Int(),
                        RoleChineseName = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.webpages_PermissionsInRoles",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        PermissionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.webpages_Permission", t => t.PermissionId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.webpages_Permission",
                c => new
                    {
                        PermissionId = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        PermissionName = c.String(nullable: false, maxLength: 200),
                        PermissionDesc = c.String(maxLength: 500),
                        PermissionImageName = c.String(maxLength: 200),
                        AdminTime = c.DateTime(),
                        PermissionActionName = c.String(maxLength: 200),
                        PermissionUrl = c.String(maxLength: 200),
                        IsOrder = c.Int(),
                        IsMenu = c.Int(),
                        PermissionChineseName = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.PermissionId);
            
            CreateTable(
                "dbo.ScratchUsers",
                c => new
                    {
                        ScratchUserID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        AddDate = c.DateTime(nullable: false),
                        ScratchCode = c.String(),
                        IP = c.String(),
                        ScratchItemID = c.Int(),
                        WeixinUserId = c.Int(),
                        ScratchID = c.Int(),
                        IsAward = c.Boolean(nullable: false),
                        Name = c.String(),
                        Phone = c.String(),
                        AwardDate = c.String(),
                    })
                .PrimaryKey(t => t.ScratchUserID)
                .ForeignKey("dbo.Scratches", t => t.ScratchID)
                .ForeignKey("dbo.ScratchItems", t => t.ScratchItemID)
                .ForeignKey("dbo.WeixinUsers", t => t.WeixinUserId)
                .Index(t => t.ScratchID)
                .Index(t => t.ScratchItemID)
                .Index(t => t.WeixinUserId);
            
            CreateTable(
                "dbo.Scratches",
                c => new
                    {
                        ScratchID = c.Int(nullable: false, identity: true),
                        ScratchTitle = c.String(),
                        ScratchDesc = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        GetURL = c.String(),
                        GetShortURL = c.String(),
                        QuickResponse = c.String(),
                        ScratchStyle = c.String(),
                        ScratchBgImage = c.String(),
                        ScratchScale = c.Int(nullable: false),
                        UserId = c.Int(),
                        Thanks = c.String(),
                        EveryDayTimes = c.Int(),
                        ResponseImageTextID = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        ID = c.Guid(nullable: false, identity: true),
                        PicUrl = c.String(),
                    })
                .PrimaryKey(t => t.ScratchID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.ResponseImageTexts", t => t.ResponseImageTextID, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ResponseImageTextID);
            
            CreateTable(
                "dbo.ScratchItems",
                c => new
                    {
                        ScratchItemID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        ScratchItemName = c.String(),
                        ScratchItemScale = c.Int(nullable: false),
                        ScratchItemAward = c.String(),
                        isOrder = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        ScratchID = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ScratchItemID)
                .ForeignKey("dbo.Scratches", t => t.ScratchID, cascadeDelete: true)
                .Index(t => t.ScratchID);
            
            CreateTable(
                "dbo.WheelUsers",
                c => new
                    {
                        WheelUserID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        WheelUserName = c.String(),
                        Identification = c.String(),
                        WheelUserEmail = c.String(),
                        WheelUserPhone = c.String(),
                        WheelUserWexinID = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        WheelTrueName = c.String(),
                        WheelRank = c.String(),
                        WheelCode = c.String(),
                        WheelAngle = c.String(),
                        UserId = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        WheelID = c.Int(),
                        IP = c.String(),
                        WheelItemID = c.Int(),
                        WheelLogID = c.Int(),
                    })
                .PrimaryKey(t => t.WheelUserID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Wheels", t => t.WheelID)
                .ForeignKey("dbo.WheelItems", t => t.WheelItemID)
                .ForeignKey("dbo.WheelLogs", t => t.WheelLogID)
                .Index(t => t.UserId)
                .Index(t => t.WheelID)
                .Index(t => t.WheelItemID)
                .Index(t => t.WheelLogID);
            
            CreateTable(
                "dbo.Wheels",
                c => new
                    {
                        WheelID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        WheelTitle = c.String(),
                        WheelDesc = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        GetURL = c.String(),
                        GetShortURL = c.String(),
                        QuickResponse = c.String(),
                        WheelStyle = c.String(),
                        WheelScale = c.Int(nullable: false),
                        EveryDayTimes = c.Int(),
                        UserId = c.Int(),
                        ResponseImageTextID = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        Mark = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WheelID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.ResponseImageTexts", t => t.ResponseImageTextID, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ResponseImageTextID);
            
            CreateTable(
                "dbo.WheelItems",
                c => new
                    {
                        WheelItemID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        WheelItemName = c.String(),
                        WheelItemScale = c.Int(nullable: false),
                        WheelItemAward = c.String(),
                        isOrder = c.Int(nullable: false),
                        MinAngle = c.String(),
                        MaxAngle = c.String(),
                        WheelID = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.WheelItemID)
                .ForeignKey("dbo.Wheels", t => t.WheelID, cascadeDelete: true)
                .Index(t => t.WheelID);
            
            CreateTable(
                "dbo.WheelLogs",
                c => new
                    {
                        WheelLogID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        WheelUserWexinID = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        WheelCode = c.String(),
                        WheelAngle = c.String(),
                        WheelID = c.Int(),
                        IP = c.String(),
                        IsAward = c.Int(nullable: false),
                        IsShare = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WheelLogID)
                .ForeignKey("dbo.Wheels", t => t.WheelID)
                .Index(t => t.WheelID);
            
            CreateTable(
                "dbo.CouponUsers",
                c => new
                    {
                        CouponUserID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        CouponUserName = c.String(),
                        Identification = c.String(),
                        CouponUserEmail = c.String(),
                        CouponUserPhone = c.String(),
                        CouponUserWexinID = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        CouponTrueName = c.String(),
                        CouponCode = c.String(),
                        UserId = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        CouponID = c.Int(),
                        IP = c.String(),
                        CouponItemID = c.Int(),
                        CouponLogID = c.Int(),
                    })
                .PrimaryKey(t => t.CouponUserID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Coupons", t => t.CouponID)
                .ForeignKey("dbo.CouponItems", t => t.CouponItemID)
                .ForeignKey("dbo.CouponLogs", t => t.CouponLogID)
                .Index(t => t.UserId)
                .Index(t => t.CouponID)
                .Index(t => t.CouponItemID)
                .Index(t => t.CouponLogID);
            
            CreateTable(
                "dbo.Coupons",
                c => new
                    {
                        CouponID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        CouponTitle = c.String(),
                        CouponDesc = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        GetURL = c.String(),
                        GetShortURL = c.String(),
                        QuickResponse = c.String(),
                        CouponStyle = c.String(),
                        CouponCount = c.Int(nullable: false),
                        CouponScale = c.Int(nullable: false),
                        UserId = c.Int(),
                        EveryDayTimes = c.Int(),
                        ResponseImageTextID = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CouponID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.ResponseImageTexts", t => t.ResponseImageTextID, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ResponseImageTextID);
            
            CreateTable(
                "dbo.CouponItems",
                c => new
                    {
                        CouponItemID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        CouponItemName = c.String(),
                        CouponItemScale = c.Int(nullable: false),
                        CouponItemAward = c.String(),
                        CouponID = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CouponItemID)
                .ForeignKey("dbo.Coupons", t => t.CouponID, cascadeDelete: true)
                .Index(t => t.CouponID);
            
            CreateTable(
                "dbo.CouponLogs",
                c => new
                    {
                        CouponLogID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        CouponUserWexinID = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        CouponCode = c.String(),
                        CouponID = c.Int(),
                        IP = c.String(),
                        IsAward = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CouponLogID)
                .ForeignKey("dbo.Coupons", t => t.CouponID)
                .Index(t => t.CouponID);
            
            CreateTable(
                "dbo.VoteUsers",
                c => new
                    {
                        VoteUserID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        VoteUserName = c.String(),
                        Identification = c.String(),
                        VoteUserEmail = c.String(),
                        VoteUserPhone = c.String(),
                        VoteUserWexinID = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        VoteTrueName = c.String(),
                        VoteIndex = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        VoteID = c.Int(),
                        IP = c.String(),
                    })
                .PrimaryKey(t => t.VoteUserID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Votes", t => t.VoteID)
                .Index(t => t.UserId)
                .Index(t => t.VoteID);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        VoteID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        VoteTitle = c.String(),
                        VoteDesc = c.String(),
                        VoteAnswer = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        GetURL = c.String(),
                        GetShortURL = c.String(),
                        QuickResponse = c.String(),
                        VoteStyle = c.String(),
                        VoteType = c.Int(nullable: false),
                        VoteIsOther = c.Int(nullable: false),
                        UserId = c.Int(),
                        EveryDayTimes = c.Int(),
                        ResponseImageTextID = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VoteID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.ResponseImageTexts", t => t.ResponseImageTextID, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ResponseImageTextID);
            
            CreateTable(
                "dbo.FightUsers",
                c => new
                    {
                        FightUserID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        FightUserName = c.String(),
                        Identification = c.String(),
                        FightUserEmail = c.String(),
                        FightUserPhone = c.String(),
                        FightUserWexinID = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        FightTrueName = c.String(),
                        FightItemSum = c.String(),
                        UserId = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        FightID = c.Int(),
                        IP = c.String(),
                    })
                .PrimaryKey(t => t.FightUserID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Fights", t => t.FightID)
                .Index(t => t.UserId)
                .Index(t => t.FightID);
            
            CreateTable(
                "dbo.Fights",
                c => new
                    {
                        FightID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        FightTitle = c.String(),
                        FightDesc = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        GetURL = c.String(),
                        GetShortURL = c.String(),
                        QuickResponse = c.String(),
                        FightStyle = c.String(),
                        UserId = c.Int(),
                        EveryDayTimes = c.Int(),
                        ResponseImageTextID = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FightID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.ResponseImageTexts", t => t.ResponseImageTextID, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ResponseImageTextID);
            
            CreateTable(
                "dbo.FightItems",
                c => new
                    {
                        FightItemID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        FightItemName = c.String(),
                        FightItemAnswers = c.String(),
                        FightItemTrueAnswer = c.String(),
                        FightID = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FightItemID)
                .ForeignKey("dbo.Fights", t => t.FightID, cascadeDelete: true)
                .Index(t => t.FightID);
            
            CreateTable(
                "dbo.FightUserItems",
                c => new
                    {
                        FightUserItemID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        FightUserWexinID = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        FightUserAnswer = c.String(),
                        AnswerResult = c.Int(nullable: false),
                        QuestionIndex = c.Int(nullable: false),
                        FightItemID = c.Int(),
                        FightUserID = c.Int(),
                        IP = c.String(),
                    })
                .PrimaryKey(t => t.FightUserItemID)
                .ForeignKey("dbo.FightItems", t => t.FightItemID)
                .ForeignKey("dbo.FightUsers", t => t.FightUserID)
                .Index(t => t.FightItemID)
                .Index(t => t.FightUserID);
            
            CreateTable(
                "dbo.FightLogs",
                c => new
                    {
                        FightLogID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        FightUserWexinID = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        FightLogAnswer = c.String(),
                        AnswerResult = c.Int(nullable: false),
                        QuestionIndex = c.Int(nullable: false),
                        FightItemID = c.Int(),
                        IP = c.String(),
                    })
                .PrimaryKey(t => t.FightLogID)
                .ForeignKey("dbo.FightItems", t => t.FightItemID)
                .Index(t => t.FightItemID);
            
            CreateTable(
                "dbo.SnowUsers",
                c => new
                    {
                        SnowUserID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        SnowUserName = c.String(),
                        Identification = c.String(),
                        SnowUserEmail = c.String(),
                        SnowUserPhone = c.String(),
                        SnowUserWexinID = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        SnowTrueName = c.String(),
                        SnowProgress = c.String(),
                        IsAward = c.Int(nullable: false),
                        UserId = c.Int(),
                        Sex = c.Int(nullable: false),
                        SnowID = c.Int(),
                        IP = c.String(),
                        Score = c.Int(nullable: false),
                        SnowLogID = c.Int(),
                    })
                .PrimaryKey(t => t.SnowUserID)
                .ForeignKey("dbo.Snows", t => t.SnowID)
                .ForeignKey("dbo.SnowLogs", t => t.SnowLogID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.SnowID)
                .Index(t => t.SnowLogID)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SnowLogs",
                c => new
                    {
                        SnowLogID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        SnowUserWexinID = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        SnowData = c.String(),
                        IsAward = c.Int(nullable: false),
                        IP = c.String(),
                        SnowID = c.Int(),
                    })
                .PrimaryKey(t => t.SnowLogID)
                .ForeignKey("dbo.Snows", t => t.SnowID)
                .Index(t => t.SnowID);
            
            CreateTable(
                "dbo.Snows",
                c => new
                    {
                        SnowID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        SnowTitle = c.String(),
                        SnowDesc = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        GetURL = c.String(),
                        GetShortURL = c.String(),
                        QuickResponse = c.String(),
                        SnowStyle = c.String(),
                        SnowBgImage = c.String(),
                        SnowScale = c.Int(nullable: false),
                        UserId = c.Int(),
                        Thanks = c.String(),
                        EveryDayTimes = c.Int(),
                        ResponseImageTextID = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        Mark = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SnowID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.ResponseImageTexts", t => t.ResponseImageTextID, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ResponseImageTextID);
            
            CreateTable(
                "dbo.SnowItems",
                c => new
                    {
                        SnowItemID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        SnowItemName = c.String(),
                        SnowScore = c.Int(nullable: false),
                        SnowItemScale = c.Int(nullable: false),
                        SnowScoreType = c.Int(nullable: false),
                        SnowID = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SnowItemID)
                .ForeignKey("dbo.Snows", t => t.SnowID, cascadeDelete: true)
                .Index(t => t.SnowID);
            
            CreateTable(
                "dbo.SnowErrorLogs",
                c => new
                    {
                        SnowErrorLogID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        Action = c.String(),
                        Message = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        IP = c.String(),
                        SnowID = c.Int(),
                    })
                .PrimaryKey(t => t.SnowErrorLogID)
                .ForeignKey("dbo.Snows", t => t.SnowID)
                .Index(t => t.SnowID);
            
            CreateTable(
                "dbo.CameraPhotoes",
                c => new
                    {
                        CameraID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        YName = c.String(),
                        Remark = c.String(),
                        AddTime = c.DateTime(),
                        IpAddress = c.String(),
                        State = c.Int(),
                        LoveNum = c.Int(),
                        IsCheck = c.Boolean(nullable: false),
                        PhotoID = c.Int(),
                    })
                .PrimaryKey(t => t.CameraID)
                .ForeignKey("dbo.PhotoWalls", t => t.PhotoID)
                .Index(t => t.PhotoID);
            
            CreateTable(
                "dbo.PhotoWalls",
                c => new
                    {
                        PhotoID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        PhotoTitle = c.String(),
                        PhotoDesc = c.String(),
                        GetURL = c.String(),
                        GetShortURL = c.String(),
                        UserId = c.Int(),
                        AddDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PhotoID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Acts",
                c => new
                    {
                        ActID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Explanation = c.String(),
                        Content = c.String(),
                        GetURL = c.String(),
                        GetShortURL = c.String(),
                        UserId = c.Int(),
                        IsTop = c.Int(),
                        TopTime = c.DateTime(),
                        Clicks = c.Int(),
                        ImageUrl = c.String(),
                        WURL = c.String(),
                        ClubName = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        CreateIp = c.String(),
                        AddDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ActID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Prefers",
                c => new
                    {
                        PreID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Explanation = c.String(),
                        Content = c.String(),
                        GetURL = c.String(),
                        GetShortURL = c.String(),
                        UserId = c.Int(),
                        IsTop = c.Int(),
                        TopTime = c.DateTime(),
                        Clicks = c.Int(),
                        ImageUrl = c.String(),
                        WURL = c.String(),
                        ClubName = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        CreateIp = c.String(),
                        AddDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PreID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.QuestionCategories",
                c => new
                    {
                        CatID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        CName = c.String(),
                        Content = c.String(),
                        Status = c.Int(),
                        GetURL = c.String(),
                        GetShortURL = c.String(),
                        UserId = c.Int(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        AddDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CatID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SetQuestions",
                c => new
                    {
                        SetQuestionID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        QuestionCategoryID = c.Int(),
                        SetQuestionName = c.String(),
                        Answers = c.String(),
                        AnswerCount = c.Int(),
                        Type = c.Int(),
                        Status = c.Boolean(),
                        OrderIndex = c.Int(),
                        IsOther = c.Boolean(),
                        UserId = c.Int(),
                        AddDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SetQuestionID);
            
            CreateTable(
                "dbo.QItemAnswers",
                c => new
                    {
                        QItemAnswerID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        SetQuestionID = c.Int(),
                        UserId = c.Int(),
                        Answer = c.String(),
                        OtherAnswer = c.String(),
                        AddDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.QItemAnswerID)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CameraLogs",
                c => new
                    {
                        CLogID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        Cookie = c.String(),
                        CameraID = c.Int(),
                        CreateIp = c.String(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CLogID);
            
            CreateTable(
                "dbo.Reads",
                c => new
                    {
                        ReadID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        ViewUrl = c.String(),
                        CreateIp = c.String(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ReadID);
            
            CreateTable(
                "dbo.Praises",
                c => new
                    {
                        PID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        ViewUrl = c.String(),
                        CreateIp = c.String(),
                        Cookie = c.String(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PID);
            
            CreateTable(
                "dbo.PayCustomers",
                c => new
                    {
                        WID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        UPhone = c.String(),
                        PayInfo = c.String(),
                        OpendId = c.String(),
                        Award = c.String(),
                        IsAward = c.Int(),
                        PayDate = c.DateTime(),
                        CreateDate = c.DateTime(),
                        nickname = c.String(),
                        headUrl = c.String(),
                    })
                .PrimaryKey(t => t.WID);
            
            CreateTable(
                "dbo.RecordWUsers",
                c => new
                    {
                        ReID = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false, identity: true),
                        ToUserName = c.String(),
                        FromUserName = c.String(),
                        HeadimgUrl = c.String(),
                        sex = c.Int(nullable: false),
                        NickName = c.String(),
                    })
                .PrimaryKey(t => t.ReID);
            
            CreateTable(
                "dbo.webpages_UsersInRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.webpages_Membership", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.webpages_UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "UserId" });
            DropIndex("dbo.QItemAnswers", new[] { "UserId" });
            DropIndex("dbo.QuestionCategories", new[] { "UserId" });
            DropIndex("dbo.Prefers", new[] { "UserId" });
            DropIndex("dbo.Acts", new[] { "UserId" });
            DropIndex("dbo.PhotoWalls", new[] { "UserId" });
            DropIndex("dbo.CameraPhotoes", new[] { "PhotoID" });
            DropIndex("dbo.SnowErrorLogs", new[] { "SnowID" });
            DropIndex("dbo.SnowItems", new[] { "SnowID" });
            DropIndex("dbo.Snows", new[] { "ResponseImageTextID" });
            DropIndex("dbo.Snows", new[] { "UserId" });
            DropIndex("dbo.SnowLogs", new[] { "SnowID" });
            DropIndex("dbo.SnowUsers", new[] { "UserId" });
            DropIndex("dbo.SnowUsers", new[] { "SnowLogID" });
            DropIndex("dbo.SnowUsers", new[] { "SnowID" });
            DropIndex("dbo.FightLogs", new[] { "FightItemID" });
            DropIndex("dbo.FightUserItems", new[] { "FightUserID" });
            DropIndex("dbo.FightUserItems", new[] { "FightItemID" });
            DropIndex("dbo.FightItems", new[] { "FightID" });
            DropIndex("dbo.Fights", new[] { "ResponseImageTextID" });
            DropIndex("dbo.Fights", new[] { "UserId" });
            DropIndex("dbo.FightUsers", new[] { "FightID" });
            DropIndex("dbo.FightUsers", new[] { "UserId" });
            DropIndex("dbo.Votes", new[] { "ResponseImageTextID" });
            DropIndex("dbo.Votes", new[] { "UserId" });
            DropIndex("dbo.VoteUsers", new[] { "VoteID" });
            DropIndex("dbo.VoteUsers", new[] { "UserId" });
            DropIndex("dbo.CouponLogs", new[] { "CouponID" });
            DropIndex("dbo.CouponItems", new[] { "CouponID" });
            DropIndex("dbo.Coupons", new[] { "ResponseImageTextID" });
            DropIndex("dbo.Coupons", new[] { "UserId" });
            DropIndex("dbo.CouponUsers", new[] { "CouponLogID" });
            DropIndex("dbo.CouponUsers", new[] { "CouponItemID" });
            DropIndex("dbo.CouponUsers", new[] { "CouponID" });
            DropIndex("dbo.CouponUsers", new[] { "UserId" });
            DropIndex("dbo.WheelLogs", new[] { "WheelID" });
            DropIndex("dbo.WheelItems", new[] { "WheelID" });
            DropIndex("dbo.Wheels", new[] { "ResponseImageTextID" });
            DropIndex("dbo.Wheels", new[] { "UserId" });
            DropIndex("dbo.WheelUsers", new[] { "WheelLogID" });
            DropIndex("dbo.WheelUsers", new[] { "WheelItemID" });
            DropIndex("dbo.WheelUsers", new[] { "WheelID" });
            DropIndex("dbo.WheelUsers", new[] { "UserId" });
            DropIndex("dbo.ScratchItems", new[] { "ScratchID" });
            DropIndex("dbo.Scratches", new[] { "ResponseImageTextID" });
            DropIndex("dbo.Scratches", new[] { "UserId" });
            DropIndex("dbo.ScratchUsers", new[] { "WeixinUserId" });
            DropIndex("dbo.ScratchUsers", new[] { "ScratchItemID" });
            DropIndex("dbo.ScratchUsers", new[] { "ScratchID" });
            DropIndex("dbo.webpages_PermissionsInRoles", new[] { "PermissionId" });
            DropIndex("dbo.webpages_PermissionsInRoles", new[] { "RoleId" });
            DropIndex("dbo.QrCodes", new[] { "UserId" });
            DropIndex("dbo.WeixinUserInActivities", new[] { "WeixinUserId" });
            DropIndex("dbo.WeixinUserInUsers", new[] { "WeixinUserId" });
            DropIndex("dbo.WeixinUserInUsers", new[] { "UserId" });
            DropIndex("dbo.ResponseKeys", new[] { "UserId" });
            DropIndex("dbo.ResponseKeys", new[] { "ResponseKeyRuleID" });
            DropIndex("dbo.ResponseKeyRules", new[] { "UserId" });
            DropIndex("dbo.ResponseVideos", new[] { "UserId" });
            DropIndex("dbo.GuessUsers", new[] { "GuessID" });
            DropIndex("dbo.GuessUsers", new[] { "UserId" });
            DropIndex("dbo.Guesses", new[] { "ResponseImageTextID" });
            DropIndex("dbo.Guesses", new[] { "UserId" });
            DropIndex("dbo.ResponseImageTexts", new[] { "UserId" });
            DropIndex("dbo.ResponseImages", new[] { "UserId" });
            DropIndex("dbo.ResponseMusics", new[] { "UserId" });
            DropIndex("dbo.SubButtons", new[] { "ButtonID" });
            DropIndex("dbo.ResponseMessages", new[] { "ResponseKeyRuleID" });
            DropIndex("dbo.ResponseMessages", new[] { "UserId" });
            DropIndex("dbo.ResponseMessages", new[] { "ResponseVideoID" });
            DropIndex("dbo.ResponseMessages", new[] { "ResponseImageTextID" });
            DropIndex("dbo.ResponseMessages", new[] { "ResponseImageID" });
            DropIndex("dbo.ResponseMessages", new[] { "ResponseMusicID" });
            DropIndex("dbo.ResponseMessages", new[] { "SubButtonID" });
            DropIndex("dbo.ResponseMessages", new[] { "ButtonID" });
            DropIndex("dbo.Buttons", new[] { "UserId" });
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.webpages_Membership");
            DropForeignKey("dbo.QItemAnswers", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.QuestionCategories", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Prefers", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Acts", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.PhotoWalls", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.CameraPhotoes", "PhotoID", "dbo.PhotoWalls");
            DropForeignKey("dbo.SnowErrorLogs", "SnowID", "dbo.Snows");
            DropForeignKey("dbo.SnowItems", "SnowID", "dbo.Snows");
            DropForeignKey("dbo.Snows", "ResponseImageTextID", "dbo.ResponseImageTexts");
            DropForeignKey("dbo.Snows", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.SnowLogs", "SnowID", "dbo.Snows");
            DropForeignKey("dbo.SnowUsers", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.SnowUsers", "SnowLogID", "dbo.SnowLogs");
            DropForeignKey("dbo.SnowUsers", "SnowID", "dbo.Snows");
            DropForeignKey("dbo.FightLogs", "FightItemID", "dbo.FightItems");
            DropForeignKey("dbo.FightUserItems", "FightUserID", "dbo.FightUsers");
            DropForeignKey("dbo.FightUserItems", "FightItemID", "dbo.FightItems");
            DropForeignKey("dbo.FightItems", "FightID", "dbo.Fights");
            DropForeignKey("dbo.Fights", "ResponseImageTextID", "dbo.ResponseImageTexts");
            DropForeignKey("dbo.Fights", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.FightUsers", "FightID", "dbo.Fights");
            DropForeignKey("dbo.FightUsers", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Votes", "ResponseImageTextID", "dbo.ResponseImageTexts");
            DropForeignKey("dbo.Votes", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.VoteUsers", "VoteID", "dbo.Votes");
            DropForeignKey("dbo.VoteUsers", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.CouponLogs", "CouponID", "dbo.Coupons");
            DropForeignKey("dbo.CouponItems", "CouponID", "dbo.Coupons");
            DropForeignKey("dbo.Coupons", "ResponseImageTextID", "dbo.ResponseImageTexts");
            DropForeignKey("dbo.Coupons", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.CouponUsers", "CouponLogID", "dbo.CouponLogs");
            DropForeignKey("dbo.CouponUsers", "CouponItemID", "dbo.CouponItems");
            DropForeignKey("dbo.CouponUsers", "CouponID", "dbo.Coupons");
            DropForeignKey("dbo.CouponUsers", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.WheelLogs", "WheelID", "dbo.Wheels");
            DropForeignKey("dbo.WheelItems", "WheelID", "dbo.Wheels");
            DropForeignKey("dbo.Wheels", "ResponseImageTextID", "dbo.ResponseImageTexts");
            DropForeignKey("dbo.Wheels", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.WheelUsers", "WheelLogID", "dbo.WheelLogs");
            DropForeignKey("dbo.WheelUsers", "WheelItemID", "dbo.WheelItems");
            DropForeignKey("dbo.WheelUsers", "WheelID", "dbo.Wheels");
            DropForeignKey("dbo.WheelUsers", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.ScratchItems", "ScratchID", "dbo.Scratches");
            DropForeignKey("dbo.Scratches", "ResponseImageTextID", "dbo.ResponseImageTexts");
            DropForeignKey("dbo.Scratches", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.ScratchUsers", "WeixinUserId", "dbo.WeixinUsers");
            DropForeignKey("dbo.ScratchUsers", "ScratchItemID", "dbo.ScratchItems");
            DropForeignKey("dbo.ScratchUsers", "ScratchID", "dbo.Scratches");
            DropForeignKey("dbo.webpages_PermissionsInRoles", "PermissionId", "dbo.webpages_Permission");
            DropForeignKey("dbo.webpages_PermissionsInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.QrCodes", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.WeixinUserInActivities", "WeixinUserId", "dbo.WeixinUsers");
            DropForeignKey("dbo.WeixinUserInUsers", "WeixinUserId", "dbo.WeixinUsers");
            DropForeignKey("dbo.WeixinUserInUsers", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.ResponseKeys", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.ResponseKeys", "ResponseKeyRuleID", "dbo.ResponseKeyRules");
            DropForeignKey("dbo.ResponseKeyRules", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.ResponseVideos", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.GuessUsers", "GuessID", "dbo.Guesses");
            DropForeignKey("dbo.GuessUsers", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Guesses", "ResponseImageTextID", "dbo.ResponseImageTexts");
            DropForeignKey("dbo.Guesses", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.ResponseImageTexts", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.ResponseImages", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.ResponseMusics", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.SubButtons", "ButtonID", "dbo.Buttons");
            DropForeignKey("dbo.ResponseMessages", "ResponseKeyRuleID", "dbo.ResponseKeyRules");
            DropForeignKey("dbo.ResponseMessages", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.ResponseMessages", "ResponseVideoID", "dbo.ResponseVideos");
            DropForeignKey("dbo.ResponseMessages", "ResponseImageTextID", "dbo.ResponseImageTexts");
            DropForeignKey("dbo.ResponseMessages", "ResponseImageID", "dbo.ResponseImages");
            DropForeignKey("dbo.ResponseMessages", "ResponseMusicID", "dbo.ResponseMusics");
            DropForeignKey("dbo.ResponseMessages", "SubButtonID", "dbo.SubButtons");
            DropForeignKey("dbo.ResponseMessages", "ButtonID", "dbo.Buttons");
            DropForeignKey("dbo.Buttons", "UserId", "dbo.UserProfile");
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.RecordWUsers");
            DropTable("dbo.PayCustomers");
            DropTable("dbo.Praises");
            DropTable("dbo.Reads");
            DropTable("dbo.CameraLogs");
            DropTable("dbo.QItemAnswers");
            DropTable("dbo.SetQuestions");
            DropTable("dbo.QuestionCategories");
            DropTable("dbo.Prefers");
            DropTable("dbo.Acts");
            DropTable("dbo.PhotoWalls");
            DropTable("dbo.CameraPhotoes");
            DropTable("dbo.SnowErrorLogs");
            DropTable("dbo.SnowItems");
            DropTable("dbo.Snows");
            DropTable("dbo.SnowLogs");
            DropTable("dbo.SnowUsers");
            DropTable("dbo.FightLogs");
            DropTable("dbo.FightUserItems");
            DropTable("dbo.FightItems");
            DropTable("dbo.Fights");
            DropTable("dbo.FightUsers");
            DropTable("dbo.Votes");
            DropTable("dbo.VoteUsers");
            DropTable("dbo.CouponLogs");
            DropTable("dbo.CouponItems");
            DropTable("dbo.Coupons");
            DropTable("dbo.CouponUsers");
            DropTable("dbo.WheelLogs");
            DropTable("dbo.WheelItems");
            DropTable("dbo.Wheels");
            DropTable("dbo.WheelUsers");
            DropTable("dbo.ScratchItems");
            DropTable("dbo.Scratches");
            DropTable("dbo.ScratchUsers");
            DropTable("dbo.webpages_Permission");
            DropTable("dbo.webpages_PermissionsInRoles");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.QrCodes");
            DropTable("dbo.WeixinUserInActivities");
            DropTable("dbo.WeixinUsers");
            DropTable("dbo.WeixinUserInUsers");
            DropTable("dbo.ResponseKeys");
            DropTable("dbo.ResponseKeyRules");
            DropTable("dbo.ResponseVideos");
            DropTable("dbo.GuessUsers");
            DropTable("dbo.Guesses");
            DropTable("dbo.ResponseImageTexts");
            DropTable("dbo.ResponseImages");
            DropTable("dbo.ResponseMusics");
            DropTable("dbo.SubButtons");
            DropTable("dbo.ResponseMessages");
            DropTable("dbo.Buttons");
            DropTable("dbo.UserProfile");
        }
    }
}
