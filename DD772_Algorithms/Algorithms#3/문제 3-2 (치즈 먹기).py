# N * N개의 칸이 있는 테이블이 있고, 치즈 몇 개가 테이블에 놓여있다. 
# 치즈를 좋아하는 생쥐 미키는 테이블의 (1,1)에서 출발해서 
# (N,N)에 도착할 때까지 많은 치즈를 먹고 싶어한다. 
# 하지만 머리가 나쁜 미키는 위로 올라가거나 오른쪽으로만 이동할 수 있다. 
# 예를 들어, 아래 그림에서 미키는 5개의 치즈 중 최대 3개의 치즈만 먹으며 이동할 수 있다.

# 테이블 크기 N과 M개의 치즈 위치가 주어졌을 때 최대로 먹을 수 있는 
# 치즈의 개수를 구하는 프로그램을 작성하시오. 
# 치즈의 위치는 (x,y)의 좌표로 주어지며 왼쪽 아래 구석의 위치를 
# (1,1)로, 맨 오른쪽 위 구석의 위치를 (N,N)으로 한다.

# 입력
# 입력은 표준입력(standard input; 키보드를 통한 입력)을 사용한다. 
# 입력은 첫 줄에 자 연수 N과 M이 주어진다. 
# 이 때, N과 M은 1 이상 10000 이하의 범위이다. 
# 다음 M개의 줄에 각각의 치즈의 위치 x와 y가 주어진다.

# 출력
# 출력은 표준출력(standard output; 모니터 화면에 출력)을 사용한다. 
# 주어진 입력에 대해 최대로 먹을 수 있는 치즈의 개수를 정수 형태로 출력한다.

# 입력 예             입력 예에 대한 출력
# 1 1                   1
# 1 1

# 3 2                   1
# 1 2                 
# 3 1

# 5 5                   3
# 2 3
# 3 2
# 4 3
# 4 5
# 5 2 

def cheeseBig(i, n):  #i, i부터 i, n까지 and i, i부터 n, i까지 치즈의 최대값을 구한다
    eatCheese[i][i] = max(eatCheese[i-1][i], eatCheese[i][i-1]) + cheeseTable[i][i]
#(i, i)에서 먹을 수 있는 치즈의 최댓값 = 왼쪽, 아래족중 큰 값 + (i, i)위치의 치즈갯수
    if i == n:#n, n의값을 구했다면
        return eatCheese[n][n]#n, n에서 먹을 수 있는 치즈의 최댓값 리턴
    else:
        for j in range (i+1, n+1): #(i+1, i)부터 (n, i) / (i, i+1)부터 (i, n)까지 치즈값
            eatCheese[i][j] = max(eatCheese[i-1][j], eatCheese[i][j-1]) + cheeseTable[i][j]
            eatCheese[j][i] = max(eatCheese[j-1][i], eatCheese[j][i-1]) + cheeseTable[j][i]
        return cheeseBig(i+1, n) #i를 하나 올려 반복(구해야하는 테이블의 열과 행이 1씩 감소)
N, M = input("").split(' ')#이하 n과 m등을 입력받고, 함수 호출하는 과정
n = int(N)
m = int(M)

cheeseTable = [[0 for col in range(n+1)]for row in range(n+1)]
eatCheese = [[0 for col in range(n+1)]for row in range(n+1)]
for i in range (m):
    Q, W = input("").split()
    q = int(Q)
    w = int(W)
    cheeseTable[q][w] = int(1)
print(cheeseBig(1, n))