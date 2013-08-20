﻿using System;
using System.Collections.Generic;

namespace HangFire
{
    public class Configuration
    {
        public static Configuration Instance { get; private set; }

        static Configuration()
        {
            Instance = new Configuration();
        }

        public static void Configure(Action<Configuration> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            action(Instance);
        }

        internal Configuration()
        {
            WorkerActivator = new WorkerActivator();
            
            RedisHost = "localhost";
            RedisPort = 6379;
            RedisPassword = null;
            RedisDb = 0;

            PollInterval = TimeSpan.FromSeconds(15);

            PerformInterceptors = new List<IPerformInterceptor>();
            EnqueueInterceptors = new List<IEnqueueInterceptor>();

            AddInterceptor(new I18NInterceptor());
        }

        public WorkerActivator WorkerActivator { get; set; }

        public string RedisHost { get; set; }
        public int RedisPort { get; set; }
        public string RedisPassword { get; set; }
        public long RedisDb { get; set; }

        public TimeSpan PollInterval { get; set; }

        public IList<IPerformInterceptor> PerformInterceptors { get; private set; }
        public IList<IEnqueueInterceptor> EnqueueInterceptors { get; private set; }

        public void AddInterceptor(IInterceptor interceptor)
        {
            var serverMiddleware = interceptor as IPerformInterceptor;
            if (serverMiddleware != null)
            {
                PerformInterceptors.Add(serverMiddleware);
            }

            var clientMiddleware = interceptor as IEnqueueInterceptor;
            if (clientMiddleware != null)
            {
                EnqueueInterceptors.Add(clientMiddleware);
            }
        }
    }
}