n = [56,324,23,24,54,83,34] 

def filp(array, count=0):
    l = len(array)                                                  #배열의 길이
    if count == l:                                                  #배열의 길이와 count의 횟수가 같아진다면, 종료
        return array

    maxi = max(array[:l-count])                                     #배열의 0~(l-count)까지의 최대값
    index = array.index(maxi)                                       #최대값의 배열에서의 위치.
    array = array[0:index+1][::-1] + array[index+1:]                #찾은 maxi를 배열의 첫번째로 위치하게 하고,
    array = list(reversed(array[0:l-count])) + array[l-count:]      #그리고 찾은 maxi가 l-count의 위치로 전체 배열을 역전한다. 
    count += 1                                                      #단계를 구분하여 진행하기 위해 count에 1증가
    
    return filp(array, count)                                       #count가 배열의 길이와 같아질때까지 반복

print(filp(n))