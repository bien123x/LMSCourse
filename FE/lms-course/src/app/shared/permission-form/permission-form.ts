import { Component, inject, OnInit, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Tree } from 'primeng/tree';
import { PERMISSIONS } from '../../core/models/constant';
import { TreeNode } from 'primeng/api';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-permission-form',
  templateUrl: './permission-form.html',
  imports: [ButtonModule, Tree, TableModule],
})
export class PermissionFormComponent implements OnInit {
  mode = signal<string>('');
  permissions = signal<string[]>([]);
  disabledKeys = signal<string[]>([]);
  treePermissions: any = [];
  selectedPermissions: TreeNode[] = [];

  constructor(private ref: DynamicDialogRef, private config: DynamicDialogConfig) {
    if (config.data) {
      this.mode.set(config.data.mode);
      if (this.mode() === 'role-permission') {
        this.permissions.set(config.data.permissions);
        console.log(this.permissions());
      } else if (this.mode() === 'user-permission') {
        const userPerms = this.config.data.userPermissions;
        const rolePerms = this.config.data.rolePermissions;

        this.permissions.set(userPerms);
        this.disabledKeys.set(rolePerms);
      }
      console.log(config.data);
    }
  }

  ngOnInit() {
    this.treePermissions = JSON.parse(JSON.stringify(PERMISSIONS));
    this.selectedPermissions = this.getSelectedNodes(this.treePermissions, this.permissions());
    console.log(this.selectedPermissions);
  }

  getSelectedNodes(nodes: TreeNode[], checkedKeys: string[]): TreeNode[] {
    let selected: TreeNode[] = [];
    for (let nodeParent of nodes) {
      for (let nodeChild of nodeParent.children!) {
        if (checkedKeys.includes(nodeChild.key!)) {
          selected.push({ ...nodeChild });
        }
        // Nếu user-permission và node thuộc role-permission → disable
        if (this.mode() === 'user-permission' && this.disabledKeys().includes(nodeChild.key!)) {
          selected.push({ ...nodeChild });
          nodeChild.selectable = false;
        }
      }
    }

    return selected;
  }

  saveRolePermissions(): (string | undefined)[] {
    const selectedKeys = this.selectedPermissions
      .filter((node) => !node.children || node.children.length === 0)
      .map((node) => node.key);
    console.log('Danh sách quyền đã chọn:', selectedKeys);
    return selectedKeys;
  }

  saveUserPermissions() {
    const selectedKeys = this.selectedPermissions
      .filter((node) => !node.children || node.children.length === 0) // chỉ lấy leaf node
      .map((node) => node.key);

    // Loại bỏ các quyền bị disable (role-permission)
    const filteredKeys = selectedKeys.filter((key) => !this.disabledKeys().includes(key!));

    console.log('User permissions (không gồm role):', filteredKeys);
    return filteredKeys;
  }

  close() {
    this.ref.close();
  }
  save() {
    // Gửi API
    if (this.mode() === 'user-permission') return this.ref.close(this.saveUserPermissions());
    this.ref.close(this.saveRolePermissions());
  }
}
