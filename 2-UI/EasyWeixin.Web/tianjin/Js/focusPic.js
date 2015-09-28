/*
 * Abstract   ：内页焦点图
 * Author     ：蔡应	
 * Time       ：2014年2月13日11:50:54
 */
 

/*内页焦点图*/
var oScrollPic = document.getElementById("scrollPic"); //焦点图片
var aScrollPic = oScrollPic.getElementsByTagName("a");
var oScrollCon = document.getElementById("scrollCon"); //内容
var aScrollCon = oScrollCon.getElementsByTagName("li");
//添加焦点
var oScrollIcon = document.getElementById("scrollIcon");
	for( var i = 0; i < aScrollPic.length ; i++ ){
		if( i==0 ){
			oScrollIcon.innerHTML += "<span class='active' ></span>" ;
		}else{
			oScrollIcon.innerHTML += "<span></span>" ;	
		}
	}
var aScrollIcon = oScrollIcon.getElementsByTagName("span");

var iNow = 0; 
var startX = 0;
var startY = 0;
var iLeft = 0; 
var x=0;
var y=0;
var touch = null; 

for( var i=0; i<aScrollPic.length ; i++){
	
	aScrollPic[i].addEventListener('touchstart',function(event){
		if (event.targetTouches.length == 1){
			touch = event.targetTouches[0];
			startX = touch.clientX ;
			startY = touch.clientY ;
			iLeft = oScrollPic.offsetLeft ;
		}
	},false);
	
	aScrollPic[i].addEventListener('touchmove', function(event) {
	  // 如果这个元素的位置内只有一个手指的话
	  if (event.targetTouches.length == 1) {
		touch = event.targetTouches[0];
		x = Number(touch.pageX); //页面触点X坐标  
		y = Number(touch.pageY); //页面触点Y坐标
		oScrollPic.style.left = iLeft+x-startX+"px"; 
	  }
	}, false);
	
	aScrollPic[i].addEventListener('touchend',function(event){
			//运动
			if( x-startX > 50){
				if( iNow >0 ){
					iNow-- ;
				}
				startMove ( oScrollPic , { "left" : -(iNow*parseInt(document.documentElement.clientWidth)) } ,5 );
				fnIcon(iNow);
			}else if( x-startX < -50){
				if( iNow < aScrollPic.length-1 ){
					iNow++ ;
				}
				startMove ( oScrollPic , { "left" : -(iNow*parseInt(document.documentElement.clientWidth)) } ,5 );
				fnIcon(iNow);
			}else {
				startMove ( oScrollPic , { "left" : iLeft } ,5 ) ;
			}
	},false);
	
	function fnIcon(a){
		for( var j=0; j<aScrollPic.length ; j++ ){
			aScrollIcon[j].className = "" ;
			if( aScrollCon.length != 1){
				aScrollCon[j].style.display = "none" ;	
			}
		}
		if( aScrollCon.length != 1 ){
			aScrollCon[a].style.display = "block" ;
		}
		aScrollIcon[a].className = "active" ;
	}
	
}
