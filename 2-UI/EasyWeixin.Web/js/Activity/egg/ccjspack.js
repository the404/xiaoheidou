/*
 * Abstract   ：原生javascript函数封装
 * Author     ：蔡应
 * Time       ：2013年10月30日17:43:48
 */

//根据Class名获取元素 需要父元素
function getByClass(oParent, sClass) {  //oParent 父元素
    var aEle = oParent.getElementsByTagName("*"); //获取父元素oParent 下所有的元素
    var aResult = [];

    for (var i = 0; i < aEle.length ; i++) {
        if (aEle[i].className == sClass) {
            aResult.push(aEle[i]);
        }
    }
    return aResult;
}

//事件绑定，避免和其他人写的相同事件冲突
function myAddEvent(obj, ev, fn) {
    if (obj.attachEvent) {
        obj.attachEvent("on" + ev, fn);
    } else {
        obj.addEventListener(ev, fn, false);
    }
}

//根据Class名获取元素不需要父元素
function getByClass02(sClass) {  //oParent 父元素
    var aEle = document.getElementsByTagName("*"); //获取父元素oParent 下所有的元素
    var aResult = [];

    for (var i = 0; i < aEle.length ; i++) {
        if (aEle[i].className == sClass) {
            aResult.push(aEle[i]);
        }
    }
    return aResult;
}

//完美运动框架
function startMove(obj, json, mul, fnEnd) {
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
            var speed = (json[attr] - cur) / mul;

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
//完美运动框架
function startMove03(obj, json, mul, fnEnd) {
    clearInterval(obj.timer);
    alert(obj);
    obj.timer = setInterval(function () {
        var bStop = true;
        for (var attr in json) { //循环json里面的数值
            var cur = 0;

            cur = parseInt(getStyle(obj, attr));

            var speed = (json[attr] - cur) / mul;

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
    }, 30)
}
//匀速运动框架
function startMove02(obj, json, speed, fnEnd) {
    clearInterval(obj.timer);
    obj.timer = setInterval(function () {
        var bStop = true;
        for (var attr in json) {
            var cur = parseInt(getStyle(obj, attr));

            if (cur != json[attr]) {
                if (json[attr] >= 0) {
                    if (cur + speed > json[attr]) {
                        speed = json[attr] - cur;
                    }
                    bStop = false;
                    obj.style[attr] = cur + speed + "px";
                } else {
                    if (cur - speed < json[attr]) {
                        speed = cur - json[attr];
                    }
                    bStop = false;
                    obj.style[attr] = cur - speed + "px";
                }
            }
        }
        if (bStop) { //如果所有的cur 都等于 json[attr] ;
            clearInterval(obj.timer);
            if (fnEnd) { fnEnd(); }
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

//获取鼠标相对于整个页面的位置
function getPos(ev) {
    var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
    var scrollLeft = document.documentElement.scrollLeft || document.body.scrollLeft;

    return { x: ev.clientX + scrollLeft, y: ev.clientY + scrollTop };
}

//滚动条滚动 缓动
function startMoveScrollTop(mul, iTarget) {
    var timerScroll = null;
    timerScroll = setInterval(function () {
        var bStop = true;
        var cur = document.documentElement.scrollTop || document.body.scrollTop;
        var speed = (iTarget - cur) / mul;

        speed = speed > iTarget ? Math.ceil(speed) : Math.floor(speed);

        if (cur != iTarget) {
            bStop = false;
            document.body.scrollTop = speed + cur;
            document.documentElement.scrollTop = speed + cur;
        } else {
            clearInterval(timerScroll);
        }

        myAddEvent(window, "mousewheel", function () {
            clearInterval(timerScroll);
        });
        /*myAddEvent( document.documentElement,"mousewheel",function(){
			clearInterval(timerScroll);
		});*/
    }, 30)
}

//滚动条滚动 匀速
function startMoveScrollTop(mul, iTarget) {
    var timerScroll = null;

    timerScroll = setInterval(function () {
        var bStop = true;
        var cur = document.documentElement.scrollTop || document.body.scrollTop;
        var speed = (iTarget - cur) / mul;

        speed = speed > iTarget ? Math.ceil(speed) : Math.floor(speed);

        if (cur != iTarget) {
            bStop = false;
            document.body.scrollTop = speed + cur;
            document.documentElement.scrollTop = speed + cur;
        } else {
            clearInterval(timerScroll);
        }

        myAddEvent(window, "mousewheel", function () {
            clearInterval(timerScroll);
        });
        /*myAddEvent( document.documentElement,"mousewheel",function(){
			clearInterval(timerScroll);
		});*/
    }, 30)
}

//css3设置
function setCss3(obj, attrObj) {
    for (var i in attrObj) {
        var newi = i;
        if (newi.indexOf("-") > 0) {
            var num = newi.indexOf("-");
            newi = newi.replace(newi.substr(num, 2), newi.substr(num + 1, 1).toUpperCase());
        }
        obj.style[newi] = attrObj[i];
        newi = newi.replace(newi.charAt(0), newi.charAt(0).toUpperCase());
        obj.style["webkit" + newi] = attrObj[i];
        obj.style["moz" + newi] = attrObj[i];
        obj.style["o" + newi] = attrObj[i];
        obj.style["ms" + newi] = attrObj[i];
    }
}