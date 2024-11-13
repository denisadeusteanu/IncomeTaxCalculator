import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'pound',
  standalone: true,
})
export class PoundPipe implements PipeTransform {
  transform(value: number): string {
    if (value === null || value === undefined) {
      return '';
    }
    return `£ ${value}`;
  }
}
