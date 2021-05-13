# 길이가 n 인 정수의 배열 A[0..n-1]가 있다. 
# A[a]*A[a+1] * ... * A[b]의 값을 최대화하는 구간 (a, b)를 
# 찾는 방법을 설계하고 분석하라. 
# 배열 A 의 원소는 양수, 음수,0 모두 가능하다.
# 예를 들어, 배열 A가 아래와 같이 주어졌을 경우 

#     (n=7), -6 12 -7 0 14 -7 5

# 답은 a = 0, b = 2 인 경우의 (-6)*12*(-7)=504 가 된다.

A = [-6, 12, -7, 0, 0, 14, -7, 5]

def find(A):
    pos = []    #배열을 나눴을 때 점점 커지는 양수가 들어갈 배열 (positive)
    neg = []    #배열을 나눴을 때 점점 작아지는 음수가 들어갈 배열 (negative)
    for i in range(len(A)): #양수배열, 음수배열 초기화
        pos.append(1)
        neg.append(1)
    i = 0
    for i in range (len(A)):    #A배열의 길이만큼 반복
        if i == 0:              #배열의 첫 번째 원소에 접근하는 경우 초기화가 필요하다.
            if A[0]<0:          #첫 번째 원소가 음수라면
                pos[0] = 1      #양수배열의 첫 번째 원소는 1로,
                neg[0] = A[0]   #음수배열의 첫 번째 원소는 A배열의 첫 번째 원소로
                continue        #초기화 한 뒤 두 번째 원소에 대해 계산한다.
            
        if A[i] == 0:           #배열의 값이 0이라면 배열을 나눠야 한다.
            if A[i+1] == 0:     #배열의 다음 원소 또한 0이라면
                continue        #초기화 작업 없이 넘어간다.
            elif A[i+1]<0:      #배열의 다음 원소가 음수라면
                pos[i+1] = 1    #양수배열의 나뉜 후 첫 번째 원소는 1로,
                neg[i+1] = A[i+1]#음수배열의 첫 번째 원소는 A배열 원소로
                i = i+1         #원래 원소의 다음 원소에 접근했으므로 i값을 1 증가시켜
                continue        #나뉜 다음 배열에 대해 접근한다.
            else:                   #배열의 다음 원소가 양수라면
                neg[i+1] = A[i+1]   #음수배열, 양수배열 모두 나뉜 후 첫 번째 원소로
                pos[i+1] = A[i+1]   #바꿔준다.
                i = i+1             #원래 원소의 다음 원소에 접근했으므로 i값을 1 증가시켜
                continue            #나뉜 다음 배열에 대해 접근한다.
          
        if A[i] > 0:                #배열의 값이 양수라면 다음 작업을 수행한다.
            pos[i] = pos[i-1] * A[i]#양수배열의 경우 이전값과 배열값을 곱한다.
            if neg[i-1]>0:          #음수배열의 경우 이전값이 0보다 크다면
                neg[i] = A[i]       #양수를 곱하면 더 커지기 때문에 A[i]로 교체한다.
            else:                   #이전값이 음수였다면
                neg[i] = neg[i-1] * A[i]#이전값과 배열값을 곱해 더 작게 만든다.
                
        else:                       #배열의 값(A[i])이 음수라면 다음 작업을 수행한다.
            if pos[i-1] > neg[i-1] * A[i]: #음수배열의 이전값과 A[i]를 곱했을때
                pos[i] = 1          #양수배열의 이전값보다 작다면 양수배열을 1로 설정한다.
            else:                   #음수배열의 이전값과 A[i]를 곱했을 때 더 큰 수가 나온다면
                pos[i] = neg[i-1] * A[i]#양수배열을 음수배열 * A[i](음수)로 설정한다.
                
            if neg[i-1] < pos[i-1] * A[i]:#음수배열의 이전값 보다
                neg[i] =A[i]        #양수배열의 이전값*A[i]가 크다면 음수배열을 초기화한다.
            else:                   #음수배열의 이전값이 양수배열의 이전값 * A[i]보다 크다면
                neg[i] = pos[i-1]*A[i]#음수배열값을더 작은값으로 설정한다.
        print(pos)                  #과정을 확인하기위해 음수, 양수배열을 찍는다.
        print(neg)
        print()
    print("답 : " , max(pos))#양수배열중 최대값을 계산한다.

find(A)