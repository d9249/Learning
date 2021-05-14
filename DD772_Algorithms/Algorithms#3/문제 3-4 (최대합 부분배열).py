# 길이가 n 인 정수의 배열 A[0..n-1]가 있다. 
# A[a] + A[a+1] + ... + A[b]의 값을 최대화하는 구간 (a, b)를 
# O(n) 시간 안에 찾는 방법을 설계하고 분석하라.
# 예를 들어, 배열 A가 아래와 같이 주어졌을 경우 

#   (n = 10), 31 -41 59 26 -53 58 97 -93 -23 84

# 답은 a = 2, b = 6 인 경우의 59+26-53+58+97=187가 된다.

# 구하고자하는 배열 A
# A = [31, -41, 59, 26, -53, 58, 97, -93, -23, 84]
# A = [-7, 4, -3, 6, 3, -8, 3, 4]
A = [-6, 12, -7, 0, 0, 14, -7, 5]
# A = [31, -41, 59, 26, -53, -158, 97, -93, 23, 84]

# Maximum sum subarray
def MaxSumArray(A):
    cache = [None] * len(A)
    cache[0] = A[0]
    for i in range(1, len(A)):
        cache[i] = max(0, cache[i-1]) + A[i]
        if (cache[i-1] < 0):
            start = i
    print("a:",start)
    print("b:",cache.index(max(cache)))
    return max(cache)

print(MaxSumArray(A))

# -가 나오면 그 다음 인덱스가 무조건 시작 인덱스 번호