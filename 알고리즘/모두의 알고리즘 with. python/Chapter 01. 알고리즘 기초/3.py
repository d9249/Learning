# n명 중 두 명을 뽑아 짝을 짖는다고 할 때 짝을 지을 수 있는 모든 조합을 출력하는 알고리즘을 만들어 보세요.
# 예를 들어 입력이 리스트 ['Tom', 'Jerry', 'Mike']라면 다음과 같은 세가지 경우를 모두 출력합니다.
# Tom - Jerry
# Tom - Mike
# Jerry - Mike

p = list(map(str, input().split()))
print(p)
x = len(p)

for i in range(0, x-1):
    for j in range(i+1, x):
        print(p[i], p[j])