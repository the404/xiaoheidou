using EasyWeixin.Model;
using EasyWeixin.Model.RelayRace;
using System.Data.Entity;

namespace EasyWeixin.Data
{
    public class EasyWeixinDbContext : DbContext
    {
        public EasyWeixinDbContext()
            : base("EasyWeixinDbContext")
        {
        }

        //public EasyWeixinDbContext(string nameOrConnectionString)
        //    : base(EFTracingUtil.GetConnection(nameOrConnectionString), true)
        //{
        //    var ctx = ((IObjectContextAdapter)this).ObjectContext;

        //    //ctx.GetTracingConnection().CommandExecuting += (s, e) =>
        //    //{
        //    //    Debug.WriteLine(e.ToTraceString());
        //    //};
        //}

        #region 过去

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<UserMembership> UserMemberships { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<PermissionsInRoles> PermissionsInRoles { get; set; }

        public DbSet<ResponseImage> ResponseImages { get; set; }

        public DbSet<ResponseMusic> ResponseMusics { get; set; }

        public DbSet<ResponseVideo> ResponseVideos { get; set; }

        public DbSet<ResponseMessage> ResponseMessages { get; set; }

        public DbSet<ResponseKey> ResponseKeys { get; set; }

        public DbSet<ResponseImageText> ResponseImageTexts { get; set; }

        public DbSet<SubButton> SubButtons { get; set; }

        public DbSet<Button> Buttons { get; set; }

        public DbSet<ResponseKeyRule> ResponseKeyRules { get; set; }

        public DbSet<GuessUser> GuessUsers { get; set; }

        public DbSet<Guess> Guesses { get; set; }

        public DbSet<ScratchUser> ScratchUsers { get; set; }

        public DbSet<Scratch> Scratchs { get; set; }

        public DbSet<ScratchItem> ScratchItems { get; set; }

        public DbSet<WheelUser> WheelUsers { get; set; }

        public DbSet<Wheel> Wheels { get; set; }

        public DbSet<WheelItem> WheelItems { get; set; }

        public DbSet<WheelLog> WheelLogs { get; set; }

        public DbSet<CouponUser> CouponUsers { get; set; }

        public DbSet<Coupon> Coupons { get; set; }

        public DbSet<CouponLog> CouponLogs { get; set; }

        //by tianxiu 2014-3-18
        public DbSet<CouponItem> CouponItem { get; set; }

        public DbSet<VoteUser> VoteUsers { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public DbSet<FightUser> FightUsers { get; set; }

        public DbSet<FightUserItem> FightUserItems { get; set; }

        public DbSet<Fight> Fights { get; set; }

        public DbSet<FightItem> FightItems { get; set; }

        public DbSet<FightLog> FightLogs { get; set; }

        public DbSet<SnowUser> SnowUsers { get; set; }

        public DbSet<Snow> Snows { get; set; }

        public DbSet<SnowItem> SnowItems { get; set; }

        public DbSet<SnowLog> SnowLogs { get; set; }

        public DbSet<SnowErrorLog> SnowErrorLogs { get; set; }

        //by tianxiu 2014-11-11
        public DbSet<CameraPhoto> CameraPhotos { get; set; }

        public DbSet<PhotoWall> PhotoWalls { get; set; }

        public DbSet<Act> Acts { get; set; }

        public DbSet<Prefer> Prefers { get; set; }

        public DbSet<QuestionCategory> QuestionCategorys { get; set; }

        public DbSet<SetQuestion> SetQuestions { get; set; }

        public DbSet<QItemAnswer> QItemAnswers { get; set; }

        public DbSet<CameraLog> CameraLog { get; set; }

        public DbSet<Read> Read { get; set; }

        public DbSet<Praise> Praise { get; set; }

        public DbSet<PayCustomer> PayCustomer { get; set; }

        public DbSet<RecordWUser> RecordWUser { get; set; }

        #endregion 过去

        //by zhangrong 2015-04-01 添加微信普通用户信息
        public DbSet<WeixinUser> WeixinUsers { get; set; }

        public DbSet<WeixinUserInUsers> WeixinUserInUsers { get; set; }

        public DbSet<WeixinUserInActivity> WeixinUserInActivities { get; set; }

        public DbSet<QrCode> QrCodes { get; set; }

        public DbSet<MainRace> MainRaces { get; set; }
        public DbSet<SubRace> SubRaces { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMembership>()
                .HasMany<Role>(r => r.Roles)
                .WithMany(u => u.UserMemberships)
                .Map(m =>
                {
                    m.ToTable("webpages_UsersInRoles");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("RoleId");
                });
        }
    }
}