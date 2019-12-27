using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wwbweibo.CrackDetect.PerProcess.Controllers
{
    /// <summary>
    /// 提供图像预处理阶段的同步任务支持
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PerProcessController : ControllerBase
    {
        [Route("index")]
        public object Index()
        {
            // todo: 在此处添加Api描述
            return new object();
        }

        [HttpGet]
        [Route("getPreProcessResult")]
        public Tuple<int, List<int>> GetPerProcessResult(int id, string b64Image)
        {
            // todo: 此处添加图像预处理的操作
            return  new Tuple<int, List<int>>(id, new List<int>());
        }
    }
}