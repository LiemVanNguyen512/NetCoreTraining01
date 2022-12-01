﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ScheduledJobs.Interfaces
{
    public interface IScheduledJobService
    {
        #region Fire And Forget

        string Enqueue(Expression<Action> functionCall);
        string Enqueue<T>(Expression<Action<T>> functionCall);

        #endregion

        #region Delayed Jobs

        string Schedule(Expression<Action> functionCall, TimeSpan delay);
        string Schedule<T>(Expression<Action<T>> functionCall, TimeSpan delay);
        string Schedule(Expression<Action> functionCall, DateTimeOffset enqueueAt);

        #endregion

        #region Continuos Jobs

        string ContinueQueueWith(string parentJobId, Expression<Action> functionCall);

        #endregion

        bool Delete(string jobId);

        bool Requeue(string jobId);
    }
}
