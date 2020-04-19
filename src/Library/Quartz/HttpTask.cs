using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using NetModular.Lib.Quartz.Abstractions;
using NetModular.Module.Quartz.Domain.Job;
using NetModular.Module.Quartz.Domain.JobHttp;
using HttpMethod = NetModular.Lib.Utils.Core.Enums.HttpMethod;

namespace NetModular.Module.Quartz.Quartz
{
    /// <summary>
    /// Http请求任务
    /// </summary>
    [Description("Http请求任务")]
    public class HttpTask : TaskAbstract
    {
        private readonly IJobRepository _repository;
        private readonly IJobHttpRepository _httpRepository;
        private readonly IHttpClientFactory _clientFactory;
        public HttpTask(ITaskLogger logger, IJobRepository repository, IJobHttpRepository httpRepository, IHttpClientFactory clientFactory) : base(logger)
        {
            _repository = repository;
            _httpRepository = httpRepository;
            _clientFactory = clientFactory;
        }

        public override async Task Execute(ITaskExecutionContext context)
        {
            var idData = context.JobExecutionContext.JobDetail.JobDataMap["id"];
            if (idData == null)
            {
                await Logger.Error("任务编号不存在");
                return;
            }

            var id = Guid.Parse(idData.ToString());
            if (id == Guid.Empty)
            {
                await Logger.Error("任务编号不存在");
                return;
            }

            var job = await _repository.GetAsync(id);
            if (job == null)
            {
                await Logger.Error("任务不存在");
                return;
            }
            if (job.JobType != JobType.Http)
            {
                await Logger.Error("不是Http类型的Job");
                return;
            }

            var jobHttp = await _httpRepository.GetByJob(id);
            if (jobHttp == null)
            {
                await Logger.Error("不是Http类型的Job");
                return;
            }

            if (jobHttp.Url.IsNull())
            {
                await Logger.Error("URL为空");
                return;
            }

            var client = _clientFactory.CreateClient();
            //设置请求头
            if (jobHttp.Headers.NotNull())
            {
                var headers = JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(jobHttp.Headers);
                if (headers.Any())
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
            }

            //设置认证方式
            if (jobHttp.AuthType == AuthType.Jwt)
            {
                if (jobHttp.Token.IsNull())
                {
                    await Logger.Error("令牌为空");
                    return;
                }

                //jwt
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + jobHttp.Token);
            }

            HttpResponseMessage responseMessage;
            //GET请求
            if (jobHttp.Method == HttpMethod.GET)
            {
                responseMessage = await client.GetAsync(jobHttp.Url);
            }
            //DELETE删除
            else if (jobHttp.Method == HttpMethod.DELETE)
            {
                responseMessage = await client.DeleteAsync(jobHttp.Url);
            }
            //Post和Put请求
            else
            {
                //设置参数
                var content = new StringContent(jobHttp.Parameters);
                content.Headers.ContentType = new MediaTypeHeaderValue(jobHttp.ContentType.ToDescription());
                responseMessage = jobHttp.Method == HttpMethod.POST ? await client.PostAsync(jobHttp.Url, content) : await client.PutAsync(jobHttp.Url, content);
            }

            await Logger.Info(await responseMessage.Content.ReadAsStringAsync());
        }
    }
}
