# 숫자 n개를 리스트로 입력받아 최솟값을 구하는 프로그램을 만들어 보세요.

n = list(map(int, input().split()))
a = len(n)
x = n[0]

for i in range(1, a):
    if n[i] < x:
        x = n[i]
print(x)