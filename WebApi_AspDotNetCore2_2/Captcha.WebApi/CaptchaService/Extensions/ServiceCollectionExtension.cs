//=====================================================

//Copyright (C) 2016-2018 Fanjia

//All rights reserved

//CLR版 本:    4.0.30319.42000

//创建时间:     2018/12/21 17:49:50

//创 建 人:   徐晓航

//======================================================

using Microsoft.Extensions.DependencyInjection;

namespace CaptchaService.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddCaptchaService(this IServiceCollection services)
        {
            services.AddSingleton<ICaptchaFactory, CaptchaFactory>();
        }
    }
}
