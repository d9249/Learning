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

# 입력 예  입력 예에 대한 출력
# 1 1           1
# 1 1

# 설명
# 3 = 3x3 행렬 2 = 치즈의 개수
# 1 2 (치즈의 좌표), 3 1(치즈의 좌표)

# 3 2           1
# 3 2                   
# 1 2                 
# 3 1

# 5 5           3
# 2 3
# 3 2
# 4 3
# 4 5
# 5 2

def Way(i, n):                                                  # i, i부터 i, n까지 and i, i부터 n, i까지 치즈의 최대값을 구한다
    E[i][i] = max(E[i-1][i], E[i][i-1]) + T[i][i]               # (i, i)에서 먹을 수 있는 치즈의 최댓값 = 왼쪽 좌표, 아래 좌표 중 큰 값 + (i, i)위치의 치즈 개수
    if i == n:                                                  # 2차원 배열의 마지막에 도달하였다면,
        return E[n][n]                                          # n, n에서 먹을 수 있는 치즈의 최댓값 리턴
    else:                                                       # 
        for j in range (i+1, n+1):                              # (i+1, i)부터 (n, i) / (i, i+1)부터 (i, n)까지 치즈값
            E[i][j] = max(E[i-1][j], E[i][j-1]) + T[i][j]       # 왼쪽에서 온 경로의 값이 더 큰지, 아래에서 온 경로의 값이 더 큰지 비교 후
            E[j][i] = max(E[j-1][i], E[j][i-1]) + T[j][i]       # 더 큰 값에 현재 좌표의 위치에 존재하는 치즈의 개수를 더한다. 
        return Way(i+1, n)                                      # i를 1씩 증가하여 반복한다. 테이블의 크기가 N*N 다음 N-1*N-1, N-2*N-2,,,으로 
                                                                # 줄어들며 N,N에 도착하게되면 종료한다.
N, M = map(int, input("").split(' '))                           # N(치즈 테이블의 크기), M(치즈의 개수)
T = [[0 for col in range(N+1)]for row in range(N+1)]            # 배열 초기화 & 생성
E = [[0 for col in range(N+1)]for row in range(N+1)]            # 배열 초기화 & 생성
for i in range (M):                                             # 치즈의 좌표를 모두 설정하기 위해 치즈의 개수만큼 반복
    Q, W = map(int, input("").split(' '))                       # Q, W(치즈의 좌표를 입력)
    T[Q][W] = int(1)                                            # 치즈의 좌프를 1로 설정.
print('개수: ',Way(1, N))                                        # 위에서 선언한 함수를 사용하여서 치즈를 최대로 먹을 수 있는 개수를 구한다.

# 해당 알고리즘의 점화식
# -1                                                if i, j < 0
# 0                                                 if i, j = 0 (치즈가 없는 경우)
# 1                                                 if i, j = 1 (치즈가 있는 경우)
# E[i][j] = max(E[i-1][j], E[i][j-1]) + T[i][j]     if i, j > 1
# E[j][i] = max(E[j-1][i], E[j][i-1]) + T[j][i]

# 요약
# 해당 알고리즘을 요약하자면, 테이블의 크기를 줄여가면서 이전 경로에서의 값을 비교하여서 2개의 경로 중 최대의 값을 가지는 경우를     
# 계속해서 더해가면서 목적지에 도착하게 되면 목적지에서의 총 합계를 반환하는 알고리즘입니다.

# 시간 복잡도 : O(n^2)
