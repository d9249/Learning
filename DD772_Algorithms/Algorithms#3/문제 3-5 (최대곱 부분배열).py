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

# 1 1 1 1 -1 -1 1 1 1 1 1 1 1 -1 1 1 1 1 -1 -1 1 1 1 1 1 
# 1 1 1 1 -1 -1 1 1 1 1 1 1 1 -1 1 1 1 1 -1
#  -1 1 1 1 1 1 1 1 -1 1 1 1 1 -1 -1 1 1 1 1 1 
#  -1 1 1 -1 1 1 -1

# B = [5,-7,14,0,0,-7,12,-6]
# A = [-6, 12, -7, 0, 0, 14, -7, 5]
# A = [2, 3, -2, 4, 5]
# A = [-7, 4, -3, 6, 3, -8, 3, 4]
# A = [-2,3,4,-5,6,-7,2]
# A = [-6, 12, -7, 0, 0, 14, -7, 5]
# A = [2, 3, -2, 4, 5]
# A = [7, 4, -3, 6, -3, 8, 3, 4]
# B = []
# C = []

# count = 0
# for i in range(len(A)):
#         if A[i] < 0:
#             count += 1

# def maxProduct(nums):
#     if not nums:
#         return 
    
#     locMin = locMax = gloMax = nums[0]

#     for i in range(1, len(nums)):
#         tmp = locMin
#         locMin = min(locMin*nums[i], nums[i], locMax*nums[i])
#         locMax = max(tmp*nums[i], nums[i], locMax*nums[i])
#         gloMax = max(gloMax, locMax)
#         B.append(max(gloMax, locMax))
        
#     # print("b:",B.index(max(B))+1)
#     return gloMax
# # print(maxProduct(A))

# def max2(nums):
#     for i in range(1, len(nums)):
#         if count % 2 == 0:
#             print("a: 0")
#         else:
#             # print("a: ")
#             # 배열을 뒤집지 않고 -값이 가장 먼저 나오는 원소의 인덱스
#             Q = maxProduct(A[A.index(A[i])+1:])

#             # 배열을 뒤집어서 -값이 가장 먼저 나오는 원소의 인덱스
#             C = A
#             C.reverse()
#             W = maxProduct(C[C.index(C[i])+1:])
#         print(W)
#         print(Q)
#     if (Q < W):
#         print(W)
#     else:
#         print(Q)
# print(max2(A))

# A = [2,3,4,-5,6,-7,2]
A = [-6, 12, -7, 0, 0, 14, -7, 5]
# A = [2, 3, -2, 4, 5]
# A = [7, 4, -3, 6, -3, 8, 3, 4]
B =[]

def MaxProduct(nums):
    if not nums:
        return 
    Count = 0
    Lmin = Lmax = Gmax = nums[0]

    for i in range(len(A)):
        if A[i] < 0:
            Count += 1
    
    for i in range(1, len(nums)):
        Tmp = Lmin
        Lmin = min(Lmin*nums[i], nums[i], Lmax*nums[i])
        Lmax = max(Tmp*nums[i], nums[i], Lmax*nums[i])
        Gmax = max(Gmax, Lmax)
        B.append(max(Gmax, Lmax))
    
    if Count % 2 == 0:
        print("a: 0")
    else:
        print("a: ")
    print("b:",B.index(max(B))+1)
    return Gmax
print(MaxProduct(A))
