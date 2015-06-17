#pragma strict

var number = 5;

function Fibo(n:int) : int { if((n==1)||(n==2)) return 1; else return Fibo(n-1) + Fibo(n-2); }

print(Fibo(number));