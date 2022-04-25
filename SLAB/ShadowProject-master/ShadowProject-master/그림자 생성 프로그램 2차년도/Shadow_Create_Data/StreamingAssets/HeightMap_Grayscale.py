import numpy as np
import scipy.misc
import argparse
import os
import cv2

count = 0
path = "./../../Deep_Learning_Image/Test_Image_Height/"
save_path = './../../Deep_Learning_Image/Test_Image_Height/'
dirlist = os.listdir(path)

print('111')
parser = argparse.ArgumentParser()

parser.add_argument('--test_folder',type=str,default ="./../../Deep_Learning_Image/Test_Image_Height/")
parser.add_argument('--save_folder',type=str,default= './../../Deep_Learning_Image/Test_Image_Height/')
parser.add_argument('--file_name', type=str, default ='0')
parser.add_argument('--screen_width', type=int , default =400)
parser.add_argument('--screen_height',type=int, default =400)
args = parser.parse_args()

print('111')




print(parser)
file_path = os.path.join(args.test_folder,args.file_name+'.txt')
txt = np.genfromtxt(file_path, encoding='utf-16')
blank_image = np.zeros((args.screen_height,args.screen_width,1),np.uint8)

# count text file size
text_count = 0
text_list = []
for i in txt:
    text_list.append(i)
    text_count += 1



print("len : ", len(text_list))
print("image len : ", len(blank_image))
new_count = 0
color = 0
max  = 0

print(blank_image.shape[0]*blank_image.shape[1])
for y in range(blank_image.shape[0]):
    for x in range(blank_image.shape[1]):
        
        if int(text_list[new_count]) < 1000 and max< int(text_list[new_count]):
            max = int(text_list[new_count])
        if text_list[new_count] > 255.0 :
            blank_image[y][x] = 255.0
        else :
            # lotte tower 52.57011
            #img[y][x] = (text_list[new_count] / 100) * 245.0
            # no maximum value adjusted
            #img[y][x] = text_list[new_count]
            blank_image[y][x] = (float(text_list[new_count]) / 100) * 245.0
            color += 1
        
        new_count += 1
        #print(new_count)

print('max : ', max)
print('color : ', color)
cv2.imwrite(args.save_folder+args.file_name+'.png',blank_image)
