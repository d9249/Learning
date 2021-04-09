# 2.2_a
# 입력으로 주어지는 배열A [0..n-1]은 오름차순으로 정렬되어 있으며 n개의 서로 다른 정수들을 원소로 가진다.
# 즉, A[0] < A[1] < ... < A[n-1] 이다.
# 원소들은 양수, 음수 혹은 0 일 수 있다.
# (a) A[i] = i를 만족하는 index i가 존재하는지 알고 싶다.
# 그런 index i가 존재하면 찾아서 i를 출력하고, 없으면 -1을 출력하는 알고리즘을 설계하고 분석하시오.

A=[-1,0,1,2,4] #대상이 되는 배열

def SamePosAndVal(A,F,L): 
    M = (F+L) // 2
    if (A[0] == 0):
        return 0
    if(F > L):
        return -1
    if (A[M] < M):
        return SamePosAndVal(A,M+1,L)
    elif(A[M] > M):
        return SamePosAndVal(A,F+1,M)
    else:
        return M

print(SamePosAndVal(A,0,len(A)-1))