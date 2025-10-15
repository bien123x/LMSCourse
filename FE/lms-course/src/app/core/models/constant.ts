export const PERMISSION = {
  System: {
    Module: 'System',
    UserManagement: {
      Module: 'UserManagement',
      Roles: {
        Module: 'Roles',
        Create: 'Roles.Create',
        Edit: 'Roles.Edit',
        Delete: 'Roles.Delete',
        ChangePermissions: 'Roles.ChangePermissions',
      },
      Users: {
        Module: 'Users',
        Create: 'Users.Create',
        Edit: 'Users.Edit',
        Delete: 'Users.Delete',
        ChangePermissions: 'Users.ChangePermissions',
        ViewDetails: 'Users.ViewDetails',
        ResetPwd: 'Users.ResetPwd',
        LockAccount: 'Users.LockAccount',
        UnLockAccount: 'Users.UnLockAccount',
      },
    },
    Auditlogs: {
      Module: 'Audilogs',
      Export: 'AuditLogs.Export',
    },
    Settings: {
      Module: 'Settings',
      Edit: 'Settings.Edit',
    },
  },
  Students: {
    Module: 'Students',
    Courses: {
      Module: 'Students.Courses',
      ViewDetails: {
        Module: 'Students.Courses.ViewDetails',
        AddToCart: 'Students.Courses.ViewDetails.AddToCart',
      },
    },
    Payments: {
      Module: 'Students.Payments',
      Checkout: 'Students.Payments.Checkout',
      ViewHistory: 'Students.Payments.ViewHistory',
    },
    Enrollments: {
      Module: 'Students.Enrollments',
      ViewDetails: {
        Module: 'Students.Enrollments.ViewDetails',
        WatchVideo: 'Students.Enrollments.ViewDetails.WatchVideo',
        DoQuiz: 'Students.Enrollments.ViewDetails.DoQuiz',
      },
    },
    Certificates: {
      Module: 'Students.Certificates',
      ViewDetails: 'Students.Certificates.ViewDetails',
    },
  },
};
