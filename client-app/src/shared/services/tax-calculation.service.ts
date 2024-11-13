import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment';
import { SalaryCalculationResult } from '../models/SalaryCalculationResult';

@Injectable({
  providedIn: 'root',
})
export class TaxCalculationService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  calculateTax(grossAnnualSalary: number): Observable<any> {
    return this.http.get<SalaryCalculationResult>(
      `${this.apiUrl}/taxCalculator/calculate`,
      {
        params: { grossAnnualSalary: grossAnnualSalary.toString() },
      }
    );
  }
}
