namespace EasyWeixin.Web.Models
{
    public class OAuth_Token
    {
        public OAuth_Token()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        //access_token	网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        //expires_in	access_token接口调用凭证超时时间，单位（秒）
        //refresh_token	用户刷新access_token
        //openid	用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
        //scope	用户授权的作用域，使用逗号（,）分隔
        public string access_token { get; set; }

        public string expires_in { get; set; }

        public string refresh_token { get; set; }

        public string openid { get; set; }

        public string scope { get; set; }
    }

    public class OAuthUser
    {
        public OAuthUser()
        { }

        #region 数据库字段

        private string _openID;
        private string _searchText;
        private string _nickname;
        private string _sex;
        private string _province;
        private string _city;
        private string _country;
        private string _headimgUrl;
        private string _privilege;

        #endregion 数据库字段

        #region 字段属性

        /// <summary>
        /// 用户的唯一标识
        /// </summary>
        public string openid
        {
            set { _openID = value; }
            get { return _openID; }
        }

        /// <summary>
        ///
        /// </summary>
        public string SearchText
        {
            set { _searchText = value; }
            get { return _searchText; }
        }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string nickname
        {
            set { _nickname = value; }
            get { return _nickname; }
        }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public string sex
        {
            set { _sex = value; }
            get { return _sex; }
        }

        /// <summary>
        /// 用户个人资料填写的省份
        /// </summary>
        public string province
        {
            set { _province = value; }
            get { return _province; }
        }

        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        public string city
        {
            set { _city = value; }
            get { return _city; }
        }

        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        public string country
        {
            set { _country = value; }
            get { return _country; }
        }

        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        public string headimgurl
        {
            set { _headimgUrl = value; }
            get { return _headimgUrl; }
        }

        public string Privilege
        {
            set { _privilege = value; }
            get { return _privilege; }
        }

        #endregion 字段属性
    }
}