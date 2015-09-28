$(function () {
    Handler.bindFunctions($('body'));
});

Handler.parallelize = function (selector, callback) {
    if (callback) setTimeout(function () {
        callback(selector);
    });
};

Handler.bindFunctions = function (selector) {
    Handler.parallelize(selector, Handler.setAjax);
    Handler.parallelize(selector, Handler.setPermissionClick);
    Handler.parallelize(selector, Handler.setRoleClick);
    Handler.parallelize(selector, Handler.setAllSelectClick);
    Handler.parallelize(selector, Handler.setDeleteConfirm);
};

Handler.setDeleteConfirm = function (selector) {
    $(".delete").each(function (index, item) {
        $(item).bind("click", function (event) {
            var oThis = this;
            return confirm("确定删除此条记录？");
        });
    });
};

Handler.setPermissionClick = function (selector) {
    var arrPermission = selector.find("#permissionCheck").find("input");
    arrPermission.each(function (index, item) {
        $(item).bind("click", function (event) {
            var sRoleId = $("#hfRoleId").val();
            var value = sRoleId + "," + $(this).val();
            var url = "/Ajax/SetPermission/" + value;
            if (!this.checked) {
                url = "/Ajax/RemovePermission/" + value;
            }

            var objUpdateMessage = $("#updateMessage");
            $.getJSON(url, function (data) {
                if (objUpdateMessage.length > 0) {
                    objUpdateMessage.fadeIn();
                    objUpdateMessage.css("color","green");
                    objUpdateMessage.html("更新成功!");
                    objUpdateMessage.fadeOut("slow");
                }
            });
        });
    });
};

Handler.setRoleClick = function (selector) {
    var arrPermission = selector.find("#roleCheck").find("input");
    arrPermission.each(function (index, item) {
        $(item).bind("click", function (event) {
            var sUserId = $("#hfUserId").val();
            var value = sUserId + "," + $(this).val();
            var url = "/Ajax/SetRole/" + value;
            if (!this.checked) {
                url = "/Ajax/RemoveRole/" + value;
            }
            var objUpdateMessage = $("#updateMessage");
            $.getJSON(url, function (data) {
                if (objUpdateMessage.length > 0) {
                    objUpdateMessage.fadeIn();
                    objUpdateMessage.css("color", "green");
                    objUpdateMessage.html("更新成功!");
                    objUpdateMessage.fadeOut("slow");
                }
            });
        });
    });
};

Handler.setAllSelectClick = function (selector) {
    var objSelectAll = selector.find("#selectAll");
    var objUnSelectAll = selector.find("#unSelectAll");

    objSelectAll.bind("click", function () {
        $(".checkbox_holder > input").each(function (index, item) {
            this.checked = true;
        });
    });

    objUnSelectAll.bind("click", function () {
        $(".checkbox_holder > input").each(function (index, item) {
            this.checked = false;
        });
    });
};

/*interior method*/

/**
* Get the querystring value from url
*/
function getQueryStringValue(url, flag) {
    var querystring = "";
    if (url.indexOf(flag) >= 0) {
        var tempStr = url.substring(url.indexOf(flag), url.length);
        if (tempStr.indexOf("&") >= 0) {
            querystring = tempStr.substring(0, tempStr.indexOf("&"));
        } else {
            querystring = tempStr;
        }
    }

    var value = querystring.substr(querystring.indexOf("=") + 1, flag.length);

    return value;
}

//get current host path url
function getCurrentHostPath() {
    var strFullPath = window.document.location.href;
    var strPathname = window.document.location.pathname;
    var hostPath = strFullPath.substring(0, strFullPath.indexOf(strPathname));
    return hostPath;
};

//屏蔽页面中不可编辑的文本框中的backspace按钮事件
function keydown(e) {
    var isie = (document.all) ? true : false;
    var key;
    var ev;
    if (isie) { //IE和谷歌浏览器
        key = window.event.keyCode;
        ev = window.event;
    } else {//火狐浏览器
        key = e.which;
        ev = e;
    }

    if (key == 8) {//IE和谷歌浏览器
        if (isie) {
            if (document.activeElement.readOnly == undefined || document.activeElement.readOnly == true) {
                return event.returnValue = false;
            }
        } else {//火狐浏览器
            if (document.activeElement.readOnly == undefined || document.activeElement.readOnly == true) {
                ev.which = 0;
                ev.preventDefault();
            }
        }
    }
}
document.onkeydown = keydown;