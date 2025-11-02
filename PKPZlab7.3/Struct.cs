using System;

namespace PKPZlab7._3
{
    public struct InternetSession
    {
        public string Login { get; init; }
        public DateTime SessionStart { get; init; }
        public DateTime SessionEnd { get; init; }
        public double DataReceivedKb { get; init; }
        public double DataSentKb { get; init; }

        public TimeSpan Duration => SessionEnd - SessionStart;
        public double TotalTrafficMb => (DataReceivedKb + DataSentKb) / 1024.0;
        public DateTime OnlineDate => SessionStart.Date;

        public InternetSession(string login, DateTime start, DateTime end, double received, double sent)
        {
            Login = login;
            SessionStart = start;
            SessionEnd = end;
            DataReceivedKb = received;
            DataSentKb = sent;
        }

        public override string ToString()
        {
            return $"User: {Login} | Date: {OnlineDate.ToString("ddMMyyyy")} | " +
                   $"Duration: {Duration:hh\\:mm\\:ss} | " +
                   $"Traffic: {TotalTrafficMb:F2} MB";
        }
    }
}