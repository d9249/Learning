import torch
import torch.nn.functional as F
import numpy as np
import pdb, os, argparse
import os
from model.CPD_models import CPD_VGG
from model.CPD_ResNet_models import CPD_ResNet
from data import test_dataset
from PIL import Image
import torchvision
import matplotlib.pyplot as plt
import torchvision.transforms as transforms
import matplotlib.pyplot as plt
from skimage import io

os.environ["CUDA_VISIBLE_DEVICES"] = '0'
parser = argparse.ArgumentParser()
parser.add_argument('--original_path', type=str, default ='../data/original/',help='../')
parser.add_argument('--height_path', type=str, default='../data/height_map/')
parser.add_argument('--result_save_path', type=str, default='../result/rough_result/')
parser.add_argument('--mask_path', type=str, default='../data/mask/')
parser.add_argument('--test_image', type=str, default='100.png',help = 'test_image_name')
parser.add_argument('--testsize', type=int, default=352, help='testing size')
parser.add_argument('--is_ResNet', type=bool, default=True, help='testing size')
opt = parser.parse_args()

def show_result(original,mask,result):
    fig = plt.figure(figsize=(15,5))
    rows = 1
    cols = 3   
    ax1 = fig.add_subplot(rows, cols, 1)
    ax1.imshow(original,aspect='auto')
    ax1.set_title('original')
    ax1.axis("off")
     
    ax2 = fig.add_subplot(rows, cols, 2)
    ax2.imshow(mask,cmap = plt.get_cmap('gray'),aspect='auto')
    ax2.set_title('mask')
    ax2.axis("off")
    
    ax3 = fig.add_subplot(rows, cols, 3)
    ax3.imshow(result,aspect='auto')
    ax3.set_title('rough_result')
    ax3.axis("off")
     
    plt.show()

if opt.is_ResNet:
    model = CPD_ResNet()
    model.load_state_dict(torch.load('./models/channel/checkpoint.pth'))

if opt.is_ResNet:
    save_path = opt.result_save_path
else:
    save_path = './results/' + dataset + '/'
if not os.path.exists(save_path):
    os.makedirs(save_path)

img_transform = transforms.Compose([
            transforms.Resize((opt.testsize, opt.testsize)),
            transforms.ToTensor()])

test_datasets=['rough_result']
model.cuda()
for dataset in test_datasets:

    original_image = Image.open(opt.original_path+opt.test_image)
    shadow_mask = Image.open(opt.mask_path+opt.test_image)
    height = Image.open(opt.height_path+opt.test_image)
    
    original_image = img_transform(original_image)
    height_image = img_transform(height)
    
    original_image = original_image.cuda()
    height = height_image.cuda()
    
    images = torch.cat((original_image.unsqueeze(0),height.unsqueeze(0)),1)
    _, res = model(images)
    res = res.sigmoid().data.cpu().numpy().squeeze()
    res = (res - res.min()) / (res.max() - res.min() + 1e-8)
    res = res>0.5
    io.imsave(opt.result_save_path+opt.test_image,res+0.0)
    ori = Image.open(opt.original_path+opt.test_image)
    result = Image.open(opt.result_save_path+opt.test_image)
    show_result(ori,shadow_mask,result)
