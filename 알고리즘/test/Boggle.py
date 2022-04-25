from pprint import pprint

a = int(input())
maze = [list(input()) for _ in range(5)]
b = int(input())
word = list(input() for _ in range(b))

pprint(maze, indent=5, width=50)
print(word)

list1 = [[-1, 0], [1, 0], [0, -1], [0, 1], [-1, 1], [1, -1], [-1, -1], [1, 1]]

#def recursive():
#    for x in range()