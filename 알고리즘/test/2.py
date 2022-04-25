num = int(input())

def three(num):
    return num/3

def two(num):
    return num/2

def mins(num):
    return num-1

while num != 1:
    if num%3 == 0:
        three(num)
        print(num)
    if num%2 == 0:
        two(num)
        print(num)
    if num%3 == 0 & num%2==0:
        mins(num)
        print(num)
else:
    print(num)