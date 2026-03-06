namespace IdentityCore.EFs.Enums
{
    public enum Step
    {
        WAITING_CONFIRM = 0,
        NORMAL = 1,
        FORGOT_PASSWORD_EMAIL = 2,
        OTP = 3,
        FORGOT_PASSWORD_UPDATE_NEW_PASSWORD = 4,
    }
}
