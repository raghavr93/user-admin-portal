import { AbstractControl } from '@angular/forms';

export function DateValidator(control: AbstractControl): { [key: string]: boolean } | null {
  var dateOfBirth = control.get('dateOfBirth').value;

  let compareDate: Date = new Date(dateOfBirth)
  let currentdate: Date = new Date();  
  let mindate: Date = new Date(2018, 12, 20);  
  
  return  compareDate<=currentdate && compareDate >= mindate  ? { 'wrong': true } : null;
}
