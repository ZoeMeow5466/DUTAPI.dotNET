namespace ZoeMeow.DUTAPI.Objects
{
    /// <summary>
    /// Trạng thái của tiết học trong thông báo lớp học phần
    /// </summary>
    public enum SubjectStatus
    {
        /// <summary>
        /// Chỉ thông báo
        /// </summary>
        Notify = -1,
        /// <summary>
        /// Nghỉ học
        /// </summary>
        Leaving = 0,
        /// <summary>
        /// Học bù
        /// </summary>
        MakeUp = 1,
    }
}
