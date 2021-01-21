import time 
import heapq 

start_time = time.time() # 시간 측정 시작 
N = int(input()) 
coinUnits = list(map(int, input().split())) 
heapq.heapify(coinUnits) # 오름차순 정렬 


result = 1 
while coinUnits: 
    num = heapq.heappop(coinUnits) 
    print(num)
    if result < num: # 덧셈 조합에서 중간에 비는 숫자가 생기는 경우 
        break 
    result += num 
    
print(result) 
end_time = time.time() # 시간 측정 종료 
print("프로그램 수행 시간: ", end_time - start_time) # 수행 시간 출력

