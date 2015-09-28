$(function () {
    $("#scratchCard p").show();

    //设置底图文本位置
    var oText = $("#scratchCard p");
    var oLeft = -oText.width() / 2 - 10,
        oTop = -oText.height() / 2 + 10;
    oText.css({ "marginLeft": oLeft, "marginTop": oTop });

    //邀请好友、活动规则按钮点击事件
    var oRuleBtn = $(".rule_btn"),
        oInviteBtn = $(".invite_btn"),
        oLine = $(".line"),
        oRuleBox = $(".rule_box");
    $(".rule_btn").click(function () {
        $(".line , .rule_box").toggle();
    });
    oInviteBtn.click(function () {
        $("html,body").animate({ scrollTop: "0px" });
        $(".shadow").show();
        $(".invite").show();
    });
    $(".shadow").click(function () {
        $(this).hide();
        $(".invite").hide();
    });
});