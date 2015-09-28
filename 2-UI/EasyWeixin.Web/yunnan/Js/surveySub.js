var aSurveyBox = $(".surveyBox");  //获取所有题目
var oPreTopic = $(".preTopic");    //上一题按钮
var oNextTopic = $(".nextTopic");  //下一题按钮
var oTopicSub = $(".topicSub");    //提交问卷按钮
var topicNum = 0;                 //题目数组标识
var answer = {};                  //存放答案数组

//by tianxiu 
var qusetion = ""; //setQuestionId
//end

var aOptions = $(".options");     //选择题
var oSuggest = $(".suggest");     //填空题
//var aItem = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K"];   //设置答案数组
var aItem = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11","12","13","14"];

aOptions.delegate('li', 'click', function () {           //给题目绑定点击事件


    var index = $(this).index();                      //当前点击的题目
    var oOptionType = $(this).parents(".surveyBox").attr("optionType");  //获取题目类型
    var sur = $(this).parents(".surveyBox");
    var oTopicId = sur.attr("topicId");           //获取当前的题号

    //by tianxiu
    var questionId = sur.attr("questionId");//每个问题的setanswerId
    qusetion += questionId + ","
    //end

    oNextTopic.addClass("nextActive");
    if (oOptionType == 0) {
        //单选题执行方法
        $(this).find(".option").addClass("active").parent().siblings().find(".option").removeClass("active");
        answer[oTopicId] = aItem[index];       //将选择第几项对应设置的答案数组内容存进表单答案数组

    }
    if (oOptionType == 1) {
        //多选题执行方法
        $(this).find(".option").toggleClass("active");
        var result = [];               //存放多选答案
        sur.find(".active").each(function (i, v) {     //遍历子元素有active类的元素存进多选答案数组
            result[i] = aItem[$(v).parent().index()];
        });
        answer[oTopicId] = result;    //将存放多选答案的数组存进表单答案数组

    }
});

//题目切换点击事件
oPreTopic.click(function () {               //上一题点击事件
    if (topicNum > 0) {
        oNextTopic.addClass("nextActive");
        topicNum--;
    } else { return; }
    tabOptions(topicNum);
});
oNextTopic.click(function () {               //下一题点击事件
    if (oNextTopic.hasClass("nextActive")) {
        if (topicNum < aSurveyBox.length - 1) {
            topicNum++;
        } else {
            return;
        }
        tabOptions(topicNum);
    } else {
        return;
    }
    if ($(aSurveyBox[topicNum]).find(".active").size() > 0) {
        oNextTopic.addClass("nextActive");
    } else {
        oNextTopic.removeClass("nextActive");
    }
});
//题目切换事件
function tabOptions(topicNum) {
    if (topicNum == aSurveyBox.length - 1) {   //最后一题显示提交问卷按钮
        oTopicSub.show();
        oNextTopic.hide();
    } else {
        oNextTopic.show();
        oTopicSub.hide();
    }
    aSurveyBox.eq(topicNum).show().siblings().hide();
};
//提交问卷按钮点击事件	
oTopicSub.click(function () {
    if ($(".surveyBox .suggest").length > 0) {
        $("textarea[optionType=text]").each(function (i, v) {   //遍历填空题
            var suggestText = $(v).val();                      //获取选择题目的值存进对象
            var oTopicId = $(v).parents(".surveyBox").attr("topicId");  //获取题目的题号
            answer[oTopicId] = suggestText;                   //将获取的值对应题目存进表单答案数组

            qusetion += $(v).parents(".surveyBox").attr("questionId"); //获取填空题的setanswerId

        });
    }
    var answernames = "";
    for (var name in answer) {
        answernames += "|" + answer[name] + ", ";
    }
    //alert("是什么?" + answernames);
    //by tianxiu 
    $.post('/ActivityQuestionaire/AddQuestion', { questionData: qusetion, answerData: answernames, User_Id: $("#uId").val() }, function (r) {
        if (r.flag) {
            // alert("提交成功!");
            window.location.href = '/yunnan/Html/succeed.shtml';
        }
        else {
            alert("提交失败!");
        }

    });
    //end
});