  
import { AbstractControl } from '@angular/forms';

export function PasswordValidator(control: AbstractControl): { [key: string]: boolean } | null {
  var dateOfBirth = control.get('dateOfBirth');

  let compareDate: Date = new Date(dateOfBirth.value)
  let currentdate: Date = new Date();  
  let mindate: Date = new Date("1920-01-01");  
  
  if (dateOfBirth.pristine) {
    return null;
  }
  compareDate <= currentdate && compareDate >= mindate 

  return (compareDate <= currentdate && compareDate >= mindate)  ? { 'wrong': true } : null;
}
