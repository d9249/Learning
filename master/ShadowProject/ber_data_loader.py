import glob
import os
from PIL import Image
import torchvision.transforms as transforms
import torch.utils.data as DATA
from skimage import color, io
import matplotlib.pyplot as plt
import torchvision
from skimage import color, io
import torch


def make_dataset():
    dataset = []
    original_img_rpath = '/home/heejin/ber/ruiquo/'
    shadow_mask_rpath = '/home/heejin/Downloads/data/unity/extra_test/Shadow_less/'
#'/home/heejin/Downloads/data/unity/extra_test/Mask_re/'
    
    for img_path in glob.glob(os.path.join(original_img_rpath, ('*.png'))):
        basename = os.path.basename(img_path)
        original_img_path = os.path.join(original_img_rpath, basename)
        shadow_mask_path = os.path.join(shadow_mask_rpath, basename)
        
        dataset.append([original_img_path, shadow_mask_path])

    return dataset

class shadow_triplets_loader(DATA.Dataset):
    def __init__(self):
        super(shadow_triplets_loader, self).__init__()
        self.train_set_path = make_dataset()

    def __getitem__(self, item):
        original_img_path, shadow_mask_path = self.train_set_path[item]
        transform = transforms.ToTensor()
        
        original_img = Image.open(original_img_path)
        shadow_mask = Image.open(shadow_mask_path)
        
        ###transform####
        
        original_img=transform(original_img.resize((352, 352)))
        shadow_mask = transform(shadow_mask.resize((352, 352)))
        
        
        return original_img, shadow_mask

    def __len__(self):
        return len(self.train_set_path)