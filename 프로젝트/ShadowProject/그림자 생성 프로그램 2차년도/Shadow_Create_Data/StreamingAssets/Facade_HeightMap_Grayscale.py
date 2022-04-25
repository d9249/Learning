import numpy as np
import scipy.misc
import os
import cv2

count = 0
path = "./SC_Facade_Height/"
dirlist = os.listdir(path)

save_path = './SC_Facade_Height_GrayScale/'
if not os.path.isdir(save_path) :
    os.mkdir(save_path)
else :
    pass

for filename in dirlist:
    print(filename)
    file_path = os.path.join(path,filename)
    txt = np.genfromtxt(file_path, encoding='utf-16')

    # count text file size
    text_count = 0
    text_list = []
    for i in txt:
        text_list.append(i)
        text_count += 1
    print("len : ",len(text_list))

    # create array of image
    img = cv2.imread('1.png',0)
    new_count = 0
    color = 0
    max  = 0
    for y in range(img.shape[0]):
        for x in range(img.shape[1]):
            if int(text_list[new_count]) < 1000 and  max < int(text_list[new_count]):
                max = int(text_list[new_count])

            if text_list[new_count] > 255.0 :
               img[y][x] = 255.0
            else :
               # lotte tower 52.57011
               #img[y][x] = (text_list[new_count] / 100) * 245.0
               # no maximum value adjusted
               #img[y][x] = text_list[new_count]
               img[y][x] = (float(text_list[new_count]) / 100) * 245.0
               color += 1
            new_count += 1

    print('max : ',max)
    print('color : ',color)
    cv2.imwrite(save_path+filename+'.png',img)
