using System;
using System.Collections.Generic;
using System.Text;

namespace ZoeMeow.DUTAPI.Objects
{
    public class SubjectFee
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
        public bool IsQuality { get; set; } = false;


        #endregion

        #region Fee information
        public double Price { get; set; } = 0.0;

        public bool Debt { get; set; } = false;

        public bool IsReStudy { get; set; } = false;

        public string VerifiedPaymentAt { get; set; } = string.Empty;
        #endregion

    }
}
