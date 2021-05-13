# 길이가 n 인 정수의 배열 A[0..n-1]가 있다. 
# A[a] + A[a+1] + ... + A[b]의 값을 최대화하는 구간 (a, b)를 
# O(n) 시간 안에 찾는 방법을 설계하고 분석하라.
# 예를 들어, 배열 A가 아래와 같이 주어졌을 경우 

#   (n = 10), 31 -41 59 26 -53 58 97 -93 -23 84

# 답은 a = 2, b = 6 인 경우의 59+26-53+58+97=187가 된다.

A = [31, -41, 59, 26, -53, 58, 97, -93, -23, 84]

sum = 0
q = 9999999                 #임의의 최솟값
left = 0
for i in range(len(A)):     #왼쪽부터 A배열의 길이만큼 반복
    sum = sum+A[i]          #합을 구하며
    if sum<0:               #0보다 작아진 경우
        if sum<q:           #최솟값보다 작은지 계산하여
            q = sum         #최솟값을 교체한다.
            left = i+1      #왼쪽부터 몇번 째 수인지 기록한다.
sum = 0
q = 9999999
i = 0
right = 0
for i in range(len(A)-left): #오른쪽의 경우도 위와 동일한 방법으로 구한다.
    sum = sum+A[len(A)-1-i]
    if sum < 0:
        if sum < q:
            q = sum
            right = i+1
print(left, right)#왼쪽, 오른쪽으로 부터 몇 번째 값인지 출력한다.
sum = 0
i = 0
for i in range(len(A)-left-right):
    sum = sum + A[i+left]
print(sum)#값을 계산하여 출력한다.