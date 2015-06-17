#pragma strict

 var number = 5.5;
 
 function factorial(n:int) : int
 {
 
 // If the number is not an integer, round it down. 
 n = Mathf.Floor(n);
 
 // The number must be equal to or bigger than zero 
 if (n < 0)
 {
     return (0);
 }
 
 if ((n == 0) || (n == 1))
 { // If the number is 0 or 1, its factorial is 1.
     return (1);
 }
 else 
 { // Make a recursive call 
     return (n * factorial(n - 1)); 
 } 
 }
 
 
 print(factorial(number));