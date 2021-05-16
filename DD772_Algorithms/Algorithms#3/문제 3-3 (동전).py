# Nadiria 라는 (상상의) 나라에서는 다음과 같은 액면가의 동전(화폐)을 사용한다고 한다. 
# $1, $4, $7, $13, $28, $52, $91, $365
# 어떤 곳이든 사람들은 돈을 주고 받을 때, 가능한 적은 개수의 동전을 사용하기를 원한다.
# 입력으로 자연수 K가 주어지면 Nadiria 화폐를 이용하여 
# $K 를 만들 수 있는 최소 동전 개수를 출력하는 알고리즘을 설계하고 분석하시오.

coins = [1,4,7,13,28,52,91,365]
# coins = [10,70,100]
def solution2(coins, money):
    dp = [0] * (money + 1)          # dp배열 초기화
    for i in range(1, money + 1):   # 1부터 money까지 순회한다.
        temp = 9999                 # temp = 임의의 큰 값
        j = 0
        while j < len(coins) and i >= coins[j]: # j가 coins의 갯수보다 작으면서, i의 값이 coins[j]보다 작은 동안 값을 비교한다. 
            temp = min(dp[i-coins[j]], temp)
            j += 1
        dp[i] = temp + 1
    return dp[money]

print(solution2(coins,140))

# 점화식
# 0                         if money <= 0
# min(dp[i-coins[j]], temp) if money > 0

# 풀이
# 

# 시간 복잡도 : O(n)