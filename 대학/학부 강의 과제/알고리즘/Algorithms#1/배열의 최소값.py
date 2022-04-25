n = [13,6,9,8,12]

def min(array, com):
    if not array:                       #array의 비교할 것이 남아있지 않다면,
        return com                      #com을 넘기면서 프로그램을 종료한다.
    now = array[0]                      #배열의 첫번째를 now로 선언.
    if now < com:                       #now가 com보다 작다면,
        return min(array[1:], now)      #배열의 첫번째수가 비교하는 수보다 작으면, now를 com로 대체한다.
    else:                               #now가 com보다 작지 않다면,
        return min(array[1:], com)      #배열의 첫번째가 비교하는 수보다 작지 않다면, com을 유지한다.

print(min(n, n[0]))