#2.3 최대합 부분배열
# 길이가 n인 정수의 배열 A[0..n-1]가 있다. 
# A[a] + A[a+1] +...+ A[b]의 값을 최대화하는 구간 (a,b)를 찾는 방법을 Divide-and-Conquer 전략을 이용하여 설계하고 분석하라.
# 예를들어, 배열A가 아래와 같이 주어졌을 경우 (n = 10),
#     31 -41 59 26 -53 58 97 -93 -23 84
# 답은 a = 2, b = 6인 경우의 59+26-53+58+97=187가 된다.
        
A = [31, -41, 59, 26, -53, 58, 97, -93, -23, 84]        # 대상이 되는 배열
Min = -99999                                            # 절대 사용하지 않을 만한 최소값과 비교하기 위한 변수

def DivideAndConquer(A):
    N = len(A)                                          # 배열 A의 길이
    def Find(Lower, High):                              # Lower : 인덱스의 최소값, High : 인덱스의 최대값, Mid : 중간인덱스
        if Lower == High:                               # 해당 문제를 풀기 위한 중요 접근 방법 : 아래의 경우의 수로 풀 수 있으며, 예외는 없다.
            return A[Lower]                             # 1. [Lower, Mid] : 기준 배열의 왼쪽에 있는 경우
        Mid = (Lower + High) // 2                       # 2. [Mid+1, High] : 기준 배열의 오른쪽에 있는 경우
                                                        # 3. 양쪽 모두의 걸쳐 있는 경우
        Left = Find(Lower, Mid)                         # 위의 과정을 진행하기 위해서 Find함수에서 분할과 정복을 사용합니다.
        Right = Find(Mid+1, High)                       # Left, Right로 Find함수를 재귀적으로 사용하여서 왼쪽, 오른쪽에서의 최대합을 찾는다.

        Temp = 0                                        # 변수를 저장할 임시공간을 선언
        LeftSection = Min                               # 왼쪽 부분의 값을 앞에서 선언한 사용하지않을만한 가장 작은 최소값을 선언
        for i in range(Mid, Lower-1, -1):               # Lower부터 Mid까지의 최대합을 구한다.
            Temp += A[i]                                # Temp의 A의 배열을 하나씩 더하면서,
            LeftSection = max(LeftSection, Temp)        # 내장 함수 max를 사용하여서, 최대합을 return한다.
        Temp = 0                                        # 변수를 저장할 임시공간을 오른쪽에서도 사용하기 위해서 0으로 다시 초기화
        RightSection = Min                              # 오른쪽 부분의 값을 앞에서 선언한 사용하지않을만한 가장 작은 최소값을 선언
        for i in range(Mid+1, High+1):                  # Mid부터 High까지의 최대합을 구한다.
            Temp += A[i]                                # 왼쪽에서 했던 반복을 오른쪽에서도 진행한다.
            RightSection = max(RightSection, Temp)
        return max(Left, Right, LeftSection + RightSection) # 마지막으로 왼쪽, 오른쪽, 두개의 합 중 가장 큰 베스트 값을 return한다.
    return Find(0, N-1)                                 # Find를 배열의 길이만큼 재귀적으로 돌며 해당 task를 풀어낸다.

# print(DivideAndConquer(A))
# 시간 복잡도 : T(n) = 2T(n/2) + O(n) = O(nlogn)
# 정확도 : 문제에서 제시되는 case의 배열을 모두 잘 해결하고, 다른 배열에 대해서도 잘 해결할 수 있기 때문에 정확도는 100%이다.

# https://jungmonster.tistory.com/126
# https://shoark7.github.io/programming/algorithm/4-ways-to-get-subarray-consecutive-sum


# 해당 문제를 풀기 위한 중요 접근 방법 : 아래의 경우의 수로 풀 수 있으며, 예외는 없다.
    # 1. [Lower, Mid] : 기준 배열의 왼쪽에 있는 경우
    # 2. [Mid+1, High] : 기준 배열의 오른쪽에 있는 경우
    # 3. 양쪽 모두의 걸쳐 있는 경우

A = [31, -41, 59, 26, -53, 58, 97, -93, -23, 84]        # 대상이 되는 배열

