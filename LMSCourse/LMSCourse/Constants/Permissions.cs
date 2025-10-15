namespace OnlineCourseConstants
{
    public static class PERMISSION
    {
        public static class System
        {
            public const string Module = "System";

            public static class UserManagement
            {
                public const string Module = "UserManagement";
                public static class Roles
                {
                    public const string Module = "Roles";
                    public const string ChangePermissions = "Roles.ChangePermissions";
                    public const string Create = "Roles.Create";
                    public const string Edit = "Roles.Edit";
                    public const string Delete = "Roles.Delete";
                }

                public static class Users
                {
                    public const string Module = "Users";
                    public const string Create = "Users.Create";
                    public const string Edit = "Users.Edit";
                    public const string Delete = "Users.Delete";
                    public const string ChangePermissions = "Users.ChangePermissions";
                    public const string ViewDetails = "Users.ViewDetails";
                    public const string ResetPwd = "Users.ResetPwd";
                    public const string LockAccount = "Users.LockAccount";
                    public const string UnLockAccount = "Users.UnLockAccount";
                }
            }
            

            public static class AuditLogs
            {
                public const string Module = "Audilogs";
                public const string Export = "AuditLogs.Export";
            }

            public static class Settings
            {
                public const string Module = "Settings";
                public const string Edit = "Settings.Edit";
            }
        }

        public static class Students
        {
            public const string Module = "Students";
            public static class Courses
            {
                public const string Module = "Students.Courses";
                public static class ViewDetails
                {
                    public const string Module = "Students.Courses.ViewDetails";
                    public const string AddToCart = "Students.Courses.ViewDetails.AddToCart";
                }
            }

            public static class Payments
            {
                public const string Module = "Students.Payments";
                public const string Checkout = "Students.Payments.Checkout";
                public const string ViewHistory = "Students.Payments.ViewHistory";
            }

            public static class Enrollments
            {
                public const string Module = "Students.Enrollments";
                public static class ViewDetails
                {
                    public const string Module = "Students.Enrollments.ViewDetails";
                    public const string WatchVideo = "Students.Enrollments.ViewDetails.WatchVideo";
                    public const string DoQuiz = "Students.Enrollments.ViewDetails.DoQuiz";
                }
            }

            public static class Certificates
            {
                public const string Module = "Students.Certificates";
                public const string ViewDetails = "Students.Certificates.ViewDetails";
            }
        }

    }
}
