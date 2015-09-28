<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Scan.aspx.cs" Inherits="EasyWeixin.Web.Scan" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>扫描二维码</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <div class="btn btn_primary" style="height: 100px; width: 100%; font-size: 20px" id="scanQRCode1">
        scanQRCode(直接返回结果)
    </div>
    <br />
    <hr />
    <div class="btn btn_primary" style="height: 100px; width: 100%; font-size: 20px; align-content: center" id="reload">
        刷新页面
    </div>
    <img src="<%=ImgSrc %>" style="margin-top: 20px; width: 200px; height: 200px; align-self: center" />
</body>
<script src="Scripts/jquery-1.7.2.min.js"></script>
<script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
<script>
    $('#reload').on('click', function () {
        document.location.reload();
    });
    $(function () {
        wx.config({
            debug: false,
            appId: "<%=WxConfig.appid%>",
            timestamp: "<%=WxConfig.timestamp%>",
            nonceStr: "<%=WxConfig.noncestr%>",
            signature: "<%=WxConfig.signature%>",
            jsApiList: [
                'checkJsApi',
                'onMenuShareTimeline',
                'onMenuShareAppMessage',
                'onMenuShareQQ',
                'onMenuShareWeibo',
                'hideMenuItems',
                'showMenuItems',
                'hideAllNonBaseMenuItem',
                'showAllNonBaseMenuItem',
                'translateVoice',
                'startRecord',
                'stopRecord',
                'onRecordEnd',
                'playVoice',
                'pauseVoice',
                'stopVoice',
                'uploadVoice',
                'downloadVoice',
                'chooseImage',
                'previewImage',
                'uploadImage',
                'downloadImage',
                'getNetworkType',
                'openLocation',
                'getLocation',
                'hideOptionMenu',
                'showOptionMenu',
                'closeWindow',
                'scanQRCode',
                'chooseWXPay',
                'openProductSpecificView',
                'addCard',
                'chooseCard',
                'openCard'
            ]
        });
        wx.ready(function () {
            wx.hideOptionMenu();
        });
        document.querySelector('#scanQRCode1').onclick = function () {
            wx.scanQRCode({
                needResult: 1,
                desc: 'scanQRCode desc',
                success: function (res) {
                    var result = res.resultStr;
                    alert(result);
                }
            });
        };
    });
</script>
</html>
