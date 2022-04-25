import glob
import os
from PIL import Image
import torchvision.transforms as transforms
import torch.utils.data as DATA
from skimage import color, io
import matplotlib.pyplot as plt
import torchvision




DATAPATH = "D:/OpenDataSet/"                    # Juwani PC
#DATAPATH = "/home/heejin/Downloads/data/"
#DATAPATH = "/HDD3/OpenDataSet/"                # ETRI Server

ORIGIN_PATH = "ISTD_Dataset/train/train_A/"
MASK_PATH = "ISTD_Dataset/train/train_B/"
SDFREE_PATH = "ISTD_Dataset/train/train_C/"


def make_dataset():
    dataset = []
#   original_img_rpath = '/home/heejin/Downloads/data/ISTD_Dataset/test/test_A'
#   shadow_mask_rpath = '/home/heejin/Downloads/data/ISTD_Dataset/test/test_B'
#   shadow_free_img_rpath = '/home/heejin/Downloads/data/ISTD_Dataset/test/test_C'

    original_img_rpath = DATAPATH + ORIGIN_PATH
    shadow_mask_rpath = DATAPATH + MASK_PATH
    shadow_free_img_rpath = DATAPATH + SDFREE_PATH

    for img_path in glob.glob(os.path.join(original_img_rpath, '*.png')):
        basename = os.path.basename(img_path)
        original_img_path = os.path.join(original_img_rpath, basename)
        shadow_mask_path = os.path.join(shadow_mask_rpath, basename)
        shadow_free_img_path = os.path.join(shadow_free_img_rpath, basename)
        #print(original_img_path, shadow_mask_path, shadow_free_img_path)
        dataset.append([original_img_path, shadow_mask_path,shadow_free_img_path])

    return dataset



class shadow_triplets_loader(DATA.Dataset):
    def __init__(self):
        super(shadow_triplets_loader, self).__init__()
        self.train_set_path = make_dataset()
        print("shadow_triplets_loader __init__ ")

    def __getitem__(self, item):
        original_img_path, shadow_mask_path,shadow_free_img_path = self.train_set_path[item]
        transform = transforms.ToTensor()
        #print(original_img_path, shadow_mask_path, shadow_free_img_path)
        
        original_img = Image.open(original_img_path)
        shadow_mask = Image.open(shadow_mask_path)
        shadow_free_img = Image.open(shadow_free_img_path)
        
        ###transform####
        
        original_img = transform(original_img.resize((224, 224)))
        shadow_mask = transform(shadow_mask.resize((224, 224)))
        shadow_free_img = transform(shadow_free_img.resize((224, 224)))
            
        return original_img, shadow_mask, shadow_free_img

    def __len__(self):
        return len(self.train_set_path)
