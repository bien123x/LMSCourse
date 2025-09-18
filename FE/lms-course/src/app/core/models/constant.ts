import { TreeNode } from 'primeng/api';

export const PERMISSIONS: TreeNode[] = [
  {
    key: 'users',
    label: 'Quản lý người dùng',
    expanded: true,
    children: [
      { key: 'UsersView', label: 'Xem người dùng', selectable: true },
      { key: 'UsersCreate', label: 'Thêm người dùng', selectable: true },
      { key: 'UsersEdit', label: 'Chỉnh sửa người dùng', selectable: true },
      { key: 'UsersDelete', label: 'Xóa người dùng', selectable: true },
    ],
  },
  {
    key: 'roles',
    label: 'Quản lý vai trò',
    expanded: true,
    children: [
      { key: 'RolesView', label: 'Xem vai trò', selectable: true },
      { key: 'RolesCreate', label: 'Thêm vai trò', selectable: true },
      { key: 'RolesEdit', label: 'Chỉnh sửa vai trò', selectable: true },
      { key: 'RolesDelete', label: 'Xóa vai trò', selectable: true },
    ],
  },
  {
    key: 'courses',
    label: 'Quản lý khóa học',
    expanded: true,
    children: [
      { key: 'CoursesView', label: 'Xem khóa học', selectable: true },
      { key: 'CoursesCreate', label: 'Thêm khóa học', selectable: true },
      { key: 'CoursesEdit', label: 'Chỉnh sửa khóa học', selectable: true },
      { key: 'CoursesDelete', label: 'Xóa khóa học', selectable: true },
    ],
  },
  {
    key: 'lessons',
    label: 'Quản lý bài học',
    expanded: true,
    children: [
      { key: 'LessonsView', label: 'Xem bài học', selectable: true },
      { key: 'LessonsCreate', label: 'Thêm bài học', selectable: true },
      { key: 'LessonsEdit', label: 'Chỉnh sửa bài học', selectable: true },
      { key: 'LessonsDelete', label: 'Xóa bài học', selectable: true },
    ],
  },
  {
    key: 'enrollments',
    label: 'Quản lý ghi danh',
    expanded: true,
    children: [
      { key: 'EnrollmentsView', label: 'Xem ghi danh', selectable: true },
      { key: 'EnrollmentsManage', label: 'Quản lý ghi danh', selectable: true },
    ],
  },
  {
    key: 'payments',
    label: 'Quản lý thanh toán',
    expanded: true,
    children: [
      { key: 'PaymentsView', label: 'Xem thanh toán', selectable: true },
      { key: 'PaymentsManage', label: 'Quản lý thanh toán', selectable: true },
    ],
  },
  {
    key: 'logs',
    label: 'Nhật ký hệ thống',
    expanded: true,
    children: [{ key: 'LogsView', label: 'Xem nhật ký', selectable: true }],
  },
  {
    key: 'system',
    label: 'Quản lý phân quyền',
    expanded: true,
    children: [
      { key: 'SystemView', label: 'Xem phân quyền', selectable: true },
      { key: 'SystemManage', label: 'Quản lý phân quyền', selectable: true },
    ],
  },
];
