import glob
import os
from PIL import Image
import torchvision.transforms as transforms
import torch.utils.data as DATA
import matplotlib.pyplot as plt
import torchvision
import torch
import cv2

def make_dataset():
    dataset = []
    original_img_rpath = '/data2/others/yun/data/Data_Height/train/original/'
    shadow_mask_rpath = '/data2/others/yun/data/Data_Height/train/mask/'
    shadow_free_img_rpath = '/data2/others/yun/data/Data_Height/train/texture/'
    
    for img_path in glob.glob(os.path.join(original_img_rpath, '*.png')):
        basename = os.path.basename(img_path)
        original_img_path = os.path.join(original_img_rpath, basename)
        shadow_mask_path = os.path.join(shadow_mask_rpath, basename)
        shadow_free_img_path = os.path.join(shadow_free_img_rpath, basename)
        
        dataset.append([original_img_path, shadow_mask_path, shadow_free_img_path])

    return dataset

class shadow_triplets_loader(DATA.Dataset):
    def __init__(self):
        super(shadow_triplets_loader, self).__init__()
        self.train_set_path = make_dataset()

    def __getitem__(self, item):
        original_img_path, shadow_mask_path,shadow_free_img_path = self.train_set_path[item]
        transform = transforms.ToTensor()
        
        original_img = Image.open(original_img_path)
        shadow_mask = Image.open(shadow_mask_path).convert('L')
        shadow_free_img = Image.open(shadow_free_img_path)
        
        original_img=transform(original_img.resize((400, 400)))
        shadow_mask = transform(shadow_mask.resize((400, 400)))
        shadow_free_img = transform(shadow_free_img.resize((400,400)))
        return original_img, shadow_mask, shadow_free_img

    def __len__(self):
        return len(self.train_set_path)