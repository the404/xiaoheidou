(function ($) {
    $.fn.photoTab = function (options) {
        var defaults = {
            timer: 4,
            speed: 400,
            moveSpeed: 300,
            width: $(".page").width()
        };

        var options = $.extend(defaults, options);

        var $imgWrapper = $(this);
        var $imgBox = $("ul", $imgWrapper);

        var imgCounts = $("li", $imgWrapper).length;

        $imgBox.css({"width": imgCounts * 100 + '%'});
        $("li", $imgWrapper).css({"width": 100 / imgCounts + '%'});

        if (imgCounts <= 1) {
            return false;
        }

        var initImgCounts = function () {
            /*var _html = "<ol>";
            for (var i = 0; i < imgCounts; i++) {
                _html += "<li></li>";
            }
            _html += "</ol>";
            $imgWrapper.append(_html);
            $("ol li:eq(0)", $imgWrapper).addClass("hover");*/
        };

        initImgCounts();

        var current = 0;

        function next(speed) {
            $imgBox.animate({'margin-left': '-100%'}, speed, function () {
				var s=options.nextFn();
				$imgBox.find('li').eq(0).css("background-image","url("+s+")");
                $imgBox.find('li').eq(0).appendTo($imgBox);
                $imgBox.css({'margin-left': 0});
            });
            current++;
            $("ol li", $imgWrapper).removeClass().eq(current % imgCounts).addClass('hover');
        }

        function prev(speed) {
			$imgBox.find('li:last').css("background-image","url("+options.preFn()+")");
            $imgBox.find('li:last').prependTo($imgBox);
            $imgBox.css({'margin-left': '-100%'});
            $imgBox.animate({'margin-left': 0}, speed);
            current--;
            $("ol li", $imgWrapper).removeClass().eq(current % imgCounts).addClass('hover');
        }

        var timer = null;

        function play() {
          
        }

        play();

        var tips = {
            bool: false,
            xw: 0,
            yw: 0
        };

        $imgWrapper[0].addEventListener('touchstart', function (evt) {
            try {
                tips.bool = true;
                var touch = evt.touches[0]; // 获取第一个触点
                tips.xw = 0;
                tips.xy = 0;
                tips.x = Number(touch.pageX);
                tips.y = Number(touch.pageY);
                clearInterval(timer);
                $imgBox.stop();
            } catch (evt) {
                alert('touchStartFunc：' + evt.message);
            }
        }, false);

        $imgWrapper[0].addEventListener('touchmove', function (evt) {
            try {
//                evt.preventDefault();
                var touch = evt.touches[0]; // 获取第一个触点
                tips.mx = Number(touch.pageX); // 页面触点X坐标
                tips.my = Number(touch.pageY); // 页面触点X坐标
                if (tips.bool) {
                    tips.xw = tips.mx - tips.x;
                    tips.yw = tips.my - tips.y;
                }
            } catch (evt) {
                alert('touchStartFunc：' + evt.message);
            }
        }, false);

        $imgWrapper[0].addEventListener('touchend', function (evt) {
            tips.bool = false;

            if (Math.abs(tips.yw) > Math.abs(tips.xw)) {
                play();
                return;
            }

            if (tips.xw < -50) {
                next(options.moveSpeed);
            }
            if (tips.xw > 50) {
                prev(options.moveSpeed);
            }
            play();
        }, false);
    }
})(jQuery);