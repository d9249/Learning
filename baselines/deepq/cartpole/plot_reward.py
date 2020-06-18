import matplotlib.pyplot as plt
import numpy as np

files = []

#비교 대상 파일 불러오기
files.append(open("../../32neurons_1.txt", 'r'))
files.append(open("../../32neurons_2.txt", 'r'))
files.append(open("../../32neurons_3.txt", 'r'))

files.append(open("../../64neurons_1.txt", 'r'))
files.append(open("../../64neurons_2.txt", 'r'))
files.append(open("../../64neurons_3.txt", 'r'))

files.append(open("../../128neurons_1.txt", 'r'))
files.append(open("../../128neurons_2.txt", 'r'))
files.append(open("../../128neurons_3.txt", 'r'))

"""
   add more files (ex. files.append(open("FILE NAME", 'r')) )
"""

_list = [[] for _ in range(len(files))]


def read_reward(reward_list, f):
   while True:
      line = f.readline()
      if not line: break
      reward_list.append(float(line))

def get_dr_list(reward_list, mag):
   t = 0
   dr_list = [reward_list[0]]
   for reward in reward_list: 
      if t > 100000:
          break
      dr_list.append(0.9*dr_list[-1] + 0.1*reward*mag)
      t += 1
   return dr_list


def meanPlot(listlist):
    listlist = np.array(listlist)
    returnList = []
    smallestLen = len(listlist[0])
    for rlist in listlist:
        if len(rlist) < smallestLen:
            smallestLen = len(rlist)

    for i in range(smallestLen):
        if i > 2000:
            break
        avg = 0
        for rewardlist in listlist:
            avg += rewardlist[i]
        avg /= len(listlist)
        returnList.append(avg)

    return returnList


for l, f in zip(_list, files):
    read_reward(l, f)

# 그래프 생성 (각 항목마다 3개의 누적 reward의 평균을 이용)

_32_neurons = [get_dr_list(_list[i], 1) for i in range(3)]
_64_neurons = [get_dr_list(_list[i], 1) for i in range(3,6)]
_128_neurons = [get_dr_list(_list[i], 1) for i in range(6,9)]

plt.plot(meanPlot(_32_neurons), label='32 neurons')
plt.plot(meanPlot(_64_neurons), label='64 neurons(baseline)')
plt.plot(meanPlot(_128_neurons), label='128 neurons')

"""
add more plotting 
(ex. 
VARIABLE_NAME = [get_dr_list(_list[i], 1) for i in range(NUM)]
plt.plot(get_dr_list(_list[INDEX OF FILE], 1), label='description about file') 
)
"""

plt.xlabel('Episode')
plt.ylabel('Accumulated Reward')
# 비교 대상에 따라 그래프 제목 변경
plt.title('# of Hidden Layer Neurons')
plt.legend()
plt.show()

for f in files:
    f.close()
