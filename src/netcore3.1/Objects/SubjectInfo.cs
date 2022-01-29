namespace ZoeMeow.DUTAPI.Objects
{
    public class SubjectInfo
    {
        /// <summary>
        /// Subject ID.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Subject name.
        /// </summary>
        public string Name { get; set; }

        public bool Equals(SubjectInfo lopHocPhan)
        {
            if (lopHocPhan.ID != this.ID ||
                lopHocPhan.Name != this.Name
                )
                return false;
            return true;
        }
    }
}
