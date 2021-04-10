#2.3 최대합 부분배열
# 길이가 n인 정수의 배열 A[0..n-1]가 있다. 
# A[a] + A[a+1] +...+ A[b]의 값을 최대화하는 구간 (a,b)를 찾는 방법을 Divide-and-Conquer 전략을 이용하여 설계하고 분석하라.
# 예를들어, 배열A가 아래와 같이 주어졌을 경우 (n = 10),
#     31 -41 59 26 -53 58 97 -93 -23 84
# 답은 a = 2, b = 6인 경우의 59+26-53+58+97=187가 된다.
        
A = [31, -41, 59, 26, -53, 58, 97, -93, -23, 84]
MIN = -2 ** 31 - 1

def divide_conquer(A):
    N = len(A)                                          # 배열 A의 길이
    def find(lo, hi):                                   # lo : 인덱스의 최소값, hi : 인덱스의 최대값, mid : 중간인덱스
        if lo == hi:
            return A[lo]
        mid = (lo + hi) // 2
    
        left = find(lo, mid)
        right = find(mid+1, hi)

        tmp = 0
        left_part = MIN
        for i in range(mid, lo-1, -1):
            tmp += A[i]
            left_part = max(left_part, tmp)
        tmp = 0
        right_part = MIN
        for i in range(mid+1, hi+1):
            tmp += A[i]
            right_part = max(right_part, tmp)

        return max(left, right, left_part + right_part)
    return find(0, N-1)

print(divide_conquer(A))

# https://jungmonster.tistory.com/126
# https://shoark7.github.io/programming/algorithm/4-ways-to-get-subarray-consecutive-sum