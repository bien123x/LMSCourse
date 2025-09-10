import { TreeNode } from 'primeng/api';

export const PERMISSIONS: TreeNode[] = [
  {
    key: 'users',
    label: 'User Management',
    children: [
      { key: 'UsersView', label: 'View Users' },
      { key: 'UsersCreate', label: 'Create Users' },
      { key: 'UsersEdit', label: 'Edit Users' },
      { key: 'UsersDelete', label: 'Delete Users' },
    ],
  },
  {
    key: 'roles',
    label: 'Role Management',
    children: [
      { key: 'RolesView', label: 'View Roles' },
      { key: 'RolesCreate', label: 'Create Roles' },
      { key: 'RolesEdit', label: 'Edit Roles' },
      { key: 'RolesDelete', label: 'Delete Roles' },
    ],
  },
  {
    key: 'courses',
    label: 'Course Management',
    children: [
      { key: 'CoursesView', label: 'View Courses' },
      { key: 'CoursesCreate', label: 'Create Courses' },
      { key: 'CoursesEdit', label: 'Edit Courses' },
      { key: 'CoursesDelete', label: 'Delete Courses' },
    ],
  },
  {
    key: 'lessons',
    label: 'Lesson Management',
    children: [
      { key: 'LessonsView', label: 'View Lessons' },
      { key: 'LessonsCreate', label: 'Create Lessons' },
      { key: 'LessonsEdit', label: 'Edit Lessons' },
      { key: 'LessonsDelete', label: 'Delete Lessons' },
    ],
  },
  {
    key: 'enrollments',
    label: 'Enrollment Management',
    children: [
      { key: 'EnrollmentsView', label: 'View Enrollments' },
      { key: 'EnrollmentsManage', label: 'Manage Enrollments' },
    ],
  },
  {
    key: 'payments',
    label: 'Payment Management',
    children: [
      { key: 'PaymentsView', label: 'View Payments' },
      { key: 'PaymentsManage', label: 'Manage Payments' },
    ],
  },
  {
    key: 'logs',
    label: 'Logs',
    children: [{ key: 'LogsView', label: 'View Logs' }],
  },
  {
    key: 'system',
    label: 'Permission Management',
    children: [
      { key: 'SystemView', label: 'View Permissions' },
      { key: 'SystemManage', label: 'Manage Permissions' },
    ],
  },
];
