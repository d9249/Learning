# 2.2_a
A=[0,2,3,4,5,6] #대상이 되는 배열

def FindIndex(A,F,L): 
    M = (F+L) // 2
    if (A[0] == 0):
        return 0 
    if(F > L):
        return -1
    if (A[M] < M):
        return FindIndex(A,M+1,L)
    elif(A[M] > M):
        return FindIndex(A,F+1,M)
    else:
        return M

print(FindIndex(A,0,len(A)-1))