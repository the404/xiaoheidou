/*
 * Author   :蔡应
 * Time		:2013年11月29日16:31:32
 * Abstract	:上海欢乐谷微信游戏——疯狂砸金蛋
 */

window.onload = function () {
    var datas = JSON.parse($("#Datas").val());
    var savedatas = [];
    var k = 0;
    var IsAward = parseInt(datas[0].IsAward);
    // alert(JSON.stringify(datas));
    for (var i = 0; i < 60; i++) {
        var int = Math.round(Math.random() * 100);
        var data = { Score: int, IsAward: 100 };
        datas.splice(datas.length, 0, data);
    }

    //游戏页
    var viewWidth = document.documentElement.clientWidth;
    var viewHeight = document.documentElement.clientHeight;
    var oSnowBox = document.getElementById("snowBox");
    //alert( viewWidth );
    oSnowBox.style.height = document.documentElement.clientHeight - 48 + "px";
    window.onresize = function () {
        oSnowBox.style.height = document.documentElement.clientHeight - 48 + "px";
        viewHeight = document.documentElement.clientHeight;
        return viewWidth = document.documentElement.clientWidth;
    }
    //画金蛋
    var timerDown = null;
    var timerSnow = null;
    var timerMove = null;
    var n = 0;
    var g = 200;
    var w1 = 100;
    var oCount = document.getElementById("count");
    var iCount = 0;
    var oTime = document.getElementById("time");
    var iTime = 30;

    //游戏结束 提交成绩

    var gameOver = document.getElementById("gameOver");
    var btnSubmit = document.getElementById("submit");
    var totalP = document.getElementById("totalP");
    var submitInfo = document.getElementById("submitInfo");
    var btnSure = document.getElementById("sure");
    var giftBox = document.getElementById("giftBox");
    var btnSave = document.getElementById("yes");

    btnSave.onclick = function () {
        var Phone = $("#Phone").val();
        var UserName = $("#UserName").val();
        if (Phone == "" || UserName == "") {
            alert("用户名和手机号都不得为空！");
        } else if (!(/^1[3|4|5|8][0-9]\d{8}$/.test(Phone))) {
            alert("手机号的格式不对！");
        } else {
            $.post("/ActivitySnowTest/SaveSnowGameData", { datas: JSON.stringify(savedatas), UserName: UserName, Phone: Phone, SnowID: $("#SnowID").val() }, function (data) {
                alert(data.Message);
                location.href = "/ActivitySnowTest/SnowRank?SnowID=" + $("#SnowID").val();
            });
        }
    };

    btnSure.onclick = function () {
        startMove(giftBox, { top: -400, opacity: 0 });
    }

    btnSubmit.onclick = function () {
        startMove(submitInfo, { top: 80, opacity: 100 });
    }

    timerSnow = setInterval(function () {
        oSnowBox.innerHTML += "<li></li>";
        var wNow = Math.round(Math.random() * 100);
        if (wNow < 50) {
            wNow = 50;
        }
        var lNow = Math.round(Math.random() * viewWidth);
        var tNow = Math.round(Math.random() * (viewHeight - 40));
        var aSnow = oSnowBox.getElementsByTagName("li");
        aSnow[n].style.width = aSnow[n].style.height = wNow + "px";
        aSnow[n].style.top = tNow + "px";
        aSnow[n].style.left = lNow + "px";

        n++;
        if (n == g) {
            clearInterval(timerSnow);
            aSnow = oSnowBox.getElementsByTagName("li");
            var timerDis = null;
            var m = 0;
            //倒计时
            timerDown = setInterval(function () {
                iTime--;
                if (iTime < 10) {
                    iTime = "0" + iTime;
                }
                oTime.innerHTML = "00:" + iTime;
                if (iTime == "00") {
                    clearInterval(timerDown);
                    clearInterval(timerDis);
                    totalP.innerHTML = iCount;
                    startMove(gameOver, { top: 80, opacity: 100 });
                }
            }, 1000);

            timerDis = setInterval(function () {
                aSnow[m].style.display = "block";
                aSnow[m].timer = setTimeout(function () {
                    if (m >= 8) {
                        aSnow[m - 8].style.display = "none";
                    }
                }, 2000);
                m++;
                if (m == g) {
                    clearInterval(timerDis);
                }
            }, 200);
            for (var i = 0; i < aSnow.length ; i++) {
                aSnow[i].boole = 0;
                aSnow[i].addEventListener('touchstart', function (event) {
                    if (this.boole == 0) {
                        this.boole = 1;
                        var int = Math.round(Math.random() * (datas.length));
                        var Score = parseInt(datas[int].Score);
                        clearInterval(this.timer);
                        this.style.background = "none";
                        this.innerHTML = Score;
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
    }, 10)

    //雪花运动框架
    function snowMove(obj, json, fnEnd) {
        clearInterval(obj.timer);
        obj.timer = setInterval(function () {
            var bStop = true;
            for (var attr in json) {
                var cur = parseInt(getStyle(obj, attr));
                var speed = (json[attr] - cur) / 100;
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
                //obj.className = "disImg";

                /*startMove( obj , { opacity :0 },function(){
                    obj.style.display = "none" ;
                    setTimeout(function(){clearInterval( obj.timer);},1000);
                });*/
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