#2.3 최대합 부분배열
# 길이가 n인 정수의 배열 A[0..n-1]가 있다. 
# A[a] + A[a+1] +...+ A[b]의 값을 최대화하는 구간 (a,b)를 찾는 방법을 Divide-and-Conquer 전략을 이용하여 설계하고 분석하라.
# 예를들어, 배열A가 아래와 같이 주어졌을 경우 (n= 10),
#     31 -41 59 26 -53 58 97 -93 -23 84
# 답은 a = 2, b = 6인 경우의 59+26-53+58+97=187가 된다.

def FindMaxSum(A, a, b):
    if a == b:
        return A[a]
    tok = (a+b)//2
    Lsum = FindMaxSum(A, a, tok)
    Rsum = FindMaxSum(A, tok+1, b)
    if Lsum<0:
        FindMaxSum(A, tok, b)
    if Rsum<0:
        FindMaxSum(A, a, tok+1)
        
        
A= [31, -41, 59, 26, -53, 58, 97, -93, -23, 84]
print(FindMaxSum(A, 0, 10))