using System.Linq;
using System.Collections.Generic;
using Autofac;
using System.Web.Http;
using System.Net.Http.Formatting;
using AutoMapper;


namespace XinTuo.Finance
{
    public class ApiConfig : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            this.ConfigWebApiJsonFormatter();

            //this.ConfigAutomapper(builder);
        }

        /// <summary>
        /// 配置webapi的序列化算法。
        /// 使用Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver作为json的序列化算法。
        /// </summary>
        private void ConfigWebApiJsonFormatter()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();

            if (jsonFormatter != null)
            {
                jsonFormatter.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            }
        }

        /// <summary>
        /// 配置automapper实现自动注入。
        /// </summary>
        /// <remarks>
        /// 参考
        /// http://kevsoft.net/2016/02/24/automapper-and-autofac-revisited.html
        /// </remarks>
        /// <param name="builder"></param>
        private void ConfigAutomapper(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(XinTuo.Finance.AutoMapperProfiles.CompanyProfile)).As<Profile>();
            
            builder.Register(context =>
            {
                //读取所有已注册的automapper profile
                var profiles = context.Resolve<IEnumerable<Profile>>();

                var config = new MapperConfiguration(x =>
                {
                    //将所有已注册的profile加入映射配置项
                    foreach (var profile in profiles)
                    {
                        x.AddProfile(profile);
                    }
                });
                return config;
            }).SingleInstance()         //采用单例模式
              .AutoActivate()           //在注入后自动激活（在ContainerBuilder.Build()方法中激活）
              .AsSelf();                //转换为它本身的类型

            //只有临时变量才需要将IComponentContext当作“tempContext”来处理
            //参考http://stackoverflow.com/a/5386634/718053
            builder.Register(tempContext=>
            {
                var ctx = tempContext.Resolve<IComponentContext>();
                var config = ctx.Resolve<MapperConfiguration>();
                
                return config.CreateMapper();
            }).As<IMapper>();

            base.Load(builder);
        }
    }
}