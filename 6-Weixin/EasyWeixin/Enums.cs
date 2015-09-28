namespace EasyWeixin
{
    /// <summary>
    /// 接收消息类型
    /// </summary>
    public enum RequestMsgType
    {
        Text, //文本
        Image, //图片
        Voice, //语音
        Video,//视频
        ShortVideo,//小视频
        Location, //地理位置
        Link, //链接信息
        Event, //事件推送
    }

    /// <summary>
    /// 当RequestMsgType类型为Event时，Event属性的类型
    /// </summary>
    public enum Event
    {
        /// <summary>
        /// 订阅
        /// </summary>
        subscribe,

        /// <summary>
        /// 取消订阅
        /// </summary>
        unsubscribe,

        /// <summary>
        /// 扫描二维码(在用户已经订阅的情况下)
        /// </summary>
        SCAN,

        /// <summary>
        /// 地理位置
        /// </summary>
        LOCATION,

        /// <summary>
        /// 自定义菜单点击事件
        /// </summary>
        CLICK,

        /// <summary>
        /// 自定义菜单点击事件跳转链接
        /// </summary>
        VIEW,

        /// <summary>
        /// 事件推送群发结果
        /// </summary>
        MASSSENDJOBFINISH,

        /// <summary>
        /// 模板信息发送完成
        /// </summary>
        TEMPLATESENDJOBFINISH,

        /// <summary>
        /// 扫码推事件
        /// </summary>
        scancode_push,

        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框
        /// </summary>
        scancode_waitmsg,

        /// <summary>
        /// 弹出系统拍照发图
        /// </summary>
        pic_sysphoto,

        /// <summary>
        /// 弹出拍照或者相册发图
        /// </summary>
        pic_photo_or_album,

        /// <summary>
        /// 弹出微信相册发图器
        /// </summary>
        pic_weixin,

        /// <summary>
        /// 弹出地理位置选择器
        /// </summary>
        location_select,

        /// <summary>
        /// 卡券通过审核
        /// </summary>
        card_pass_check,

        /// <summary>
        /// 卡券未通过审核
        /// </summary>
        card_not_pass_check,

        /// <summary>
        /// 领取卡券
        /// </summary>
        user_get_card,

        /// <summary>
        /// 删除卡券
        /// </summary>
        user_del_card,

        /// <summary>
        /// 多客服接入会话
        /// </summary>
        kf_create_session,

        /// <summary>
        /// 多客服关闭会话
        /// </summary>
        kf_close_session,

        /// <summary>
        /// 多客服转接会话
        /// </summary>
        kf_switch_session,

        /// <summary>
        /// 进入会话（似乎已从官方API中移除）
        /// </summary>
        ENTER
    }

    /// <summary>
    /// 发送消息类型
    /// </summary>
    public enum ResponseMsgType
    {
        Text,
        News,
        Music,
        Image,
        Video
    }

    /// <summary>
    /// 菜单按钮类型
    /// </summary>
    public enum ButtonType
    {
        click,

        /// <summary>
        /// 暂时已被禁用
        /// </summary>
        view
    }

    /// <summary>
    /// 上传媒体文件类型
    /// </summary>
    public enum UploadMediaFileType
    {
        /// <summary>
        /// 图片
        /// </summary>
        image,

        /// <summary>
        /// 语音
        /// </summary>
        voice,

        /// <summary>
        /// 视频
        /// </summary>
        video,

        /// <summary>
        /// thumb
        /// </summary>
        thumb
    }

    /// <summary>
    /// 返回码（JSON）
    /// </summary>
    public enum ReturnCode
    {
        系统繁忙 = -1,
        请求成功 = 0,
        验证失败 = 40001,
        不合法的凭证类型 = 40002,
        不合法的OpenID = 40003,
        不合法的媒体文件类型 = 40004,
        不合法的文件类型 = 40005,
        不合法的文件大小 = 40006,
        不合法的媒体文件id = 40007,
        不合法的消息类型 = 40008,
        不合法的图片文件大小 = 40009,
        不合法的语音文件大小 = 40010,
        不合法的视频文件大小 = 40011,
        不合法的缩略图文件大小 = 40012,
        不合法的APPID = 40013,

        //不合法的access_token      =             40014,
        不合法的access_token = 40014,

        不合法的菜单类型 = 40015,

        //不合法的按钮个数             =          40016,
        //不合法的按钮个数              =         40017,
        不合法的按钮个数1 = 40016,

        不合法的按钮个数2 = 40017,
        不合法的按钮名字长度 = 40018,
        不合法的按钮KEY长度 = 40019,
        不合法的按钮URL长度 = 40020,
        不合法的菜单版本号 = 40021,
        不合法的子菜单级数 = 40022,
        不合法的子菜单按钮个数 = 40023,
        不合法的子菜单按钮类型 = 40024,
        不合法的子菜单按钮名字长度 = 40025,
        不合法的子菜单按钮KEY长度 = 40026,
        不合法的子菜单按钮URL长度 = 40027,
        不合法的自定义菜单使用用户 = 40028,
        缺少access_token参数 = 41001,
        缺少appid参数 = 41002,
        缺少refresh_token参数 = 41003,
        缺少secret参数 = 41004,
        缺少多媒体文件数据 = 41005,
        缺少media_id参数 = 41006,
        缺少子菜单数据 = 41007,
        access_token超时 = 42001,
        需要GET请求 = 43001,
        需要POST请求 = 43002,
        需要HTTPS请求 = 43003,
        多媒体文件为空 = 44001,
        POST的数据包为空 = 44002,
        图文消息内容为空 = 44003,
        多媒体文件大小超过限制 = 45001,
        消息内容超过限制 = 45002,
        标题字段超过限制 = 45003,
        描述字段超过限制 = 45004,
        链接字段超过限制 = 45005,
        图片链接字段超过限制 = 45006,
        语音播放时间超过限制 = 45007,
        图文消息超过限制 = 45008,
        接口调用超过限制 = 45009,
        创建菜单个数超过限制 = 45010,
        不存在媒体数据 = 46001,
        不存在的菜单版本 = 46002,
        不存在的菜单数据 = 46003,
        解析JSON_XML内容错误 = 47001
    }
}