def findMax(arr,start,last):
    def findMidMax(arr,start,mid,last):                 # 왼쪽, 오른쪽에 있는 경우가 아닌 중간을 포함해 걸쳐있는 경우를 위한 함수 선언.
        max_left, max_right = 0, 0                      # 변수를 저장할 임시공간을 선언
        left = -99999                                   # 절대 사용하지 않을 만한 최소값과 비교하기 위한 변수
        right = -99999
        sum = 0                                         # 합계를 비교하기 위해 저장해둘 변수 선언
        for i in range(mid, start-1, -1):               # start부터 mid까지의 최대 합을 구한다.
            sum += arr[i]                               # sum에 나누어진 배열의 값을 더해가며 비교한다.
            if sum > left:                              # 미리 선언 해둔 left값을 이용하여서 sum과의 크기 비교에 사용한다.
                left = sum                              # 이렇게 구해진 최대값을 left값으로 넘기고
                max_left = i                            # 해당 원소의 인덱스 번호를 따로 저장한다.
        sum = 0                                         # 위와 비슷한 방식을 진행하기 위해서 sum변수를 0으로 초기화후 다시 사용한다.
        for j in range(mid+1, last+1):                  # Mid부터 last까지의 최대 합을 구한다.
            sum += arr[j]                               # 해당 for문에서는 오른쪽으로 나누어진 배열의 가장 큰 원소의 값과 인덱스를 찾는다.
            if sum > right:                             # 
                right = sum                             # 
                max_right = j                           # 
        return (max_left, max_right, left+right)        # 그리고, 해당 부분에서 왼쪽과 오른쪽의 최대 값의 인덱스 번호와 왼쪽과 오른쪽의 최대값을 합을 리턴한다.

    if (start == last):                                 # 배열을 끝까지 나누어서 길이가 1이 되는 경우,
        return (arr[start],start,last)                  # 배열의 원소값과 인덱스 번호의 시작과 끝을 return한다.
    else:
        mid = (start+last) // 2                                                 # 배열의 중간이 되는 원소의 인덱스 번호
        leftSum, leftStart, leftLast = findMax(arr, start, mid)                 # 왼쪽 배열을 계속해서 나누어가면서 길이가 1이 될 때까지 재귀호출한다.
        rightSum, rightStart, rightLast = findMax(arr, mid+1, last)             # 오른쪽 배열을 계속해서 나누어가면서 길이가 1이 될 때까지 재귀호출한다.
        midsumStart, midsumLast, midsum = findMidMax(arr, start, mid, last)     # 배열의 중간을 기준으로 양쪽 모두의 걸쳐 있는 경우 findMidMax함수를
                                                                                # 호출하여 앞서 설명한 함수를 재귀호출한다.
    if leftSum >= rightSum and leftSum >= midsum:       # 왼쪽의 최대합이 오른쪽의 최대합과, 중간의 최대합보다 크다면,
        return (leftSum, leftStart, leftLast)           # 왼쪽의 최대합, 시작 인덱스 번호, 끝 인덱스 번호를 return 한다.
    elif (rightSum >= leftSum and rightSum >= midsum):  # 이 과정은 위와 같은 과정이지만, 
        return (rightSum,rightStart, rightLast)         # 오른쪽의 최대합이 왼쪽의 최대합과, 중간의 최대합보다 더 큰 경우이다.
    else:                                               # 그리고 나머지의 경우는 중간의 최대합이 더 큰 경우이기 때문에
        return (midsum,midsumStart,midsumLast)          # 중간의 최대합, 중간 배열의 인덱스 시작 번호와 끝 번호를 return하며, 종료한다.

# mm = findMax(A, 0, len(A)-1)
# print("a:", mm[1], "b:", mm[2], "최대 합:", mm[0])

# 시간 복잡도 : T(n) = 2T(n/2) + O(n) = O(nlogn)
# 정확도 : 문제에서 제시되는 case의 배열을 모두 잘 해결하고, 다른 배열에 대해서도 잘 해결할 수 있기 때문에 정확도는 100%이다.

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
for i in range(len(A)-left):    #오른쪽의 경우도 위와 동일한 방법으로 구한다.
    sum = sum+A[len(A)-1-i]
    if sum<0:
        if sum<q:
            q = sum
            right = i+1

print(left, right)         #왼쪽, 오른쪽으로 부터 몇 번째 값인지 출력한다.
sum = 0
i = 0
for i in range(len(A)-left-right):
    sum = sum+A[i+left]
print(sum)         #값을 계산하여 출력한다.