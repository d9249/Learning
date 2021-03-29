import cv2 as cv
import numpy as np
import os

def change_texture(mdir,tdir,texture_with_mask_dir,texture_mask_dir , index):
    texture_list = os.listdir(tdir)

    for text_name in texture_list:
        print(text_name)
        tpath = os.path.join(tdir+text_name)
        mpath = os.path.join(mdir+text_name)
        gray = cv.imread(mpath,0)
        gray[gray > 100] = 255
        texture = cv.imread(tpath,1)
        b,g,r = cv.split(texture) 
        
        
        for y in range(0,texture.shape[0],1):
            for x in range(0,texture.shape[1],1):
                if(gray[y][x] == 255):
                    continue
                else :
                    gray_value =  float((int(gray[y][x])) / 100.0)
                    gray_value = gray_value / index
                    b[y][x] = (float(b[y][x]) * float(gray_value))
                    g[y][x] = (float(g[y][x]) * float(gray_value))
                    r[y][x] = (float(r[y][x]) * float(gray_value))

        texture = cv.merge([b,g,r])
        cv.imwrite(texture_with_mask_dir+text_name,texture)
        cv.imwrite(texture_mask_dir + text_name, gray)
    return


if __name__ == '__main__':

    # data dir
    mask_dir = 'SC_Mask/'
    texture_dir = 'SC_Texture/'
    # saving dir
    texture_mask_dir = 'SC_Texture_Mask/'
    if not os.path.isdir(mask_dir):
        print("no mask dir")
    if not os.path.isdir(texture_dir):
        print("no texture dir")

    if not os.path.isdir(texture_mask_dir):
        print('no texture mask saving dir \nmake saving dir')
        os.mkdir(texture_mask_dir)


    for i in range(0,1,1):
        texture_with_mask_dir = 'SC_Texture_With_Mask'
        texture_with_mask_dir = texture_with_mask_dir  + '_' + (i+1).__str__() +'/'
        if not os.path.isdir(texture_with_mask_dir):
            print('no texture with mask saving dir \nmake saving dir')
            os.mkdir(texture_with_mask_dir)
        change_texture(mask_dir, texture_dir, texture_with_mask_dir , texture_mask_dir,i+1)
