# 길이가 n 인 정수의 배열 A[0..n-1]가 있다. 
# A[a]*A[a+1] * ... * A[b]의 값을 최대화하는 구간 (a, b)를 
# 찾는 방법을 설계하고 분석하라. 
# 배열 A 의 원소는 양수, 음수,0 모두 가능하다.
# 예를 들어, 배열 A가 아래와 같이 주어졌을 경우 

#     (n=7), -6 12 -7 0 14 -7 5

# 답은 a = 0, b = 2 인 경우의 (-6)*12*(-7)=504 가 된다.

# 풀이

# 전체의 배열의 마이너스가 짝수라면 무조건 다 곱한거 = 시작 인덱스 0
# 중간의 0이 있다면 0을 기준으로 앞뒤로
# 홀수라면 -가 있는 인덱스의 바로 앞

# 1. 모든 수의 절대값기준으로 최대값을 구해
# 그 연산안에 -의 원소의 개수를 찾아
# 짝수인 것들중에서 가장 큰수 = 최대값
# 홀수인경우 - 볼필요조차 없어져

# A = [2, 4, -2, 4, 5]
# A = [2, 1, -2, 4, 5]
# A = [-7, 4, -3, 6, 3, -8, 3, 4]
# A = [-2,3,4,-5,6,-7,2]
# A = [-6, 12, -7, 0, 0, 14, -7, 5]
# A = [2, 3, -2, 4, 5]
# A = [2,3,4,-5,6,-7,2]
# A = [-6, 12, -7, 0, 0, 14, -7, 5]
# A = [2, 11, -2, 4, 5]
# A = [7, 4, -3, 6, -3, 8, 3, 4]
# A = [7, -4, -3, 6, -3, 8, 3, 4]

A = [-6, 12, -7, 0, 0, 14, -7, 5]
B = []

def maxProduct(nums):
    if not nums:
        return 
    locMin = locMax = gloMax = nums[0]
    for i in range(1, len(nums)):
        tmp = locMin
        locMin = min(locMin*nums[i], nums[i], locMax*nums[i])
        locMax = max(tmp*nums[i], nums[i], locMax*nums[i])
        gloMax = max(gloMax, locMax)
        B.append(max(gloMax, locMax))
    return gloMax

def max2(nums):
    count = 0
    C = A
    D = list(reversed(C))

    for i in range(len(A)):
        if A[i] < 0:
            count += 1
            Z = i # D 배열의 처음으로 마주하는 - 원소
    for i in range(len(D)):
        if D[i] < 0:
            X = i # A 배열의 처음으로 마주하는 - 원소

    T = maxProduct(D[:X])
    Y = maxProduct(A[:Z])
    if count % 2 == 0:
        print("a: 0")
        print("b:", len(A)-1)
        print(maxProduct(A))
    else:
        if T < Y:
            print("a: 0")
            print("b:", len(A)-len(A[:Z]))
            print(maxProduct(A))
        elif T > Y:
            print("a:",Z+1)
            print("b:", len(A)-1)
            print(maxProduct(A))
max2(A)

    # print("A:",A)
    # print("AR",D)
    # print(Z)
    # print(A)
    # print(A[:Z])
    # print(maxProduct(A[:Z]))
    # print(X) 
    # print(D[:X])
    # XD = list(reversed(D[:X]))
    # print(XD)
    # print(maxProduct(D[:X]))
# def MaxProduct(nums):
#     if not nums:
#         return 
#     Count = 0
#     Lmin = Lmax = Gmax = nums[0]

#     for i in range(len(A)):
#         if A[i] < 0:
#             Count += 1
    
#     for i in range(1, len(nums)):
#         Tmp = Lmin
#         Lmin = min(Lmin*nums[i], nums[i], Lmax*nums[i])
#         Lmax = max(Tmp*nums[i], nums[i], Lmax*nums[i])
#         Gmax = max(Gmax, Lmax)
#         B.append(max(Gmax, Lmax))
    
#     if Count % 2 == 0:
#         print("a: 0")
#     else:
#         print("a: ")
#     print("b:",B.index(max(B))+1)
#     return Gmax
# print(MaxProduct(A))
