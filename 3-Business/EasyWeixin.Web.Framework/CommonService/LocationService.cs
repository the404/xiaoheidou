﻿using EasyWeixin.Entities.GoogleMap;
using EasyWeixin.Entities.Response;
using EasyWeixin.Helpers;
using Senparc.Weixin.MP.Entities;
using System.Collections.Generic;

namespace EasyWeixin.Web.Framework.CommonService
{
    public class LocationService
    {
        public ResponseMessageNews GetResponseMessage(RequestMessageLocation requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);

            var markersList = new List<Markers>();
            markersList.Add(new Markers()
            {
                X = requestMessage.Location_X,
                Y = requestMessage.Location_Y,
                Color = "red",
                Label = "S",
                Size = MarkerSize.Default,
            });
            var mapSize = "480x600";
            var mapUrl = GoogleMapHelper.GetGoogleStaticMap(19 /*requestMessage.Scale*//*微信和GoogleMap的Scale不一致，这里建议使用固定值*/,
                                                            markersList, mapSize);
            responseMessage.Articles.Add(new Article()
            {
                Description = string.Format("您刚才发送了地理位置信息。Location_X：{0}，Location_Y：{1}，Scale：{2}，标签：{3}",
                              requestMessage.Location_X, requestMessage.Location_Y,
                              requestMessage.Scale, requestMessage.Label),
                PicUrl = mapUrl,
                Title = "定位地点周边地图",
                Url = mapUrl
            });
            //responseMessage.Articles.Add(new Article()
            //{
            //    Title = "微信公众平台SDK 官网链接",
            //    Description = "Senparc.Weixin.MK SDK地址",
            //    PicUrl = "http://weixin.senparc.com/images/logo.jpg",
            //    Url = "http://weixin.senparc.com"
            //});

            return responseMessage;
        }
    }
}