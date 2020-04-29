//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Hosting.Builder;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;
//using Nestor.Tools.Infrastructure.SetUp;
//using System;

//namespace Nestor.Tools.Infrastructure.Extensions
//{
//    public static class AppBuilderExtension
//    {
//        #region Setup Database
//        public static IApplicationBuilder UseSetupDatabaseMiddleware(this IApplicationBuilder builder)
//        {
//            return UseSetupDatabaseMiddleware(builder, null);
//        }

//        public static IApplicationBuilder UseSetupDatabaseMiddleware(this IApplicationBuilder builder, Action<SetUpDatabaseOptions> action)
//        {
//            var options = new SetUpDatabaseOptions();
//            action?.Invoke(options);
//            builder.UseMiddleware<SetUpDatabaseMiddleware>(options);
//            return builder;
//        }
//        #endregion

//        #region Initialize Data
//        public static IApplicationBuilder UseInitializeDataMiddleware(this IApplicationBuilder builder)
//        {
//            return UseInitializeDataMiddleware(builder, null);
//        }

//        public static IApplicationBuilder UseInitializeDataMiddleware(this IApplicationBuilder builder, Action<InitializeDataOptions> action)
//        {
//            var options = new InitializeDataOptions();
//            action?.Invoke(options);
//            builder.UseMiddleware<InitializeDataMiddleware>(options);
//            return builder;
//        }
//        #endregion
//    }   
//}
