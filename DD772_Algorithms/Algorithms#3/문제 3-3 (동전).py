# Nadiria 라는 (상상의) 나라에서는 다음과 같은 액면가의 동전(화폐)을 사용한다고 한다. 
# $1, $4, $7, $13, $28, $52, $91, $365
# 어떤 곳이든 사람들은 돈을 주고 받을 때, 가능한 적은 개수의 동전을 사용하기를 원한다.
# 입력으로 자연수 K가 주어지면 Nadiria 화폐를 이용하여 
# $K 를 만들 수 있는 최소 동전 개수를 출력하는 알고리즘을 설계하고 분석하시오.

def ad(n):
    while(True):
        if n == 0:
            return 0
        if n-365>=0:
            return ad(n-365) + 1
        if n-91>=0:
            return ad(n-365) +1
        if n-52>=0:
            return ad(n-365) +1
        if n-28>=0:
            return ad(n-365) +1
        if n-13>=0:
            return ad(n-365) +1
        if n-7>=0:
            return ad(n-365) +1
        if n-4>=0:
            return ad(n-365) +1
        if n-1>=0:
            return ad(n-365) +1
            
print(ad(365))