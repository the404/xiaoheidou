using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyWeixin.Web.Hubs;
using System.Collections.Generic;
using Newtonsoft.Json;
using EasyWeixin.Helpers;
using System.IO;

namespace EasyWeixin.Web.Tests
{
    [TestClass]
    public class AwarHubTest
    {
        [TestMethod]
        public void CreateConfig()
        {
            //将每个用户的这个templateId和appId组合写死在这里
            var result = new List<TemplateData>();
            result.Add(new TemplateData("深圳互动力", "wx00f1a2e4c8da9fff", "LPuzUzNn70pSGEUUxil7RrQJt5ZMM8U23ZE_0MtUYV8"));
            result.Add(new TemplateData("深圳互动力", "wx00f1a2e4c8da9fff", "LPuzUzNn70pSGEUUxil7RrQJt5ZMM8U23ZE_0MtUYV8"));
            result.Add(new TemplateData("深圳互动力", "wx00f1a2e4c8da9fff", "LPuzUzNn70pSGEUUxil7RrQJt5ZMM8U23ZE_0MtUYV8"));
            result.Add(new TemplateData("深圳互动力", "wx00f1a2e4c8da9fff", "LPuzUzNn70pSGEUUxil7RrQJt5ZMM8U23ZE_0MtUYV8"));
            result.Add(new TemplateData("深圳互动力", "wx00f1a2e4c8da9fff", "LPuzUzNn70pSGEUUxil7RrQJt5ZMM8U23ZE_0MtUYV8"));
            result.Add(new TemplateData("深圳互动力", "wx00f1a2e4c8da9fff", "LPuzUzNn70pSGEUUxil7RrQJt5ZMM8U23ZE_0MtUYV8"));


            var message = JsonConvert.SerializeObject(result);
            File.WriteAllText(@"C:\temlateconfig.json", message);
        }

        [TestMethod]
        public void GetConfig()
        {
            var path = @"C:\temlateconfig.json";
            CreateConfig();
            var message = File.ReadAllText(path);

            var result = JsonConvert.DeserializeObject<List<TemplateData>>(message);
        }
    }
}
