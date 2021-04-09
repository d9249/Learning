n = [13,6,9,8,12]

def min(array, com):
    if not array:                               #array의 비교할 것이 남아있지 않다면,
        return com                              #com을 넘기면서 프로그램을 종료한다.
    now = array[0]                              #배열의 첫번째를 now로 선언.
    if now < com:                               #now가 com보다 작다면,
        return min(array[1:], now)              #배열의 첫번째수가 비교하는 수보다 작으면, now를 com로 대체한다.
    else:                                       #now가 com보다 작지 않다면,
        return min(array[1:], com)              #배열의 첫번째가 비교하는 수보다 작지 않다면, com을 유지한다.

def selection_sort(array, count=0):
    if count == len(array):                     #array의 길이 만큼, count가 진행되었다면,
        return array                            #array를 리턴.

    mini = min(array[count:], array[count])     #min함수를 사용하여서 배열의 가장 최솟값을 찾아 mini에 저장
    index = array.index(mini)                   #최솟값 mini의 index번호를 찾는다.
    array[index] = array[count]                 
    array[count] = mini                         #단계(count)의 맞게 찾은 최솟값을 대상의 array와 위치를 바꾼다.
    count += 1                                  #다음 최소값을 찾기 위해서, count를 1증가 시켜 다음 단계를 진행.
    return selection_sort(array, count)         

print(selection_sort(n))
