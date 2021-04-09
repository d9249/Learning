# 2.1_(c)
# “정렬후 회전된 배열”이란 (5, 8, 9, 2, 3, 4)와 같은 배열을 말한다.
# 즉, 정렬이 된 후에 회전 연산이 0회 이상 적용된 배열이다.
# 회전 연산이란 배열의 마지막 원소가 처음으로 이동하고 나머지 원소들이 오른쪽으로 한 칸씩 이동하는 것을 말한다.
# 예를들어, (2, 3, 4, 5, 8, 9)는 정렬된 배열이고 여기에 회전 연산을 1회 적용하면 (9, 2, 3, 4, 5, 8)이 되고
# 여기에 회전 연산을 추가로 2회 적용하면 (5, 8, 9, 2, 3, 4)가 된다.
# 따라서 (5, 8, 9, 2, 3, 4)는 정렬후 회전된 배열이다.
# (c) 정렬후 회전된 배열A [0..n-1]와 k가 주어질 때, A 안에서 k를 탐색하는 알고리즘을 설계하고 분석하시오.
# 즉, A의 원소 중에 k가 있으면 그 위치(index)를 출력하고 없으면 -1을 출력합니다.

def Find(A, i, k):
    if A[i] == k:
        return i
    if i == len(A)-1:
        return -1
    else:
        return Find(A, i+1, k)
    
A= [5, 8, 9, 81, 99, -99, -98, 2, 3, 4]
i = 0
k = 7
print(Find(A, i, k))