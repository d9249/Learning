# 2.1_(c)
# “정렬후 회전된 배열”이란 (5, 8, 9, 2, 3, 4)와 같은 배열을 말한다.
# 즉, 정렬이 된 후에 회전 연산이 0회 이상 적용된 배열이다.
# 회전 연산이란 배열의 마지막 원소가 처음으로 이동하고 나머지 원소들이 오른쪽으로 한 칸씩 이동하는 것을 말한다.
# 예를들어, (2, 3, 4, 5, 8, 9)는 정렬된 배열이고 여기에 회전 연산을 1회 적용하면 (9, 2, 3, 4, 5, 8)이 되고
# 여기에 회전 연산을 추가로 2회 적용하면 (5, 8, 9, 2, 3, 4)가 된다.
# 따라서 (5, 8, 9, 2, 3, 4)는 정렬후 회전된 배열이다.
# (c) 정렬후 회전된 배열A [0..n-1]와 k가 주어질 때, A 안에서 k를 탐색하는 알고리즘을 설계하고 분석하시오.
# 즉, A의 원소 중에 k가 있으면 그 위치(index)를 출력하고 없으면 -1을 출력합니다.

A = [5, 8, 9, 2, 3, 4]        # 해당하는 배열
k = 8                         # 찾고자하는 k의 값
I = 0                         # 최대값의 인덱스 번호를 계산하기 위한 변수

def EleIndOfArrFind(A, k, I):
    if(len(A) == 1):                           # 아래의 과정을 반복해서 나온 결과의 배열 길이가 1이라면,
        if(A[0] == k):                         # 해당 배열의 원소와 찾고자하는 k의 값을 비교한다.
            print(I)                           # 같다면 계산한 Index를 출력하고,
        else:                                  # 맞지 않다면, -1을 출력한다.
            print(-1)
    else:                                      # 먼저 1단계로 찾고자하는 배열을 반으로 슬라이싱을 한다.
        A1 = A[:len(A)//2]
        A2 = A[len(A)//2:]                     # 먼저, 배열의 범위에 속하는 지 알아보는 이유는 맨끝의 원소가 맨앞의 원소보다 크다면, 정렬된 배열이라는 반증이기 때문에 해당 과정을 통해 문제를 푼다.
        if(A1[-1] > A1[0]):                    # 잘린 배열 A1의 뒷부분의 마지막 원소가 잘린 배열의 처음 원소보다 크다면,
            if(k <= A1[-1] and k >= A1[0]):    # 먼저 A1의 배열의 범위 안에 속하는지 알아보고, 그렇지 않다면 배열 A2에 k가 범위에 속하는지 알아본다.
                EleIndOfArrFind(A1, k, I)      # 만약에 A1[-1] > A1[0]을 만족한다면, k가 A1의 속하는지 알아낸다.
            else:                              # 속하지 않는다면, A2에 속하는지 알아내기 위해 A2를 재귀호출의 매개변수로 넘긴다.
                I += len(A)//2                 # 그리고, 배열의 인덱스를 알아내기 위해서 I의 배열의 값의 반을 더해줌으로써
                EleIndOfArrFind(A2, k, I)      # 인덱스의 번호를 따로 계산한다.
        else:
            if(k <= A2[-1] and k >= A2[0]):    # 이 경우 잘린 A1 배열의 뒷부분의 마지막 원소가 잘린 배열의 처음보다 크지않은 경우이기 때문의 A2의 범위에 속하는지 먼저 확인을 진행하고,
                I += len(A)//2                 # 인덱스의 번호를 계산.
                EleIndOfArrFind(A2, k, I)      # A2의 범위에 속한다면, A2를 재귀로 반복한다.
            else:
                EleIndOfArrFind(A1, k, I)      # 그렇지 않다면, A1을 배열로 사용한다.

EleIndOfArrFind(A, k, I)

# 시간복잡도 : T(n) = T(n/2) + O(1) = O(logn)
# 정확도 : 위의 설명으로 코드를 진행한다면, k값의 인덱스를 정확하게 찾아낼 수 있으므로 정확도는 100%이다.