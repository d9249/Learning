# 문제 1의 1부터 n까지의 합 구하기를 재귀 호출로 만들어 보세요.
n = int(input())
def recu(n):
    if n <= 1:
        return 1
    return n + recu(n-1)

print(recu(n))