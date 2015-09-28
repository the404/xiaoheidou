/*
 * Abstract   ：iScroll插件调用
 * Author     ：蔡应	
 * Time       ：2014年2月13日11:50:54
 */
 

var oIScrollBox = document.getElementById("scrollWrap");  
	
	/*设置iScrollBox高度*/
	oIScrollBox.style.height = (document.documentElement.clientHeight)+"px" ;
	window.onresize = function(){
		oIScrollBox.style.height = (document.documentElement.clientHeight)+"px" ;	
	}
	
	/*调用iScroll插件*/
	$(function(){
		var myscroll= new iScroll("scrollWrap",{fixedScrollbar:true});
	})
		
	
	
