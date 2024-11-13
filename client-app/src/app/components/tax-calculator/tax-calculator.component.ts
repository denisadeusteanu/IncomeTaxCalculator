import { Component } from '@angular/core';
import { TaxCalculationService } from '../../../shared/services/tax-calculation.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SalaryCalculationResult } from '../../../shared/models/SalaryCalculationResult';
import { PoundPipe } from '../../../shared/pipes/pound.pipe';

@Component({
  selector: 'app-tax-calculator',
  standalone: true,
  imports: [FormsModule, CommonModule, PoundPipe],
  templateUrl: './tax-calculator.component.html',
})
export class TaxCalculatorComponent {
  grossAnnualSalary: number = 0;
  taxResult: SalaryCalculationResult | null = null;
  error: string | null = null;

  constructor(private taxService: TaxCalculationService) {}

  calculateTax() {
    this.resetResults();
    this.taxService.calculateTax(this.grossAnnualSalary).subscribe({
      next: (response) => {
        this.taxResult = response;
      },
      error: (error) => {
        console.error('Error calculating tax:', error);
        this.error = 'Error calculating tax! Please check the salary input.';
      },
    });
  }

  private resetResults() {
    this.taxResult = null;
    this.error = null;
  }
}
