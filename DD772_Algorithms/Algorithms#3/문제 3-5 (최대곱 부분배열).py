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

A = [2, 4, -2, 4, 5, 2, 0]
# A = [2, 4, -2, 4, 5]
# A = [2, 11, -2, 4, 5]
# A = [-7, 4, -3, 6, 3, -8, 3, 4]
# A = [-2,3,4,-5,6,-7,2]
# A = [-6, 12, -7, 0, 0, 14, -7, 5]
# A = [2, 3, -2, 4, 5]
# A = [2,3,4,-5,6,-7,2]
# A = [-6, 12, -7, 0, 0, 14, -7, 5]
# A = [2, 11, -2, 4, 5]
# A = [7, 4, -3, 6, -3, 8, -3, 4]
# A = [7, -4, -3, 6, -3, 8, 3, 4]
# A = [-6, 12, 7, 0, 0, 14, 7, 5]
# A = [-6, 12, -7, 0, 0, 14, -7, 5]           # 
# B = []                                      # 

X = [0 for i in range(len(A))]
X[-1] = A[-1]
def Maxmul(A):
  Smin = [0 for i in range(len(A))]                         #
  Smax = [0 for i in range(len(A))]                         #
  Smin[0] = A[0]                                            #
  Smax[0] = A[0]                                            #
  start,end = 0,0                                           #
  for k in range(1,len(A)):
    Smin[k] = min(Smin[k-1]*A[k], Smax[k-1]*A[k], A[k])
    Smax[k] = max(Smin[k-1]*A[k], Smax[k-1]*A[k], A[k])

  if (max(Smax) < max(Smin)):
    end = Smin.index(max(Smin))
  else:
    end = Smax.index(max(Smax))
  
  for k in range(len(A)-1,0,-1):
    X[k-1] = X[k] *A[k-1]
    if (X[k] == max(Smax)):
      start = k

  print("a:",start,"b:",end)
  return max(Smin,Smax)
  
print(max(Maxmul(A)))

# 점화식
# -1                                                    if 배열의 길이 <= 0
# Smin[k] = min(Smin[k-1]*A[k], Smax[k-1]*A[k], A[k])   if 배열의 길이 > 1
# Smax[k] = max(Smin[k-1]*A[k], Smax[k-1]*A[k], A[k])

# 풀이

# 시간 복잡도 : O(n)

# def MaxProduct(A):                          # 
#     if not A:                               # 
#         return                              # 
#     Lmin = Lmax = Gmax = A[0]               # 
#     for i in range(1, len(A)):              # 
#         Tmp = Lmin                          # 
#         Lmin=min(Lmin*A[i], A[i], Lmax*A[i])# 
#         Lmax=max(Tmp*A[i], A[i], Lmax*A[i]) # 
#         Gmax=max(Gmax, Lmax)                # 
#         B.append(max(Gmax, Lmax))           # 
#     return Gmax                             # 

# def ZeroToArray(A):                         # 
#     for i in range(len(A)):                 # 
#         if A[i] == 0:                       # 
#             return i-1                      # 

# def MaxMain(A):                             # 
#     count = 0                               # 
#     C = A                                   # 
#     D = list(reversed(C))                   # 
#     for i in range(len(A)):                 # 
#         if A[i] < 0:                        # 
#             count += 1                      # 
#             Z = i                           # D 배열의 처음으로 마주하는 - 원소
#     for i in range(len(D)):                 # 
#         if D[i] < 0:                        # 
#             X = i                           # A 배열의 처음으로 마주하는 - 원소
#     T = MaxProduct(D[:X])                   # 뒷부분
#     Y = MaxProduct(A[:Z])                   # 앞부분
#     if count % 2 == 0:                      # 
#         print("a: 0")                       # 
#         print("b:", len(A)-1)               # 
#         print(MaxProduct(A))                # 
#     else:                                   # 
#         if T < Y:                           # 앞이 더 큰경우
#             print("a: 0")                   # 
#             if (ZeroToArray!=None):         # 
#                 print("b:",ZeroToArray(A))  # 
#             else:                           # 
#                 print("b:", len(A[:Z])-1)   # 
#             print(MaxProduct(A))            # 
#         elif T > Y:                         # 뒤가 더 큰경우
#             print("a:",len(A)-len(D[:X]))   # 
#             print("b:", len(A)-1)           # 
#             print(MaxProduct(A))            # 

# MaxMain(A)
