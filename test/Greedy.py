n = int(input())
coin = list(map(int, input().split()))
coin.sort()
target = 1
for c in coin:
    if c <= target:
        target+=c
        print(target)
    else:
        break
print(target)