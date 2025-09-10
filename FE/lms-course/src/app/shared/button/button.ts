import { Component, input, output } from '@angular/core';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-button',
  templateUrl: './button.html',
  imports: [ButtonModule],
})
export class ButtonComponent {
  label = input<string>('Button');
  icon = input<string>('');
  loading = input<boolean>(false);
  disabled = input<boolean>(false);
  styleClass = input<string>('');

  click = output();
}
