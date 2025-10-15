import { ChangeDetectorRef, Component, inject, OnInit, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Tree } from 'primeng/tree';
import { TreeNode } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { PermissionService } from '../../core/services/permission.service';

@Component({
  selector: 'app-permission-form',
  templateUrl: './permission-form.html',
  imports: [ButtonModule, Tree, TableModule],
})
export class PermissionFormComponent implements OnInit {
  private permisionService = inject(PermissionService);
  private cd = inject(ChangeDetectorRef);
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
    }
  }

  ngOnInit() {
    this.permisionService.getTreePermissions().subscribe((res) => {
      this.treePermissions = res;
      console.log('Tree', this.treePermissions);
      this.selectedPermissions = this.getSelectedNodes(this.treePermissions, this.permissions());
      console.log('Select', this.selectedPermissions);
      console.log('Permission', this.permissions());
      this.cd.markForCheck();
    });
  }

  getSelectedNodes(nodes: TreeNode[], checkedKeys: string[]): TreeNode[] {
    let selected: TreeNode[] = [];

    for (let node of nodes) {
      // Nếu node nằm trong danh sách quyền được chọn
      if (checkedKeys.includes(node.data.code!)) {
        node.expanded = true;
        selected.push(node);
      }

      // Nếu đang ở user-permission và node thuộc role-permission → disable
      if (this.mode() === 'user-permission' && this.disabledKeys().includes(node.data.code!)) {
        node.selectable = false;
        node.expanded = true;
        selected.push(node);
      }

      // Duyệt đệ quy xuống children
      if (node.children && node.children.length > 0) {
        selected = [...selected, ...this.getSelectedNodes(node.children, checkedKeys)];
      }
    }

    return selected;
  }

  saveRolePermissions(): (string | undefined)[] {
    let selected: string[] = [];

    // lấy tick đầy đủ
    const fullChecked = this.selectedPermissions.map((n: any) => n.data.code);
    selected.push(...fullChecked);

    // lấy tick partial
    this.getPartialChecked(this.treePermissions, selected);

    console.log('Danh sách quyền đã chọn (full + partial):', selected);
    return selected;
  }
  private getPartialChecked(nodes: any[], selected: string[]) {
    if (!nodes) return;

    for (let node of nodes) {
      if (node.partialSelected) {
        selected.push(node.data.code);
      }
      this.getPartialChecked(node.children, selected);
    }
  }

  saveUserPermissions() {
    let selected: string[] = [];

    // lấy tick đầy đủ
    const fullChecked = this.selectedPermissions.map((n: any) => n.data.code);
    selected.push(...fullChecked);

    // lấy tick partial
    this.getPartialChecked(this.treePermissions, selected);

    // Loại bỏ các quyền bị disable (role-permission)
    const filteredKeys = selected.filter((key) => !this.disabledKeys().includes(key!));

    const userPermissionsInDisable = this.permissions().filter((key) =>
      this.disabledKeys().includes(key)
    );

    return [...filteredKeys, ...userPermissionsInDisable];
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
