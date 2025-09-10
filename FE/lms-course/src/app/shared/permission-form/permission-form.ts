import { Component, inject, OnInit, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Tree } from 'primeng/tree';
import { PERMISSIONS } from '../../core/models/constant';
import { TreeNode } from 'primeng/api';

@Component({
  selector: 'app-permission-form',
  templateUrl: './permission-form.html',
  imports: [ButtonModule, Tree],
})
export class PermissionFormComponent implements OnInit {
  mode = signal<string>('');
  permissions = signal<string[]>([]);
  treePermissions = PERMISSIONS;
  selectedPermissions: TreeNode[] = [];

  constructor(private ref: DynamicDialogRef, private config: DynamicDialogConfig) {
    if (config.data) {
      this.mode.set(config.data.mode);
      if (this.mode() === 'role-permission') {
        this.permissions.set(config.data.permissions);
        console.log(this.permissions());
      }
      console.log(config.data);
    }
  }

  ngOnInit() {
    this.selectedPermissions = this.getSelectedNodes(this.treePermissions, this.permissions());
  }

  getSelectedNodes(nodes: TreeNode[], checkedKeys: string[]): TreeNode[] {
    let selected: TreeNode[] = [];
    for (let node of nodes) {
      if (checkedKeys.includes(node.key!)) {
        selected.push(node);
      }
      if (node.children) {
        selected = [...selected, ...this.getSelectedNodes(node.children, checkedKeys)];
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

  close() {
    this.ref.close();
  }
  save() {
    // Gửi API
    this.ref.close(this.saveRolePermissions());
  }
}
