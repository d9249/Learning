import glob
import os
from PIL import Image
import torchvision.transforms as transforms
import torch.utils.data as DATA
import matplotlib.pyplot as plt
import torchvision
import torch


def make_dataset():
    dataset = []
    original_img_rpath = '/data2/others/yun/data/Data_Height/test/original/'
    shadow_mask_rpath = '/data2/others/yun/data/Data_Height/test/mask/'
    shadow_free_rpath = '/data2/others/yun/data/Data_Height/test/texture/'
    
    for img_path in glob.glob(os.path.join(original_img_rpath, ('*.png'))):
        basename = os.path.basename(img_path)
        original_img_path = os.path.join(original_img_rpath, basename)
        shadow_mask_path = os.path.join(shadow_mask_rpath, basename)
        shadow_free_path = os.path.join(shadow_free_rpath, basename)
        dataset.append([original_img_path, shadow_mask_path, shadow_free_path])

    return dataset

class shadow_triplets_loader(DATA.Dataset):
    def __init__(self):
        super(shadow_triplets_loader, self).__init__()
        self.train_set_path = make_dataset()

    def __getitem__(self, item):
        original_img_path, shadow_mask_path, shadow_free_path = self.train_set_path[item]
        transform = transforms.ToTensor()
        
        original_img = Image.open(original_img_path)
        shadow_mask = Image.open(shadow_mask_path).convert('L')
        shadow_free = Image.open(shadow_free_path)
        
        name = os.path.basename(original_img_path)
        
        ###transform###
        original_img=transform(original_img.resize((400,400)))
        shadow_mask = transform(shadow_mask.resize((400,400)))
        shadow_free = transform(shadow_free.resize((400,400)))

        return original_img, shadow_mask, shadow_free,name

    def __len__(self):
        return len(self.train_set_path)