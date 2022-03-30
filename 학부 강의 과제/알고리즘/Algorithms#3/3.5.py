A = [-6, 12, -7, 0, 0, 14, -7, 5]
B = []

def MaxProduct(arr):
    if not arr:
        return 
    Lmin = Lmax = Gmax = arr[0]
    for i in range(1, len(arr)):
        tmp = Lmin
        Lmin = min(Lmin*arr[i], arr[i], Lmax*arr[i])
        Lmax = max(tmp*arr[i], arr[i], Lmax*arr[i])
        Gmax = max(Gmax, Lmax)
        B.append(max(Gmax, Lmax))
    return Gmax

def main(arr):
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

    T = MaxProduct(D[:X])
    Y = MaxProduct(A[:Z])
    if count % 2 == 0:
        print("a: 0")
        print("b:", len(A)-1)
        print(MaxProduct(A))
    else:
        if T < Y:
            print("a: 0")
            print("b:", len(A)-len(A[:Z]))
            print(MaxProduct(A))
        elif T > Y:
            print("a:",Z+1)
            print("b:", len(A)-1)
            print(MaxProduct(A))
main(A)