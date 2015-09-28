/*
 * Author   :陈翊
 * Time		:2014年10月20日
 * Abstract	:微信游戏——恶魔快跑
 */

window.onload = function () {
    //by tianxiu
    var datas = JSON.parse($("#Datas").val());
    var savedatas = [];
    var k = 0;
    var IsAward = parseInt(datas[0].IsAward);
    for (var i = 0; i < 60; i++) {
        var int = Math.round(Math.random() * 100);
        var data = { Score: int, IsAward: 100 }
        datas.splice(datas.length, 0, data)
    }

    //游戏页
    var viewWidth = document.documentElement.clientWidth;
    var oSnowBox = document.getElementById("snowBox");
    oSnowBox.style.height = document.documentElement.clientHeight - 40 + "px";
    window.onresize = function () {
        oSnowBox.style.height = document.documentElement.clientHeight - 40 + "px";
        return viewWidth = document.documentElement.clientWidth;
    }
    //画雪花
    var timerDown = null;
    var timerSnow = null;
    var timerMove = null;
    var n = 0;
    var g = 100;
    var w1 = 100;
    var oCount = document.getElementById("count");
    var iCount = 0;
    var oTime = document.getElementById("time");
    var iTime = 30;
    var z = 0;

    //游戏结束 提交成绩
    var gameOver = document.getElementById("gameOver");
    var btnSubmit = document.getElementById("submit");
    var totalP = document.getElementById("totalP");
    var submitInfo = document.getElementById("submitInfo");
    var btnSure = document.getElementById("sure");

    //by tianxiu
    var btnSave = document.getElementById("yes");

    //by tianxiu
    btnSave.onclick = function () {
        var Phone = $("#Phone").val();
        var UserName = $("#UserName").val();
        //var CardId = $("#CardId").val();
        if (Phone == "" || UserName == "") {
            alert("用户名和手机号都不得为空！");
        } else if (!(/^1[3|4|5|8][0-9]\d{8}$/.test(Phone))) {
            alert("手机号的格式不对！");
            //} else if (!(/(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/.test(CardId))) {
            //    alert("身份证输入不合法");
        } else {
            $.post("/ActivityGhost/SaveSnowGameData", { datas: JSON.stringify(savedatas), UserName: UserName, Phone: Phone, SnowID: $("#SnowID").val() }, function (data) {
                alert(data.Message);
                location.href = "/ActivityGhost/GhostRank?SnowID=" + $("#SnowID").val();
            });
        }
    }
    btnSubmit.onclick = function () {
        startMove(gameOver, { top: -400, opacity: 0 });
        startMove(submitInfo, { top: 40, opacity: 100 });
    }

    timerSnow = setInterval(function () {
        z = Math.round(Math.random() * 3) + 1;
        oSnowBox.innerHTML += '<li style="background-image:url(../images/Activity/Ghost/fruit_0' + z + '.png);"><span></span></li>';
        var wNow = Math.round(Math.random() * 100);
        if (wNow < 50) {
            wNow = 50;
        }
        var lNow = Math.round(Math.random() * viewWidth);
        var aSnow = oSnowBox.getElementsByTagName("li");
        aSnow[n].style.width = 2 * wNow + "px"; //随机宽度
        aSnow[n].style.height = wNow + "px"; //随机高度
        aSnow[n].style.left = lNow + "px";
        n++;
        if (n == 100) {
            clearInterval(timerSnow);
            //倒计时
            timerDown = setInterval(function () {
                iTime--;
                if (iTime < 10) {
                    iTime = "0" + iTime;
                }
                oTime.innerHTML = "00:" + iTime;
                if (iTime == "00") {
                    clearInterval(timerDown);
                    clearInterval(timerMove);
                    totalP.innerHTML = iCount;
                    startMove(gameOver, { top: 40, opacity: 100 });
                }
            }, 1000)
            //雪花运动
            var aSnow = oSnowBox.getElementsByTagName("li");
            var aKnife = oSnowBox.getElementsByTagName("span");
            var m = 0;
            timerMove = setInterval(function () {
                var disL = Math.round(Math.random() * 200 - 100);
                snowMove(aSnow[m], { top: oSnowBox.offsetHeight + 100, left: aSnow[m].offsetLeft + disL });
                m++;
                if (m == 130) {
                    clearInterval(timerMove);
                }
            }, 500);
            for (var i = 0; i < aSnow.length ; i++) {
                aSnow[i].boole = 0;
                aSnow[i].index = i;

                aSnow[i].addEventListener('touchstart', function (event) {
                    if (this.boole == 0) {
                        this.boole = 1;
                        //var int = Math.round(Math.random() * 10);
                        //clearInterval(this.timer);
                        //this.getElementsByTagName("span")[0].style.display = "block";

                        //this.style.background = "block";
                        //this.className = "active";
                        //setTimeout(function () {
                        //    this.style.display = "none";
                        //}, 2000)
                        //iCount += int;
                        //oCount.innerHTML = iCount;
                        //totalP.innerHTML = iCount;

                        //var int = Math.round(Math.random() * (datas.length));
                        //var Score = parseInt(datas[int].Score);
                        //clearInterval(this.timer);
                        //this.innerHTML = Score;
                        //this.style.background ="none";
                        ////aKnife[this.index].style.display = "block";
                        //this.getElementsByTagName("span")[0].style.display = "block";
                        ////this.innerHTML = int ;
                        //this.className = "active" ;
                        //setTimeout(function(){
                        //	this.style.display = "none" ;
                        //},1000)
                        //iCount += Score;
                        //oCount.innerHTML = iCount;

                        ////by tianxiu
                        //totalP.innerHTML = iCount;
                        //k += 1;
                        //if (k == 5 && IsAward > 0 && IsAward < 100) {
                        //    startMove(giftBox, { top: 80, opacity: 100 });
                        //}
                        //savedatas.splice(savedatas.length, 0, datas[int])
                        //datas.splice(int, 1);
                        var int = Math.round(Math.random() * (datas.length));
                        var Score = parseInt(datas[int].Score);
                        clearInterval(this.timer);
                        this.getElementsByTagName("span")[0].style.display = "block";

                        //this.style.background = "block";
                        //aKnife[this.index].style.display = "block";
                        //this.innerHTML = Score;
                        this.className = "active";
                        setTimeout(function () {
                            this.style.display = "none";
                        }, 2000)
                        iCount += Score;
                        oCount.innerHTML = iCount;
                        totalP.innerHTML = iCount;
                        k += 1;
                        if (k == 5 && IsAward > 0 && IsAward < 100) {
                            startMove(giftBox, { top: 80, opacity: 100 });
                        }
                        savedatas.splice(savedatas.length, 0, datas[int])
                        datas.splice(int, 1);
                    }
                }, false);
                aSnow[i].addEventListener('touchend', function (event) {
                    aSnow[i].removeEventListener('touchstart', false);
                }, false);
            }
        }
    }, 10);

    //雪花运动框架
    function snowMove(obj, json, fnEnd) {
        clearInterval(obj.timer);
        obj.timer = setInterval(function () {
            var bStop = true;
            for (var attr in json) {
                var cur = parseInt(getStyle(obj, attr));
                var speed = (json[attr] - cur) / 60;
                speed = speed > 0 ? Math.ceil(speed) : Math.floor(speed);
                if (cur != json[attr]) {  //若果有cur ！= json[attr] (目标值)
                    bStop = false;
                    obj.style[attr] = speed + cur + "px";
                }
            }
            if (bStop) { //如果所有的cur 都等于 json[attr] ;
                clearInterval(obj.timer);
                if (fnEnd) fnEnd();
            }
            setTimeout(function () {
                obj.style.display = "none";
                clearInterval(obj.timer);
            }, 3000)
        }, 30);
    }
}
//完美运动框架
function startMove(obj, json, fnEnd) {
    clearInterval(obj.timer);
    obj.timer = setInterval(function () {
        var bStop = true;
        for (var attr in json) { //循环json里面的数值
            var cur = 0;
            if (attr == "opacity") {
                cur = Math.round(parseFloat(getStyle(obj, "opacity")) * 100);
            } else {
                cur = parseInt(getStyle(obj, attr));
            }
            var speed = (json[attr] - cur) / 5;

            speed = speed > 0 ? Math.ceil(speed) : Math.floor(speed);
            if (cur != json[attr]) {  //若果有cur ！= json[attr] (目标值)
                bStop = false;
                if (attr == "opacity") {
                    obj.style["filter"] = "alpha(opacity:" + (speed + cur) + ")";
                    obj.style["opacity"] = (speed + cur) / 100;
                } else {
                    obj.style[attr] = speed + cur + "px";
                }
            }
        }
        if (bStop) { //如果所有的cur 都等于 json[attr] ;
            clearInterval(obj.timer);
            if (fnEnd) fnEnd();
        }
    }, 30)
}
//获取行间样式
function getStyle(obj, attr) {
    if (obj.currentStyle) {
        return obj.currentStyle[attr];
    } else {
        return getComputedStyle(obj, false)[attr];
    }
}