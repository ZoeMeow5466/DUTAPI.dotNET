using System;
using System.Collections.Generic;
using System.Text;

namespace ZoeMeow.DUTAPI
{
    public class SubjectSchedule
    {
        #region Basic Information
        /// <summary>
        /// Subject ID.
        /// </summary>
        public string ID { get; set; } = string.Empty;

        /// <summary>
        /// Subject name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Subject credit
        /// </summary>
        public float Credit { get; set; } = 0;
        #endregion

        #region Subject Information
        public bool IsHighQuality { get; set; } = false;

        public string Lecturer { get; set; } = string.Empty;

        public string ScheduleStudy { get; set; } = string.Empty;

        public string Weeks { get; set; } = string.Empty;

        public string PointFomula { get; set; } = string.Empty;
        #endregion

        #region Subject Examination Information
        public string GroupExam { get; set; } = string.Empty;

        public bool IsGlobalExam { get; set; } = false;

        public string RoomExam { get; set; } = string.Empty;

        public string DateExamInString { get; set; } = string.Empty;

        public long DateExamInUnix { get; set; } = 0;

        #endregion

        public bool Equals(SubjectSchedule sub)
        {
            if (base.Equals(sub))
                return true;

            // Basic information
            if (sub.ID != this.ID ||
                sub.Name != this.Name
                )
                return false;

            // Subject information
            if (sub.Credit != this.Credit ||
                sub.IsHighQuality != this.IsHighQuality ||
                sub.Lecturer != this.Lecturer ||
                sub.ScheduleStudy != this.ScheduleStudy ||
                sub.Weeks != this.Weeks ||
                sub.PointFomula != this.PointFomula
                )
                return false;

            // Subject Examination Information
            if (sub.GroupExam != this.GroupExam ||
                sub.IsGlobalExam != this.IsGlobalExam ||
                sub.RoomExam != this.RoomExam ||
                sub.DateExamInUnix != this.DateExamInUnix ||
                sub.DateExamInString != this.DateExamInString
                )
                return false;

            return true;
        }
    }
}
