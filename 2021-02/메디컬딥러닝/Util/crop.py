# import os
# import glob
# from PIL import Image

# files = glob.glob('C:/Users/dodo9/Downloads/crop_data/4/*.jpg')

# for f in files:
#     img = Image.open(f)
#     img_resize = img.resize((224, 224))
#     title, ext = os.path.splitext(f)
#     img_resize.save(title + '_224' + ext)


# import os, shutil
 
# for root, subdirs, files in os.walk('C:/Users/dodo9/Downloads/crop_data/4/'):
#     for f in files:
#         if '_224' in f:
#             file_to_move = os.path.join(root, f)
#             shutil.move(file_to_move, 'C:/Users/dodo9/Downloads/crop_data/224_4/')

# DPhi test 분포
num0 = 39.42798774259448
num1 = 18.07967313585291
num2 = 26.353421859039837
num3 = 13.125638406537282
num4 = 3.0132788559754853

# sum = num0+num1+num2+num3+num4
# print(sum)

# train data 분포
n0 = 3857/9786
n1 = 1770/9786
n2 = 2578/9786
n3 = 1286/9786
n4 = 295/9786

nn0 = 39.413447782546496
nn1 = 18.087063151440833
nn2 = 26.343756386674844
nn3 = 13.14122215409769
nn4 = 3.014510525240139

s1 = n0+n1+n2+n3+n4
ss1 = nn0+nn1+nn2+nn3+nn4
print(s1, ss1)



