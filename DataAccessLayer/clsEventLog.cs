using System.Diagnostics;

namespace DataAccessLayer.Tests
{
    internal class clsEventLog
    {

        /// <summary>
        /// Logs an error message to the Windows Event Log.
        /// </summary>
        public static void LogError(string Message, string SourceName = "DVLD_Project")
        {
            if (string.IsNullOrEmpty(Message))
                return;

            try
            {
                if (EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, "Application");
                }

                // Write the error message to the event log
                EventLog.WriteEntry(SourceName, Message, EventLogEntryType.Error);
            }
            catch
            {

            }
        }
    }
}
