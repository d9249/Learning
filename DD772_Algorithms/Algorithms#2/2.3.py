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

print(DivideAndConquer(A))
# 시간 복잡도 : T(n) = 2T(n/2) + O(n) = O(nlogn)
# 정확도 : 문제에서 제시되는 case의 배열을 모두 잘 해결하고, 다른 배열에 대해서도 잘 해결할 수 있기 때문에 정확도는 100%이다.

# https://jungmonster.tistory.com/126
# https://shoark7.github.io/programming/algorithm/4-ways-to-get-subarray-consecutive-sum