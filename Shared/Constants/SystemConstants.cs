using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Constants
{
    public static class SystemConstants
    {
        public const string DefaultConnection = "DefaultConnectionString";
        public class AppSettings
        {
            public const string Token = "Token";
            public const string BaseAddress = "BaseAddress";
            public const string UserServiceAddress = "UserServiceAddress";
            public const string CourseServiceAddress = "CourseServiceAddress";
            public const string RetryCount = "RetryCount";
            public const string RetryAttemptSeconds = "RetryAttemptSeconds";
        }
    }
}
