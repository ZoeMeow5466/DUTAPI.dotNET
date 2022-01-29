namespace ZoeMeow.DUTAPI.Objects
{
    /// <summary>
    /// Trạng thái của tài khoản khi đăng nhập
    /// </summary>
    public enum LoginStatus
    {
        /// <summary>
        /// Tài khoản đã bị khóa
        /// </summary>
        AccountLocked = -1,
        /// <summary>
        /// Đã đăng xuất (chưa đăng nhập),
        /// </summary>
        LoggedOut = 0,
        /// <summary>
        /// Đã đăng nhập
        /// </summary>
        LoggedIn = 1,
    }
}
