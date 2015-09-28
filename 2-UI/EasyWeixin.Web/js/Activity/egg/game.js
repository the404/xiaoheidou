/*
 * Abstract   ：微信游戏--砸金蛋
 * Author     ：蔡应
 * Time       ：2015年1月30日16:53:10
 */

//	设置页面缩放
fnScale();
function fnScale() {
    var pageWrap = document.getElementById("page_wrap");
    iScale = document.body.clientWidth / 320;

    pageWrap.style.transform = "scale(" + iScale + ")";
    pageWrap.style.webkitTransform = "scale(" + iScale + ")";
}

window.onresize = function () {
    fnScale();
}
//	砸金蛋
var iWin = 1;
var oUrl = window.location.href;
console.log(oUrl);
fnSmashEgg();
function fnSmashEgg() {
    var parEgg = document.getElementById("egg_list");
    var aEgg = parEgg.getElementsByTagName("li");
    var aEggBreak = parEgg.getElementsByTagName("span");
    var oHammer = document.getElementById("hammer");
    var iThis = i;
    var eggBreak = 0;
    if (parseInt($("#Isza").val()) >= 0) {
        eggBreak = parseInt($("#Isza").val());
    }

    for (var i = aEgg.length - 1; i >= 0; i--) {
        aEgg[i].index = i;
        aEgg[i].addEventListener('touchstart', function (event) {
            iThis = this.index;
            if (eggBreak) {
                aEggBreak[iThis].style.webkitTransform = "rotate(-5deg)";
                //eggBreak = 0;
                eggBreak = 0;
                // console.log(aEggBreak[iThis].offsetLeft);
                // console.log(aEggBreak[iThis].offsetTop);

                oHammer.style.left = aEggBreak[iThis].offsetLeft + 20 + "px";
                oHammer.style.top = aEggBreak[iThis].offsetTop - 30 + "px";

                //by tianxiu
                var WheelID = $("#WheelID").val();
                var UserWexinID = $("#UserWexinID").val();

                $.post("/ActivityEgg/GetEggitem", { WheelID: WheelID, UserWexinID: UserWexinID }, function (data) {
                    if (data.message == undefined) {
                        setTimeout(function () {
                            if (data.WheelItemID > 0) {
                                window.location.href = "/ActivityEgg/Eggwinning?sn=" + data.WheelCode + "&nickname=" + data.nickname + "&awardname=" + data.WheelItemName;
                                //win_alert(data.WheelItemName + 'SN码为：' + data.WheelCode, function () {
                                //    $('#PrizeClass').html(data.WheelItemName);
                                //    $('#SnNumber').html(data.WheelCode);
                                //    $('#GetPrize input[name=MobilePhone]').focus();
                                //});
                            } else {
                                window.location.href = "/ActivityEgg/Eggno_winning?nickname=" + data.nickname;
                            }
                        }, 100);
                    } else {
                        alert(data.message);
                        return false;
                    }
                }, 'json');

                //end
            }
            // console.log(1);
        }, false);
    };

    // var touchX = 0;
    // var touchY = 0;

    // parEgg.addEventListener('touchstart',function(event){
    // 	if (event.targetTouches.length == 1){
    // 		var scrollTop = document.documentElement.scrollTop || document.body.scrollTop ;
    // 		console.log(scrollTop);
    // 		touchX = event.changedTouches[0].clientX/iScale;
    // 		touchY = event.changedTouches[0].clientY/iScale-50+scrollTop;
    // 	}
    // },false);
    // parEgg.addEventListener('touchend',function(event){
    // 	oHammer.style.left = touchX+"px";
    // 	oHammer.style.top = touchY+"px";
    // },false);
}

//点击屏幕获取角度
// gameBox.addEventListener('touchstart',function(event){
// 	if (event.targetTouches.length == 1){
// 		clearInterval(timerAngle);
// 	}
// },false);
// gameBox.addEventListener("touchend",function(event){
// 	//return iAngle;
// 	if(iAngle>=0){
// 		iFloor = 18-iAngle/5+iWind;
// 		if( iFloor >18 ){
// 			iFloor = 36-iFloor;
// 			iLeft = 216;
// 		}else if( iFloor <0 ){
// 			iFloor = 0;
// 			iLeft = 0;
// 		}else {
// 			iLeft = 0;
// 		}
// 	}else{
// 		iFloor = -18-iAngle/5-iWind ;
// 		if( iFloor <-18 ){
// 			iFloor = 36+iFloor;
// 			iLeft = 0;
// 		}else if( iFloor >0 ){
// 			iFloor = 0;
// 			iLeft = 216;
// 		}else {
// 			iLeft = 216;
// 		}
// 	}
// 	fnAniGame(iLeft);
// 	//console.log("iAngle:"+(iAngle/5)+"层级:"+(18-Math.abs(iAngle/5))+"风力:"+iWind+"最终层数:"+iFloor+"分数："+iCount);
// })