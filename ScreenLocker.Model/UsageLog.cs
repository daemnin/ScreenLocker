using System;

namespace ScreenLocker.Model
{
    public class UsageLog
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        #region Navigation Properties
        public int StudentId { get; set; }
        public Student Student { get; set; }
        #endregion
    }
}