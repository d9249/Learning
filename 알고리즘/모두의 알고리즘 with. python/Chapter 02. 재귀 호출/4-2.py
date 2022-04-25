# 문제 2의 숫자 n개 중에서 최댓값 찾기를 재귀 호출로 만들어 보세요.

n = list(map(int, input().split()))
# a = len(n)

# def max(n):
#     x = n[0]
#     # for i in range(1, n):
#     if n > x:
#         x = n
#     return [for i in max(n)]

def find_max(a):
    n=len(a)
    if n==1:
        return a
    elif a[n-1]>=a[n-2]:
        del a[n-2]
    elif a[n-1]<=a[n-2]:
        del a[n-1]
    return find_max(a)

print(find_max(n))