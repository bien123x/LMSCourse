import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { TreeNode } from 'primeng/api';
import { map, Observable } from 'rxjs';
export interface PermissionDto {
  id: number;
  name: string;
  code: string;
  children: PermissionDto[];
}
@Injectable({
  providedIn: 'root',
})
export class PermissionService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7202/Permissions';

  getTreePermissions(): Observable<TreeNode[]> {
    return this.http
      .get<PermissionDto[]>(this.apiUrl)
      .pipe(map((data) => this.mapToTreeNode(data)));
  }

  private mapToTreeNode(nodes: PermissionDto[]): TreeNode[] {
    return nodes.map((node) => {
      const hasChildren = node.children && node.children.length > 0;
      return {
        label: node.name,
        data: { id: node.id, code: node.code },
        children: hasChildren ? this.mapToTreeNode(node.children) : [],
        // leaf: !hasChildren,
      } as TreeNode;
    });
  }
}
