﻿using DaxStudio.Common.Enums;
using System.Collections.Generic;

namespace DaxStudio.QueryTrace.Interfaces
{
    public interface IQueryTraceHub
    {
        void StartAsync();
        void Stop();
        void ConfigureTrace(ADOTabular.ADOTabularConnection connection, List<DaxStudioTraceEventClass> events);

        void OnTraceError(string message);
        void OnTraceWarning(string message);
        void OnTraceComplete();
        void OnTraceComplete(DaxStudioTraceEventArgs[] capturedEvents);
        void OnTraceEvent(DaxStudioTraceEventArgs traceEvent);
        void OnTraceStarting();
        void OnTraceStarted();
        void OnTraceStopped();
        void GetStatus(QueryTraceStatus status);
        void UpdateEvents (List<DaxStudioTraceEventClass> events);
    }
}
