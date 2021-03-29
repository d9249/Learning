import glob
import os
from PIL import Image
import torchvision.transforms as transforms
import torch.utils.data as data
from skimage import color, io
import matplotlib.pyplot as plt
import torchvision

def make_dataset():
    dataset = []
    
    original_img_rpath = './test/image/'
    shadow_mask_rpath = './test/mask/'
    
    for img_path in glob.glob(os.path.join(original_img_rpath, '*.png')):
        basename = os.path.basename(img_path)
        
        original_img_path = os.path.join(original_img_rpath, basename)
        shadow_mask_path = os.path.join(shadow_mask_rpath, basename)
        dataset.append([original_img_path, shadow_mask_path])#,shadow_free_img_path])
        
    
    return dataset


class shadow_triplets_loader(data.Dataset):
    def __init__(self):
        super(shadow_triplets_loader, self).__init__()
        self.train_set_path = make_dataset()

    def __getitem__(self, item):
        original_img_path, shadow_mask_path = self.train_set_path[item]
        
        transform = transforms.ToTensor()
        original_img = Image.open(original_img_path).convert('RGB')
        shadow_mask = Image.open(shadow_mask_path)
        image_name = []
        image_name.append([original_img_path[:-4]])
        
        original_img = transform(original_img)
        shadow_mask = transform(shadow_mask)
        return original_img, shadow_mask,image_name#, shadow_free_img,image_name
    
    def __len__(self):
        return len(self.train_set_path)
