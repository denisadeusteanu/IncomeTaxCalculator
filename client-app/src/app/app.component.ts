import { Component } from '@angular/core';
import { TaxCalculatorComponent } from './components/tax-calculator/tax-calculator.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [TaxCalculatorComponent],
  templateUrl: './app.component.html',
})
export class AppComponent {}
