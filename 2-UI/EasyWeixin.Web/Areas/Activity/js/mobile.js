var $groupId;
$(function () {
    QueryString.Initial();
    $groupId = QueryString.GetValue('groupId');
    LinkSignalr();
});

function LinkSignalr() {
    //建立与signalr服务器之间的连接，等待收到指令

    //var connection = $.hubConnection("/signalr", { useDefaultPath: false });
    var connection = $.hubConnection();
    var awardHubProxy = connection.createHubProxy('awardHub');
    //这个方法是等到手机连接好了之后然后群发的
    awardHubProxy.on('startAward', function (awardNum) {
        startAward(awardNum);
    });
    awardHubProxy.on('showErrorMessage', function (message) {
        alert(message);
    });
    awardHubProxy.on('checkSend', function (data) {
        if ($groupId == data)
            awardHubProxy.invoke('send');
        else
            alert("groupId错误");
    });

    //awardHubProxy.state.groupId = $groupId;
    connection.qs = { 'groupId': $groupId, 'mobile': 'mobile' };
    if ($groupId == undefined || $groupId == '') {
        alert("请扫描正确的二维码！");
    }
    connection.start();
};
QueryString = {
    data: {},
    Initial: function () {
        var aPairs, aTmp;
        var queryString = new String(window.location.search);
        queryString = queryString.substr(1, queryString.length); //remove   "?"
        aPairs = queryString.split("&");
        for (var i = 0; i < aPairs.length; i++) {
            aTmp = aPairs[i].split("=");
            this.data[aTmp[0]] = aTmp[1];
        }
    },
    GetValue: function (key) {
        return this.data[key];
    }
}

/*  抽奖 */
var iNow = 0;                               //当前值
iCount = $(".lottery-tab td").length - 1, //中奖的总个数
iSpeed = 100,                           //速度
iCycle = 60,                            //至少步数
iStepNow = 0,                           //当前步
timer = null,                           //定时器
iKey = 0,                               //中奖值
iOldKey = 0,                            //前一个中奖位置
bMove = true,                           //是否在抽奖
arrLottery = [];                        //中奖数组

for (var i = 0; i < iCount; i++) {
    arrLottery.push($(".tab-" + i)[0]);// 排序数组
};
function startAward(num) {
    if (bMove) {
        iKey = num;
        setClass();
        bMove = false;
    };
    return false;
}
//$(".start-btn").on("click",function(){
//    if(bMove){
//        setClass();
//        iKey=Math.floor(Math.random()*11)+1;
//        bMove=false;
//        console.log("当前中奖："+iKey+" 上一个是："+iOldKey);
//    };
//    return false;
//});
function setClass() {
    iNow++;
    iStepNow++;
    if (iStepNow == iCycle + iKey - iOldKey) {
        clearTimeout(timer);
        iSpeed = 100;
        iStepNow = 0;
        iOldKey = iKey;       //初始化
        setTimeout(function () {
            bMove = true;
            awardShow(iNow);
        }, 200);
    } else {
        if (iStepNow < iCycle + iKey - iOldKey - 10) {
            iSpeed -= 10;       //加速
        } else if (iStepNow > iCycle + iKey - iOldKey - 10) {
            iSpeed += 70;       //减速
            if (iSpeed > 500) {
                iSpeed = 500;   //结尾固定减速
            };
        };
        if (iNow >= iCount) {
            iNow = 0;           //超过还原值;
        };
        if (iSpeed < 40) {
            iSpeed = 40;        //中间固定速度;
        };
        $(arrLottery).removeClass("focus");
        $(arrLottery).eq(iNow).addClass("focus");
        timer = setTimeout(setClass, iSpeed);
    };
};

var awardText;
function awardShow(num) {
    awardText = $(arrLottery).eq(num - 1).text();
    $(".award-info").text(awardText);
    $(".award-wrap").show();
    $(".award-wrap").bind("click", function (e) {
        $(this).hide();
        return false;
    });
};