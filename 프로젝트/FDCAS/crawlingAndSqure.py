from google_images_download import google_images_download   #importing the library
import os
from PIL import Image


image_name = "Corpse Pose" #크롤링할 키워드와 폴더명
# 사진 크롤링
response = google_images_download.googleimagesdownload()   #class instantiation

arguments = {"keywords":image_name,"limit":100,"print_urls":True,"format":"jpg","extra_size":"-es 800,800"}   #creating list of arguments
paths = response.download(arguments)   #passing the arguments to the function
print(paths)

# 이미지 변환
input_dir = "downloads/"+image_name
output_dir = "output_images/"+image_name

if output_dir not in os.listdir():
    os.mkdir(output_dir)

files = os.listdir(input_dir)

for el in files:
    splt = el.split(".")
    ext = splt.pop()
    if ext in "jpg jpeg png bmp JPG JPEG PNG BMP":
        image = Image.open(input_dir + "/" + el)
        x, y = image.size
        if x>y:
            new_size = x
            x_offset = 0
            y_offset = int((x-y)/2)
            background_color ="black"
            new_image = Image.new("RGBA", (new_size,new_size), background_color)
            new_image.paste(image, (x_offset,y_offset))

            outfile_name= ".".join(splt) + ".png"
            new_image.save(output_dir + "/"+ outfile_name)
        elif y>x :
            new_size = y
            x_offset = int((y-x)/2)
            y_offset = 0
            background_color ="black"
            new_image = Image.new("RGBA", (new_size,new_size), background_color)
            new_image.paste(image, (x_offset,y_offset))

            outfile_name= ".".join(splt) + ".png"
            new_image.save(output_dir + "/"+ outfile_name)



  #printing absolute paths of the downloaded images 