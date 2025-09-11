import { TreeNode } from 'primeng/api';

export const PERMISSIONS: TreeNode[] = [
  {
    key: 'users',
    label: 'User Management',
    expanded: true,
    children: [
      { key: 'UsersView', label: 'View Users', selectable: true },
      { key: 'UsersCreate', label: 'Create Users', selectable: true },
      { key: 'UsersEdit', label: 'Edit Users', selectable: true },
      { key: 'UsersDelete', label: 'Delete Users', selectable: true },
    ],
  },
  {
    key: 'roles',
    label: 'Role Management',
    expanded: true,
    children: [
      { key: 'RolesView', label: 'View Roles', selectable: true },
      { key: 'RolesCreate', label: 'Create Roles', selectable: true },
      { key: 'RolesEdit', label: 'Edit Roles', selectable: true },
      { key: 'RolesDelete', label: 'Delete Roles', selectable: true },
    ],
  },
  {
    key: 'courses',
    label: 'Course Management',
    expanded: true,
    children: [
      { key: 'CoursesView', label: 'View Courses', selectable: true },
      { key: 'CoursesCreate', label: 'Create Courses', selectable: true },
      { key: 'CoursesEdit', label: 'Edit Courses', selectable: true },
      { key: 'CoursesDelete', label: 'Delete Courses', selectable: true },
    ],
  },
  {
    key: 'lessons',
    label: 'Lesson Management',
    expanded: true,
    children: [
      { key: 'LessonsView', label: 'View Lessons', selectable: true },
      { key: 'LessonsCreate', label: 'Create Lessons', selectable: true },
      { key: 'LessonsEdit', label: 'Edit Lessons' },
      { key: 'LessonsDelete', label: 'Delete Lessons', selectable: true },
    ],
  },
  {
    key: 'enrollments',
    label: 'Enrollment Management',
    expanded: true,
    children: [
      { key: 'EnrollmentsView', label: 'View Enrollments', selectable: true },
      { key: 'EnrollmentsManage', label: 'Manage Enrollments', selectable: true },
    ],
  },
  {
    key: 'payments',
    label: 'Payment Management',
    expanded: true,
    children: [
      { key: 'PaymentsView', label: 'View Payments', selectable: true },
      { key: 'PaymentsManage', label: 'Manage Payments', selectable: true },
    ],
  },
  {
    key: 'logs',
    label: 'Logs',
    expanded: true,
    children: [{ key: 'LogsView', label: 'View Logs', selectable: true }],
  },
  {
    key: 'system',
    label: 'Permission Management',
    expanded: true,
    children: [
      { key: 'SystemView', label: 'View Permissions', selectable: true },
      { key: 'SystemManage', label: 'Manage Permissions', selectable: true },
    ],
  },
];
