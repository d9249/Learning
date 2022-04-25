# DD772 Algorithms Kyonggi univ.

Code created in Algorithm class

과제#1 ~ #3로 해당 학기의 과제는 종료.


# 2021 기말대비 Algorithms

[2018 중간고사](https://www.notion.so/2018-e0a670c1e1944f9e81e2815950a8ce54)

[2018 기말고사](https://www.notion.so/2018-c1eec3f6b1b54baead15be7aa9f7bfca)

[2020 기말고사](https://www.notion.so/2020-b9ee3ad48914452dbf021adc2cc2ab8a)

[2021 중간고사](https://www.notion.so/2021-87a4f52e1c9f4d9e88c121e67b0cbfbf)

## 2018 중간고사

**[1~5] 아래에서 옳은 표현에는 O, 틀린 표현에는 X를 표시하시오. [각 2점]**

1. $3n^2 - 100n + 2 = O(n^3)$ O
2. $3 = \omega(n log n + 3n - 10)$ X
3. $2n log_2{n} + 12n − 3 = \omega(10n log n + 18n^{0.5} + 30)$  O
4. $2n log_2n + 12n − 3 = \theta(nlogn + n^{1.2})$ X
5. $3^n = O(4^{n/2})$ X

**[6~10] 아래의 함수 ()에 대한 점화식을 풀어 ( )의 꼴로 표기하시오. [각 3점]**

**6. $f(n) = 3f(n/5)+10$** 

$r = 3, c = 5, f(n) = O(1), log_5{3}, case3, O(nlog_53)$ 

**7. $f(n)=5f(n/4)+2nlogn$**

$r = 5, c = 4, f(n) = O(nlogn) log_45, case3, O(nlog_45)$

**8. $f(n) = f(n-4)+n^2logn$**

$f(n) = O(n^2logn)$

**9. $f(n)=9f(n/3)+n^2$**

$r = 9, c = 3, log_39 =2, O(n^2)$  case2

**10. $f(n) = 125f(n/8)+n^{2.5}logn$**

$r = 125, c = 8, f(n) = n^{2.5}logn$ 

$log_{8}125 = 125/8=2.3xxx, case1,  O(n^{2.5}logn)$

## 2018 기말고사

[1~5] 아래에서 옳은 표현에는 O, 틀린 표현에는 X를 표시하시오. (각 2점)

[1] $3n^2 -100n +2=O(n^3)$

[2] $3 = \omega(nlogn+3n-10)$

[3] $2nlog{_2}n+12n-3=\omega(10nlogn+18n^{0.5}+30)$

[4] $2nlog{_2}n+12n-3=\theta(nlogn+n^{1.2})$

[5] $3^{n}=O(4^{n/2})$

## 2021 중간고사

$3n^2-7n+10=\omega(1)$

답 : 참

$3n^2-7+10=O(n)$

답 : 거짓

$3n^2-7+10=\theta(n)$

답 : 거짓

$16nlog^2n-7n+10=\omega(n^2logn)$

답 : 거짓

$16nlog^2n-7n+10=\theta(n^2logn)$

답 :  거짓 ?

$2n^3-n^2log^2n+3n-20=O(2n^3-n^2log^2n)$

답 : 참 or 거짓

$n^3=\omega(2nlogn-10n+5)$

답 : 참 or 거짓

$2n^3+3n-20=\theta(n^3log^7n-10n+5)$

답 : 참 or 거짓

$2n^3+3n-20=\omega(n^3-10n+5)$

답 : 참

$3n^2-7n+10=\theta(n^3)$

정답 : 참 or 거짓

$n^3=\theta(2n^3-10n+5)$

답 : 참

$n^3=O(2n^3-10n+5)$

답 : 참 or 거짓

$3^n=\omega(4^{n/2})$

답 : 참

$3n^2-7n+10=\omega(1)$

정답 : 참 or 거짓

$3n^2-7n+10=\theta(n)$

답 : 참 or 거짓

$3n^2-7n+10=\omega(logn)$

답 : 참 or 거짓

$2^{2log_3n}=O(n^2)$

답 : 거짓

$16nlog^2n-7n+10=\omega(nlogn)$

답 : 참 or 거짓

$16nlog^2n-7n+10=O(n^2logn)$

답 : 참 or 거짓

$16nlog^2n-7n+10=O(n^{100})$

답 : 참 or 거짓

 

$2n^3-n^2log^2n+3n-20=\theta(2n^3-n^2log^2n)$

답 : 참

아래 점화식을 만족하는 함수에 대해 가장 의미있는 수식을 고르시오

$F(n)=F(n/2)+2n$

1. $F(n)=O(n^2)$
2. $F(n)=O(nlogn)$
3. $F(n)=O(n)$
4. $F(n)=O(logn)$
5. $F(n)=O(n^2logn)$

답 : 

아래 점화식을 만족하는 함수에 대해 가장 의미있는 수식을 고르시오

$F(n)=F(6n/7)+14$

1. $F(n)=O(nlogn)$
2. $F(n)=O(n^2)$
3. $F(n)=O(n)$
4. $F(n)=O(logn)$
5. $F(n)=O(n^3)$

답 : 

아래 점화식을 만족하는 함수에 대해 가장 의미있는 수식을 고르시오

$F(n)=F(n/2)+2F(n/3)+3F(n/4)+n^2$

1. $F(n)=O(n^2logn)$
2. $F(n)=O(nlogn)$
3. $F(n)=O(logn)$
4. $F(n)=O(n^2)$
5. $F(n)=O(n)$

답 : 

아래 점화식을 만족하는 함수에 대해 가장 의미있는 수식을 고르시오

$F(n)=F(n-10)+10logn-3$

1. $F(n)=O(nlogn)$
2. $F(n)=O(n^2)$
3. $F(n)=O(n^2logn)$
4. $F(n)=O(n)$
5. $F(n)=O(logn)$

답 : 

아래 점화식을 만족하는 함수에 대해 가장 의미있는 수식을 고르시오

$F(n)=9F(n/3)+n^2$

1. $F(n)=O(logn)$
2. $F(n)=O(n^2logn)$
3. $F(n)=O(n)$
4. $F(n)=O(nlogn)$
5. $F(n)=O(n^2)$

답 : 5번

아래 점화식을 만족하는 함수에 대해 가장 의미있는 수식을 고르시오

$F(n)=2F(n/3)+3nlogn$

1. $F(n)=O(n^3)$
2. $F(n)=O(nlogn)$
3. $F(n)=O(n)$
4. $F(n)=O(logn)$
5. $F(n)=O(n^2)$

답 : 2번

아래 점화식을 만족하는 함수에 대해 가장 의미있는 수식을 고르시오

$F(n)=F(n-2)+10nlogn$

1. $F(n)=O(n^2logn)$
2. $F(n)=O(n)$
3. $F(n)=O(n^2)$
4. $F(n)=O(nlogn)$
5. $F(n)=O(logn)$

답 : 1번

아래 점화식을 만족하는 함수에 대해 가장 의미있는 수식을 고르시오

$F(n)=F(n/10)+F(n/15)+2F(n/6)+n^{0.5}$

1. $F(n)=O(n^{0.7737})$
2. $F(n)=O(n^{0.5})$
3. $F(n)=O(n^{0.5}logn)$
4. $F(n)=O(nlogn)$
5. $F(n)=O(n)$

답 :
