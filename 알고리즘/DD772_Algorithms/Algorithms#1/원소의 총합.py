n = [13,6,9,8,12]

def total(array, sum=0):
    if not array:                   #array의 비교할 것이 남아있지 않다면,
        return sum                  #sum을 넘기면서 프로그램을 종료한다.
    sum += array[0]                 #sum에 첫번째 원소를 더한다.
    return total(array[1:], sum)    #이미 더한 원소를 빼기위해서 0번째 원소를 뺀 배열과 더해져있는 sum을 리턴한다.

print(total(n))