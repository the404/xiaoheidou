$(document).ready(function () {
    $('.control-group').show();
    var url = window.location.href;

    var arr = url.replace(location.host, "").split("/");

    function imgOpen(alink) {
        if (alink.parent('li').children('ul').html() != null) {
            alink.parent('li').parent('ul').children('li').children('ul').hide(slideSpeed);
            alink.parent('li').parent('ul').children('li').children('img').attr('src', '/images/imgOffClosed.png');
            alink.delay(100).is(':hidden');
            if (alink.parent('li').children('ul').css('display') == "block") {
                alink.parent('li').children('ul').hide(slideSpeed);
                alink.attr('src', '/images/imgOffClosed.png');
            } else {
                alink.parent('li').children('ul').show(slideSpeed);
                alink.attr('src', '/images/imgOnOpen.png');
            }
            return false;
        }
    }

    $("div.left-secondary-nav div.tab-pane a").each(function () {
        if (url.indexOf($(this).attr("href")) >= 0) {
            $(this).parents("div.tab-pane").addClass("active").siblings().removeClass("active");

            var id = $(this).parents("div.tab-pane").attr("id");
            $("#myTab li a").each(function () {
                if ($(this).attr("href") == "#" + id) {
                    $(this).parent().addClass("active").siblings().removeClass("active");
                }
            });
            imgOpen($(this));
        }
        else {
            if (arr[0] == "User" || arr[0] == "Role" || arr[0] == "Permission") {
                $("div.tab-pane#User").addClass("active").siblings().removeClass("active");
                $("#myTab li a[href=#User]").parent().addClass("active").siblings().removeClass("active");
                $("div.tab-pane#User a").each(function () {
                    if ($(this).attr("href").split('/')[1] == arr[0]) {
                        imgOpen($(this));
                        return;
                    }

                });
            }
            else if (arr[0] == "PersonalCenter") {
                $("div.tab-pane#PersonalCenter").addClass("active").siblings().removeClass("active");
                $("#myTab li a[href=#PersonalCenter]").parent().addClass("active").siblings().removeClass("active");
                $("div.tab-pane#PersonalCenter a").each(function () {
                    if ($(this).attr("href").indexOf(arr[0]) > 0) {
                        imgOpen($(this));
                    }

                });
            }
            else if (arr[0] == "SelfDefiningMenu") {
                $("div.tab-pane#SelfDefiningMenu").addClass("active").siblings().removeClass("active");
                $("#myTab li a[href=#SelfDefiningMenu]").parent().addClass("active").siblings().removeClass("active");
                $("div.tab-pane#SelfDefiningMenu a").each(function () {
                    if ($(this).attr("href").indexOf(arr[0]) > 0) {
                        imgOpen($(this));
                    }

                });
            }
            else if (arr[0] == "ResponseImage" || arr[0] == "ResponseImageText" || arr[0] == "ResponseMusic" || arr[0] == "ResponseVideo") {
                $("div.tab-pane#Response").addClass("active").siblings().removeClass("active");
                $("#myTab li a[href=#Response]").parent().addClass("active").siblings().removeClass("active");
                $("div.tab-pane#Response a").each(function () {
                    if ($(this).attr("href").indexOf(arr[0]) > 0) {
                        imgOpen($(this));
                    }

                });
            }
            else if (arr[0] == "ResponseMessage") {
                $("div.tab-pane#ResponseMessage").addClass("active").siblings().removeClass("active");
                $("#myTab li a[href=#ResponseMessage]").parent().addClass("active").siblings().removeClass("active");
                $("div.tab-pane#ResponseMessage a").each(function () {
                    if ($(this).attr("href").indexOf(arr[0]) > 0) {
                        imgOpen($(this));
                    }

                });
            }
            else if (arr[0] == "GuessUser") {
                $("div.tab-pane#Activity").addClass("active").siblings().removeClass("active");
                $("#myTab li a[href=#Activity]").parent().addClass("active").siblings().removeClass("active");
                $("div.tab-pane#Activity a").each(function () {
                    if ($(this).attr("href").split('/')[1] == arr[0]) {
                        imgOpen($(this));
                        return;
                    }

                });
            }

        }

    })
    $('#header').click(function () {
        if ($('#header img').length > 0 && $('#header img').attr('status') == 'ok') {
            $('.popWrap img').attr('src', $('#header img').attr('src'));
            $('.popWrap').show();
        }
    });
    $('.popWrap').click(function () {
        $('.popWrap').hide();
    });
    //隐藏短链接
    if ($('.control-group:eq(1)').text().indexOf('短链接'))
        $('.control-group:eq(1)').hide();
});
